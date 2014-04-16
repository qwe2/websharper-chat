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
    [<JavaScript>]
    type Message =
        | Msg of string * string
        | Error of string

    type User = 
        {
            Name: string
        }
    
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
        match UserSession.GetLoggedInUser () with
            | Some token ->
                    UserSession.Logout ()
                    clients.TryRemove token |> ignore
            | None -> ()                                     

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

        member this.Auth (success: string -> unit, failmsg: string): unit =
            match AuthUser this.WebSocketContext with
                | None       -> this.SendError failmsg
                | Some token -> success token

        override this.OnOpen() =
            let fn token = async {
                                    let! uname = SQLConnection.GetUsernameByToken token
                                    match uname with
                                        | None      -> this.SendError "You are not logged in."
                                        | Some user -> LoginUser token user (Some (this :> _))
                                } |> Async.Start
            this.Auth (fn, "You are not logged in")

        override this.OnMessage(message: string) =
            let fn (token: string) = 
                clients.Broadcast (token, message)
            this.Auth (fn, "You are not logged in")

        override this.OnClose() = 
            ()

type ChatWebSocket() = 
    interface IHttpHandler with
        member this.ProcessRequest context =
            if context.IsWebSocketRequest then
                context.AcceptWebSocketRequest(new Chat.WebSocketChatHandler())
        member this.IsReusable = true            
