namespace WebsharperChat

open System
open System.Collections.Generic
open System.Security
open System.Text
open System.Threading
open System.Web
open Microsoft.Web.WebSockets
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.JQuery


module Chat =
    let mutable clients = new WebSocketCollection()

    type WebSocketChatHandler() =
        inherit WebSocketHandler()

        override this.OnOpen() = 
            clients.Add(this)

        override this.OnMessage(message: string) =
            clients.Broadcast(message)
            
        override this.OnClose() = 
            clients.Remove this |> ignore


type ChatWebSocket() = 
    interface IHttpHandler with
        member this.ProcessRequest context =
            if context.IsWebSocketRequest then
                context.AcceptWebSocketRequest(new Chat.WebSocketChatHandler())
        member this.IsReusable = true

(*    type User = 
        {
            Name: string
        }

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
            
