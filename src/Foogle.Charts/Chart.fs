namespace Foogle

open FSharp.Data
open Foogle.Internal

type value = System.IConvertible

module GeoChart =
  type DisplayMode =
    | Region
    | Markers
    | Text
    | Auto
 
  let inline internal formatOptions region mode colorAxis sizeAxis = 
    JsonValue.Record [|
      match region with Some r -> yield "region", JsonValue.String r | _ -> ()
      yield ("displayMode", JsonValue.String(match mode with Some Region -> "region" | Some Markers -> "markers" | Some Text -> "text" | _ -> "auto"))
      match colorAxis with Some(c1, c2) -> yield "colorAxis", JsonValue.Record [| "colors", JsonValue.Array [| JsonValue.String c1; JsonValue.String c2 |] |] | _ -> ()
      match sizeAxis with Some(s1, s2) -> yield "sizeAxis", JsonValue.Record [| "minValue", JsonValue.Number(decimal s1); "maxValue", JsonValue.Number(decimal s2) |] | _ -> ()
    |]

module PieChart =
  let inline internal formatOptions title pieHole = 
    JsonValue.Record [|
      match title with Some t -> yield ("title", JsonValue.String t) | _ -> ()
      match pieHole with Some h -> yield ("pieHole", JsonValue.Float h) | _ -> ()
    |]

type Chart =
  static member GeoChart(data:seq<string * #value>, ?label, ?region, ?mode, ?colorAxis, ?sizeAxis) =
    { Data = FormatValues.val2 label data
      Options = GeoChart.formatOptions region mode colorAxis sizeAxis 
      Kind = "GeoChart" }

  static member GeoChart(data:seq<string * #value * #value>, ?labels, ?region, ?mode, ?colorAxis, ?sizeAxis) =
    let label1, label2 = defaultArg labels ("Value 1", "Value 2")
    { Data = Internal.formatTable [ yield [ ""; label1; label2 ]; for k, v1, v2 in data do yield [ k; v1; v2 ] ]
      Options = GeoChart.formatOptions region mode colorAxis sizeAxis 
      Kind = "GeoChart" }

  static member PieChart(data:seq<string * #value>, ?label, ?title, ?pieHole) =
    { Data = FormatValues.val2 label data
      Options = PieChart.formatOptions title pieHole
      Kind = "PieChart"}