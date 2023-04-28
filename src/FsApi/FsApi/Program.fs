open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

exception ArgError of string

let args = Environment.GetCommandLineArgs().[1..]
let builder = WebApplication.CreateBuilder(args)
let app = builder.Build()

app.MapGet("/hello", Func<String>(fun () ->
    "Ok")) |> ignore

app.MapGet("/ex/{verb}", Func<String, String>(fun verb ->
    match verb with
    | "ok" -> "Ok"
    | "err" -> raise (ArgError("Err"))
    | "catch" ->
        try
            raise (ArgError("Catch"))
        with
            | ArgError msg as e ->
                app.Logger.LogError(e.ToString())
                "Err"
    | verb -> raise (System.ArgumentException(verb)))) |> ignore

app.Run()