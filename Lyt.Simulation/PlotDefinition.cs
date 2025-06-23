namespace Lyt.Simulation;

public enum PlotKind
{
    Absolute,
    Normalized,
}

public class PlotDefinition(string name, PlotKind kind, List<string> equations)
{
    public string Name { get; private set; } = name;

    public PlotKind Kind { get; private set; } = kind;

    public List<string> Equations { get; private set; } = equations;
}
