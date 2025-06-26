namespace Lyt.World3.Charts;

using LiveChartsCore.SkiaSharpView;

public sealed partial class ChartViewModel : ViewModel<ChartView>
{
    // red : 244 67 54
    // blu :  33 150 240
    // gre : 139 195 74
    private static readonly SKColor s_gray = new(195, 195, 195);
    private static readonly SKColor s_gray1 = new(160, 160, 160);
    private static readonly SKColor s_gray2 = new(90, 90, 90);
    private static readonly SKColor s_dark3 = new(60, 60, 60, 128);

    private static readonly SKColor s_lightBlue = new(205, 210, 255, 50);
    private static readonly SKColor s_blue = new(33, 150, 240);
    private static readonly SKColor s_red = new(244, 67, 54);
    private static readonly SKColor s_green = new(139, 195, 74);

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
    public LabelVisual title;

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

    public ChartViewModel()
    {
        this.Series = [];
        this.ScrollbarSeries = [];
        this.Title = new();
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

        this.model.Start(this.model.Parameters.Get("Delta Time"));
        for (int k = 1; k <= 220; ++k)
        {
            this.model.Tick();
        }

        int length = (int)((this.model.Time - this.model.InitialTime()) / this.model.DeltaTime);
        List<ObservablePoint> points1 = new(length);
        List<ObservablePoint> points2 = new(length);
        List<ObservablePoint> points3 = new(length);

        double[] dataY1 = [.. this.model.GetLog("population")];
        double[] dataY2 = [.. this.model.GetLog("foodPerCapita")];
        double[] dataY3 = [.. this.model.GetLog("lifeExpectancy")];

        for (int i = 0; i < length; ++i)
        {
            double xi = this.model.InitialTime() + i * this.model.DeltaTime;
            points1.Add(new ObservablePoint(xi, Math.Round(dataY1[i])));
            points2.Add(new ObservablePoint(xi, Math.Round(dataY2[i])));
            points3.Add(new ObservablePoint(xi, Math.Round(dataY3[i])));
        }

        var popTitle = new LabelVisual
        {
            Text = "Population ~ Food per Capita ~ Life Expectancy",
            TextSize = 24,
            Paint = new SolidColorPaint(s_gray),
            Padding = new LiveChartsCore.Drawing.Padding(4)
        };

        var pop = CreateSerie(points1, s_blue, scaleAt: 0);
        var fpc = CreateSerie(points2, s_red, scaleAt: 1);
        var le = CreateSerie(points3, s_green, scaleAt: 2);
        le.LineSmoothness = 0.0;

        var xAxis = CreateAxis("Year", s_gray);
        this.xAxisLabeler =  xAxis.Labeler;
        xAxis.NamePadding = new LiveChartsCore.Drawing.Padding(4);
        xAxis.Padding = new LiveChartsCore.Drawing.Padding(4);
        var popAxis = CreateAxis("Population", s_blue);
        var foodAxis = CreateAxis("Food per Capita", s_red);
        foodAxis.Labeler = this.IntegerLabeler;
        var leAxis = CreateAxis("Life Expectancy", s_green);
        leAxis.Labeler = this.IntegerLabeler; 

        this.ScrollableAxes = [xAxis];
        this.YAxes = [popAxis, foodAxis, leAxis];
        this.Series = [pop, fpc, le];
        this.ScrollbarSeries = [pop];
        this.InvisibleX = [new Axis { IsVisible = false }];
        this.InvisibleY = [new Axis { IsVisible = false }];
        this.Thumbs = [new RectangularSection { Fill = new SolidColorPaint(s_lightBlue) }];
        this.Title = popTitle;
        this.Frame =
            new()
            {
                Fill = new SolidColorPaint(s_dark3),
                Stroke = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                }
            };

        // force the left margin and the right margin to be the same in both charts, this will
        // align the start and end point of the "draw margin", no matter the size of
        // the labels in the Y axis of both chart.
        float auto = LiveChartsCore.Measure.Margin.Auto;
        this.DrawMargin = new(320, auto, 50, auto);

        Schedule.OnUiThread(
            50,
            () =>
            {
                this.View.InvalidateVisual();
                this.View.Chart.IsVisible = true;
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
        IReadOnlyCollection<ObservablePoint> points, SKColor color, int scaleAt)
        => new()
        {
            Values = points,
            LineSmoothness = 0.7,
            Stroke = new SolidColorPaint(color),
            Fill = null,
            GeometrySize = 0,
            // it will be scaled at the Axis[scaleAt] instance 
            ScalesYAt = scaleAt,
        };

    private static Axis CreateAxis(string name, SKColor color)
        => new()
        {
            Name = name,
            NameTextSize = 14,
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 12),
            Padding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
            TextSize = 16,
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
