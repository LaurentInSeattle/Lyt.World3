namespace Lyt.World3.Shell;

using static MessagingExtensions;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private readonly World3Model world3Model;
    private readonly WorldModel worldModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    private bool mainToolbarIsVisible;

    private ViewSelector<ActivatedView>? viewSelector;
    public bool isFirstActivation;

    #region To please the XAML viewer 

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Should never be executed 
    public ShellViewModel()
    {
    }
#pragma warning restore CS8618 

    #endregion To please the XAML viewer 

    public ShellViewModel(World3Model world3Model, IToaster toaster)
    {
        this.world3Model = world3Model;
        this.worldModel = world3Model.WorldModel;
        this.toaster = toaster;

        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    private void OnLanguageChanged(LanguageChangedMessage message)
    {
        // We need to destroy and recreate the tray icon, so that it will be properly localized
        // since its native menu will not respond to dynamic property changes 
    }

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
        this.SetupWorkflow();
        this.Logger.Debug("OnViewLoaded SetupWorkflow complete");

        // Ready 
        //this.toaster.Host = this.View.ToasterHost;
        //if (true)
        //{
        //    this.toaster.Show(
        //        this.Localize("Shell.Ready"), this.Localize("Shell.Greetings"),
        //        1_600, InformationLevel.Info);
        //}

        this.MainToolbarIsVisible = true;

        // Run the world model 
        this.world3Model.Run();

        // Activate the language view
        this.isFirstActivation = true; 
        Select (ActivatedView.Language);

        this.Logger.Debug("OnViewLoaded complete");
    }

    private static async void OnExit()
    {
        var application = App.GetRequiredService<IApplicationBase>();
        await application.Shutdown();
    }

    private void SetupWorkflow()
    {
        if (this.View is not ShellView view)
        {
            throw new Exception("No view: Failed to startup...");
        }

        var selectableViews = new List<SelectableView<ActivatedView>>();

        void Setup<TViewModel, TControl, TToolbarViewModel, TToolbarControl>(
                ActivatedView activatedView, Control control)
            where TViewModel : ViewModel<TControl>
            where TControl : Control, IView, new()
            where TToolbarViewModel : ViewModel<TToolbarControl>
            where TToolbarControl : Control, IView, new()
        {
            var vm = App.GetRequiredService<TViewModel>();
            vm.CreateViewAndBind();
            var vmToolbar = App.GetRequiredService<TToolbarViewModel>();
            vmToolbar.CreateViewAndBind();
            selectableViews.Add(
                new SelectableView<ActivatedView>(activatedView, vm, control, vmToolbar));
        }

        void SetupNoToolbar<TViewModel, TControl>(
                ActivatedView activatedView, Control control)
            where TViewModel : ViewModel<TControl>
            where TControl : Control, IView, new()
        {
            var vm = App.GetRequiredService<TViewModel>();
            vm.CreateViewAndBind();
            selectableViews.Add(
                new SelectableView<ActivatedView>(activatedView, vm, control, null));
        }

        Setup<LanguageViewModel, LanguageView, LanguageToolbarViewModel, LanguageToolbarView>(
            ActivatedView.Language, view.FlagButton);

        SetupNoToolbar<SettingsViewModel, SettingsView>( ActivatedView.Settings, view.SettingsButton);

        SetupNoToolbar<ResultsViewModel, ResultsView>(ActivatedView.Results, view.ResultsButton);

        // Needs to be kept alive as a class member, or else callbacks will die (and wont work) 
        this.viewSelector =
            new ViewSelector<ActivatedView>(
                this.Messenger,
                this.View.ShellViewContent,
                this.View.ShellViewToolbar,
                this.View.SelectionGroup,
                selectableViews,
                this.OnViewSelected);
    }

    private void OnViewSelected(ActivatedView activatedView)
    {
        if (this.isFirstActivation)
        {
            this.Profiler.MemorySnapshot(this.ViewBase!.GetType().Name + ":  Activated");
        }

        this.isFirstActivation = false;
    }

#pragma warning disable IDE0079 
#pragma warning disable CA1822 // Mark members as static

    [RelayCommand]
    public void OnSettings() => Select(ActivatedView.Settings);

    [RelayCommand]
    public void OnResults() => Select(ActivatedView.Results);

    [RelayCommand]
    public void OnLanguage() => Select(ActivatedView.Language);

    [RelayCommand]
    public void OnClose() => OnExit();

#pragma warning restore CA1822
#pragma warning restore IDE0079

}
