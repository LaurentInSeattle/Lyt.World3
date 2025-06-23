namespace Lyt.Simulation.Equations; 

public sealed class Rate : Equation
{
    public Rate(Simulator model, string name, int number, string units = "dimensionless") 
        : base(model, name, number, units) 
        => model.OnNewRate(this);
}
