namespace Lyt.World3.Shell;

//using static MessagingExtensions;
//using static ViewActivationMessage;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private static readonly SKColor s_gray = new(195, 195, 195);
    private static readonly SKColor s_gray1 = new(160, 160, 160);
    private static readonly SKColor s_gray2 = new(90, 90, 90);
    private static readonly SKColor s_dark3 = new(60, 60, 60, 128);
    private static readonly SKColor s_blue = new(25, 118, 210);
    private static readonly SKColor s_red = new(229, 57, 53);
    private static readonly SKColor s_yellow = new(198, 167, 0);

    // private readonly World world;
    private readonly WorldModel model;
    
    //private readonly AstroPicModel astroPicModel;
    private readonly IToaster toaster;

    #region To please the XAML viewer 

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Should never be executed 
    public ShellViewModel()
    {
    }
#pragma warning restore CS8618 

    #endregion To please the XAML viewer 

    public ShellViewModel(/* AstroPicModel astroPicModel, */ IToaster toaster)
    {
        // this.world = new();
        this.model = new();

        // this.astroPicModel = astroPicModel;
        this.toaster = toaster;

        //this.Messenger.Subscribe<ViewActivationMessage>(this.OnViewActivation);
        //this.Messenger.Subscribe<ToolbarCommandMessage>(this.OnToolbarCommand);
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);

        //this.YAxes = [];
        this.Series = [];
        this.Title = new();
        this.Frame = new();
    }

    private void OnLanguageChanged(LanguageChangedMessage message)
    {
        // We need to destroy and recreate the tray icon, so that it will be properly localized
        // since its native menu will not respond to dynamic property changes 
    }

    // private void OnToolbarCommand(ToolbarCommandMessage _) => this.rotatorTimer.Reset();

    public override void OnViewLoaded()
    {
        this.Logger.Debug("OnViewLoaded begins");

        base.OnViewLoaded();
        if (this.View is null)
        {
            throw new Exception("Failed to startup...");
        }

        // Select default language 
        string preferredLanguage = "en-US"; //  this.astroPicModel.Language;
        this.Logger.Debug("Language: " + preferredLanguage);
        this.Localizer.SelectLanguage(preferredLanguage);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(preferredLanguage);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(preferredLanguage);

        this.Logger.Debug("OnViewLoaded language loaded");

        // Create all statics views and bind them 
        ShellViewModel.SetupWorkflow();
        this.Logger.Debug("OnViewLoaded SetupWorkflow complete");

        // Ready 
        //this.toaster.Host = this.View.ToasterHost;
        //if (true)
        //{
        //    this.toaster.Show(
        //        this.Localize("Shell.Ready"), this.Localize("Shell.Greetings"),
        //        1_600, InformationLevel.Info);
        //}

        Schedule.OnUiThread(1_000, this.Test, DispatcherPriority.Normal);

        this.Logger.Debug("OnViewLoaded complete");
    }

    private void Test()
    {
        if (this.View is null)
        {
            throw new Exception("Failed to startup...");
        }

        this.model.Start(this.model.Parameters.Get("Delta Time"));
        // this.world.Initialize();
        for (int k = 1; k <= 400; ++k)
        {
            this.model.Tick();

            // this.world.Update(k);
        }

        // if (Debugger.IsAttached) { Debugger.Break(); }

        int length = (int)((this.model.Time - this.model.InitialTime()) / this.model.DeltaTime);
        List<ObservablePoint> points1 = new(length);
        List<ObservablePoint> points2 = new(length);
        List<ObservablePoint> points3 = new(length);

        //double[] dataX = new double[length];
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

        var pop = new LineSeries<ObservablePoint>
        {
            Values = points1,
            LineSmoothness = 0.7,
            Fill = null,
            GeometrySize = 0,
            ScalesYAt = 0 // it will be scaled at the Axis[0] instance 
        };

        //var popColor = pop.Stroke;
        //if (Debugger.IsAttached) { Debugger.Break(); }

        var fpc = new LineSeries<ObservablePoint>
        {
            Values = points2,
            LineSmoothness = 0.7,
            Fill = null,
            GeometrySize = 0,
            ScalesYAt = 1 // it will be scaled at the Axis[1] instance 
        };

        var le = new LineSeries<ObservablePoint>
        {
            Values = points3,
            LineSmoothness = 0.8,
            Fill = null,
            GeometrySize = 0,
            ScalesYAt = 2 // it will be scaled at the Axis[2] instance 
        };

        var popTitle = new LabelVisual
        {
            Text = "Population ~ Food per Capita ~ Life Expectancy",
            TextSize = 24,
            Paint = new SolidColorPaint(s_gray),
            Padding = new LiveChartsCore.Drawing.Padding(4)
        };

        var popAxis =
            new Axis
            {
                Name = "Population",
                NameTextSize = 14,
                NamePaint = new SolidColorPaint(s_blue),
                LabelsPaint = new SolidColorPaint(s_blue),
                TicksPaint = new SolidColorPaint(s_blue),
                SubticksPaint = new SolidColorPaint(s_blue),
                NamePadding = new LiveChartsCore.Drawing.Padding(0, 12),
                Padding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
                TextSize = 16,
                ShowSeparatorLines = false,
                DrawTicksPath = true
            }; 
        var foodAxis =
            new Axis 
            {
                Name = "Food per Capita",
                NameTextSize = 14,
                NamePaint = new SolidColorPaint(s_red),
                NamePadding = new LiveChartsCore.Drawing.Padding(0, 12),
                Padding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
                TextSize = 16,
                LabelsPaint = new SolidColorPaint(s_red),
                TicksPaint = new SolidColorPaint(s_red),
                SubticksPaint = new SolidColorPaint(s_red),
                DrawTicksPath = true,
                ShowSeparatorLines = false,
                // Position = LiveChartsCore.Measure.AxisPosition.End
            };

        var leAxis =
            new Axis 
            {
                Name = "Life Expectancy",
                NameTextSize = 14,
                NamePaint = new SolidColorPaint(s_yellow),
                NamePadding = new LiveChartsCore.Drawing.Padding(0, 12),
                Padding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
                TextSize = 16,
                LabelsPaint = new SolidColorPaint(s_yellow),
                TicksPaint = new SolidColorPaint(s_yellow),
                SubticksPaint = new SolidColorPaint(s_yellow),
                DrawTicksPath = true,
                ShowSeparatorLines = false,
                // Position = LiveChartsCore.Measure.AxisPosition.End
            };

        this.YAxes = [popAxis, foodAxis, leAxis];
        this.Series = [pop, fpc, le]; 
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

        Schedule.OnUiThread(
            50, 
            ()=> 
            { 
                this.View.InvalidateVisual();
                this.View.Chart.IsVisible = true;
            } , DispatcherPriority.Background);
    }

    private static async void OnExit()
    {
        var application = App.GetRequiredService<IApplicationBase>();
        await application.Shutdown();
    }

    private static void SetupWorkflow()
    {
    }

#pragma warning disable IDE0079 
#pragma warning disable CA1822 // Mark members as static

    [RelayCommand]
    public void OnClose() => OnExit();

#pragma warning restore CA1822
#pragma warning restore IDE0079

    [ObservableProperty]
    public bool mainToolbarIsVisible ;

    [ObservableProperty]
    public ISeries[] series;

    [ObservableProperty]
    public LabelVisual title;

    [ObservableProperty]
    public DrawMarginFrame frame;

    [ObservableProperty]
    public ICartesianAxis[]? yAxes;
}
