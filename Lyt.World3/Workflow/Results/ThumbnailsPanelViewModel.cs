namespace Lyt.World3.Workflow.Results;

public interface ISelectListener
{
    void OnSelect(object selectedObject);
}

public sealed partial class ThumbnailsPanelViewModel 
    : ViewModel<ThumbnailsPanelView>, ISelectListener
{
    private readonly ResultsViewModel resultsViewModel; 

    [ObservableProperty]
    private ObservableCollection<ThumbnailViewModel> thumbnails;

    [ObservableProperty]
    private List<string> providerNames;

    [ObservableProperty]
    private int providersSelectedIndex; 
        
    private ThumbnailViewModel? selectedThumbnail; 

    public ThumbnailsPanelViewModel(ResultsViewModel resultsViewModel)
    {
        this.resultsViewModel = resultsViewModel;

        this.Thumbnails = [];
        this.ProviderNames = [];
        //this.providers =
        //    [.. ( from provider in this.astroPicModel.Providers
        //      orderby provider.Name
        //      select provider )];
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
    }

    public ThumbnailViewModel? SelectedThumbnail => this.selectedThumbnail; 

    public void OnSelect(object selectedObject)
    {
        if (selectedObject is ThumbnailViewModel thumbnailViewModel)
        {
            this.selectedThumbnail = thumbnailViewModel; 
            //var pictureMetadata = thumbnailViewModel.Metadata;
            //if (this.selectedMetadata is null || this.selectedMetadata != pictureMetadata)
            //{
            //    this.selectedMetadata = pictureMetadata;
            //    this.collectionViewModel.Select(pictureMetadata, thumbnailViewModel.ImageBytes);
            //}

            this.UpdateSelection();
        }
    }

    internal void UpdateSelection()
    {
        //if (this.selectedMetadata is not null)
        //{
        //    foreach (ThumbnailViewModel thumbnailViewModel in this.Thumbnails)
        //    {
        //        if (thumbnailViewModel.Metadata == this.selectedMetadata)
        //        {
        //            thumbnailViewModel.ShowSelected();
        //        }
        //        else
        //        {
        //            thumbnailViewModel.ShowDeselected(this.selectedMetadata);
        //        }
        //    }
        //}
    }
}
