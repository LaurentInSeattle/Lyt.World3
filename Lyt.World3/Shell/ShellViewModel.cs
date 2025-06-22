namespace Lyt.World3.Shell;

//using static MessagingExtensions;
//using static ViewActivationMessage;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private readonly World world; 
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
        this.world = new();

        // this.astroPicModel = astroPicModel;
        this.toaster = toaster;

        //this.Messenger.Subscribe<ViewActivationMessage>(this.OnViewActivation);
        //this.Messenger.Subscribe<ToolbarCommandMessage>(this.OnToolbarCommand);
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
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

        this.world.Initialize();
        for (int k = 1; k <= 200; ++k)
        {
            this.world.Update(k);
        }

        if (Debugger.IsAttached) { Debugger.Break(); }
        double[] dataX = [.. this.world.Time];
        double[] dataY = [.. this.world.Population.Pop];
        AvaPlot? avaPlot = this.View.Find<AvaPlot>("AvaPlot");
        if (avaPlot is not null)
        {
            var plot = avaPlot.Plot;
            plot.Add.Palette = new ScottPlot.Palettes.Penumbra();
            Scatter scatter = plot.Add.Scatter(dataX, dataY);
            scatter.LineWidth = 5;

            // change figure colors
            plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            plot.DataBackground.Color = ScottPlot.Color.FromHex("#1f1f1f");

            // change axis and grid colors
            plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");

            avaPlot.Refresh();
        }
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
}
