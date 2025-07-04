namespace Lyt.World3.Workflow.Results;

public interface ISelectListener
{
    void OnSelect(object selectedObject);
}

public sealed partial class ThumbnailsPanelViewModel
    : ViewModel<ThumbnailsPanelView>, ISelectListener
{
    private readonly ResultsViewModel resultsViewModel;
    private string? selectedKey;

    [ObservableProperty]
    private ObservableCollection<ThumbnailViewModel> thumbnails;

    private ThumbnailViewModel? selectedThumbnail;

    public ThumbnailsPanelViewModel(ResultsViewModel resultsViewModel)
    {
        this.resultsViewModel = resultsViewModel;
        this.Thumbnails = [];
    }

    internal void LoadThumbnails(IEnumerable<ChartViewModel> miniCharts)
    {
        List<ThumbnailViewModel> allThumbnails = new(16);
        foreach (var miniChart in miniCharts)
        {
            if (miniChart.ViewBase is MiniChartView miniChartView)
            {
                allThumbnails.Add(
                    new ThumbnailViewModel(this, miniChartView, isLarge: false));
            }
        }

        this.Thumbnails = new(allThumbnails);
        this.OnSelect(this.Thumbnails[0]);

        // Update again and delay a bit so that the thumbnails views are bound to their VMs 
        Schedule.OnUiThread(50, this.UpdateSelection, DispatcherPriority.Background);
    }

    public ThumbnailViewModel? SelectedThumbnail => this.selectedThumbnail;

    public void OnSelect(object selectedObject)
    {
        if (selectedObject is ThumbnailViewModel thumbnailViewModel)
        {
            this.selectedThumbnail = thumbnailViewModel;
            string key = thumbnailViewModel.ChartViewModel.PlotDefinition.Name;
            this.selectedKey = key;
            this.resultsViewModel.Select(key);
            this.UpdateSelection();
        }
    }

    internal void UpdateSelection()
    {
        if (this.selectedKey is not null)
        {
            foreach (ThumbnailViewModel thumbnailViewModel in this.Thumbnails)
            {
                string key = thumbnailViewModel.ChartViewModel.PlotDefinition.Name;
                if (key == this.selectedKey)
                {
                    thumbnailViewModel.ShowSelected();
                }
                else
                {
                    thumbnailViewModel.ShowDeselected();
                }
            }
        }
    }
}
