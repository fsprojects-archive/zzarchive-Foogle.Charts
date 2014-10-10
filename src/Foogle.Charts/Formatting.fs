module Foogle.Formatting.Common

open FSharp.Data
open Foogle
open Microsoft.FSharp.Reflection

// ------------------------------------------------------------------------------------------------
// JSON Formatting helpers
// ------------------------------------------------------------------------------------------------

let formatOpt f = function Some v -> f v | _ -> [] 
let formatOptStr k = formatOpt (fun v -> [ k, JsonValue.String v ])
let formatOptBool k = formatOpt (fun v -> [ k, JsonValue.Boolean v ])
let formatOptStrArray k = formatOpt (fun v -> [ k, v |> Array.map JsonValue.String |> JsonValue.Array ])
let inline formatOptNum k = formatOpt (fun v -> [ k, JsonValue.Number (decimal v) ])
let formatDefStr k def v = [ k, JsonValue.String (defaultArg v def) ]

let formatRecd k l = [k, JsonValue.Record(Array.ofList l)]
let formatOptRecd k f = function Some v -> [k, JsonValue.Record(Array.ofList (f v))] | _ -> [] 

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

/// Format table as a list of lists of values, ignoring the header
let formatTableData (table:Table) = 
  let formatRow row = JsonValue.Array(row |> Seq.map formatValue |> Array.ofSeq)
  JsonValue.Array(table.Rows |> Seq.map formatRow |> Array.ofSeq)
