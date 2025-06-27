namespace Lyt.World3.Charts;

using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Layouts;
using LiveChartsCore.SkiaSharpView.SKCharts;
using Lyt.Simulation;

public sealed partial class ChartViewModel : ViewModel<ChartView>
{
    // red : 244 67 54
    // blu :  33 150 240
    // gre : 139 195 74
    private static readonly SKColor s_gray = new(195, 195, 195);
    private static readonly SKColor s_gray1 = new(160, 160, 160);
    private static readonly SKColor s_gray2 = new(90, 90, 90);
    private static readonly SKColor s_dark = new(10, 10, 40, 220);
    private static readonly SKColor s_dark3 = new(60, 60, 60, 128);

    private static readonly SKColor s_lightBlue = new(205, 210, 255, 50);

    private static readonly SKColor s_blue = new(33, 150, 240);
    private static readonly SKColor s_red = new(244, 67, 54);
    private static readonly SKColor s_green = new(139, 195, 74);
    private static readonly SKColor s_yellow = new(13, 195, 140);
    private static readonly SKColor s_pink = new(199, 195, 74);

    private static readonly SKColor[] colors = [ s_blue, s_red, s_green, s_yellow, s_pink, ]; 

    private static SKColor Color(int i ) => colors[i % colors.Length];

    private readonly PlotDefinition plotDefinition;
    private readonly int curveCount;

    private WorldModel? model;
    private bool isPointerDown;
    private Func<double, string>? xAxisLabeler; 

    [ObservableProperty]
    public LiveChartsCore.Measure.Margin drawMargin;

    [ObservableProperty]
    public ISeries[] series;

    [ObservableProperty]
    public ISeries[] scrollbarSeries;

    [ObservableProperty]
    public string title;

    [ObservableProperty]
    public DrawMarginFrame frame;

    [ObservableProperty]
    public Axis[]? scrollableAxes;
        
    [ObservableProperty]
    public RectangularSection[]? thumbs;

    [ObservableProperty]
    public ICartesianAxis[]? yAxes;

    [ObservableProperty]
    public Axis[]? invisibleX;

    [ObservableProperty]
    public Axis[]? invisibleY;

    public ChartViewModel(PlotDefinition plotDefinition)
    {
        this.plotDefinition = plotDefinition;
        this.curveCount = plotDefinition.Curves.Count;

        this.Series = [];
        this.ScrollbarSeries = [];
        this.Title = string.Empty;
        this.Frame = new();
        this.DrawMargin = new();
    }

