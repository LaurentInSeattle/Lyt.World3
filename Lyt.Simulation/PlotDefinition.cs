namespace Lyt.Simulation;

public sealed record class Curve(
    string EquationName,
    string Name, 
    bool HasAxis = false,
    bool UseIntegers = false, 
    double ScaleFactor = 1.0, 
    double LineSmoothness = 0.7, 
    int scaleUsingAxisIndex = 0); 

public sealed record class PlotDefinition(
    string Name, 
    string Title, 
    string Description, 
    List<Curve> Curves);
