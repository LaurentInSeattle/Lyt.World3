namespace Lyt.World3.Model.PollutionSector;

using static MathUtilities;

public sealed class Pollution : Sector
{
    #region Documentation 
    #endregion Documentation 

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

    #region Constants, State and Rates 

    public List<double> Ppolx { get; private set; } = [];

    #endregion Constants, State and Rates 

    public void InitializeConstants()
    {
    }

    // Initialize the Capital sector ( == initial loop with k=0).
    public override void Initialize()
    {
        try
        {
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // Update one loop of the Capital sector.
    public override void Update(int k, int j, int jk, int kl)
    {
        try
        {
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

}
