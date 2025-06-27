namespace Lyt.World3.Workflow.Language;

using static ViewActivationMessage;
using static MessagingExtensions;

public sealed partial class LanguageToolbarViewModel : ViewModel<LanguageToolbarView>
{
#pragma warning disable IDE0079 
#pragma warning disable CA1822 // Mark members as static

    [RelayCommand]
    public void OnNext()
    {
        bool programmaticNavigation = true; 
        ActivateView(ActivatedView.Results, programmaticNavigation);
    } 

#pragma warning restore CA1822
#pragma warning restore IDE0079
}
