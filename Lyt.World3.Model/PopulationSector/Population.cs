namespace Lyt.World3.Model.PopulationSector;

using static MathUtilities;

/// <summary>
/// Population sector with four age levels. Can be run independantly from other sectors with 
/// exogenous inputs. The initial code is defined p.170.
/// </summary>
public sealed class Population : Sector
{
    #region Documentation 
    /*
        Parameters
        ----------
        year_min : float, optional
            start year of the simulation [year]. The default is 0.
        year_max : float, optional
            end year of the simulation [year]. The default is 75.
        dt : float, optional
            time step of the simulation [year]. The default is 1.
        iphst : float, optional
            implementation date of new policy on health service time [year].
            The default is 1940.
        verbose : bool, optional
            print information for debugging. The default is False.

        Attributes
        ----------
        p1i : float, optional
            p2 initial [persons]. The default is 65e7.
        p2i : float, optional
            p2 initial [persons]. The default is 70e7.
        p3i : float, optional
            p3 initial [persons]. The default is 19e7.
        p4i : float, optional
            p4 initial [persons]. The default is 6e7.
        dcfsn : float, optional
            desired completed family size normal []. The default is 4.
        fcest : float, optional
            fertility control effectiveness set time [year]. The default is 4000.
        hsid : float, optional
            health services impact delay [years]. The default is 20.
        ieat : float, optional
            income expectation averaging time [years]. The default is 3.
        len : float, optional
            life expectancy normal [years]. The default is 28.
        lpd : float, optional
            lifetime perception delay [years]. The default is 20.
        mtfn : float, optional
            maximum total fertility normal []. The default is 12.
        pet : float, optional
            population equilibrium time [year]. The default is 4000.
        rlt : float, optional
            reproductive lifetime [years]. The default is 30.
        sad : float, optional
            social adjustment delay [years]. The default is 20.
        zpgt : float, optional
            time when desired family size equals 2 children [year]. The default is
            4000.

        **Population sector**

        p1 : numpy.ndarray
            population, ages 0-14 [persons]. It is a state variable.
        p2 : numpy.ndarray
            population, ages 15-44 [persons]. It is a state variable.
        p3 : numpy.ndarray
            population, ages 45-64 [persons]. It is a state variable.
        p4 : numpy.ndarray
            population, ages 65+ [persons]. It is a state variable.
        pop : numpy.ndarray
            population [persons].
        mat1 : numpy.ndarray
            maturation rate, age 14-15 [persons/year].
        mat2 : numpy.ndarray
            maturation rate, age 44-45 [persons/year].
        mat3 : numpy.ndarray
            maturation rate, age 64-65 [persons/year].

        **Death rate subsector**

        d : numpy.ndarray
            deaths per year [persons/year].
        d1 : numpy.ndarray
            deaths per year, ages 0-14 [persons/year].
        d2 : numpy.ndarray
            deaths per year, ages 15-44 [persons/year].
        d3 : numpy.ndarray
            deaths per year, ages 45-64 [persons/year].
        d4 : numpy.ndarray
            deaths per year, ages 65+ [persons/year].
        cdr : numpy.ndarray
            crude death rate [deaths/1000 person-years].
        ehspc : numpy.ndarray
            effective health services per capita [dollars/person-year].
        fpu : numpy.ndarray
            fraction of population urban [].
        hsapc : numpy.ndarray
            health services allocations per capita [dollars/person-year].
        le : numpy.ndarray
            life expectancy [years].
        lmc : numpy.ndarray
            lifetime multiplier from crowding [].
        lmf : numpy.ndarray
            lifetime multiplier from food [].
        lmhs : numpy.ndarray
            lifetime multiplier from health services [].
        lmhs1 : numpy.ndarray
            lmhs, value before time=pyear [].
        lmhs2 : numpy.ndarray
            lmhs, value after time=pyear [].
        lmp : numpy.ndarray
            lifetime multiplier from persistent pollution [].
        m1 : numpy.ndarray
            mortality, ages 0-14 [deaths/person-year].
        m2 : numpy.ndarray
            mortality, ages 15-44 [deaths/person-year].
        m3 : numpy.ndarray
            mortality, ages 45-64 [deaths/person-year].
        m4 : numpy.ndarray
            mortality, ages 65+ [deaths/person-year].

        **Birth rate subsector**

        b : numpy.ndarray
            births per year [persons/year].
        aiopc : numpy.ndarray
            average industrial output per capita [dollars/person-year].
        cbr : numpy.ndarray
            crude birth rate [births/1000 person-years].
        cmi : numpy.ndarray
            crowding multiplier from industrialization [].
        cmple : numpy.ndarray
            compensatory multiplier from perceived life expectancy [].
        diopc : numpy.ndarray
            delayed industrial output per capita [dollars/person-year].
        dtf : numpy.ndarray
            desired total fertility [].
        dcfs : numpy.ndarray
            desired completed family size [].
        fcapc : numpy.ndarray
            fertility control allocations per capita [dollars/person-year].
        fce : numpy.ndarray
            fertility control effectiveness [].
        fcfpc : numpy.ndarray
            fertility control facilities per capita [dollars/person-year].
        fie : numpy.ndarray
            family income expectation [].
        fm : numpy.ndarray
            fecundity multiplier [].
        frsn : numpy.ndarray
            family response to social norm [].
        fsafc : numpy.ndarray
            fraction of services allocated to fertility control [].
        mtf : numpy.ndarray
            maximum total fertility [].
        nfc : numpy.ndarray
            need for fertility control [].
        ple : numpy.ndarray
            perceived life expectancy [years].
        sfsn : numpy.ndarray
            social family size norm [].
        tf : numpy.ndarray
            total fertility [].

     */
    #endregion Documentation 

