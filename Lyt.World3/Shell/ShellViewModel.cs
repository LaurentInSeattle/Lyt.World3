namespace Lyt.World3.Shell;

using static Lyt.World3.Messaging.ViewActivationMessage;

//using static MessagingExtensions;
//using static ViewActivationMessage;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private readonly World3Model world3Model;
    private readonly WorldModel worldModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    private bool mainToolbarIsVisible;

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

        this.Messenger.Subscribe<ViewActivationMessage>(this.OnViewActivation);
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

        this.MainToolbarIsVisible = true; 
        Schedule.OnUiThread(1_000, this.Test, DispatcherPriority.Normal);

        this.Logger.Debug("OnViewLoaded complete");
    }

    private void Test()
    {
        // var vm = new ChartViewModel(this.model.GetPlotDefinitionByName("Summary"));
        var vm = new ChartViewModel(this.worldModel.GetPlotDefinitionByName("Population"));
        vm.CreateViewAndBind(); 
        this.View.ShellViewContent.Content = vm.View;
        vm.DataBind(this.worldModel); 
    }

    private static async void OnExit()
    {
        var application = App.GetRequiredService<IApplicationBase>();
        await application.Shutdown();
    }

    private static void SetupWorkflow()
    {
        App.GetRequiredService<LanguageViewModel>().CreateViewAndBind();
        App.GetRequiredService<LanguageToolbarViewModel>().CreateViewAndBind();
        App.GetRequiredService<SettingsViewModel>().CreateViewAndBind();
        App.GetRequiredService<ResultsViewModel>().CreateViewAndBind();
    }

    [RelayCommand]
    public void OnSettings() => this.OnViewActivation(ActivatedView.Settings);

    [RelayCommand]
    public void OnResults() => this.OnViewActivation(ActivatedView.Results);

    [RelayCommand]
    public void OnLanguage() => this.OnViewActivation(ActivatedView.Language);

#pragma warning disable IDE0079 
#pragma warning disable CA1822 // Mark members as static

    [RelayCommand]
    public void OnClose() => OnExit();

#pragma warning restore CA1822
#pragma warning restore IDE0079

    private void OnViewActivation(ViewActivationMessage message)
        => this.OnViewActivation(message.View, message.ActivationParameter, false);

    private void OnViewActivation(ActivatedView activatedView, object? parameter = null, bool isFirstActivation = false)
    {
        ViewModel? CurrentViewModel()
        {
            object? currentView = this.View.ShellViewContent.Content;
            if (currentView is Control control &&
                control.DataContext is ViewModel currentViewModel)
            {
                return currentViewModel;
            }

            return null;
        }

        if (activatedView == ActivatedView.GoBack)
        {
            // We always go back to the Settings View 
            activatedView = ActivatedView.Settings;
        }

        bool programmaticNavigation = false;
        ActivatedView hasBeenActivated = ActivatedView.Exit;
        ViewModel? currentViewModel = null;
        if (parameter is bool navigationType)
        {
            programmaticNavigation = navigationType;
            currentViewModel = CurrentViewModel();
        }

        void NoToolbar() => this.View.ShellViewToolbar.Content = null;

        void SetupToolbar<TViewModel, TControl>()
            where TViewModel : ViewModel<TControl>
            where TControl : Control, IView, new()
        {
            if (this.View is null)
            {
                throw new Exception("No view: Failed to startup...");
            }

            var newViewModel = App.GetRequiredService<TViewModel>();
            this.View.ShellViewToolbar.Content = newViewModel.View;
        }

        switch (activatedView)
        {
            default:
            case ActivatedView.Settings:
                if (!(programmaticNavigation && currentViewModel is SettingsViewModel))
                {
                    NoToolbar();
                    // SetupToolbar<SettingsToolbarViewModel, SettingsToolbarView>();
                    this.Activate<SettingsViewModel, SettingsView>(isFirstActivation, parameter);
                    hasBeenActivated = ActivatedView.Settings;
                }
                break;

            case ActivatedView.Results:
                if (!(programmaticNavigation && currentViewModel is ResultsViewModel))
                {
                    NoToolbar();
                    // SetupToolbar<ResultsToolbarViewModel, ResultsToolbarView>();
                    this.Activate<ResultsViewModel, ResultsView>(isFirstActivation, null);
                    hasBeenActivated = ActivatedView.Results;
                }
                break;

            case ActivatedView.Language:
                if (this.world3Model.IsFirstRun)
                {
                    SetupToolbar<LanguageToolbarViewModel, LanguageToolbarView>();
                }
                else
                {
                    NoToolbar();
                }

                this.Activate<LanguageViewModel, LanguageView>(isFirstActivation, null);
                hasBeenActivated = ActivatedView.Language;
                break;
        }

        // Reflect in the navigation toolbar the programmatic change 
        if (programmaticNavigation && (hasBeenActivated != ActivatedView.Exit))
        {
            if (this.View is not ShellView view)
            {
                throw new Exception("No view: Failed to startup...");
            }

            var selector = view.SelectionGroup;
            var button = hasBeenActivated switch
            {
                ActivatedView.Results => view.ResultsButton,
                ActivatedView.Settings => view.SettingsButton,
                ActivatedView.Language => view.FlagButton,
                _ => view.SettingsButton,
            };
            selector.Select(button);
        }

        //bool mainToolbarIsHidden =
        //    this.world3Model.IsFirstRun || CurrentViewModel() is SettingsViewModel;
        //this.MainToolbarIsVisible = !mainToolbarIsHidden;
    }

    private void Activate<TViewModel, TControl>(bool isFirstActivation, object? activationParameters)
        where TViewModel : ViewModel<TControl>
        where TControl : Control, IView, new()
    {
        if (this.View is null)
        {
            throw new Exception("No view: Failed to startup...");
        }

        var newViewModel = App.GetRequiredService<TViewModel>();
        object? currentView = this.View.ShellViewContent.Content;
        if (currentView is Control control && control.DataContext is ViewModel currentViewModel)
        {
            if (newViewModel == currentViewModel)
            {
                return;
            }

            currentViewModel.Deactivate();
        }


        newViewModel.Activate(activationParameters);
        this.View.ShellViewContent.Content = newViewModel.View;
        if (!isFirstActivation)
        {
            this.Profiler.MemorySnapshot(newViewModel.View.GetType().Name + ":  Activated");
        }
    }

}
