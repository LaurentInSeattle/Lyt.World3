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
        //new PlotDefinition(
        //    "Summary",
        //    "Essential Summary",
        //    "- TODO -",
        //    [
        //        new ("population", "Population", HasAxis: true),
        //        new ("nonrenewableResourceFractionRemaining", "Resources Left", HasAxis: true, ScaleUsingAxisIndex:1),
        //        new ("foodPerCapita", "Food Per Capita", HasAxis: true, ScaleUsingAxisIndex:2),
        //        new ("industrialOutputPerCapita", "Industrial Output Per Capita", HasAxis: true, ScaleUsingAxisIndex:3),
        //        new ("indexOfPersistentPollution", "Persistent Pollution", HasAxis: true, ScaleUsingAxisIndex:4),
        //    ]),
        new PlotDefinition(
            "Population",
            "Population per Age Groups",
            "- TODO -",
            [
                new ("population", "All", HasAxis: true),
                new ("population0To14", "Age 0 to 14 years"),
                new ("population0To44", "Age 0 to 44 years"),
                new ("population0To64", "Age 0 to 64 years"),
                new ("lifeExpectancy", "Life Expectancy", HasAxis: true, UseIntegers: true, ScaleUsingAxisIndex:1),
            ]),
        //new PlotDefinition(
        //    "Agriculture",
        //    "Agriculture and Pollution",
        //    "- TODO -",
        //    [
        //        new ("foodPerCapita", "Food Per Capita", HasAxis: true, ScaleUsingAxisIndex:0),
        //        new ("arableLand", "Arable Land", HasAxis: true, ScaleUsingAxisIndex:1),
        //        new ("landFertility", "Land Fertility", HasAxis: true, ScaleUsingAxisIndex:2),
        //        new ("landYield", "Land Yield", HasAxis: true, ScaleUsingAxisIndex:3),                
        //        new ("indexOfPersistentPollution", "Persistent Pollution", HasAxis: true, ScaleUsingAxisIndex:4),
        //    ]),
            /*
        new PlotDefinition("Industry", PlotKind.Absolute,
            [
                "industrialOutput",
            ]),
        new PlotDefinition("Services", PlotKind.Absolute,
            [
                "serviceOutput",
            ]),
        new PlotDefinition("Agriculture", PlotKind.Absolute,
            [
                "food",
            ]),
        new PlotDefinition("Resources", PlotKind.Absolute,
            [
                "nonrenewableResources",
            ]),
        new PlotDefinition("Pollution", PlotKind.Absolute,
            [
                "persistentPollution",
            ]),
        new PlotDefinition("Arable Land", PlotKind.Absolute,
            [
                "arableLand",
            ]),
        new PlotDefinition("Life Expectancy", PlotKind.Absolute,
            [
                "lifeExpectancy",
            ]),

        new PlotDefinition("Food Per Capita", PlotKind.Absolute,
            [
                "foodPerCapita",
            ]),
            */
    ];

}