    public void DataBind(WorldModel model)
    {

        this.model = model;

        if (this.View is null)
        {
            throw new Exception("No view: Failed to startup?");
        }

        if (this.model is null) // || ?? 
        {
            throw new Exception("Null or Invalid model");
        }

        // Step #1: Generate data for X axis (years)
        int length = (int)((this.model.Time - this.model.InitialTime()) / this.model.DeltaTime);
        double[] dataX = new double[length];
        for (int i = 0; i < length; ++i)
        {
            double xi = this.model.InitialTime() + i * this.model.DeltaTime;
            dataX[i] = xi;
        }

        // Step #2: Extract data from the model to create collections of Observable points
        var pointsList = new List<List<ObservablePoint>>(curveCount);
        for (int i = 0; i < curveCount; ++i)
        {
            var curve = plotDefinition.Curves[i];
            double[] dataY = [.. this.model.GetData(curve.EquationName)];
            var points = new List<ObservablePoint>(length);
            for (int k = 0; k < length; ++k)
            {
                double xk = dataX[k];
                double yk = dataY[k];
                if (curve.UseIntegers)
                {
                    yk = Math.Round(yk);
                }

                yk = yk * curve.ScaleFactor;
                points.Add(new ObservablePoint(xk,yk));
            }

            pointsList.Add(points);
        }

        // Step #3: Create Plot Title
        //var popTitle = new LabelVisual
        //{
        //    Text = this.plotDefinition.Title,
        //    TextSize = 28,
        //    Paint = new SolidColorPaint(s_gray),
        //    Padding = new LiveChartsCore.Drawing.Padding(4)
        //};
        this.Title = this.plotDefinition.Title;

        // Step #4: Create Series 
        var series = new List<LineSeries<ObservablePoint>>();
        for (int i = 0; i < curveCount; ++i)
        {
            var curve = plotDefinition.Curves[i];
            var points = pointsList[i];
            var color = Color(i);
            int scaleAt = curve.ScaleUsingAxisIndex;
            var serie = CreateSerie(points, curve.Name, color, scaleAt);
            serie.LineSmoothness = curve.LineSmoothness;
            series.Add(serie);
        }

        this.Series = series.ToArray();
        this.ScrollbarSeries = [series[0]];

        // Step #5: Create X Axis and scrolling stuff 
        var xAxis = CreateAxis("Year", s_gray);
        this.xAxisLabeler =  xAxis.Labeler;
        xAxis.NamePadding = new LiveChartsCore.Drawing.Padding(4);
        xAxis.Padding = new LiveChartsCore.Drawing.Padding(4);
        this.ScrollableAxes = [xAxis];
        this.InvisibleX = [new Axis { IsVisible = false }];
        this.InvisibleY = [new Axis { IsVisible = false }];
        this.Thumbs = [new RectangularSection { Fill = new SolidColorPaint(s_lightBlue) }];

        // Step #6: Create Y Axises 
        var axes = new List<Axis>();
        for (int i = 0; i < curveCount; ++i)
        {
            var curve = plotDefinition.Curves[i];
            if ( curve.HasAxis)
            {
                var color = Color(i);
                var axis = CreateAxis(curve.Name, color);
                if (curve.UseIntegers)
                {
                    axis.Labeler = this.IntegerLabeler;
                }

                axes.Add(axis);
            }
        }

        this.YAxes = axes.ToArray();

        // Step #7 : Customize legend and tooltips
        var cartesianChart = this.View.Chart;
        cartesianChart.LegendBackgroundPaint = new SolidColorPaint(s_dark);
        cartesianChart.LegendTextPaint = new SolidColorPaint(s_gray);
        cartesianChart.LegendTextSize = 13;
        cartesianChart.TooltipBackgroundPaint = new SolidColorPaint(s_dark);
        cartesianChart.TooltipTextPaint = new SolidColorPaint(s_gray);
        cartesianChart.TooltipTextSize = 12;

        // Still needed ? 
        //this.Frame =
        //    new()
        //    {
        //        Fill = new SolidColorPaint(s_dark3),
        //        Stroke = new SolidColorPaint
        //        {
        //            Color = s_gray,
        //            StrokeThickness = 1
        //        }
        //    };

        // Step #8 : Adjust margins 
        // force the left margin and the right margin to be the same in both charts, this will
        // align the start and end point of the "draw margin", no matter the size of
        // the labels in the Y axis of both chart.
        float auto = LiveChartsCore.Measure.Margin.Auto;
        float left = 90.0f * axes.Count + 20.0f; 
        this.DrawMargin = new(left, auto, 50, auto);

        Schedule.OnUiThread(
            50,
            () =>
            {
                this.View.InvalidateVisual();
                this.View.Chart.IsVisible = true;

                // Nope, doesn't work
                //
                //Debugger.Break();
                //if ( cartesianChart.Legend is SKDefaultLegend legend )
                //{
                //    if ( legend.Content is StackLayout layout )
                //    {
                //        layout.HorizontalAlignment = Align.End;
                //        layout.Measure();
                //    }
                //}
            }, DispatcherPriority.Background);
    }

