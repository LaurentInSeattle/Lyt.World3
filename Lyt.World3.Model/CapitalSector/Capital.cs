namespace Lyt.World3.Model.CapitalSector;

public sealed class Capital: Sector
{
    public Capital(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }

    public List<double> Iopc { get; private set; } = [];

    public List<double> Sopc { get; private set; } = [];
}
