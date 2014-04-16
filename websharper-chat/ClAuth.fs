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
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Sitelets
open IntelliFactory.WebSharper.Web

module ClAuth =   

    [<Inline "window.location=$url">]
    let Redirect (url: string) = ()

    [<Rpc>]
    let Login (username: string) (password: string) =
        async {
            match UserSession.GetLoggedInUser() with
                | Some _ -> return true            
                | None   -> let! resp = SQLConnection.Authenticate username password
                            match resp with
                                |Some token -> Chat.LoginUser token username
                                               return true
                                | None -> return false
        }
  
    [<JavaScript>]
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
            let pw =
                Controls.Password ""
                |> Validator.IsNotEmpty "Enter Password"
                |> Enhance.WithTextLabel "Password"
            let loginF =
                Formlet.Yield (fun n pw -> (n, pw))
                <*> uName <*> pw
 
            Formlet.Do {
                let! (u, p) = 
                    loginF
                    |> Enhance.WithCustomSubmitAndResetButtons
                        {Enhance.FormButtonConfiguration.Default with Label = Some "Login"}
                        {Enhance.FormButtonConfiguration.Default with Label = Some "Reset"}
                return!
                    WithLoadingPane (Login u p) <| fun loggedIn ->
                        if loggedIn then
                            Redirect redirectUrl
                            Formlet.Return ()
                        else
                            WarningPanel "Login failed"
            }
            |> Enhance.WithFormContainer        
            
type LoginControl(redirectUrl: string) =
    inherit Web.Control()
 
    new () = new LoginControl("?")
    [<JavaScript>]
    override this.Body = ClAuth.LoginForm redirectUrl :> _    
        
