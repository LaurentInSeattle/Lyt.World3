namespace Lyt.World3.Workflow.Language;

using static MessagingExtensions;

public sealed partial class LanguageToolbarViewModel : ViewModel<LanguageToolbarView>
{
#pragma warning disable IDE0079
#pragma warning disable CA1822 // Mark members as static

    // Relay commands MUST not be static 
    // OR ELSE: Exception thrown: Common Language Runtime detected an invalid program.
    [RelayCommand]
    public void OnNext() => Select(ActivatedView.Results);

#pragma warning restore CA1822
#pragma warning restore IDE0079
}
