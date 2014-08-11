module Foogle.Google

open Foogle
open FSharp.Data
open Microsoft.FSharp.Reflection

// ------------------------------------------------------------------------------------------------
// Functions that convert `FoogleChart` to a JSON value that Google Charts accept
// ------------------------------------------------------------------------------------------------

type GoogleChart = 
  { Kind : string
    Options : JsonValue 
    Data : JsonValue }

// ------------------------------------------------------------------------------------------------
// Formatting helpers
// ------------------------------------------------------------------------------------------------

module Formatting = 
  let formatOpt f = function Some v -> f v | _ -> [] 
  let formatOptStr k = formatOpt (fun v -> [ k, JsonValue.String v ])
  let inline formatOptNum k = formatOpt (fun v -> [ k, JsonValue.Number (decimal v) ])
  let formatDefStr k def v = [ k, JsonValue.String (defaultArg v def) ]

  let formatUnion (case:'T) =
    let case, _ = FSharpValue.GetUnionFields(case, typeof<'T>)
    case.Name 

  let formatDefUnionLo k def (case:option<'T>) = 
    case |> Option.map (fun case ->
      let case, _ = FSharpValue.GetUnionFields(case, typeof<'T>)
      case.Name.ToLower() ) |> formatDefStr k def

  /// Format value in a JavaScript-friendly way
  let formatValue (v:obj) =
    match v with
    | :? int as n -> JsonValue.Number(decimal n)
    | :? decimal as d -> JsonValue.Number(d)
    | :? float as f -> JsonValue.Float(f)
    | o -> JsonValue.String(o.ToString())

  /// Format table as a list of lists of values
  let formatTable (table:Table) = 
    let formatRow row = 
      JsonValue.Array(row |> Seq.map formatValue |> Array.ofSeq)
    let table = seq { 
      yield [ for l in table.Labels -> box l ]
      for r in table.Rows do yield r }
    JsonValue.Array(table |> Seq.map formatRow |> Array.ofSeq)

// ------------------------------------------------------------------------------------------------
// Turn Foogle chart to a Google chart
// ------------------------------------------------------------------------------------------------

open Formatting

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