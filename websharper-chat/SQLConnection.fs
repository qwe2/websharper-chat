namespace WebsharperChat

open System
open System.Security
open System.Text
open System.Linq
open System.Data
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq

module Auth =
    type private Key = class end
    let private key = typeof<Key>.GUID.ToString()

    let GenToken (username: string) =
        sprintf "%s:%s" username key |> Utils.Hash

module SQLConnection =
    type internal DbSchema = SqlDataConnection<"Data Source=(localdb)\Projects;Initial Catalog=WebSharperChat;Integrated Security=True;\
                                                Connect Timeout=30;Encrypt=False;TrustServerCertificate=False">
    let private Db = DbSchema.GetDataContext()
    let private Users = Db.Users    

    let Authenticate (username: string) (password: string) =
        async {
            let hash = Utils.Hash password
            let usrs = query { 
                for user in Users do 
                where (user.Username = username && user.Password = hash)
                select user
            }

            if usrs.Count() = 1 then
                let token = Auth.GenToken username
                return Some token
            else
                return None
        }

    let AddUser (username: string) (password: string) =
        let usr = new DbSchema.ServiceTypes.Users(Username = username, 
                                                  Password = Utils.Hash password)
        Users.InsertOnSubmit(usr)
        Db.DataContext.SubmitChanges()


