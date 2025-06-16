namespace Lyt.World3.Model.PopulationSector;

public sealed class Population : Sector
{
    public Population(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
