module Foogle.Formatting.Highcharts

open Foogle
open FSharp.Data
open Foogle.Formatting.Common

// ------------------------------------------------------------------------------------------------
// Functions that convert `FoogleChart` to a JSON value that Highcharts accept
// ------------------------------------------------------------------------------------------------

type HighchartsChart = 
  { JSON : JsonValue }


// ------------------------------------------------------------------------------------------------
// Turn Foogle chart to a Highcharts chart
// ------------------------------------------------------------------------------------------------

let private PageTemplate = """<!DOCTYPE html>
<html style="overflow:hidden;width:100%;height:100%;">
  <head>
    <title>Foogle Chart</title>
    <script src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <script src="http://code.highcharts.com/modules/exporting.js"></script>
  </head>
  <body style="overflow:hidden;width:100%;height:100%;font-family:calibri,arial,helvetica,sans-serif;" id="bodyElement">
    <div id="chart_div" style="width:95%;height:95%;"></div>
    <script type="text/javascript">
      $(function () {
        $('#chart_div').highcharts(
          [SCRIPT]
        );
      });
    </script>
  </body>
</html>"""

let CreateHighchartsChart (chart:FoogleChart) = 
  let options = 
   [| // Format specific chart options
      match chart.Chart with
      | AreaChart(g) ->
          failwith "TODO: Implement AreaChart"
      | BarChart(g) ->
          failwith "TODO: Implement BarChart"
      | GeoChart(g) ->
          failwith "TODO: Do highcharts support geo?"
      | PieChart(p) ->
          yield! formatRecd "chart" ["type", JsonValue.String "pie"]

      yield "series", JsonValue.Array [|
          JsonValue.Record [|
            "name", JsonValue.String(chart.Data.Labels |> Seq.nth 1) // TODO: Assumes 2 columns in table
            "data", formatTableData chart.Data
          |] |]

      // Format common chart options
      let opts = chart.Options
      yield! opts.Title |> formatOptRecd "title" (fun title -> [ "text", JsonValue.String title ])

      // TODO: opts.ColorAxis

   |] |> JsonValue.Record
  { JSON = options }

let HighchartsHtml (fch:HighchartsChart) =
  let data = fch.JSON.ToString(JsonSaveOptions.DisableFormatting)
  PageTemplate.Replace("[SCRIPT]", data)