namespace Lyt.World3.Model.CapitalSector;

using static MathUtilities;

/// <summary>
///     Capital sector. The initial code is defined p.253.
/// </summary>
public sealed class Capital: Sector
{
    #region Documentation 
    /*
        Attributes
        ----------
        ici : float, optional
            industrial capital initial [dollars]. The default is 2.1e11.
        sci : float, optional
            service capital initial [dollars]. The default is 1.44e11.
        iet : float, optional
            industrial equilibrium time [years]. The default is 4000.
        iopcd : float, optional
            industrial output per capita desired [dollars/person-year]. The
            default is 400.
        lfpf : float, optional
            labor force participation fraction []. The default is 0.75.
        lufdt : float, optional
            labor utilization fraction delay time [years]. The default is 2.
        icor1 : float, optional
            icor, value before time=pyear [years]. The default is 3.
        icor2 : float, optional
            icor, value after time=pyear [years]. The default is 3.
        scor1 : float, optional
            scor, value before time=pyear [years]. The default is 1.
        scor2 : float, optional
            scor, value after time=pyear [years]. The default is 1.
        alic1 : float, optional
            alic, value before time=pyear [years]. The default is 14.
        alic2 : float, optional
            alic, value after time=pyear [years]. The default is 14.
        alsc1 : float, optional
            alsc, value before time=pyear [years]. The default is 20.
        alsc2 : float, optional
            alsc, value after time=pyear [years]. The default is 20.
        fioac1 : float, optional
            fioac, value before time=pyear []. The default is 0.43.
        fioac2 : float, optional
            fioac, value after time=pyear []. The default is 0.43.

        **Industrial subsector**

        ic : numpy.ndarray
            industrial capital [dollars]. It is a state variable.
        io : numpy.ndarray
            industrial output [dollars/year].
        icdr : numpy.ndarray
            industrial capital depreciation rate [dollars/year].
        icir : numpy.ndarray
            industrial capital investment rate [dollars/year].
        icor : numpy.ndarray
            industrial capital-output ratio [years].
        iopc : numpy.ndarray
            industrial output per capita [dollars/person-year].
        alic : numpy.ndarray
            average lifetime of industrial capital [years].
        fioac : numpy.ndarray
            fraction of industrial output allocated to consumption [].
        fioacc : numpy.ndarray
            fioac constant [].
        fioacv : numpy.ndarray
            fioac variable [].
        fioai : numpy.ndarray
            fraction of industrial output allocated to industry [].

        **Service subsector**

        sc : numpy.ndarray
            service capital [dollars]. It is a state variable.
        so : numpy.ndarray
            service output [dollars/year].
        scdr : numpy.ndarray
            service capital depreciation rate [dollars/year].
        scir : numpy.ndarray
            service capital investment rate [dollars/year].
        scor : numpy.ndarray
            service capital-output ratio [years].
        sopc : numpy.ndarray
            service output per capita [dollars/person-year].
        alsc : numpy.ndarray
            average lifetime of service capital [years].
        isopc : numpy.ndarray
            indicated service output per capita [dollars/person-year].
        isopc1 : numpy.ndarray
            isopc, value before time=pyear [dollars/person-year].
        isopc2 : numpy.ndarray
            isopc, value after time=pyear [dollars/person-year].
        fioas : numpy.ndarray
            fraction of industrial output allocated to services [].
        fioas1 : numpy.ndarray
            fioas, value before time=pyear [].
        fioas2 : numpy.ndarray
            fioas, value after time=pyear [].

        **Job subsector**

        j : numpy.ndarray
            jobs [persons].
        jph : numpy.ndarray
            jobs per hectare [persons/hectare].
        jpicu : numpy.ndarray
            jobs per industrial capital unit [persons/dollar].
        jpscu : numpy.ndarray
            jobs per service capital unit [persons/dollar].
        lf : numpy.ndarray
            labor force [persons].
        cuf : numpy.ndarray
            capital utilization fraction [].
        luf : numpy.ndarray
            labor utilization fraction [].
        lufd : numpy.ndarray
            labor utilization fraction delayed [].
        pjas : numpy.ndarray
            potential jobs in agricultural sector [persons].
        pjis : numpy.ndarray
            potential jobs in industrial sector [persons].
        pjss : numpy.ndarray
            potential jobs in service sector [persons].

    */
    #endregion Documentation 

    public Capital(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
        => Sector.InitializeLists(this, this.N, double.NaN);

    // No delays in the Resource Sector 
    protected override void SetDelayFunctions() { }

    #region Constants, State and Rates 

    // Constants 

    //     Industrial subsector

    //     Services subsector

    //     Jobs subsector

    public List<double> Iopc { get; private set; } = [];

    public List<double> Sopc { get; private set; } = [];

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
