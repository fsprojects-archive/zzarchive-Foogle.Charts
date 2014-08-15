module Formatters
#I "../../packages/FSharp.Formatting.2.4.21/lib/net40"
#r "FSharp.Markdown.dll"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "../../packages/FAKE/tools/FakeLib.dll"
#I "../../packages/FSharp.Data.2.0.9/lib/net40"
#r "FSharp.Data.dll"
#I "../../bin"
#r "Foogle.Charts.dll"

open Fake
open System.IO
open FSharp.Literate
open FSharp.Markdown
open Foogle

// --------------------------------------------------------------------------------------
// Build FSI evaluator
// --------------------------------------------------------------------------------------

/// Builds FSI evaluator that can render System.Image, F# Charts, series & frames
let createFsiEvaluator root output =

  /// Counter for saving files
  let createCounter () = 
    let count = ref 0
    (fun () -> incr count; !count)
  let imageCounter = createCounter ()
  let foogleCounter = createCounter ()

  let transformation (value:obj, typ:System.Type) =
    match value with 
    | :? System.Drawing.Image as img ->
        // Pretty print image - save the image to the "images" directory 
        // and return a DirectImage reference to the appropriate location
        let id = imageCounter().ToString()
        let file = "chart" + id + ".png"
        ensureDirectory (output @@ "images")
        img.Save(output @@ "images" @@ file, System.Drawing.Imaging.ImageFormat.Png) 
        Some [ Paragraph [DirectImage ("Chart", (root + "/images/" + file, None))]  ]

    | :? FoogleChart as fch -> 

        // TODO: Does not work for LaTex!
        match fch.Options.Engine with
        | Some Engine.Highcharts ->
            let hch = Foogle.Formatting.Highcharts.CreateHighchartsChart(fch)
            let count = foogleCounter()
            let id = "foogle_" + count.ToString()
            let opts = hch.JSON.ToString(FSharp.Data.JsonSaveOptions.DisableFormatting)
            let html = """
              <div id="[ID]" style="height:400px; margin:0px 45px 0px 25px"></div>
              <script type="text/javascript">
                $(function () {
                  $('#[ID]').highcharts(
                    [SCRIPT]
                  );
                });
              </script>""".Replace("[ID]", id).Replace("[SCRIPT]", opts)
            [ InlineBlock(html) ] |> Some

        | _ ->
            let fch = Foogle.Formatting.Google.CreateGoogleChart(fch)

            let count = foogleCounter()
            let id = "foogle_" + count.ToString()
            let data = fch.Data.ToString(FSharp.Data.JsonSaveOptions.DisableFormatting)
            let opts = fch.Options.ToString(FSharp.Data.JsonSaveOptions.DisableFormatting)

            let script =
              [ "foogleCharts.push(function() {"
                sprintf "var data = google.visualization.arrayToDataTable(%s);" data
                sprintf "var options = %s;" opts
                sprintf "var chart = new google.visualization.%s(document.getElementById('%s'));" fch.Kind id
                "chart.draw(data, options);"
                "});" ]
            let htmlChart = 
              """<script type="text/javascript">""" + (String.concat "\n" script) + "</script>" +
              (sprintf "<div id=\"%s\" style=\"height:400px; margin:0px 45px 0px 25px\"></div>" id)
         
            let htmlOnce = """
              <script type="text/javascript">
                if (typeof foogleCharts === 'undefined') {
                  var foogleCharts = [];
                  function foogleInit() {
                    for (var i = 0; i < foogleCharts.length; i++) {
                      foogleCharts[i]();
                    }
                  }
                  google.load('visualization', '1', { 'packages': ['corechart','geochart'] });
                  google.setOnLoadCallback(foogleInit);
                }
              </script>"""

            // TODO: We should really only insert 'htmlOnce' once per file
            // but F# Formatting does not tell us the context, so we use 
            // ugly JS 'typeof foogleCharts === 'undefined'' hack HERE!
            let html = htmlOnce + htmlChart 
            [ InlineBlock(html) ] |> Some
    | _ -> None 
    
  // Create FSI evaluator, register transformations & return
  let fsiEvaluator = FsiEvaluator()
  fsiEvaluator.RegisterTransformation(transformation)
  fsiEvaluator