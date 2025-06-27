namespace Lyt.World3.Workflow.Results;

public sealed class ResultsViewModel : ViewModel<ResultsView>
{
    private readonly World3Model world3Model;
    private readonly WorldModel worldModel;

    public ResultsViewModel(World3Model world3Model)
    {
        this.world3Model = world3Model;
        this.worldModel = world3Model.WorldModel;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        var vm = new ChartViewModel(WorldModel.GetPlotDefinitionByName("Summary"));
        // var vm = new ChartViewModel(WorldModel.GetPlotDefinitionByName("Population"));
        vm.CreateViewAndBind();
        this.View.Content = vm.View;
        vm.DataBind(this.worldModel);
    }
}
