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
        var vmSummary = new ChartViewModel(WorldModel.GetPlotDefinitionByName("Summary"));
        vmSummary.CreateViewAndBind();
        var vmPop = 
            new ChartViewModel(WorldModel.GetPlotDefinitionByName("Population"), isMini:true);
        var viewPop = new MiniChartView();
        vmPop.Bind(viewPop);

        var gridChildren = this.View.MainGrid.Children;
        gridChildren.Add(viewPop);
        viewPop.SetValue(Grid.ColumnProperty, 0);
        viewPop.SetValue(Control.HeightProperty, 400);

        gridChildren.Add(vmSummary.View);
        vmSummary.View.SetValue(Grid.ColumnProperty, 1);

        vmSummary.DataBind(this.worldModel);
        vmPop.DataBind(this.worldModel);
    }
}
