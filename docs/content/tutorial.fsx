(*** hide ***)
#I "../../bin"

(**
Tutorial
========

Reference
*)
#r "../../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#I "../../bin"
#load "Foogle.Charts.fsx"
open Foogle
open System.Windows.Forms

(** 
Geo chart
*)
(*** define-output:geo1 ***)
let pop =
  [ "Germany", 200
    "United States", 300
    "Brazil", 400
    "Canada", 500
    "France", 600
    "RU", 700]
Chart.GeoChart(pop, Label="Popularity")
(*** include-it:geo1 ***)

(** 
Another geo chart
*)
let cities = 
  [ "Rome",      2761477, 1285.31
    "Milan",     1324110, 181.76
    "Naples",    959574,  117.27
    "Turin",     907563,  130.17
    "Palermo",   655875,  158.9
    "Genoa",     607906,  243.60
    "Bologna",   380181,  140.7
    "Florence",  371282,  102.41
    "Fiumicino", 67370,   213.44
    "Anzio",     52192,   43.43
    "Ciampino",  38262,   11.0]

(*** define-output:geo2 ***)
Chart.GeoChart
  ( cities, Labels = ["Population"; "Area"], Region = "IT", 
    DisplayMode = GeoChart.Markers )
|> Chart.WithColorAxis(Colors = ["#ff0000"; "#0000ff"])    
|> Chart.WithTitle("Italy")
(*** include-it:geo2 ***)

(**
Pie charts
*)
(*** define-output:pie1 ***)
let tasks = 
  [ "Work",     11
    "Eat",      2
    "Commute",  2
    "Watch TV", 2
    "Sleep",    7 ]

Chart.PieChart(tasks, Label="Hours per Day")
|> Chart.WithTitle(Title = "Daily activities")
(*** include-it:pie1 ***)

(*** define-output:pie2 ***)
Chart.PieChart(tasks, Label="Hours per Day")
|> Chart.WithTitle(Title = "Daily activities")
|> Chart.WithPie(PieHole = 0.5)
(*** include-it:pie2 ***)

(*** define-output:pie3 ***)
Chart.PieChart(tasks, Label="Hours per Day")
|> Chart.WithTitle(Title = "Daily activities")
|> Chart.WithOutput(Engine.Highcharts)
(*** include-it:pie3 ***)

(**
Another Pie Chart - How Much Pizza I Ate Last Night - https://google-developers.appspot.com/chart/interactive/docs/quick_start
**)

let pizzaEaten = 
  [ "Mushrooms",    3
    "Onines",       1
    "Olives",       1
    "Zucchini",     1
    "Pepperoni",    2 ]

Chart.PieChart(pizzaEaten, Label="# of slices")
|> Chart.WithTitle(Title = "How Much Pizza I Ate Last Night")
|> Chart.WithOutput(Engine.Google)
