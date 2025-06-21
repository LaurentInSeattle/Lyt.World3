namespace Lyt.World3.Model.CapitalSector;

/// <summary> Capital sector. The initial code is defined p.253.  </summary>
public sealed class Capital : Sector
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

    public Capital(World world) : base(world)
        => Sector.InitializeLists(this, this.N, double.NaN);

    // delays in the Capital Sector : LUF, IOPC
    public override void SetDelayFunctions()
    {
        base.CreateSmooth(new Named(this.Luf));
        base.CreateSmooth(new Named(this.Iopc));
        base.CreateDelayInfThree(new Named(this.Iopc));
    }

    #region Constants, State and Rates 

    // Constants 
    //
    //   industrial capital initial[dollars]. The default is 2.1e11.
    public double Ici { get; private set; }

    //       service capital initial[dollars]. The default is 1.44e11.
    public double Sci { get; private set; }

    //       industrial equilibrium time[years]. The default is 4000.
    public double Iet { get; private set; }

    //        industrial output per capita desired[dollars / person - year]. The default is 400.
    public double Iopcd { get; private set; }

    //        labor force participation fraction[]. The default is 0.75.
    public double Lfpf { get; private set; }

    //        labor utilization fraction delay time[years]. The default is 2.
    public double Lufdt { get; private set; }

    //        icor1, value before time = pyear[years]. The default is 3.
    public double Icor1 { get; private set; }

    //        icor2, value after time = pyear[years]. The default is 3.
    public double Icor2 { get; private set; }

    //        scor1, value before time = pyear[years].The default is 1.
    public double Scor1 { get; private set; }

    //        scor2, value after time = pyear[years].The default is 1.
    public double Scor2 { get; private set; }

    //        alic1, value before time = pyear[years].The default is 14.
    public double Alic1 { get; private set; }

    //        alic2, value after time = pyear[years].The default is 14.
    public double Alic2 { get; private set; }

    //        alsc, value before time = pyear[years].The default is 20.
    public double Alsc1 { get; private set; }

    //        alsc, value after time = pyear[years].The default is 20.
    public double Alsc2 { get; private set; }

    //         fioac, value before time = pyear[].The default is 0.43.
    public double Fioac1 { get; private set; }

    //         fioac, value after time = pyear[].The default is 0.43.
    public double Fioac2 { get; private set; }

    //     Industrial subsector
    //
    // industrial capital[dollars]. It is a state variable.
    public List<double> Ic { get; private set; } = [];

    // industrial output[dollars / year].
    public List<double> Io { get; private set; } = [];

    // industrial capital depreciation rate[dollars / year].
    public List<double> Icdr { get; private set; } = [];

    // industrial capital investment rate[dollars / year].
    public List<double> Icir { get; private set; } = [];

    //  industrial capital-output ratio[years].
    public List<double> Icor { get; private set; } = [];

    //  industrial output per capita[dollars / person - year].
    public List<double> Iopc { get; private set; } = [];

    //  average lifetime of industrial capital[years].
    public List<double> Alic { get; private set; } = [];

    //  fraction of industrial output allocated to consumption[].
    public List<double> Fioac { get; private set; } = [];

    //  fioac constant[].
    public List<double> Fioacc { get; private set; } = [];

    //  fioac variable[].
    public List<double> Fioacv { get; private set; } = [];

    //  fraction of industrial output allocated to industry[].
    public List<double> Fioai { get; private set; } = [];

    //     Services subsector
    //
    //  service capital[dollars]. It is a state variable.
    public List<double> Sc { get; private set; } = [];

    //         service output[dollars / year].
    public List<double> So { get; private set; } = [];

    //         service capital depreciation rate[dollars / year].
    public List<double> Scdr { get; private set; } = [];

    //         service capital investment rate[dollars / year].
    public List<double> Scir { get; private set; } = [];

    //         service capital-output ratio[years].
    public List<double> Scor { get; private set; } = [];

    //         service output per capita[dollars / person - year].
    public List<double> Sopc { get; private set; } = [];

    //         average lifetime of service capital[years].
    public List<double> Alsc { get; private set; } = [];

    //         indicated service output per capita[dollars / person - year].
    public List<double> Isopc { get; private set; } = [];

    //        isopc, value before time = pyear[dollars / person - year].
    public List<double> Isopc1 { get; private set; } = [];

    //         isopc, value after time = pyear[dollars / person - year].
    public List<double> Isopc2 { get; private set; } = [];

    //        fraction of industrial output allocated to services[].    
    public List<double> Fioas { get; private set; } = [];

    //         fioas, value before time = pyear[].
    public List<double> Fioas1 { get; private set; } = [];

    //        fioas, value after time = pyear[].
    public List<double> Fioas2 { get; private set; } = [];


    //     Jobs subsector

    //         jobs[persons].
    public List<double> J { get; private set; } = [];

    //         jobs per hectare[persons / hectare].
    public List<double> Jph { get; private set; } = [];

    //         jobs per industrial capital unit[persons / dollar].
    public List<double> Jpicu { get; private set; } = [];

    //         jobs per service capital unit[persons / dollar].
    public List<double> Jpscu { get; private set; } = [];

    //         labor force[persons].
    public List<double> Lf { get; private set; } = [];

    //         capital utilization fraction[].
    public List<double> Cuf { get; private set; } = [];

    //         labor utilization fraction[].
    public List<double> Luf { get; private set; } = [];

    //         labor utilization fraction delayed[].
    public List<double> Lufd { get; private set; } = [];

    //         potential jobs in agricultural sector[persons].
    public List<double> Pjas { get; private set; } = [];

    //         potential jobs in industrial sector[persons].
    public List<double> Pjis { get; private set; } = [];

    //         potential jobs in service sector[persons].
    public List<double> Pjss { get; private set; } = [];

    #endregion Constants, State and Rates 

    public void InitializeConstants(
        double ici = 2.1e11,
        double sci = 1.44e11,
        double iet = 4000,
        double iopcd = 400,
        double lfpf = 0.75,
        double lufdt = 2,
        double icor1 = 3,
        double icor2 = 3,
        double scor1 = 1,
        double scor2 = 1,
        double alic1 = 14,
        double alic2 = 14,
        double alsc1 = 20,
        double alsc2 = 20,
        double fioac1 = 0.43,
        double fioac2 = 0.43)
    {
        this.Ici = ici;
        this.Sci = sci;
        this.Iet = iet;
        this.Iopcd = iopcd;
        this.Lfpf = lfpf;
        this.Lufdt = lufdt;
        this.Icor1 = icor1;
        this.Icor2 = icor2;
        this.Scor1 = scor1;
        this.Scor2 = scor2;
        this.Alic1 = alic1;
        this.Alic2 = alic2;
        this.Alsc1 = alsc1;
        this.Alsc2 = alsc2;
        this.Fioac1 = fioac1;
        this.Fioac2 = fioac2;
    }

    // Initialize the Capital sector ( == initial loop with k=0).
    public override void Initialize()
    {
        try
        {
            //  Set initial conditions

            //// industrial subsector
            //this.UpdateAlic(0);
            //this.UpdateIcdr(0);
            //this.UpdateIcor(0);
            //this.UpdateIo(0);
            //this.UpdateIopc(0);
            //this.UpdateFioac(0);
            //;
            //// service subsector 
            //this.UpdateIsopc(0);
            //this.UpdateAlsc(0);
            //this.UpdateScdr(0);
            //this.UpdateScor(0);
            //this.UpdateSo(0);
            //this.UpdateSopc(0);
            //this.UpdateFioas(0);
            //this.UpdateScir(0);

            //// back to industrial sector 
            //this.UpdateFioai(0);
            //this.UpdateIcir(0);

            //// job subsector     
            //this.UpdateJpicu(0);
            //this.UpdatePjis(0);
            //this.UpdateJpscu(0);
            //this.UpdatePjss(0);
            //this.UpdateJph(0);
            //this.UpdatePjas(0);
            //this.UpdateJ(0);
            //this.UpdateLf(0);
            //this.UpdateLuf(0);
            //this.UpdateLufd(0);

            //// recompute supplementary initial conditions
            //this.UpdateCuf(0);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // Update one loop of the Capital sector.
    public override void Update(int k)
    {
        //int jk = k - 1; 
        //int kl = k; 
        int j = k - 1;
        if ( j < 0 )
        {
            j = 0;
        }

        try
        {
            // job subsector                 
            this.UpdateLufd(k);
            this.UpdateCuf(k);

            // industrial subsector              
            this.UpdateIc(k, j);
            this.UpdateAlic(k);
            this.UpdateIcdr(k);
            this.UpdateIcor(k);
            this.UpdateIo(k);
            this.UpdateIopc(k);
            this.UpdateFioac(k);

            // service subsector         
            this.UpdateSc(k, j);
            this.UpdateIsopc(k);
            this.UpdateAlsc(k);
            this.UpdateScdr(k);
            this.UpdateScor(k);
            this.UpdateSo(k);
            this.UpdateSopc(k);
            this.UpdateFioas(k);
            this.UpdateScir(k);

            // back to industrial sector 
            this.UpdateFioai(k);
            this.UpdateIcir(k);

            // back to job subsector             
            this.UpdateJpicu(k);
            this.UpdatePjis(k);
            this.UpdateJpscu(k);
            this.UpdatePjss(k);
            this.UpdateJph(k);
            this.UpdatePjas(k);
            this.UpdateJ(k);
            this.UpdateLf(k);
            this.UpdateLuf(k);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // job subsector
    //
    // From step k=0 requires: LUF, else nothing
    // [DependsOn("LUF")]
    private void UpdateLufd(int k)
        => this.Lufd[k] = this.Smooth(nameof(this.Luf), k, this.Lufdt);

    // From step k requires: LUFD
    [DependsOn("LUFD")]
    private void UpdateCuf(int k)
        => this.Cuf[k] =
                k == 0 ? 
                    1.0 : 
                    nameof(this.Cuf).Interpolate(this.Lufd[k]);

    // industrial subsector              
    //
    // State variable, requires previous step only
    private void UpdateIc(int k, int j)
    {
        this.Ic[k] =
            k == 0 ?
                this.Ici :
                this.Ic[j] + this.Dt * (this.Icir[j] - this.Icdr[j]);
    }

    // From step k requires: nothing
    private void UpdateAlic(int k)
        => this.Alic[k] = this.ClipPolicyYear(this.Alic2, this.Alic1, k);

    // From step k requires: IC ALIC
    [DependsOn("IC"), DependsOn("ALIC")]
    private void UpdateIcdr(int k)
        => this.Icdr[k] = this.Ic[k] / this.Alic[k];

    // From step k requires: nothing
    private void UpdateIcor(int k)
        => this.Icor[k] = this.ClipPolicyYear(this.Icor2, this.Icor1, k);

    // From step k requires: IC FCAOR CUF ICOR
    [DependsOn("IC"), DependsOn("FCAOR"), DependsOn("CUF"), DependsOn("ICOR")]
    private void UpdateIo(int k)
        => this.Io[k]
            = (this.Ic[k] * (1 - this.Resource.Fcaor[k]) * this.Cuf[k] / this.Icor[k]);

    // From step k requires: IO POP
    [DependsOn("IO"), DependsOn("POP")]
    private void UpdateIopc(int k)
        => this.Iopc[k] = this.Io[k] / this.Population.Pop[k];

    // From step k requires: IOPC
    [DependsOn("IOPC")]
    private void UpdateFioac(int k)
    {
        this.Fioacv[k] = nameof(this.Fioacv).Interpolate(this.Iopc[k] / this.Iopcd);
        this.Fioacc[k] = this.ClipPolicyYear(this.Fioac2, this.Fioac1, k);
        this.Fioac[k] =
            MathUtilities.Clip(this.Fioacv[k], this.Fioacc[k], this.Time[k], this.Iet);
    }

    // service subsector         
    //
    // State variable, requires previous step only
    private void UpdateSc(int k, int j)
    {
        this.Sc[k] =
            k == 0 ?
                this.Sci :
                this.Sc[j] + this.Dt * (this.Scir[j] - this.Scdr[j]);
    }

    // From step k requires: IOPC
    [DependsOn("IOPC")]
    private void UpdateIsopc(int k)
    {
        this.Isopc1[k] = nameof(this.Isopc1).Interpolate(this.Iopc[k]);
        this.Isopc2[k] = nameof(this.Isopc2).Interpolate(this.Iopc[k]);
        this.Isopc[k] = this.ClipPolicyYear(this.Isopc2[k], this.Isopc1[k], k);
    }

    // From step k requires: nothing
    private void UpdateAlsc(int k)
        => this.Alsc[k] = this.ClipPolicyYear(this.Alsc2, this.Alsc1, k);

    // From step k requires: SC ALSC
    [DependsOn("SC"), DependsOn("ALSC")]
    private void UpdateScdr(int k)
        => this.Scdr[k] = this.Sc[k] / this.Alsc[k];

    // From step k requires: nothing
    private void UpdateScor(int k)
        => this.Scor[k] = this.ClipPolicyYear(this.Scor2, this.Scor1, k);

    // From step k requires: SC CUF SCOR
    [DependsOn("SC"), DependsOn("CUF"), DependsOn("SCOR")]
    private void UpdateSo(int k)
        => this.So[k] = this.Sc[k] * this.Cuf[k] / this.Scor[k];

    // From step k requires: SO POP
    [DependsOn("SO"), DependsOn("POP")]
    private void UpdateSopc(int k)
        => this.Sopc[k] = this.So[k] / this.Population.Pop[k];

    // From step k requires: SOPC ISOPC
    [DependsOn("SOPC"), DependsOn("ISOPC")]
    private void UpdateFioas(int k)
    {
        this.Fioas1[k] = nameof(this.Fioas1).Interpolate(this.Sopc[k] / this.Isopc[k]);
        this.Fioas2[k] = nameof(this.Fioas2).Interpolate(this.Sopc[k] / this.Isopc[k]);
        this.Fioas[k] = this.ClipPolicyYear(this.Fioas2[k], this.Fioas1[k], k);
    }

    // From step k requires: IO FIOAS
    [DependsOn("IO"), DependsOn("FIOAS")]
    private void UpdateScir(int k)
        => this.Scir[k] = this.Io[k] * this.Fioas[k];

    // back to industrial sector 
    //
    // From step k requires: FIOAA FIOAS FIOAC
    [DependsOn("FIOAA"), DependsOn("FIOAS"), DependsOn("FIOAC")]
    private void UpdateFioai(int k)
        => this.Fioai[k] = (1 - this.Agriculture.Fioaa[k] - this.Fioas[k] - this.Fioac[k]);

    // From step k requires: IO FIOAI
    [DependsOn("IO"), DependsOn("FIOAI")]
    private void UpdateIcir(int k)
        => this.Icir[k] = this.Io[k] * this.Fioai[k];

    // back to job subsector             
    //
    // From step k requires: IOPC
    [DependsOn("IOPC")]
    private void UpdateJpicu(int k) 
        => this.Jpicu[k] = nameof(this.Jpicu).Interpolate(this.Iopc[k]);

    // From step k requires: IC JPICU
    [DependsOn("IC"), DependsOn("JPICU")]
    private void UpdatePjis(int k) 
        => this.Pjis[k] = this.Ic[k] * this.Jpicu[k];

    // From step k requires: SOPC
    [DependsOn("SOPC")]
    private void UpdateJpscu(int k) 
        => this.Jpscu[k] = nameof(this.Jpscu).Interpolate(this.Sopc[k]);

    // From step k requires: SC JPSCU
    [DependsOn("SC"), DependsOn("JPSCU")]
    private void UpdatePjss(int k) 
        => this.Pjss[k] = this.Sc[k] * this.Jpscu[k];

    // From step k requires: AIPH
    [DependsOn("SOPC")]
    private void UpdateJph(int k) 
        => this.Jph[k] = nameof(this.Jph).Interpolate(this.Agriculture.Aiph[k]);

    // From step k requires: JPH AL
    [DependsOn("JPH"), DependsOn("AL")]
    private void UpdatePjas(int k) 
        => this.Pjas[k] = this.Jph[k] * this.Agriculture.Al[k];

    // From step k requires: PJIS PJAS PJSS
    [DependsOn("PJIS"), DependsOn("PJAS"), DependsOn("PJSS")]
    private void UpdateJ(int k) 
        => this.J[k] = this.Pjis[k] + this.Pjas[k] + this.Pjss[k];

    // From step k requires: P2 P3
    [DependsOn("P2"), DependsOn("P3")]
    private void UpdateLf(int k) 
        => this.Lf[k] = (this.Population.P2[k] + this.Population.P3[k]) * this.Lfpf;

    // From step k requires: J LF
    [DependsOn("J"), DependsOn("LF")]
    private void UpdateLuf(int k) 
        => this.Luf[k] = this.J[k] / this.Lf[k];
}