    public Population(World world) : base(world)
        // Initialize the state and rate variables of the population sector
        => Sector.InitializeLists(this, this.N, double.NaN);

    public override void SetDelayFunctions()
    {
        // "HSAPC" 
        base.CreateSmooth(new(this.Hsapc));

        // "LE", "FCAPC"
        base.CreateDelayInfThree(new(this.Le));
        base.CreateDelayInfThree(new(this.Fcapc));
    }

    public void InitializeConstants(
        double p1i = 65e7, double p2i = 70e7, double p3i = 19e7, double p4i = 6e7,
        double dcfsn = 4, double fcest = 4000, double hsid = 20, double ieat = 3,
        double len = 28, double lpd = 20, double mtfn = 12,
        double pet = 4000, double rlt = 30, double sad = 20, double zpgt = 4000
        )
    {
        this.P1i = p1i;
        this.P2i = p2i;
        this.P3i = p3i;
        this.P4i = p4i;
        this.Dcfsn = dcfsn;
        this.Fcest = fcest;
        this.Hsid = hsid;
        this.Ieat = ieat;
        this.Len = len;
        this.Lpd = lpd;
        this.Mtfn = mtfn;
        this.Pet = pet;
        this.Rlt = rlt;
        this.Sad = sad;
        this.Zpgt = zpgt;
    }

    #region Constants 
    // Constants 

    // pop 1 initial[persons]. The default is 65e7.
    public double P1i { get; private set; } = 65e7;

    // pop 2 initial[persons]. The default is 70e7.
    public double P2i { get; private set; } = 70e7;

    // pop 3 initial[persons]. The default is 19e7.
    public double P3i { get; private set; } = 19e7;

    // pop 4 initial[persons]. The default is 6e7.
    public double P4i { get; private set; } = 6e7;

    // Desired completed family size normal[]. The default is 4.
    public double Dcfsn { get; private set; } = 4.0;

    // fertility control effectiveness set time[year]. The default is 4000.
    public double Fcest { get; private set; } = 4000;

    // health services impact delay[years]. The default is 20.
    public double Hsid { get; private set; } = 20;

    // income expectation averaging time[years]. The default is 3.
    public double Ieat { get; private set; } = 3;

    // life expectancy normal[years]. The default is 28.
    public double Len { get; private set; } = 28;

    // lifetime perception delay[years]. The default is 20.
    public double Lpd { get; private set; } = 20;

    //  maximum total fertility normal[]. The default is 12.
    public double Mtfn { get; private set; } = 12;

    // population equilibrium time[year]. The default is 4000.
    public double Pet { get; private set; } = 4000;

