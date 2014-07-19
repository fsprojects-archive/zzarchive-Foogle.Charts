module Foogle.Internal

open FSharp.Data
open System.Globalization

/// Format value in a JavaScript-friendly way
let formatValue (v:obj) =
  match v with
  | :? int as n -> JsonValue.Number(decimal n)
  | :? decimal as d -> JsonValue.Number(d)
  | :? float as f -> JsonValue.Float(f)
  | o -> JsonValue.String(o.ToString())

/// Format table as a list of lists of values
let formatTable (table:seq<#seq<obj>>) = 
  let formatRow row = 
    JsonValue.Array(row |> Seq.map formatValue |> Array.ofSeq)
  JsonValue.Array(table |> Seq.map formatRow |> Array.ofSeq)

module FormatValues = 
  let val2 label data = formatTable [ yield [ ""; defaultArg label "Value" ]; for k, v in data do yield [ k; v ] ]

let pageTemplate = """<!DOCTYPE html>
<html style="overflow:hidden;width:100%;height:100%;">
  <head>
    <title>Foogle Chart</title>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
  </head>
  <body style="overflow:hidden;width:100%;height:100%;font-family:calibri,arial,helvetica,sans-serif;" id="bodyElement">
    <div id="chart_div" style="width:95%;height:95%;"></div>
    <script type="text/javascript">
      function main() {
        [SCRIPT]
      }
      google.load('visualization', '1', { 'packages': ['corechart','geochart'] });
      google.setOnLoadCallback(main);
    </script>
  </body>
</html>"""

let chartHtml (fch:FoogleChart) =
  let data = fch.Data.ToString(JsonSaveOptions.DisableFormatting)
  let opts = fch.Options.ToString(JsonSaveOptions.DisableFormatting)
  let script =
    [ sprintf "var data = google.visualization.arrayToDataTable(%s);" data
      sprintf "var options = %s;" opts
      sprintf "var chart = new google.visualization.%s(document.getElementById('chart_div'));" fch.Kind
      "chart.draw(data, options);" ]
    |> String.concat "\n"
  pageTemplate.Replace("[SCRIPT]", script)