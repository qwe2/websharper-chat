namespace WebsharperChat

open System
open System.Collections.Concurrent
open System.Security
open System.Text
open System.Threading

open System.Web
open System.Net.WebSockets
open Microsoft.Web.WebSockets

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

open System.Web.Security
open System.Security.Principal

module Chat =
    type User = 
        {
            Name: string
        }

    [<JavaScript>]
    type Message =
        | Msg         of string * string
        | Error       of string
        | Userlist    of string array
        | Userconnect of bool * string

    module private MessageEncoder =
        module J = IntelliFactory.WebSharper.Core.Json

        let private jP = J.Provider.Create()
        let private enc = jP.GetEncoder<Message>()
        let ToJString (msg: Message) =
            enc.Encode msg
            |> jP.Pack
            |> J.Stringify

    type WebSocketContainer() =
        inherit ConcurrentDictionary<string, User * WebSocketHandler option>()
        
        member this.Broadcast(message: Message) =
            for elem in this do
                let (_, wso) = elem.Value
                wso |> Option.iter (fun ws -> MessageEncoder.ToJString message |> ws.Send)                    

        member this.Broadcast(usr: User, message: string) =
            for elem in this do
                let (_, wso) = elem.Value
                match wso with
                    | Some ws ->
                        Msg (usr.Name, message)
                        |> MessageEncoder.ToJString
                        |> ws.Send
                    | None -> ()

        member this.Broadcast(token: string, message: string) =
            match this.TryGetValue(token) with
                | (true, (u, _)) -> this.Broadcast(u, message)
                | (false, _) -> ()

    let mutable clients = new WebSocketContainer()

    let LoginUser (token: string) (username: string) (ws: WebSocketHandler option) = 
        clients.AddOrUpdate(token, ({ Name = username }, ws),
            fun _ usr -> 
                match usr with
                | (u, _) -> ({u with Name = username}, ws))
        |> ignore

    let LogoutUser =
        try
            match UserSession.GetLoggedInUser () with
                | Some token ->
                        UserSession.Logout ()
                        clients.TryRemove token |> ignore
                | None -> ()

        with :? NullReferenceException -> ()       

    let AuthUser(ctx: WebSocketContext) =
        match ctx.CookieCollection.[FormsAuthentication.FormsCookieName] with
        | null -> None
        | cookie ->
            let ticket = FormsAuthentication.Decrypt cookie.Value
            let principal = GenericPrincipal(FormsIdentity(ticket), [||])
            if principal.Identity.IsAuthenticated then
                Some principal.Identity.Name
            else 
                None

    type WebSocketChatHandler() =
        inherit WebSocketHandler()

        member this.SendError (msg: string) =
            Error msg
            |> MessageEncoder.ToJString
            |> this.Send                        

        member this.Auth (success: string -> unit, fail: unit -> unit): unit =
            match AuthUser this.WebSocketContext with
                | None       -> fail ()
                | Some token -> success token

        override this.OnOpen() =
            let fn token = 
                match clients.TryGetValue(token) with
                    | (true, (user, _)) ->
                        async {                                    
                            clients.Broadcast(Userconnect(true, user.Name))
                            LoginUser token user.Name (Some (this :> _))
                            Array.ofSeq clients.Values
                            |> Array.map (function
                                            | u, _ -> u.Name)
                            |> Userlist
                            |> MessageEncoder.ToJString
                            |> this.Send                                                       
                        } |> Async.Start
                    | (false, _) ->
                        this.SendError "You are not logged in"
                        
            this.Auth (fn, fun () -> this.SendError "You are not logged in")

        override this.OnMessage(message: string) =
            let fn (token: string) = 
                clients.Broadcast (token, message)
            this.Auth (fn, fun () -> this.SendError "You are not logged in")

        override this.OnClose() = 
            let fn token =
                let (u, _) = clients.[token]
                clients.Broadcast(Userconnect(false, u.Name))
            this.Auth (fn, fun () -> ())

type ChatWebSocket() = 
    interface IHttpHandler with
        member this.ProcessRequest context =
            if context.IsWebSocketRequest then
                context.AcceptWebSocketRequest(new Chat.WebSocketChatHandler())
        member this.IsReusable = true            
