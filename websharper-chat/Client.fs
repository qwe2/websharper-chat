namespace WebsharperChat

open System
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.JQuery
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Html5

module Log =
    [<Name "console.log">]
    [<Stub>]
    let consoleLog (obj: Object) = ()


[<JavaScript>]
module ChatClient =
    type Anim = { scrollTop: int }

    let RenderMsg (data: Chat.Message) =
        P [ Strong [ Text (data.Username + ": ") ] ] -< [ Text data.Msg; Attr.Class "bg-info" ]

    let RenderError (msg: string) =
        P [ Text msg; Attr.Class "bg-danger" ]

    let Append (elem: Element) =
        let cb = ById("chatbox").AppendChild(elem.Dom).ParentNode
        JQuery.Of(cb).ScrollTop(JQuery.Of(cb).Height()) |> ignore

    let SetEventHandlers (ws: WebSocket) =
        ws.Onmessage <- (fun d -> 
                            let data = As<Chat.Message> <| Json.Parse (d.Data.ToString())
                            RenderMsg data |> Append)

        ws.Onerror <- (fun() -> RenderError "Something went wrong." |> Append)
        ws

    let Connect href =
        SetEventHandlers <| WebSocket href

    let SendText (ws: WebSocket) (textbox: Element) =
        let text = textbox.Value.Trim()
        if text <> "" then
            ws.Send text
        textbox.Value <- ""

    let Main () =
        let ws = Connect ("ws://" + Window.Self.Location.Host + "/chatsocket")
        let textbox = Input [ Text ""; Attr.Id "message-box"; Attr.Class "form-control"; Attr.Type "text" ] 
        textbox |>! OnKeyPress (fun input char -> if char.CharacterCode = 13 then SendText ws textbox) |> ignore
        Div [ Attr.Class "container" ] -< 
        [ 
            Div [ Attr.Id "chatbox" ]
            textbox
        ]