    [RelayCommand]
    public void ChartUpdated(ChartCommandArgs args)
    {
        // Debug.WriteLine("Chart updated");

        if (this.Thumbs is null || this.Thumbs.Length == 0)
        {
            Debug.WriteLine("no thumbs");
            return;
        }

        var cartesianChart = (ICartesianChartView)args.Chart;
        var x = cartesianChart.XAxes.First();

        // update the scroll bar thumb when the chart is updated (zoom/pan)
        // this will let the user know the current visible range
        RectangularSection thumb = this.Thumbs[0];
        thumb.Xi = x.MinLimit;
        thumb.Xj = x.MaxLimit;

        if (this.ScrollableAxes is null || this.ScrollableAxes.Length == 0)
        {
            Debug.WriteLine("No ScrollableAxes");
            return;
        }
        var xAxis = this.ScrollableAxes[0];
        if (thumb.Xj - thumb.Xi > 20)
        {
            if (this.xAxisLabeler is not null)
            {
                xAxis.Labeler = this.xAxisLabeler;
            }
        }
        else
        {
            xAxis.Labeler = this.IntegerLabeler;
        }
    }

    [RelayCommand]
    public void PointerDown(PointerCommandArgs args) => this.isPointerDown = true;

    [RelayCommand]
    public void PointerUp(PointerCommandArgs args) => this.isPointerDown = false;

    [RelayCommand]
    public void PointerMove(PointerCommandArgs args)
    {
        if (!this.isPointerDown)
        {
            return;
        }

        // if (Debugger.IsAttached) { Debugger.Break(); } 

        if (this.Thumbs is null || this.Thumbs.Length == 0)
        {
            Debug.WriteLine("No thumbs");
            return;
        }

        if (this.ScrollableAxes is null || this.ScrollableAxes.Length == 0)
        {
            Debug.WriteLine("No ScrollableAxes");
            return;
        }

        var xAxis = this.ScrollableAxes[0];
        var chartView = (ICartesianChartView)args.Chart;
        var positionInData = chartView.ScalePixelsToData(args.PointerPosition);
        var thumb = this.Thumbs[0];
        double? currentRange = thumb.Xj - thumb.Xi;
        if (!currentRange.HasValue)
        {
            Debug.WriteLine("No value for currentRange");
            return;
        }

        // update the scroll bar thumb when the user is dragging the chart
        double? xi = positionInData.X - currentRange / 2;
        double? xj = positionInData.X + currentRange / 2;

        // Debug.WriteLine("Position: " + thumb.Xi.ToString() + "  " + thumb.Xj.ToString());
        if (xj - xi > 20)
        {
            if (this.xAxisLabeler is not null)
            {
                xAxis.Labeler = this.xAxisLabeler;
            }
        }
        else
        {
            xAxis.Labeler = this.IntegerLabeler;
        }

        if (xj - xi <= 3)
        {
            return;
        }

        // update the chart visible range
        if (xi >= 1900)
        {
            thumb.Xi = xi;
            this.ScrollableAxes[0].MinLimit = xi;
        }

        if (xj <= 2120)
        {
            thumb.Xj = xj;
            this.ScrollableAxes[0].MaxLimit = xj;
        }
    }

    private static LineSeries<ObservablePoint> CreateSerie(
        IReadOnlyCollection<ObservablePoint> points, string name, SKColor color, int scaleAt)
        => new()
        {
            Values = points,
            Name = name,
            LineSmoothness = 0.7,
            // "fat' lines: 2 pixels 
            Stroke = new SolidColorPaint(color, 2.0f),             
            Fill = null,
            GeometrySize = 0,
            // it will be scaled at the Axis[scaleAt] instance 
            ScalesYAt = scaleAt,
        };

    private static Axis CreateAxis(string name, SKColor color)
        => new()
        {
            Name = name,
            // Value changes the axis width: See Step #8 above 
            NameTextSize = 14,
            TextSize = 14,
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 12),
            Padding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
            // MUST create a new Paint object for each property or else rendering gets messed up
            NamePaint = new SolidColorPaint(color),
            LabelsPaint = new SolidColorPaint(color),
            TicksPaint = new SolidColorPaint(color),
            SubticksPaint = new SolidColorPaint(color),
            DrawTicksPath = true,
            ShowSeparatorLines = false,
        };

    private string IntegerLabeler(double value)
    {
        int intValue = (int)(value + 0.5);
        if (Math.Abs(intValue - value) > 0.001)
        {
            return string.Empty;
        }

        return intValue.ToString("D");
    }
}
