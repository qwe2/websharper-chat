namespace WebsharperChat

open System
open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

type Action =
    | Loginpage
    | Chatpage

module Controls =

    [<Sealed>]
    type EntryPoint() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            ChatClient.Main() :> _

module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Action> =        
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Title = title
                Body = body context
            }

module Site =

    let ChatPage =
        Skin.WithTemplate "Chat" <| fun ctx ->
                                        [
                                            Div [new Controls.EntryPoint()]
                                        ] 

    let LoginPage =
        Skin.WithTemplate "Login"  <| fun ctx ->
                                        [
                                            Div [new LoginControl(ctx.Link <| Action.Chatpage)]
                                        ]
                            

    let Main =
        let basehome = Sitelet.Content "/" Loginpage LoginPage

        let home =
            {
                Router = basehome.Router
                Controller =
                    {
                        Handle = fun action ->
                            try
                                match UserSession.GetLoggedInUser () with
                                    | None   -> basehome.Controller.Handle action
                                    | Some _ -> Content.Redirect Action.Chatpage
                            with :? NullReferenceException ->
                                Content.Redirect Action.Chatpage
                    }
            }

        let authenticated = 
            let filter: Sitelet.Filter<Action> =
                {
                    VerifyUser = fun _ -> true
                    LoginRedirect = fun _ -> Action.Loginpage
                }
            Sitelet.Protect filter <| Sitelet.Content "/chat" Action.Chatpage ChatPage

        Sitelet.Sum [
            home
            authenticated          
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Loginpage; Chatpage]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
