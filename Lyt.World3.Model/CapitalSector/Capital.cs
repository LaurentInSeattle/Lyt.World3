namespace Lyt.World3.Model.CapitalSector;

public sealed class Capital: Sector
{
    public Capital(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
