namespace Lyt.World3.Model.AgricultureSector;

public sealed class Agriculture : Sector
{
    public Agriculture(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
