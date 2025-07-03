namespace Lyt.World3.Workflow.Results;

public sealed partial class ThumbnailViewModel : ViewModel<ThumbnailView>
{
    public const double LargeFontSize = 24.0;
    public const double SmallFontSize = 16.0;

    public const double LargeBorderHeight = 260;
    public const double SmallBorderHeight = 212;

    public const double LargeImageHeight = 200;
    public const double SmallImageHeight = 160;

    public const int LargeThumbnailWidth = 360;
    public const int SmallThumbnailWidth = 240;

    private readonly ISelectListener parent;
    private readonly MiniChartView miniChartView;
    private readonly ChartViewModel chartViewModel;
    private readonly bool isLarge;

    [ObservableProperty]
    private double fontSize;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private object? miniChart;

    [ObservableProperty]
    private double borderHeight;

    [ObservableProperty]
    private double imageHeight;

    /// <summary> 
    /// </summary>
    public ThumbnailViewModel(
        ISelectListener parent, 
        MiniChartView miniChartView,
        bool isLarge = true )        
    {
        this.parent = parent;
        this.miniChartView = miniChartView;
        this.isLarge = isLarge;

        if (this.miniChartView.DataContext is ChartViewModel chartViewModel) 
        {
            this.chartViewModel = chartViewModel;
        }
        else
        {
            throw new ArgumentException("Chart is unbound"); 
        }
        
        this.BorderHeight = isLarge ? LargeBorderHeight : SmallBorderHeight;
        this.ImageHeight = isLarge ? LargeImageHeight : SmallImageHeight;
        this.FontSize = isLarge ? LargeFontSize : SmallFontSize;
        this.miniChart =
            // new Rectangle() { Fill = Brushes.DarkOrange }; 
            miniChartView;
        this.SetThumbnailTitle(); 
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    public ChartViewModel ChartViewModel => this.chartViewModel;

    // We need to reload the thumbnail view title, so that it will be properly localized
    private void OnLanguageChanged(LanguageChangedMessage message)
        => this.SetThumbnailTitle();

    internal void OnSelect() => this.parent.OnSelect(this);

    internal void ShowDeselected(/* PictureMetadata metadata */ )
    {
        //if (this.Metadata == metadata)
        //{
        //    return;
        //}

        if (this.IsBound)
        {
            this.View.Deselect();
        }
    }

    internal void ShowSelected()
    {
        if (this.IsBound)
        {
            this.View.Select();
        } 
    }

    private void SetThumbnailTitle() 
        => this.Title = this.chartViewModel.Title;
}
