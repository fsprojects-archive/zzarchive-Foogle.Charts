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
let pop =
  [ "Germany", 200
    "United States", 300
    "Brazil", 400
    "Canada", 500
    "France", 600
    "RU", 700]
Chart.GeoChart(pop, Label="Popularity")
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

Chart.GeoChart
  ( cities, Labels = ["Population"; "Area"], Region = "IT", 
    DisplayMode = GeoChart.Markers )
|> Chart.WithColorAxis(Colors = ["#ff0000"; "#0000ff"])    
|> Chart.WithTitle("Italy")

(**
Pie charts
*)
let tasks = 
  [ "Work",     11
    "Eat",      2
    "Commute",  2
    "Watch TV", 2
    "Sleep",    7 ]

Chart.PieChart(tasks, Label="Hours per Day")
|> Chart.WithTitle(Title = "Daily activities")

Chart.PieChart(tasks, Label="Hours per Day")
|> Chart.WithTitle(Title = "Daily activities")
|> Chart.WithPie(PieHole = 0.5)