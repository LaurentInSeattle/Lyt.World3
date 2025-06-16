namespace Lyt.World3.Model.ResourceSector;

public sealed class Resource: Sector
{
    public Resource(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
