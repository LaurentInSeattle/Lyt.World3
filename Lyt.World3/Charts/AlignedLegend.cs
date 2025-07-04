namespace Lyt.World3.Charts;

using LiveChartsCore.Measure;

public sealed class AlignedLegend : SKDefaultLegend
{
    // From: SKDefaultLegend
    //private bool _isInitialized;
    //private DrawnTask? _drawnTask;
    private readonly FieldInfo isInitialized;
    private readonly FieldInfo drawnTask;

    // From: Chart 
    //internal float _titleHeight = 0f;
    //internal LvcSize _legendSize;
    private readonly FieldInfo titleHeight;
    private readonly FieldInfo legendSize;

    public AlignedLegend()
    {
        // The ugly HACK of the day ! 
        Type legend = typeof(SKDefaultLegend);
        MemberInfo[] members = legend.GetMember("_isInitialized", BindingFlags.Instance | BindingFlags.NonPublic) ;
        isInitialized = (FieldInfo) members[0];
        members = legend.GetMember("_drawnTask", BindingFlags.Instance | BindingFlags.NonPublic);
        drawnTask = (FieldInfo)members[0];
        Type chart = typeof(Chart);
        members = chart.GetMember("_titleHeight", BindingFlags.Instance | BindingFlags.NonPublic);
        titleHeight = (FieldInfo )members[0];
        members = chart.GetMember("_legendSize", BindingFlags.Instance | BindingFlags.NonPublic);
        legendSize = (FieldInfo)members[0];
    }

    public override void Draw(Chart chart)
    {
        bool _isInitialized = (bool) isInitialized.GetValue(this)!; 
        if (!_isInitialized)
        {
            base.Initialize(chart);
            // _isInitialized = true;
            isInitialized.SetValue(this, true);
        }

        var _drawnTask = (DrawnTask)drawnTask.GetValue(this)!;
        if (_drawnTask is null || _drawnTask.IsEmpty)
        {
            _drawnTask = chart.Canvas.AddGeometry(this);
            _drawnTask.ZIndex = 10099;
        }

        var actualChartSize = chart.ControlSize;
        var _legendSize = (LvcSize) legendSize.GetValue(chart)!;

        // This ONLY aligns to the right
        // TODO: Add enum to CTOR to better adjust (left, right, middle) for horizontal
        // TODO: Add enum to CTOR to better adjust (top, bottom, middle) for vertical
        this.X = actualChartSize.Width - _legendSize.Width;
        
        float _titleHeight = (float)titleHeight.GetValue(chart)!;
        this.Y = _titleHeight;

        if (chart.LegendPosition == LegendPosition.Hidden && _drawnTask is not null)
        {
            chart.Canvas.RemovePaintTask(_drawnTask);
            // _drawnTask = null;
            drawnTask.SetValue(this, null);
        }
    }
}
