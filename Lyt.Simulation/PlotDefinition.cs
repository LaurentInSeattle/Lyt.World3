namespace Lyt.Simulation;

public enum CurveFormatter
{
    None, 
    Integer,
    Percentage, 
    Population,
    FloatOne,
}

public sealed record class Curve(
    string EquationName,
    string Name,
    string AxisName = "",
    bool HasAxis = false,
    bool UseIntegers = false, 
    double ScaleFactor = 1.0, 
    double LineSmoothness = 0.7, 
    int ScaleUsingAxisIndex = 0,
    CurveFormatter CurveFormatter = CurveFormatter.None); 

public sealed record class PlotDefinition(
    string Name, 
    string Title, 
    string Description, 
    List<Curve> Curves);
