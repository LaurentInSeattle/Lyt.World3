namespace Lyt.World3.Messaging;

public sealed record class ViewActivationMessage(
    ViewActivationMessage.ActivatedView View, object? ActivationParameter = null)
{
    public enum ActivatedView
    {
        Settings,
        Results,

        GoBack,
        Exit,
        Language,
    }
}
