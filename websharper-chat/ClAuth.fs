namespace WebsharperChat

open System
open System.Collections.Generic
open System.Security
open System.Text
open System.Threading
open System.Web
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.JQuery
open IntelliFactory.WebSharper.Sitelets
open IntelliFactory.WebSharper.Piglets
open IntelliFactory.WebSharper.Piglets.Piglet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper.JQuery

module ClAuth =   
    [<Inline "window.location=$url">]
    let Redirect (url: string) = ()

    [<Rpc>]
    let Login (username: string) (password: string) =
        let resp = SQLConnection.Authenticate username password |> Async.RunSynchronously
        match resp with
            | Some token -> 
                  UserSession.LoginUser token
                  Chat.LoginUser token username None
                  true
            | None -> false
        
    [<JavaScript>]
    let LoginPiglet =
        Piglet.Return (fun x y -> x, y)
        <*> (Piglet.Yield ""
            |> Validation.Is Validation.NotEmpty "Enter Username")
        <*> (Piglet.Yield ""
            |> Validation.Is Validation.NotEmpty "Enter password")
        |> Piglet.WithSubmit
        |> Piglet.Validation.Is (function
                                    | name, pw -> Login name pw) "Invalid username or password."
        
    [<JavaScript>]                                        
    let RenderLoginForm x y submit =
        let uni = Controls.Input x
        uni.AddClass("form-control")

        let pwi = Controls.Input y
        pwi.AddClass("form-control")
        pwi.SetAttribute("type", "password")

        let btn = Controls.Submit submit
        btn.AddClass("btn btn-primary")
                       
        JQuery.Of(pwi.Dom).Keyup(
            fun _ e -> 
                if e.Which  = 13 then 
                    e.PreventDefault()
                    JQuery.Of(btn.Dom).Click() |> ignore
        ) |> ignore
                                    
        Div [ Attr.Class "form-horizontal container"; Attr.Style "margin-top: 2em; width: 50%; min-width: 200px" ] -<
        [
            Div [ Attr.Class "form-group" ] -< [
                Label [ Attr.For uni.Id; Attr.Class "col-sm-2 control-label"; Text "Username" ]
                Div [ Attr.Class "col-sm-10" ] -< [ uni ]
            ]
            Div [ Attr.Class "form-group" ] -< [
                Label [ Attr.For pwi.Id; Attr.Class "col-sm-2 control-label"; Text "Password" ]
                Div [ Attr.Class "col-sm-10" ] -< [ pwi ]
            ]
            Div [ Attr.Class "form-group" ] -< [
                Div [ Attr.Class "col-sm-offset-2 col-sm-10" ] -< [ btn ]
            ]
            Div [ Attr.Id "errors" ] |> Controls.ShowErrors submit (fun errors ->
                                                    errors |> List.map (fun msg -> P [ Attr.Style "color: red" ] -< [ Text msg ]))
                                            
        ]

    [<JavaScript>]
    let LoginForm redirectUrl =
        LoginPiglet
//        |> Piglet.Run (function
//                            | name, pw -> 
//                                if Login name pw then                                    
//                                    Redirect redirectUrl
//                                else
//                                    let msg =  P [ Attr.Style "color: red" ] -< [ Text "Invalid username or password!" ]
//                                    ById("errors").AppendChild(msg.Dom) |> ignore
//                      )
        |> Piglet.Run (fun _ -> Redirect redirectUrl)
        |> Piglet.Render RenderLoginForm
            
type LoginControl(redirectUrl: string) =
    inherit Web.Control()
 
    new () = new LoginControl("?")
    [<JavaScript>]
    override this.Body = ClAuth.LoginForm redirectUrl :> _ 
        
