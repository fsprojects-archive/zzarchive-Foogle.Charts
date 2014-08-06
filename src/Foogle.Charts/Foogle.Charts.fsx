#nowarn "211"
#I "../../bin"
#r "Foogle.Charts.dll"
open Foogle
open System.Windows.Forms
open Foogle.SimpleHttp
open System.IO

let server : HttpServer option ref = ref None
let tempDir = Path.GetTempFileName()
do File.Delete(tempDir)
do Directory.CreateDirectory(tempDir) |> ignore

fsi.AddPrinter(fun (chart:FoogleChart) ->
  match !server with 
  | None -> server := Some (HttpServer.Start("http://localhost:8081/", tempDir))
  | _ -> ()

  File.WriteAllText(Path.Combine(tempDir, "index.html"), Internal.chartHtml chart)
  let form = new Form(Width=800, Height=500, Visible=true)
  let web = new WebBrowser(Dock=DockStyle.Fill)
  form.Controls.Add(web) 
  printfn "%s" (Internal.chartHtml chart)
  web.Navigate("http://localhost:8081/index.html")
  "(Foogle Chart)" )