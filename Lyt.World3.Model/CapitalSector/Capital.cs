namespace Lyt.World3.Model.CapitalSector;

/// <summary>
///     Capital sector. The initial code is defined p.253.
/// </summary>
public sealed class Capital: Sector
{
    public Capital(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
        => Sector.InitializeLists(this, this.N, double.NaN);

    // No delays in the Resource Sector 
    protected override void SetDelayFunctions() { }

    public List<double> Iopc { get; private set; } = [];

    public List<double> Sopc { get; private set; } = [];
}
