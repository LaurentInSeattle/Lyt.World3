namespace Lyt.World3.Model.CapitalSector;

/// <summary> Capital sector. The initial code is defined p.253.  </summary>
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
    //
    //   industrial capital initial[dollars]. The default is 2.1e11.
    public double Ici { get ; private set ; }

    //       service capital initial[dollars]. The default is 1.44e11.
    public double Sci { get ; private set ; }

    //       industrial equilibrium time[years]. The default is 4000.
    public double Iet { get ; private set ; }

    //        industrial output per capita desired[dollars / person - year]. The default is 400.
    public double Iopcd { get ; private set ; }

    //        labor force participation fraction[]. The default is 0.75.
    public double Lfpf { get ; private set ; }

    //        labor utilization fraction delay time[years]. The default is 2.
    public double Lufdt { get ; private set ; }

    //        icor1, value before time = pyear[years]. The default is 3.
    public double Icor1 { get ; private set ; }

    //        icor2, value after time = pyear[years]. The default is 3.
    public double Icor2 { get ; private set ; }

    //        scor1, value before time = pyear[years].The default is 1.
    public double Scor1 { get ; private set ; }

    //        scor2, value after time = pyear[years].The default is 1.
    public double Scor2 { get ; private set ; }

    //        alic1, value before time = pyear[years].The default is 14.
    public double Alic1 { get ; private set ; }

    //        alic2, value after time = pyear[years].The default is 14.
    public double Alic2 { get ; private set ; }

    //        alsc, value before time = pyear[years].The default is 20.
    public double Alsc1 { get ; private set ; }

    //        alsc, value after time = pyear[years].The default is 20.
    public double Alsc2 { get ; private set ; }

    //         fioac, value before time = pyear[].The default is 0.43.
    public double Fioac1 { get ; private set ; }

    //         fioac, value after time = pyear[].The default is 0.43.
    public double Fioac2 { get ; private set ; }

    //     Industrial subsector
    //
    // industrial capital[dollars]. It is a state variable.
    public List<double> Ic { get ; private set; } = [];

    // industrial output[dollars / year].
    public List<double> Io { get ; private set; } = [];

    // industrial capital depreciation rate[dollars / year].
    public List<double> Icdr { get ; private set; } = [];

    // industrial capital investment rate[dollars / year].
    public List<double> Icir { get ; private set; } = [];

    //  industrial capital-output ratio[years].
    public List<double> Icor { get ; private set; } = [];

    //  industrial output per capita[dollars / person - year].
    public List<double> Iopc { get ; private set; } = [];

    //  average lifetime of industrial capital[years].
    public List<double> Alic { get ; private set; } = [];

    //  fraction of industrial output allocated to consumption[].
    public List<double> Fioac { get ; private set; } = [];

    //  fioac constant[].
    public List<double> Fioacc { get ; private set; } = [];

    //  fioac variable[].
    public List<double> Fioacv { get ; private set; } = [];

    //  fraction of industrial output allocated to industry[].
    public List<double> Fioai { get ; private set; } = [];

    //     Services subsector
    //
    //  service capital[dollars]. It is a state variable.
    public List<double> Sc { get ; private set; } = [];

    //         service output[dollars / year].
    public List<double> So { get ; private set; } = []; 

    //         service capital depreciation rate[dollars / year].
    public List<double> Scdr { get ; private set; } = [];

    //         service capital investment rate[dollars / year].
    public List<double> Scir { get ; private set; } = [];

    //         service capital-output ratio[years].
    public List<double> Scor { get ; private set; } = [];

    //         service output per capita[dollars / person - year].
    public List<double> Sopc { get ; private set; } = [];

    //         average lifetime of service capital[years].
    public List<double> Alsc { get ; private set; } = [];

    //         indicated service output per capita[dollars / person - year].
    public List<double> Isopc { get ; private set; } = [];

    //        isopc, value before time = pyear[dollars / person - year].
    public List<double> Isopc1 { get ; private set; } = [];

    //         isopc, value after time = pyear[dollars / person - year].
    public List<double> Isopc2 { get ; private set; } = [];

    //        fraction of industrial output allocated to services[].    
    public List<double> Fioas { get ; private set; } = [];

    //         fioas, value before time = pyear[].
    public List<double> Fioas1 { get ; private set; } = [];

    //        fioas, value after time = pyear[].
    public List<double> Fioas2 { get ; private set; } = [];


    //     Jobs subsector

    //         jobs[persons].
    public List<double> J { get ; private set; } = [];

    //         jobs per hectare[persons / hectare].
    public List<double> Jph { get ; private set; } = [];

    //         jobs per industrial capital unit[persons / dollar].
    public List<double> Jpicu { get ; private set; } = [];

    //         jobs per service capital unit[persons / dollar].
    public List<double> Jpscu { get ; private set; } = [];

    //         labor force[persons].
    public List<double> Lf { get ; private set; } = [];

    //         capital utilization fraction[].
    public List<double> Cuf { get ; private set; } = [];

    //         labor utilization fraction[].
    public List<double> Luf { get ; private set; } = [];

    //         labor utilization fraction delayed[].
    public List<double> Lufd { get ; private set; } = [];

    //         potential jobs in agricultural sector[persons].
    public List<double> Pjas { get ; private set; } = [];

    //         potential jobs in industrial sector[persons].
    public List<double> Pjis { get ; private set; } = [];

    //         potential jobs in service sector[persons].
    public List<double> Pjss { get ; private set; } = []; 

    #endregion Constants, State and Rates 

    public void InitializeConstants(
        double ici= 2.1e11, 
        double sci= 1.44e11, 
        double iet= 4000,
        double iopcd= 400, 
        double lfpf= 0.75, 
        double lufdt= 2, 
        double icor1= 3, 
        double icor2= 3,
        double scor1= 1, 
        double scor2= 1, 
        double alic1= 14, 
        double alic2= 14,
        double alsc1= 20, 
        double alsc2= 20, 
        double fioac1= 0.43, 
        double fioac2= 0.43)
    {
        this.Ici = ici;
        this.Sci = sci         ;
        this.Iet = iet         ;
        this.Iopcd = iopcd     ;
        this.Lfpf = lfpf       ;
        this.Lufdt = lufdt     ;
        this.Icor1 = icor1     ;
        this.Icor2 = icor2     ;
        this.Scor1 = scor1     ;
        this.Scor2 = scor2     ;
        this.Alic1 = alic1     ;
        this.Alic2 = alic2     ;
        this.Alsc1 = alsc1     ;
        this.Alsc2 = alsc2     ;
        this.Fioac1 = fioac1   ;
        this.Fioac2 = fioac2   ;
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