    // reproductive lifetime[years]. The default is 30.
    public double Rlt { get; private set; } = 30;

    // social adjustment delay[years]. The default is 20.
    public double Sad { get; private set; } = 20;

    // time when desired family size equals 2 children[year]. The default is 4000.
    public double Zpgt { get; private set; } = 4000;

    #endregion Constants 

    #region States 
    // States 

    // population, ages 0-14 [persons]. It is a state variable.
    public List<double> P1 { get; private set; } = [];

    // population, ages 15-44 [persons]. It is a state variable.
    public List<double> P2 { get; private set; } = [];

    // population, ages 45-64 [persons]. It is a state variable.
    public List<double> P3 { get; private set; } = [];

    // population, ages 65+ [persons]. It is a state variable.
    public List<double> P4 { get; private set; } = [];

    // Total population[persons].
    public List<double> Pop { get; private set; } = [];

    #endregion States 

    #region Rates 
    // Rates 

    // maturation rate, age 14-15 [persons/year].
    public List<double> Mat1 { get; private set; } = [];

    // maturation rate, age 44-45 [persons/year].
    public List<double> Mat2 { get; private set; } = [];

    // maturation rate, age 64-65 [persons/year].
    public List<double> Mat3 { get; private set; } = [];

    // Death Rate Subsector 

    // deaths per year [persons/year].
    public List<double> D { get; private set; } = [];

    // deaths per year, ages 0-14 [persons/year].
    public List<double> D1 { get; private set; } = [];

    // deaths per year, ages 15-44 [persons/year].
    public List<double> D2 { get; private set; } = [];

    // deaths per year, ages 45-64 [persons/year].
    public List<double> D3 { get; private set; } = [];

    //    deaths per year, ages 65+ [persons/year].
    public List<double> D4 { get; private set; } = [];

    // crude death rate [deaths/1000 person-years].
    public List<double> Cdr { get; private set; } = [];

    // effective health services per capita [dollars/person-year].
    public List<double> Ehspc { get; private set; } = [];

    // fraction of population urban [].
    public List<double> Fpu { get; private set; } = [];

    // health services allocations per capita [dollars/person-year].
    public List<double> Hsapc { get; private set; } = [];

    // life expectancy [years].
    public List<double> Le { get; private set; } = [];

    // lifetime multiplier from crowding [].
    public List<double> Lmc { get; private set; } = [];

    // lifetime multiplier from food [].
    public List<double> Lmf { get; private set; } = [];

    // lifetime multiplier from health services [].
    public List<double> Lmhs { get; private set; } = [];

    // lmhs, value before time=pyear [].
    public List<double> Lmhs1 { get; private set; } = [];

    // lmhs, value after time=pyear [].
    public List<double> Lmhs2 { get; private set; } = [];

    // lifetime multiplier from persistent pollution [].
    public List<double> Lmp { get; private set; } = [];

    // mortality, ages 0-14 [deaths/person-year].
    public List<double> M1 { get; private set; } = [];

    // mortality, ages 15-44 [deaths/person-year].
    public List<double> M2 { get; private set; } = [];

    // mortality, ages 45-64 [deaths/person-year].
    public List<double> M3 { get; private set; } = [];

    // mortality, ages 65+ [deaths/person-year].
    public List<double> M4 { get; private set; } = [];

    // Birth Rate Subsector 

    // births per year [persons/year].
    public List<double> B { get; private set; } = [];

    // average industrial output per capita [dollars/person-year].
    public List<double> Aiopc { get; private set; } = [];

    // crude birth rate [births/1000 person-years].
    public List<double> Cbr { get; private set; } = [];

    // crowding multiplier from industrialization [].
    public List<double> Cmi { get; private set; } = [];

    // compensatory multiplier from perceived life expectancy [].
    public List<double> Cmple { get; private set; } = [];

    // delayed industrial output per capita [dollars/person-year].
    public List<double> Diopc { get; private set; } = [];

    // desired total fertility [].
    public List<double> Dtf { get; private set; } = [];

    // desired completed family size [].
    public List<double> Dcfs { get; private set; } = [];

    // fertility control allocations per capita [dollars/person-year].
    public List<double> Fcapc { get; private set; } = [];

