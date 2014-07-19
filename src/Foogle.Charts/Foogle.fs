namespace Foogle
open FSharp.Data

type FoogleChart = 
  { Kind : string
    Data : JsonValue
    Options : JsonValue }

