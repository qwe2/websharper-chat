namespace WebsharperChat

open System
open System.Security
open System.Text

module Utils =
    let private sha256 = Cryptography.SHA256.Create()
    let Hash (s: string) =
        let hash = new StringBuilder()
        Encoding.UTF8.GetBytes s
            |> sha256.ComputeHash
            |> Array.iter (fun by -> hash.Append(by.ToString("x2")) |> ignore)
        hash.ToString()

    let ct a _ = a