    // fertility control effectiveness [].
    public List<double> Fce { get; private set; } = [];

    // fertility control facilities per capita [dollars/person-year].
    public List<double> Fcfpc { get; private set; } = [];

    // family income expectation [].
    public List<double> Fie { get; private set; } = [];

    // fecundity multiplier [].
    public List<double> Fm { get; private set; } = [];

    // family response to social norm [].
    public List<double> Frsn { get; private set; } = [];

    // fraction of services allocated to fertility control [].
    public List<double> Fsafc { get; private set; } = [];

    // maximum total fertility [].
    public List<double> Mtf { get; private set; } = [];

    // need for fertility control [].
    public List<double> Nfc { get; private set; } = [];

    // perceived life expectancy [years].
    public List<double> Ple { get; private set; } = [];

    // social family size norm [].
    public List<double> Sfsn { get; private set; } = [];

    // total fertility [].
    public List<double> Tf { get; private set; } = [];

    #endregion Rates 

    private void UpdateP1(int k, int j)
    {
        if (k == 0)
        {
            this.P1[0] = this.P1i;
        }
        else
        {
            this.P1[k] = this.P1[j] + this.Dt * (this.B[j] - this.D1[j] - this.Mat1[j]);
        }
    }
    
    private void UpdateP2(int k, int j)
    {
        if (k == 0)
        {
            this.P2[0] = this.P2i;
        }
        else
        {
            this.P2[k] = this.P2[j] + this.Dt * (this.Mat1[j] - this.D2[j] - this.Mat2[j]);
        }
    }

    private void UpdateP3(int k, int j)
    {
        if (k == 0)
        {
            this.P3[0] = this.P3i;
        }
        else
        {
            this.P3[k] = this.P3[j] + this.Dt * (this.Mat2[j] - this.D3[j] - this.Mat3[j]);
        }
    }

    private void UpdateP4(int k, int j)
    {
        if (k == 0)
        {
            this.P4[0] = this.P4i;
        }
        else
        {
            this.P4[k] = this.P4[j] + this.Dt * (this.Mat3[j] - this.D4[j]);
        }
    }

    [DependsOn("P1"), DependsOn("P2"), DependsOn("P3"), DependsOn("P4")]
    private void UpdatePop(int k)
        => this.Pop[k] = this.P1[k] + this.P2[k] + this.P3[k] + this.P4[k];

    // From step k requires: POP
    [DependsOn("POP")]
    private void UpdateFpu(int k) 
        => this.Fpu[k] = (nameof(this.Fpu)).Interpolate(this.Pop[k]);

    // From step k requires: PPOLX ( in Pollution sector ) 
    [DependsOn("PPOLX")]
    private void UpdateLmp(int k) 
        => this.Lmp[k] = (nameof(this.Lmp)).Interpolate(this.Pollution.Ppolx[k]);

    // From step k requires: FPC ( in Agri. sector ) 
    [DependsOn("FPC")]
    private void UpdateLmf(int k)
        => this.Lmf[k] =
            (nameof(this.Lmf)).Interpolate(this.Agriculture.Fpc[k] / this.Agriculture.Sfpc);

    // From step k requires: IOPC ( In Capital sector ) 
    [DependsOn("IOPC")]
    private void UpdateCmi(int k) 
        => this.Cmi[k] = (nameof(this.Cmi)).Interpolate(this.Capital.Iopc[k]);

    // From step k requires: SOPC ( in Capital.Service sector ) 
    [DependsOn("SOPC")]
    private void UpdateHsapc(int k) 
        => this.Hsapc[k] = (nameof(this.Hsapc)).Interpolate(this.Capital.Sopc[k]);

    // From step k=0 requires: HSAPC, else nothing
    [DependsOn("HSAPC")]
    private void UpdateEhspc(int k)
         => this.Ehspc[k] = this.Smooth((nameof(this.Hsapc)), k, this.Hsid);

