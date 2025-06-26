namespace Lyt.World3.Shell;

using Lyt.World3.Charts;

//using static MessagingExtensions;
//using static ViewActivationMessage;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private readonly WorldModel model;
    
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
        // var vm = new ChartViewModel(this.model.GetPlotDefinitionByName("Summary"));
        var vm = new ChartViewModel(this.model.GetPlotDefinitionByName("Population"));
        vm.CreateViewAndBind(); 
        this.View.ShellViewContent.Content = vm.View;
        vm.DataBind(this.model); 
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
