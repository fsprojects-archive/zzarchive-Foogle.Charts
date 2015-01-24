module Foogle.Formatting.Common

open System
open FSharp.Data
open Foogle
open Microsoft.FSharp.Reflection

// ------------------------------------------------------------------------------------------------
// JSON Formatting helpers
// ------------------------------------------------------------------------------------------------

let formatOpt f = function Some v -> f v | _ -> [] 
let formatOptStr k = formatOpt (fun v -> [ k, JsonValue.String v ])
let inline formatOptNum k = formatOpt (fun v -> [ k, JsonValue.Number (decimal v) ])
let formatDefStr k def v = [ k, JsonValue.String (defaultArg v def) ]

let formatBool k b = [k, JsonValue.Boolean(b)]
let formatOptBool k = formatOpt (fun v -> formatBool k v)
let formatRecd k l = [k, JsonValue.Record(Array.ofList l)]
let formatOptRecd k f = function Some v -> [k, JsonValue.Record(Array.ofList (f v))] | _ -> [] 

let formatUnion (case:'T) =
  let case, _ = FSharpValue.GetUnionFields(case, typeof<'T>)
  case.Name 

let formatDefUnionLo k def (case:option<'T>) = 
  case |> Option.map (fun case ->
    let case, _ = FSharpValue.GetUnionFields(case, typeof<'T>)
    case.Name.ToLower() ) |> formatDefStr k def

let formatDateTimeUnixTicks(dt:DateTime) =
    let epoch = new System.DateTime(1970, 1, 1, 0, 0, 0)
    let current = dt.ToUniversalTime()
    let result = current.Subtract(epoch)
    result.TotalMilliseconds |> int64;

/// Format value in a JavaScript-friendly way
let formatValue (v:obj) =
  match v with
  | :? int as n -> JsonValue.Number(decimal n)
  | :? decimal as d -> JsonValue.Number(d)
  | :? float as f -> JsonValue.Float(f)
  | :? DateTime as n -> JsonValue.Number(decimal <| formatDateTimeUnixTicks n)
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