    // From step k requires: EHSPC
    [DependsOn("EHSPC")]
    private void UpdateLmhs(int k)
    {
        this.Lmhs1[k] = (nameof(this.Lmhs1)).Interpolate(this.Ehspc[k]);
        this.Lmhs2[k] = (nameof(this.Lmhs2)).Interpolate(this.Ehspc[k]);
        this.Lmhs[k] = MathUtilities.Clip(this.Lmhs2[k], this.Lmhs1[k], this.Time[k], this.Iphst);
    }

    // From step k requires: CMI FPU
    [DependsOn("CMI"), DependsOn("FPU")]
    private void UpdateLmc(int k) 
        => this.Lmc[k] = 1.0 - this.Cmi[k] * this.Fpu[k];

    // From step k requires: LMF LMHS LMP LMC
    [DependsOn("LMF"), DependsOn("LMHS"), DependsOn("LMP"), DependsOn("LMC")]
    private void UpdateLe(int k)
        => this.Le[k] = this.Len * this.Lmf[k] * this.Lmhs[k] * this.Lmp[k] * this.Lmc[k];

    // From step k requires: LE
    [DependsOn("LE")]
    private void UpdateM1(int k)
        => this.M1[k] = (nameof(this.M1)).Interpolate(this.Le[k]);

    // From step k requires: LE
    [DependsOn("LE")]
    private void UpdateM2(int k)
        => this.M2[k] = (nameof(this.M2)).Interpolate(this.Le[k]);

    // From step k requires: LE
    [DependsOn("LE")]
    private void UpdateM3(int k)
        => this.M3[k] = (nameof(this.M3)).Interpolate(this.Le[k]);

    // From step k requires: LE
    [DependsOn("LE")]
    private void UpdateM4(int k)
        => this.M4[k] = (nameof(this.M4)).Interpolate(this.Le[k]);

    // From step k requires: P1 M1
    [DependsOn("P1"), DependsOn("M1")]
    private void UpdateMat1(int k)
        => this.Mat1[k] = this.P1[k] * (1.0 - this.M1[k]) / 15.0;

    // From step k requires: P2 M2
    [DependsOn("P2"), DependsOn("M2")]
    private void UpdateMat2(int k)
        => this.Mat2[k] = this.P2[k] * (1.0 - this.M2[k]) / 30.0;

    // From step k requires: P3 M3
    [DependsOn("P3"), DependsOn("M3")]
    private void UpdateMat3(int k)
        => this.Mat3[k] = this.P3[k] * (1.0 - this.M3[k]) / 20.0;

    // From step k requires: P1 M1
    [DependsOn("P1"), DependsOn("M1")]
    private void UpdateD1(int k)
        => this.D1[k] = this.P1[k] * this.M1[k];

    // From step k requires: P2 M2
    [DependsOn("P2"), DependsOn("M2")]
    private void UpdateD2(int k)
        => this.D2[k] = this.P2[k] * this.M2[k];

    // From step k requires: P3 M3
    [DependsOn("P3"), DependsOn("M3")]
    private void UpdateD3(int k)
        => this.D3[k] = this.P3[k] * this.M3[k];

    // From step k requires: P4 M4
    [DependsOn("P4"), DependsOn("M4")]
    private void UpdateD4(int k)
        => this.D4[k] = this.P4[k] * this.M4[k];

    // From step k requires: nothing
    private void UpdateD(int k, int j)
        => this.D[k] = 
            k == 0 ? 
                0.0 : 
                this.D1[j] + this.D2[j] + this.D3[j] + this.D4[j];

    // From step k requires: D POP 
    [DependsOn("D"), DependsOn("POP")]
    private void UpdateCdr(int k)
        => this.Cdr[k] = 1000.0 * this.D[k] / this.Pop[k];

    // From step k=0 requires: IOPC, else nothing
    [DependsOn("IOPC")]
    private void UpdateAiopc(int k)
        => this.Aiopc[k] = this.Smooth("Iopc", k, this.Ieat);

    // From step k=0 requires: IOPC, else nothing
    [DependsOn("IOPC")]
    private void UpdateDiopc(int k)
        => this.Diopc[k] = this.DelayInfThree("Iopc", k, this.Sad);

    // From step k requires: IOPC AIOPC
    [DependsOn("IOPC"), DependsOn("AIOPC")]
    private void UpdateFie(int k)
        => this.Fie[k] = (this.Capital.Iopc[k] - this.Aiopc[k]) / this.Aiopc[k];

