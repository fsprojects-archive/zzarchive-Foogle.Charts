#if INTERACTIVE
#r "../../packages/NUnit.2.6.3/lib/nunit.framework.dll"
#r "../../packages/FsUnit.1.3.0.1/Lib/Net40/FsUnit.NUnit.dll"
#r "../../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#I "../../bin"
#r "FSharp.Data.dll"
#r "Foogle.Charts.dll"
#else
module Foogle.Tests.FormattingTests
#endif
open FsUnit
open FSharp.Data
open NUnit.Framework
open Foogle

[<Test>]
let ``Can format simple table`` () =
  let actual =
    { Labels = [ "Country"; "Popularity "] 
      Rows = 
        [ [ "Germany"; 200 ]
          [ "France"; 300 ]
          [ "United States"; 400] ] }
    |> Formatting.Common.formatTable

  let expected =
    JsonValue.Array
      [|JsonValue.Array [|JsonValue.String "Country"; JsonValue.String "Popularity "|];
        JsonValue.Array [|JsonValue.String "Germany"; JsonValue.Number 200M|];
        JsonValue.Array [|JsonValue.String "France"; JsonValue.Number 300M|];
        JsonValue.Array [|JsonValue.String "United States"; JsonValue.Number 400M|]|]
  actual |> should equal expected

[<Test>]
let ``Can format simple table (ignore header)`` () =
  let actual =
    { Labels = [ ""; ""] 
      Rows = 
        [ [ "Germany"; 200 ]
          [ "France"; 300 ]
          [ "United States"; 400] ] }
    |> Formatting.Common.formatTableData

  let expected =
    JsonValue.Array
      [|JsonValue.Array [|JsonValue.String "Germany"; JsonValue.Number 200M|];
        JsonValue.Array [|JsonValue.String "France"; JsonValue.Number 300M|];
        JsonValue.Array [|JsonValue.String "United States"; JsonValue.Number 400M|]|]
  actual |> should equal expected
