namespace Lyt.World3.Workflow.Language;

using static MessagingExtensions;

public sealed partial class LanguageToolbarViewModel : ViewModel<LanguageToolbarView>
{
    [RelayCommand]
    public static void OnNext() => Select (ActivatedView.Results);
}
