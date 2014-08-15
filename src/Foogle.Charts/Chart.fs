namespace Foogle

// ------------------------------------------------------------------------------------------------
// Primitive chart types supported by Foogle
// ------------------------------------------------------------------------------------------------

type Chart =
  static member GeoChart(data:seq<string * #value>, ?Label, ?Region, ?DisplayMode) =
    { Data = Table.fromKeyValue Label data
      Options = Options.Empty
      Chart = GeoChart { GeoChart.Region = Region; GeoChart.DisplayMode = DisplayMode } }

  static member GeoChart(data:seq<string * #value * #value>, ?Labels, ?Region, ?DisplayMode) =
    { Data = Table.fromKey2Values Labels data
      Options = Options.Empty
      Chart = GeoChart { GeoChart.Region = Region; GeoChart.DisplayMode = DisplayMode } }

  static member PieChart(data:seq<string * #value>, ?Label, ?PieHole) =
    { Data = Table.fromKeyValue Label data
      Options = Options.Empty
      Chart = PieChart { PieChart.PieHole = PieHole } }


// ------------------------------------------------------------------------------------------------
// Extensions that provide functional access to configuration via pipelining
// ------------------------------------------------------------------------------------------------

type Chart with
  static member WithTitle(?Title:string) = 
    fun (fc:FoogleChart) -> 
      { fc with Options = { fc.Options with Title = Title } }

  static member WithPie(?PieHole:float) = 
    fun (fc:FoogleChart) -> 
      { fc with Chart = match fc.Chart with PieChart pc -> PieChart { pc with PieHole = PieHole } | _ -> invalidOp "WithPie only works on pie charts" }

  static member WithOutput(?Engine) = 
    fun (fc:FoogleChart) -> 
      { fc with Options = { fc.Options with Engine = Engine } }

  /// Specifies a mapping between color column values and colors or a gradient scale. 
  ///
  /// ## Parameters
  ///  - `MinValue` - If present, specifies a minimum value for chart color data. Color data 
  ///    values of this value and lower will be rendered as the first color in the 
  ///    `colorAxis.colors` range.
  ///
  ///  - `MaxValue` - If present, specifies a maximum value for chart color data. Color data 
  ///    values of this value and higher will be rendered as the last color in the 
  ///    `colorAxis.colors` range.
  ///
  ///  - `Values` - If present, controls how values are associated with colors. Each value is 
  ///    associated with the corresponding color in the colorAxis.colors array. These values apply 
  ///    to the chart color data. Coloring is done according to a gradient of the values specified 
  ///    here. Not specifying a value for this option is equivalent to specifying 
  ///    `[minValue, maxValue]`.
  ///
  ///  - `Colors` - Colors to assign to values in the visualization. An array of strings, where 
  ///    each element is a color. You must have at least two values; the gradient will include all 
  ///    your values, plus calculated intermediary values, with the first color as the smallest 
  ///    value, and the last color as the highest.
  static member WithColorAxis(?MinValue:float, ?MaxValue:float, ?Values:seq<float>, ?Colors:seq<Color>) = 
    fun (fc:FoogleChart) -> 
      let ca = { MinValue = MinValue; MaxValue = MaxValue; Values = Values; Colors = Colors }
      { fc with Options = { fc.Options with ColorAxis = Some ca } }

// ------------------------------------------------------------------------------------------------
// Extension methods that provide object-oriented access to configuration
// ------------------------------------------------------------------------------------------------

[<AutoOpen>]
module FoogleChartExtensions = 
  type FoogleChart with
    /// Specifies a mapping between color column values and colors or a gradient scale. 
    ///
    /// ## Parameters
    ///  - `MinValue` - If present, specifies a minimum value for chart color data. Color data 
    ///    values of this value and lower will be rendered as the first color in the 
    ///    `colorAxis.colors` range.
    ///
    ///  - `MaxValue` - If present, specifies a maximum value for chart color data. Color data 
    ///    values of this value and higher will be rendered as the last color in the 
    ///    `colorAxis.colors` range.
    ///
    ///  - `Values` - If present, controls how values are associated with colors. Each value is 
    ///    associated with the corresponding color in the colorAxis.colors array. These values apply 
    ///    to the chart color data. Coloring is done according to a gradient of the values specified 
    ///    here. Not specifying a value for this option is equivalent to specifying 
    ///    `[minValue, maxValue]`.
    ///
    ///  - `Colors` - Colors to assign to values in the visualization. An array of strings, where 
    ///    each element is a color. You must have at least two values; the gradient will include all 
    ///    your values, plus calculated intermediary values, with the first color as the smallest 
    ///    value, and the last color as the highest.
    member fc.WithColorAxis(?MinValue:float, ?MaxValue:float, ?Values:seq<float>, ?Colors:seq<Color>) = 
      let ca = { MinValue = MinValue; MaxValue = MaxValue; Values = Values; Colors = Colors }
      { fc with Options = { fc.Options with ColorAxis = Some ca } }