namespace WebsharperChat

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Html5

[<JavaScript>]
module Client =

    let Render text color =
        P [Text text; Attr.Style <| "color: " + color]

    let Append data =
        ById("chatbox").AppendChild((Render data "black").Dom) |> ignore

    let SetEventHandlers (ws: WebSocket) =
        ws.Onmessage <- (fun(data) -> Append <| data.Data.ToString() )
        ws.Onerror <- (fun() -> Render "Error" "red" |> ignore)
        ws

    let Connect href =
        SetEventHandlers <| WebSocket href

    let SendText (ws: WebSocket) (textbox: Element) =
        ws.Send textbox.Value
        textbox.Value <- ""

    let Main () =
        let ws = Connect ("ws://" + Window.Self.Location.Host + "/chat")
        let textbox = Input [ Text ""; Attr.Id "message-box"; Attr.Class "form-control"; Attr.Type "text" ] 
        textbox |>! OnKeyPress (fun input char -> if char.CharacterCode = 13 then SendText ws textbox) |> ignore
        Div [ 
            Div [ Attr.Id "chatbox" ]
            textbox
        ]
