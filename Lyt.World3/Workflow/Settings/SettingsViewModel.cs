namespace Lyt.World3.Workflow.Settings;

public sealed class SettingsViewModel : ViewModel<SettingsView>
{
    private readonly World3Model world3Model;
    private readonly WorldModel worldModel;

    public SettingsViewModel(World3Model world3Model)
    {
        this.world3Model = world3Model;
        this.worldModel = world3Model.WorldModel;
    }

    public override void OnViewLoaded() 
    {
        base.OnViewLoaded();

    }
}
