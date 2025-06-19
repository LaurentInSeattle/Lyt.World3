namespace Lyt.World3.Model.PollutionSector;

public sealed class Pollution : Sector
{
    public Pollution(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) 
        : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
        => Sector.InitializeLists(this, this.N, double.NaN);

    // No delays in the Resource Sector 
    protected override void SetDelayFunctions() { }

    public List<double> Ppolx { get; private set; } = [];
}
