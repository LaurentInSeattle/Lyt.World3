namespace Lyt.World3.Model.ResourceSector;

using static MathUtilities;

/// <summary> Nonrenewable Resource sector. The initial code is defined p.405. </summary>
public sealed class Resource : Sector
{
    #region Documentation 
    /*
        nri : float, optional
            nonrenewable resources initial[resource units]. The default is 1e12.
        nruf1 : float, optional
            nruf value before time=pyear[]. The default is 1.
        nruf2 : float, optional
            nruf value after time=pyear[]. The default is 1.
        nr : numpy.ndarray
            nonrenewable resources[resource units]. It is a state variable.
        nrfr : numpy.ndarray
            nonrenewable resource fraction remaining[].
        nruf : numpy.ndarray
            nonrenewable resource usage factor[].
        nrur : numpy.ndarray
            nonrenewable resource usage rate[resource units / year].
        pcrum : numpy.ndarray
            per capita resource usage multiplier[resource units / person - year].
        fcaor : numpy.ndarray
            fraction of capital allocated to obtaining resources[].
        fcaor1 : numpy.ndarray
            fcaor value before time=pyear[].
        fcaor2 : numpy.ndarray
            fcaor value after time=pyear[].
    */
    #endregion Documentation 

    public Resource(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) 
        : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose) 
        => Sector.InitializeLists(this, this.N, double.NaN);

    #region Constants, State and Rates 

    // Constants 

    // nonrenewable resources initial[resource units]. The default is 1e12.
    public double Nri { get; private set; }

    //  nruf value before time=pyear[]. The default is 1.
    public double Nruf1 { get; private set; }

    // nruf value after time=pyear[]. The default is 1.
    public double Nruf2 { get; private set; }

    // State 
    // nonrenewable resources[resource units]. It is a state variable.
    public List<double> Nr { get; private set; } = [];

    // Rates 
    // nonrenewable resource fraction remaining[].
    public List<double> Nrfr { get; private set; } = [];

    // nonrenewable resource usage factor[].
    public List<double> Nruf { get; private set; } = [];

    // nonrenewable resource usage rate[resource units / year].
    public List<double> Nrur { get; private set; } = [];

    // per capita resource usage multiplier[resource units / person - year].
    public List<double> Pcrum { get; private set; } = [];

    // fraction of capital allocated to obtaining resources[].
    public List<double> Fcaor { get; private set; } = [];

    // fcaor value before time=pyear[].
    public List<double> Fcaor1 { get; private set; } = [];

    // fcaor value after time=pyear[].
    public List<double> Fcaor2 { get; private set; } = [];

    #endregion Constants, State and Rates 

    // No delays in the Resource Sector 
    protected override void SetDelayFunctions() { }

    public void InitializeConstants(double nri = 1e12, double nruf1 = 1, double nruf2 = 1)
    {
        this.Nri = nri;
        this.Nruf1 = nruf1;
        this.Nruf2 = nruf2;
    }

    // Initialize the resource sector ( == initial loop with k=0).
    public void Initialize()
    {
        try
        {
            this.Nr[0] = this.Nri;
            this.UpdateNrfr(0);
            this.UpdateFcaor(0);
            this.UpdateNruf(0);
            this.UpdatePcrum(0);
            this.UpdateNrur(0, 0);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // Update one loop of the resource sector.
    public void Update(int k, int j, int jk, int kl)
    {
        try
        {
            this.UpdateNr(k, j, jk);
            this.UpdateNrfr(k);
            this.UpdateFcaor(k);
            this.UpdateNruf(k);
            this.UpdatePcrum(k);
            this.UpdateNrur(k, kl);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // State variable, requires previous step only
    private void UpdateNr(int k, int j, int jk)
        => this.Nr[k] = this.Nr[j] - this.Dt * this.Nrur[jk];

    // From step k requires: NR
    private void UpdateNrfr(int k)
        => this.Nrfr[k] = this.Nr[k] / this.Nri;

    // From step k requires: NRFR
    private void UpdateFcaor(int k)
    {
        this.Fcaor1[k] = (nameof(this.Fcaor1)).Interpolate(this.Nrfr[k]);
        this.Fcaor2[k] = (nameof(this.Fcaor2)).Interpolate(this.Nrfr[k]);
        this.Fcaor[k] =
            Clip(this.Fcaor2[k], this.Fcaor1[k], this.Time[k], this.PolicyYear);

    }

    // From step k requires: nothing
    private void UpdateNruf(int k)
        => this.Nruf[k] = Clip(this.Nruf2, this.Nruf1, this.Time[k], this.PolicyYear);

    // From step k requires: IOPC
    private void UpdatePcrum(int k)
        => this.Pcrum[k] = (nameof(this.Pcrum)).Interpolate(this.Capital.Iopc[k]);

    // From step k requires: POP PCRUM NRUF
    private void UpdateNrur(int k, int kl)
        => this.Nrur[kl] = this.Population.Pop[k] * this.Pcrum[k] * this.Nruf[k];
}
