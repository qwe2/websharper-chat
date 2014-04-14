namespace WebsharperChat

open System
open System.Security
open System.Text
open System.Linq
open System.Data
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq

module SQLConnection =
    type internal DbSchema = SqlDataConnection<"Data Source=(localdb)\Projects;Initial Catalog=WebSharperChat;Integrated Security=True;\
                                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False">
    let private Db = DbSchema.GetDataContext()
    let private Users = Db.Users    

    let Authenticate (username: string) (password: string) =
        let hash = Utils.Hash password
        let usrs = query { 
            for user in Users do 
            where (user.Username = username && user.Password = hash)
            select user
        }
        for usr in usrs do
            usr.Lastlogin <- new Nullable<DateTime>(DateTime.Now)
        
        Db.DataContext.SubmitChanges()
        usrs.Count() = 1
        

    let AddUser (username: string) (password: string) =
        let usr = new DbSchema.ServiceTypes.Users(Username = username, 
                                                  Password = Utils.Hash password)
        Users.InsertOnSubmit(usr)
        Db.DataContext.SubmitChanges()


