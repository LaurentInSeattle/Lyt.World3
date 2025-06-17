namespace Lyt.World3.Model.ResourceSector;

public sealed class Resource: Sector
{
    public Resource(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
