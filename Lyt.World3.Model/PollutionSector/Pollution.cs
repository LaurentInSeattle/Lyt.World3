namespace Lyt.World3.Model.PollutionSector;

public sealed class Pollution : Sector
{
    public Pollution(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }
}