    // From step k requires: DIOPC
    [DependsOn("DIOPC")]
    private void UpdateSfsn(int k)
        => this.Sfsn[k] = (nameof(this.Sfsn)).Interpolate(this.Diopc[k]);

    // From step k requires: FIE
    [DependsOn("FIE")]
    private void UpdateFrsn(int k)
    {
        if (k == 0)
        {
            this.Frsn[0] = 0.82;
        }
        else
        {
            this.Frsn[k] = (nameof(this.Frsn)).Interpolate(this.Fie[k]);
        }
    }
    
    // From step k requires: FRSN SFSN
    [DependsOn("FRSN"), DependsOn("SFSN")]
    private void UpdateDcfs(int k)
        => this.Dcfs[k] =
            Clip(2.0, this.Dcfsn * this.Frsn[k] * this.Sfsn[k], this.Time[k], this.Zpgt);

    // From step k=0 requires: LE, else nothing
    [DependsOn("LE")]
    private void UpdatePle(int k)
        => this.Ple[k] = this.DelayInfThree(nameof(this.Le), k, this.Lpd);

    // From step k requires: PLE
    [DependsOn("PLE")]
    private void UpdateCmple(int k)
        => this.Cmple[k] = (nameof(this.Cmple)).Interpolate(this.Ple[k]);

    // From step k requires: DCFS CMPLE
    [DependsOn("DCFS"), DependsOn("CMPLE")]
    private void UpdateDtf(int k)
        => this.Dtf[k] = this.Dcfs[k] * this.Cmple[k];

    // From step k requires: LE
    [DependsOn("LE")]
    private void UpdateFm(int k)
        => this.Fm[k] = (nameof(this.Fm)).Interpolate(this.Le[k]);

    // From step k requires: FM
    [DependsOn("FM")]
    private void UpdateMtf(int k)
        => this.Mtf[k] = this.Mtfn * this.Fm[k];

    // From step k requires: MTF DTF
    [DependsOn("MTF"), DependsOn("DTF")]
    private void UpdateNfc(int k)
        => this.Nfc[k] = this.Mtf[k] / this.Dtf[k] - 1.0;

    // From step k requires: NFC
    [DependsOn("NFC")]
    private void UpdateFsafc(int k)
        => this.Fsafc[k] = (nameof(this.Fsafc)).Interpolate(this.Nfc[k]);

    // From step k requires: FSAFC SOPC
    [DependsOn("FSAFC"), DependsOn("SOPC")]
    private void UpdateFcapc(int k)
        => this.Fcapc[k] = this.Fsafc[k] * this.Capital.Sopc[k]; //  from Capital: Service Output

    // From step k=0 requires: FCAPC, else nothing
    [DependsOn("FCAPC")]
    private void UpdateFcfpc(int k)
        => this.Fcfpc[k] = this.DelayInfThree(nameof(this.Fcapc), k, this.Hsid);

    // From step k requires: FCFPC
    [DependsOn("FCFPC")]
    private void UpdateFce(int k)
    {
        double clippedFce = "Fce_ToClip".Interpolate(this.Fcfpc[k]);
        this.Fce[k] = Clip(1.0, clippedFce, this.Time[k], this.Fcest);
    }

    // From step k requires: MTF FCE DTF
    [DependsOn("MTF"), DependsOn("FCE"), DependsOn("DTF")]
    private void UpdateTf(int k)
        => this.Tf[k] = 
            Math.Min (this.Mtf[k], (this.Mtf[k] * (1 - this.Fce[k]) + this.Dtf[k] * this.Fce[k]));

    // From step k requires: POP
    [DependsOn("POP"), DependsOn("B")]
    private void UpdateCbr(int k, int j)
        => this.Cbr[k] = 1000 * this.B[j] / this.Pop[k];

    // From step k requires: D P2 TF
    [DependsOn("D"), DependsOn("P2"), DependsOn("TF")]
    private void UpdateB(int k)
        => this.B[k] = 
            Clip(this.D[k], this.Tf[k] * this.P2[k] * 0.5 / this.Rlt, this.Time[k], this.Pet);
}
