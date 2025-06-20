namespace Lyt.World3.Model.AgricultureSector;

using static MathUtilities;

public sealed class Agriculture : Sector
{
    #region Documentation 
    #endregion Documentation 

    public Agriculture(World world) : base(world)
        => Sector.InitializeLists(this, this.N, double.NaN);

    #region Constants, State and Rates 

    public List<double> Al { get; private set; } = [];

    public List<double> Aiph { get; private set; } = [];

    public List<double> Sopc { get; private set; } = [];

    public List<double> Fioaa { get; private set; } = [];

    public double Sfpc { get; private set; } = 1.0;

    public List<double> Fpc { get; private set; } = [];

    #endregion Constants, State and Rates 

    // No delays in the Resource Sector 
    protected override void SetDelayFunctions() { }

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
