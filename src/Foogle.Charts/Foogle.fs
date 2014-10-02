namespace Foogle

// ------------------------------------------------------------------------------------------------
// Foogle data source
// ------------------------------------------------------------------------------------------------

/// Represents a value that can be passed as data point
/// This can be any numeric type and vairous other .NET types
type value = System.IConvertible

/// Represents a data source for the Foogle chart
type Table = 
  { Labels : string list
    Rows : seq<obj list> }

/// Various helper functions for creating `Table` from sequences of inputs
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module internal Table = 

  let fromKeyValue label (values:seq<string * #value>) = 
    { Labels = [""; defaultArg label "Value" ]
      Rows = values |> Seq.map (fun (k, v) -> [box k; box v]) }
  
  let fromKey2Values labels (values:seq<string * #value * #value>) = 
    { Labels = "" :: (defaultArg labels [ "Value 1"; "Value 2" ])
      Rows = values |> Seq.map (fun (k, v1, v2) -> [box k; box v1; box v2]) }

  let from2Key2Values labels (values:seq<string * string * #value * #value>) = 
    { Labels = (defaultArg labels ["Position"; "Name"; "Start"; "End"])
      Rows = values |> Seq.map (fun (k, k1, v1, v2) -> [box k; box k1; box v1; box v2]) }


// ------------------------------------------------------------------------------------------------
// Foogle chart options
// ------------------------------------------------------------------------------------------------

/// A color is represented as HTML string in the usual format `#rrggbb` 
/// or a well-known HTML color name like `red`.
type Color = string

/// An object that specifies a mapping between color column values and colors or a gradient scale. 
type ColorAxis = 
  { /// If present, specifies a minimum value for chart color data. Color data 
    /// values of this value and lower will be rendered as the first color in the 
    /// `colorAxis.colors` range.
    MinValue : float option

    /// If present, specifies a maximum value for chart color data. Color data 
    /// values of this value and higher will be rendered as the last color in the 
    /// `colorAxis.colors` range.
    MaxValue : float option
    
    /// If present, controls how values are associated with colors. Each value is 
    /// associated with the corresponding color in the colorAxis.colors array. These values apply 
    /// to the chart color data. Coloring is done according to a gradient of the values specified 
    /// here. Not specifying a value for this option is equivalent to specifying 
    /// `[minValue, maxValue]`.
    Values : seq<float> option

    /// Colors to assign to values in the visualization. An array of strings, where 
    /// each element is a color. You must have at least two values; the gradient will include all 
    /// your values, plus calculated intermediary values, with the first color as the smallest 
    /// value, and the last color as the highest.
    Colors : seq<Color> option }


/// Specifies the preferred rendering engine for the chart
type Engine = 
  | Google
  | Highcharts

/// Specifies common options that are shared by all Foogle charts
type Options = 
  { /// Text to display above the chart.
    Title : string option
    /// An object that specifies a mapping between color column values and colors or a gradient scale. 
    ColorAxis : ColorAxis option 
    /// Specifies the preferred rendering engine for the chart
    Engine : Engine option }

  /// Returns a default empty configuration
  static member Empty = { Title = None; ColorAxis = None; Engine = None }


// ------------------------------------------------------------------------------------------------
// Options for specific charts
// ------------------------------------------------------------------------------------------------

/// Specifies additional options that are specific for the GeoChart chart type
module GeoChart =
  /// Which type of geochart this is. The `DataTable` format must match the value specified.
  type DisplayMode =
    /// Color the regions on the geochart.
    | Region
    /// Place markers on the regions.
    | Markers
    /// Label the regions with text from the DataTable.
    | Text
    /// Choose based on the format of the DataTable.
    | Auto

  /// Specifies additional options that are specific for the GeoChart chart type
  type Options = 
    { /// The area to display on the geochart. (Surrounding areas will be displayed as well.) 
      /// Can be one of the following:
      ///
      ///  - `"world"` - A geochart of the entire world.
      ///  - A continent or a sub-continent, specified by its 3-digit code, e.g., `011` for Western Africa.
      ///  - A country, specified by its ISO 3166-1 alpha-2 code, e.g., `AU` for Australia.
      ///  - A state in the United States, specified by its ISO 3166-2:US code, e.g., `US-AL` for Alabama. 
      ///    Note that the resolution option must be set to either 'provinces' or 'metros'.
      Region : string option
      /// Which type of geochart this is. The `DataTable` format must match the value specified.
      DisplayMode : DisplayMode option } 


/// Specifies additional options that are specific for the PieChart chart type
module PieChart = 
  /// Specifies additional options that are specific for the GeoChart chart type
  type Options = 
    { /// If between 0 and 1, displays a donut chart. The hole with have a radius equal to 
      /// number times the radius of the chart.
      PieHole : float option }


/// Specifies additional options that are specific for the PieChart chart type
module TimeLine = 
  /// Specifies additional options that are specific for the GeoChart chart type
  type Options = 
    { /// If set to false, omits row labels. The default is to show them.
      ShowRowLabels : bool option
      ///If set to false, omits bar labels. The default is to show them.
      ShowBarLabels : bool option
       }

//// Specifies the chart kind and chart-specific options
type ChartKind = 
  | GeoChart of GeoChart.Options
  | PieChart of PieChart.Options
  | Timeline of TimeLine.Options

// ------------------------------------------------------------------------------------------------
// Foogle chart data type - in the top-level namespace
// ------------------------------------------------------------------------------------------------

/// Represents a fully constructed chart with data
type FoogleChart =
  { Chart : ChartKind
    Data : Table
    Options : Options }