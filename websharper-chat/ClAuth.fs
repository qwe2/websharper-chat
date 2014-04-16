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
            | Some token -> UserSession.LoginUser token
                            Chat.LoginUser token username None
                            true
            | None -> false
        
        
  
    [<JavaScript>]
    let LoginPiglet =
        Piglet.Return (fun x y -> Login x y)
        <*> (Piglet.Yield ""
            |> Validation.Is Validation.NotEmpty "Enter Username")
        <*> (Piglet.Yield ""
            |> Validation.Is Validation.NotEmpty "Enter password")
        |> Validation.Is id "Invalid username or password"
        |> Piglet.WithSubmit
        
        
    [<JavaScript>]                                        
    let RenderLoginForm x y submit =
        let uni = Controls.Input x
        uni.AddClass("form-control")

        let pwi = Controls.Input y
        pwi.AddClass("form-control")
        pwi.SetAttribute("type", "password")

        let btn = Controls.Submit submit
        btn.AddClass("btn btn-primary")

                       
        JQuery.Of(IntelliFactory.WebSharper.Dom.Document.Current).Keypress(
            fun _ e -> 
                if e.Which = 13 then 
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
            Div [] |> Controls.ShowErrors submit (fun errors ->
                                                    errors |> List.map (fun msg -> P [ Attr.Style "color: red" ] -< [ Text msg ]))

                                            
        ]

    [<JavaScript>]
    let LoginForm redirectUrl =
        LoginPiglet         
        |> Piglet.Run (fun _ -> Redirect redirectUrl)                     
        |> Piglet.Render RenderLoginForm
    

    (*[<JavaScript>]
    let WarningPanel label =
        Formlet.Do {
            let! _ =
                Formlet.OfElement <| fun _ ->
                    Div [Attr.Class "warningPanel"] -< [Text label]
            return! Formlet.Never ()
        }

    [<JavaScript>]
    let WithLoadingPane (a: Async<'T>) (f: 'T -> Formlet<'U>) : Formlet<'U> =
        let loadingPane =
            Formlet.BuildFormlet <| fun _ ->
                let elem = 
                    Div [Attr.Class "loadingPane"]
                let state = new Event<Result<'T>>()
                async {
                    let! x = a
                    do state.Trigger (Result.Success x)
                    return ()
                }
                |> Async.Start
                elem, ignore, state.Publish
        Formlet.Replace loadingPane f

    [<JavaScript>]
    let LoginForm (redirectUrl: string) : Formlet<unit> =
            let uName =
                Controls.Input ""
                |> Validator.IsNotEmpty "Enter Username"
                |> Enhance.WithTextLabel "Username"
                |> Enhance.WithCssClass "form-control"
            let pw =
                Controls.Password ""
                |> Validator.IsNotEmpty "Enter Password"
                |> Enhance.WithTextLabel "Password"
                |> Enhance.WithCssClass "form-control"
            let loginF =
                Formlet.Yield (fun n pw -> (n, pw))
                <*> uName <*> pw
 
            Formlet.Do {
                let! (u, p) = 
                    loginF
                    |> Enhance.WithCustomSubmitAndResetButtons
                        {Enhance.FormButtonConfiguration.Default with Label = Some "Login"; Class = Some "btn btn-primary"}
                        {Enhance.FormButtonConfiguration.Default with Label = Some "Reset"; Class = Some "btn btn-default"}
                return!
                    WithLoadingPane (Login u p) <| fun loggedIn ->
                        if loggedIn then
                            Redirect redirectUrl
                            Formlet.Return ()
                        else
                            WarningPanel "Login failed"
            }
            |> Enhance.WithFormContainer *)
            
type LoginControl(redirectUrl: string) =
    inherit Web.Control()
 
    new () = new LoginControl("?")
    [<JavaScript>]
    override this.Body = ClAuth.LoginForm redirectUrl :> _ 
        
