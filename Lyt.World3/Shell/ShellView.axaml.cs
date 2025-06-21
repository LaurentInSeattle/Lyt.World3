namespace Lyt.World3.Shell;

using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;

public partial class ShellView : UserControl, IView
{
    public ShellView()
    {
        this.InitializeComponent();
        this.Loaded += (s, e) =>
        {
            if (this.DataContext is not null && this.DataContext is ViewModel viewModel)
            {
                viewModel.OnViewLoaded();
            }

            double[] dataX = [1, 2, 3, 4, 5 , 6];
            double[] dataY = [1, 4, 9, 16, 25, 36];
            AvaPlot? avaPlot = this.Find<AvaPlot>("AvaPlot");
            if (avaPlot is not null)
            {
                var plot = avaPlot.Plot;
                plot.Add.Palette = new ScottPlot.Palettes.Penumbra(); 
                Scatter scatter = plot.Add.Scatter(dataX, dataY);
                scatter.LineWidth = 5;

                // change figure colors
                plot.FigureBackground.Color = Color.FromHex("#181818");
                plot.DataBackground.Color = Color.FromHex("#1f1f1f");

                // change axis and grid colors
                plot.Axes.Color(Color.FromHex("#d7d7d7"));
                plot.Grid.MajorLineColor = Color.FromHex("#404040");

                avaPlot.Refresh();
            } 
        };
    }
}
