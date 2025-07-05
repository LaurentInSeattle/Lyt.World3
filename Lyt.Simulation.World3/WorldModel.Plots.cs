namespace Lyt.Simulation.World3;

public sealed partial class WorldModel : Simulator
{
    public static PlotDefinition GetPlotDefinitionByName(string name)
    {
        var plotDefinition =
            (from p in Plots
             where p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
             select p).FirstOrDefault();
        return
            plotDefinition is null ?
                throw new ArgumentException("No such plot", nameof(name)) :
                plotDefinition;
    }

#pragma warning disable CA2211 
    // Non-constant fields should not be visible
    public static List<PlotDefinition> Plots =
#pragma warning restore CA2211 
    [
        new PlotDefinition(
            "Summary",
            "Summary",
            "- TODO -",
            [
                new ("population", "Population", HasAxis: true, CurveFormatter: CurveFormatter.Population),
                new ("nonrenewableResourceFractionRemaining", "Resources Left", 
                    HasAxis: true, ScaleUsingAxisIndex:1, CurveFormatter: CurveFormatter.Percentage),
                new ("foodPerCapita", "Food Per Capita", 
                    HasAxis: true, ScaleUsingAxisIndex:2, CurveFormatter: CurveFormatter.Integer),
                new ("industrialOutputPerCapita", "Industrial Output Per Capita", 
                    HasAxis: true, ScaleUsingAxisIndex:3, CurveFormatter: CurveFormatter.Integer),
                new ("indexOfPersistentPollution", "Persistent Pollution", 
                    HasAxis: true, ScaleUsingAxisIndex:4, CurveFormatter : CurveFormatter.FloatOne),
            ]),
        new PlotDefinition(
            "Population",
            "Population per Age Groups",
            "- TODO -",
            [
                new ("population", "All", HasAxis: true, CurveFormatter: CurveFormatter.Population),
                new ("population0To14", "Age 0 to 14 years", CurveFormatter: CurveFormatter.Population),
                new ("population0To44", "Age 0 to 44 years", CurveFormatter: CurveFormatter.Population),
                new ("population0To64", "Age 0 to 64 years", CurveFormatter: CurveFormatter.Population),
                new ("lifeExpectancy", "Life Expectancy", 
                    HasAxis: true, UseIntegers: true, LineSmoothness:0.0, ScaleUsingAxisIndex:1),
            ]),
        new PlotDefinition(
            "Agriculture",
            "Agriculture and Pollution",
            "- TODO -",
            [
                new ("foodPerCapita", "Food Per Capita", 
                    HasAxis: true, ScaleUsingAxisIndex:0, CurveFormatter : CurveFormatter.Integer),
                new ("arableLand", "Arable Land", 
                    HasAxis: true, ScaleUsingAxisIndex:1, CurveFormatter: CurveFormatter.Population),
                new ("landFertility", "Land Fertility", 
                    HasAxis: true, ScaleUsingAxisIndex:2, CurveFormatter : CurveFormatter.FloatOne),
                new ("landYield", "Land Yield", 
                    HasAxis: true, ScaleUsingAxisIndex:3, CurveFormatter : CurveFormatter.FloatOne),
                new ("indexOfPersistentPollution", "Persistent Pollution", 
                    HasAxis: true, ScaleUsingAxisIndex:4, CurveFormatter : CurveFormatter.FloatOne),
            ]),
        new PlotDefinition(
            "Capital",
            "Industry and Services",
            "- TODO -",
            [
                new ("industrialCapital", "Industrial Capital",
                    HasAxis: true, ScaleUsingAxisIndex:0, CurveFormatter : CurveFormatter.Population),
                new ("serviceCapital", "Service Capital",
                    HasAxis: false, ScaleUsingAxisIndex:0, CurveFormatter: CurveFormatter.Population),
                new ("jobs", "Jobs",
                    HasAxis: true, ScaleUsingAxisIndex:1, CurveFormatter : CurveFormatter.Population),
                new ("laborUtilizationFraction", "Employment",
                    HasAxis: true, ScaleUsingAxisIndex:2, CurveFormatter : CurveFormatter.Percentage),
                new ("nonrenewableResourceFractionRemaining", "Resources Left",
                    HasAxis: false, ScaleUsingAxisIndex:2, CurveFormatter: CurveFormatter.Percentage),
            ]),
        new PlotDefinition(
            "Land",
            "Land Utilization",
            "- TODO -",
            [
                new ("arableLand", "Arable Land",
                    HasAxis: true, ScaleUsingAxisIndex:0, CurveFormatter: CurveFormatter.Population),
                new ("urbanIndustrialLand", "Industrial & Urban Land",
                    HasAxis: false, ScaleUsingAxisIndex:0, CurveFormatter: CurveFormatter.Population),
                new ("landFractionCultivated", "Land Cultivated",
                    HasAxis: true, ScaleUsingAxisIndex:1, CurveFormatter: CurveFormatter.Percentage),
                new ("indexOfPersistentPollution", "Persistent Pollution",
                    HasAxis: true, ScaleUsingAxisIndex:2, CurveFormatter : CurveFormatter.FloatOne),
                new ("landFertility", "Land Fertility",
                    HasAxis: true, ScaleUsingAxisIndex:3, CurveFormatter: CurveFormatter.Population),
            ]),
            /*
            */
    ];

}
