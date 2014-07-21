namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Foogle.Charts")>]
[<assembly: AssemblyProductAttribute("Foogle.Charts")>]
[<assembly: AssemblyDescriptionAttribute("Foogle Charts is an easy to use F# wrapper for Google Charts visualization library")>]
[<assembly: AssemblyVersionAttribute("0.0.0")>]
[<assembly: AssemblyFileVersionAttribute("0.0.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.0"
