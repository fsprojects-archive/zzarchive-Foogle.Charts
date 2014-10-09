module Foogle.Formatting.Google

open Foogle
open FSharp.Data
open Foogle.Formatting.Common

// ------------------------------------------------------------------------------------------------
// Functions that convert `FoogleChart` to a JSON value that Google Charts accept
// ------------------------------------------------------------------------------------------------

type GoogleChart = 
  { Kind : string
    Options : JsonValue 
    Data : JsonValue }

// ------------------------------------------------------------------------------------------------
// Turn Foogle chart to a Google chart
// ------------------------------------------------------------------------------------------------

let private PageTemplate = """<!DOCTYPE html>
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

let CreateGoogleChart (chart:FoogleChart) = 
  let options = 
   [| // Format specific chart options
      match chart.Chart with
      | AreaChart(g) ->
          yield! formatOptBool "isStacked" g.IsStacked
      | GeoChart(g) ->
          yield! formatOptStr "region" g.Region 
          yield! formatDefUnionLo "displayMode" "auto" g.DisplayMode
      | PieChart(p) ->
          yield! formatOptNum "pieHole" p.PieHole
      
      // Format common chart options
      let opts = chart.Options
      yield! formatOptStr "title" opts.Title
      yield! opts.ColorAxis |> formatOpt (fun c ->
        [ yield! c.MinValue |> formatOptNum "minValue" 
          yield! c.MaxValue |> formatOptNum "maxValue" 
          yield! c.Colors |> formatOpt (fun clrs -> 
            [ "colors", JsonValue.Array [| for c in clrs -> JsonValue.String c |] ])  ])

      //match sizeAxis with Some(s1, s2) -> yield "sizeAxis", JsonValue.Record [| "minValue", JsonValue.Number(decimal s1); "maxValue", JsonValue.Number(decimal s2) |] | _ -> ()
   |] |> JsonValue.Record

  { Kind = formatUnion chart.Chart
    Data = formatTable chart.Data
    Options = options }

let GoogleChartHtml (fch:GoogleChart) =
  let data = fch.Data.ToString(JsonSaveOptions.DisableFormatting)
  let opts = fch.Options.ToString(JsonSaveOptions.DisableFormatting)
  let script =
    [ sprintf "var data = google.visualization.arrayToDataTable(%s);" data
      sprintf "var options = %s;" opts
      sprintf "var chart = new google.visualization.%s(document.getElementById('chart_div'));" fch.Kind
      "chart.draw(data, options);" ]
    |> String.concat "\n"
  PageTemplate.Replace("[SCRIPT]", script)