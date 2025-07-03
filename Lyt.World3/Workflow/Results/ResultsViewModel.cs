namespace Lyt.World3.Workflow.Results;

public sealed partial class ResultsViewModel : ViewModel<ResultsView>
{
    private readonly World3Model world3Model;
    private readonly WorldModel worldModel;

    private readonly Dictionary<string, ChartViewModel> charts;
    private readonly Dictionary<string, ChartViewModel> miniCharts;

    [ObservableProperty]
    private ThumbnailsPanelViewModel thumbnailsPanelViewModel;

    [ObservableProperty]
    private View? selectedChart;

    public ResultsViewModel(World3Model world3Model)
    {
        this.world3Model = world3Model;
        this.worldModel = world3Model.WorldModel;

        this.charts = [];
        this.miniCharts = [];
        this.ThumbnailsPanelViewModel = new ThumbnailsPanelViewModel(this);
        this.CreateCharts(); 
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);
        Dispatch.OnUiThread(this.BindCharts);
    }

    private void BindCharts()
    {
        foreach (var plot in WorldModel.Plots)
        {
            string key = plot.Name;
            var vm = this.charts[key];
            vm.DataBind(this.worldModel);
            var miniVm = this.miniCharts[key];
            miniVm.DataBind(this.worldModel);
        }

        this.ThumbnailsPanelViewModel.LoadThumbnails(this.miniCharts.Values);
        this.SelectedChart = this.charts["Population"].View;
    }

    private void CreateCharts()
    {
        foreach (var plot in WorldModel.Plots)
        {
            string key = plot.Name;
            var vm = new ChartViewModel(WorldModel.GetPlotDefinitionByName(key));
            vm.CreateViewAndBind();
            var miniVm = new ChartViewModel(WorldModel.GetPlotDefinitionByName(key), isMini: true);
            var mini = new MiniChartView();
            miniVm.Bind(mini);
            this.charts.Add(key, vm);
            this.miniCharts.Add(key, miniVm);
        }
    }
}
