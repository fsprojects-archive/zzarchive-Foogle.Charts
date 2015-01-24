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
open System
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
A timeline chart
*)

(*** define-output:timeline1 ***)
let presidents = 
    [
       "President",          "George Washington", new DateTime(1789, 3, 29), new DateTime(1797, 2, 3)
       "President",          "John Adams",        new DateTime(1797, 2, 3),  new DateTime(1801, 2, 3)
       "President",          "Thomas Jefferson",  new DateTime(1801, 2, 3),  new DateTime(1809, 2, 3)
       "Vice President",     "John Adams",        new DateTime(1789, 3, 20), new DateTime(1797, 2, 3)
       "Vice President",     "Thomas Jefferson",  new DateTime(1797, 2, 3),  new DateTime(1801, 2, 3)
       "Vice President",     "Aaron Burr",        new DateTime(1801, 2, 3),  new DateTime(1805, 2, 3)
       "Vice President",     "George Clinton",    new DateTime(1805, 2, 3),  new DateTime(1812, 3, 19)
       "Secretary of State", "John Jay",          new DateTime(1789, 8, 25), new DateTime(1790, 2, 21)
       "Secretary of State", "Thomas Jefferson",  new DateTime(1790, 2, 21), new DateTime(1793, 11, 30)
       "Secretary of State", "Edmund Randolph",   new DateTime(1794, 1, 1),  new DateTime(1795, 7, 19)
       "Secretary of State", "Timothy Pickering", new DateTime(1795, 7, 19), new DateTime(1800, 4, 11)
       "Secretary of State", "Charles Lee",       new DateTime(1800, 4, 12), new DateTime(1800, 5, 4)
       "Secretary of State", "John Marshall",     new DateTime(1800, 5, 12), new DateTime(1801, 2, 3)
       "Secretary of State", "Levi Lincoln",      new DateTime(1801, 2, 4),  new DateTime(1801, 4, 1)
       "Secretary of State", "James Madison",     new DateTime(1801, 4, 1),  new DateTime(1809, 2, 2)
    ]

Chart.Timeline(presidents)
|> Chart.WithTitle(Title = "Presidents")
(*** include-it:timeline1 ***)