#nowarn "211"
#I "../../bin"
#r "Foogle.Charts.dll"
open Foogle
open Foogle.SimpleHttp
open System.Windows.Forms
open System.IO

let server : HttpServer option ref = ref None
let tempDir = Path.GetTempFileName()
do File.Delete(tempDir)
do Directory.CreateDirectory(tempDir) |> ignore

fsi.AddPrinter(fun (chart:FoogleChart) ->
  match !server with 
  | None -> server := Some (HttpServer.Start("http://localhost:8081/", tempDir))
  | _ -> ()

  let googleChart = Google.CreateGoogleChart(chart)
  File.WriteAllText(Path.Combine(tempDir, "index.html"), Google.GoogleChartHtml googleChart)
  let form = new Form(Width=800, Height=500, Visible=true)
  let web = new WebBrowser(Dock=DockStyle.Fill)
  printfn "%s" (Google.GoogleChartHtml googleChart)
  form.Controls.Add(web) 
  web.Navigate("http://localhost:8081/index.html")
  "(Foogle Chart)" )