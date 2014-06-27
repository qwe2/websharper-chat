namespace WebsharperChat

open System
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.JQuery
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Html5

[<JavaScript>]
module ChatClient =

    let Append (elem: Element) =
        let cb = ById("chatbox").AppendChild(elem.Dom).ParentNode
        JQuery.Of(cb).ScrollTop(JQuery.Of(cb).Height()) |> ignore

    let ShowUserList (lst: Element) =
        let ul = JQuery.Of("#userlist")
        ul.Children().Each(fun (a : Dom.Element) _ -> JQuery.Of(a).Remove() |> ignore) |> ignore
        ul.Append(lst.Dom) |> ignore

    let AppendUser (name: string) =
        let n =  LI [ Text name ]
        ById("userli").AppendChild(n.Dom) |> ignore

    let RemoveUser (name: string) =
        JQuery.Of("#userli").Children().Each(fun (a : Dom.Element) _ -> let n = JQuery.Of(a)
                                                                        JavaScript.Log(n.Text())
                                                                        if n.Text() = name then
                                                                            n.Remove() |> ignore
                                             ) |> ignore        

    let RenderError (msg: string) =
        P [ Text msg; Attr.Class "bg-danger" ]

    let RenderMsg (user: string) (msg: string) =
        P [ Strong [ Text (user + ": ") ] ] -< [ Text msg; Attr.Class "bg-info" ]

    let RenderUserlist (lst: string array) =
        UL [ Attr.Id "userli" ] -< Array.map (fun x -> LI [ Text x ]) lst

    let RenderInfo (msg: string) =
        P [ Text msg; Attr.Class "bg-warning" ]
            
    let HandleMessage = 
        function
            | Chat.Error msg                 -> RenderError msg |> Append
            | Chat.Msg (u, m)                -> RenderMsg u m |> Append
            | Chat.Userlist lst              -> RenderUserlist lst |> ShowUserList
            | Chat.Userconnect (true, name)  -> (name + " has connected") |> RenderInfo |> Append
                                                AppendUser name
            | Chat.Userconnect (false, name) -> (name + " has disconnected") |> RenderInfo |> Append
                                                RemoveUser name

    let SetEventHandlers (ws: WebSocket) =
        ws.Onmessage <- (fun d -> 
                            let data = As<Chat.Message> <| Json.Parse (d.Data.ToString())
                            HandleMessage data)

        ws.Onerror <- (fun() -> RenderError "Something went wrong." |> Append)
        ws

    let Connect href =
        SetEventHandlers <| WebSocket href

    let SendText (ws: WebSocket) (textbox: Element) =
        let text = textbox.Value.Trim()
        if text <> "" then
            ws.Send text
        textbox.Value <- ""

    let Main (logout: string) =
        let ws = Connect ("ws://" + Window.Self.Location.Host + "/chatsocket")
        let textbox = Input [ Text ""; Attr.Id "message-box"; Attr.Class "form-control col-md-12"; Attr.Type "text" ] 
        textbox |>! OnKeyPress (fun input char -> if char.CharacterCode = 13 then SendText ws textbox) |> ignore
        Div [ Attr.Class "container" ] -< 
        [ 
            A [ Attr.HRef logout; Attr.Class "btn btn-primary"; Text "Log out" ]             
            Div [ Attr.Class "row styled_container" ] -<
            [
                Div [ Attr.Id "chatbox"; Attr.Class "col-md-10" ]
                Div [ Attr.Id "userlist"; Attr.Class "col-md-2" ]
            ]
            Div [ Attr.Class "row" ] -<
            [
                textbox
            ]
            
        ]

type ChatControl(logout: string) =
    inherit Web.Control()

    [<JavaScript>]
    override __.Body =
        ChatClient.Main(logout) :> _