namespace WebsharperChat

open System
open System.Collections.Concurrent
open System.Security
open System.Text
open System.Threading
open System.Web
open Microsoft.Web.WebSockets
open System.Web.Script.Serialization
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

module Chat =
    [<JavaScript>]
    type Message =
        {
            Username: string
            Msg:      string
        }

    type User = 
        {
            Name: string
            Token: string
        }

    let private serializer = new JavaScriptSerializer()

    type WebSocketContainer() =
        inherit ConcurrentDictionary<string, User * WebSocketHandler>()
        
        member this.Broadcast(usr: User, message: string) =
            for elem in this do
                let (_, ws) = elem.Value
                serializer.Serialize({ Username = usr.Name; Msg = message })
                |> ws.Send

        member this.Broadcast(token: string, message: string) =
            match this.TryGetValue(token) with
                | (true, (u, _)) -> this.Broadcast(u, message)
                | (false, _) -> ()

    let mutable clients = new WebSocketContainer()

    type WebSocketChatHandler() =
        inherit WebSocketHandler()

        override this.OnOpen() = ()
//            match UserSession.GetLoggedInUser() with
//                | None -> ()
//                | Some token -> System.Diagnostics.Debug.WriteLine(token)
            

        override this.OnMessage(message: string) =
            ()            

        override this.OnClose() = 
            ()

type ChatWebSocket() = 
    interface IHttpHandler with
        member this.ProcessRequest context =
            if context.IsWebSocketRequest then
                context.AcceptWebSocketRequest(new Chat.WebSocketChatHandler())
        member this.IsReusable = true

(*    

    type Message =
        {
            Time: int
            Sender: User
            Text: String
        }

    module Auth =
        type Key = class end

        let Key = typeof<Key>.GUID.ToString()

        type Token =
            {
                Name: String
                Hash: String
            }

        let private sha256 = Cryptography.SHA256.Create()

        let Generate (name: String): Token =
            let hash = new StringBuilder()
            sprintf "%s:%s" name Key
            |> Encoding.Unicode.GetBytes
            |> sha256.ComputeHash
            |> Array.iter (fun by -> hash.Append(by.ToString("X2")) |> ignore)
            {
                Name = name
                Hash = hash.ToString()
            }

        let Validate (token: Token): bool =
            Generate token.Name = token
            *)
            
