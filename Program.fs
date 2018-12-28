open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.FileProviders
open Giraffe
open Giraffe.GiraffeViewEngine

type User = { login:string; email:string }

let userList:(User list) = [ 
    { login = "sean"; email = "sean@example.com" }
    { login = "lucka"; email = "lucka@example.com" }
    { login = "alfie"; email = "alfie@example.com" }
    { login = "ivy"; email = "ivy@example.com" }
]

let ngApp = tag "app-root"  [] []

let ngScripts = 
  [ "runtime.js"; "polyfills.js"; "styles.js"; "vendor.js"; "main.js" ]
  |> List.map (function js -> script [ _type "text/javascript"; _src js ] [] )

let index = 
  html [ _lang "en" ] [
      head [] [
          meta [ _charset "utf-8"; _name "viewport"; _content "width=device-width, initial-scale=1" ] 
          title [] [ str "Angular + Giraffe" ]
          ``base`` [ _href "/app/" ]
          link [ _rel "icon"; _type "icon"; _href "favicon.ico" ]
      ]

      body [] 
          (ngApp :: ngScripts)
  ]

let webApp =
    choose [ 
        route "/" >=> (index |> renderHtmlDocument |> htmlString)
        route "/users" >=> (json userList)
    ]

let configureApp (app : IApplicationBuilder) =
  app.UseStaticFiles(
      StaticFileOptions(
              FileProvider = new PhysicalFileProvider(
                      Path.Combine(Directory.GetCurrentDirectory(), "frontend", "dist")),
                      RequestPath = PathString("/app")))
      .UseGiraffe(webApp)
           
let configureServices (services : IServiceCollection) =
  services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
      .UseKestrel()
      .Configure(Action<IApplicationBuilder> configureApp)
      .ConfigureServices(configureServices)
      .Build()
      .Run()
    0