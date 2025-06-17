namespace Lyt.World3.Model.AgricultureSector;

public sealed class Agriculture : Sector
{
    public Agriculture(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }

    public double Sfpc { get; private set; } = 1.0;

    public List<double> Fpc { get; private set; } = [];
}
