(*** hide ***)
#I "../../bin"
#r "../../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#I "../../bin"
#load "Foogle.Charts.fsx"
open Foogle
open System.Windows.Forms

(**
Foogle: F# Library for Google Charts
====================================

This example demonstrates using a function defined in this sample library.
Here are some pie charts:

*)
let tasks = 
  [ "Work", 11; "Eat", 2; "Commute", 2
    "Watch TV", 2; "Sleep", 7 ]

Chart.PieChart
  ( tasks, label="Hours per Day", 
    title="Daily activities" )

Chart.PieChart
  ( tasks, label="Hours per Day", 
    title="Daily activities", pieHole=0.5 )
    

(**
Getting the library
-------------------

TODO: How to get the library

Samples & documentation
-----------------------

The library comes with comprehensible documentation. 
It can include a tutorials automatically generated from `*.fsx` files in [the content folder][content]. 
The API reference is automatically generated from Markdown comments in the library implementation.

 * [Tutorial](tutorial.html) contains a further explanation of this sample library.

 * [API Reference](reference/index.html) contains automatically generated documentation for all types, modules
   and functions in the library. This includes additional brief samples on using most of the
   functions.
 
Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork 
the project and submit pull requests. If you're adding new public API, please also 
consider adding [samples][content] that can be turned into a documentation. You might
also want to read [library design notes][readme] to understand how it works.

The library is available under Public Domain license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/fsprojects/FSharp.ProjectScaffold/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.ProjectScaffold
  [issues]: https://github.com/fsprojects/FSharp.ProjectScaffold/issues
  [readme]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/README.md
  [license]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/LICENSE.txt
*)
