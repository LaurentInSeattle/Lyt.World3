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

    public Population(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, 
        double iphst,
        bool isVerbose = false) 
            : base(world, yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {
        // Initialize the state and rate variables of the population sector
        this.InitializeLists(this.N, double.NaN);
        this.SetDelayFunctions();
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

    // Initialize the population sector ( == initial loop with k=0).
    public void Initialize()
    {
        try
        {
            // Set initial conditions
            this.Frsn[0] = 0.82;
            this.P1[0] = this.P1i;
            this.P2[0] = this.P2i;
            this.P3[0] = this.P3i;
            this.P4[0] = this.P4i;
            this.Pop[0] = this.P1[0] + this.P2[0] + this.P3[0] + this.P4[0];

            // Death rate subsector
            //
            // connect World3 sectors to Population
            // pop from initialisation
            this.UpdateFpu(0);
            this.UpdateLmp(0);
            this.UpdateLmf(0);
            this.UpdateCmi(0);
            this.UpdateHsapc(0);

            // inside Population sector
            //
            this.UpdateEhspc(0);
            this.UpdateLmhs(0);
            this.UpdateLmc(0);
            this.UpdateLe(0);
            //
            this.UpdateM1(0);
            this.UpdateM2(0);
            this.UpdateM3(0);
            this.UpdateM4(0);
            //
            this.UpdateMat1(0, 0);
            this.UpdateMat2(0, 0);
            this.UpdateMat3(0, 0);

            this.UpdateD1(0, 0);
            this.UpdateD2(0, 0);
            this.UpdateD3(0, 0);
            this.UpdateD4(0, 0);
            this.UpdateD(0, 0); // replace (0, -1) by (0, 0) at init
            this.UpdateCdr(0);

            // Birth rate subsector
            //
            //  connect World3 sectors to Population
            // Industrial Output > Population
            this.UpdateAiopc(0);
            this.UpdateDiopc(0);
            this.UpdateFie(0);

            // inside Population sector
            //
            this.UpdatesSfsn(0);
            this.UpdateFrsn(0);
            this.UpdateDcfs(0);
            this.UpdatePle(0);
            this.UpdateCmple(0);
            this.UpdateDtf(0);

            this.UpdateFm(0);
            this.UpdateMtf(0);
            this.UpdateNfc(0);

            this.UpdateFsafc(0);
            this.UpdateFcapc(0);
            this.UpdateFcfpc(0);
            this.UpdateFce(0);

            this.UpdateTf(0);
            this.UpdateCbr(0, 0); // replace (0, -1) by (0, 0) at init
            this.UpdateB(0, 0);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    public void Update(int k, int j, int jk, int kl)
    {
        try
        {
            this.UpdateStateP1(k, j, jk);
            this.UpdateStateP2(k, j, jk);
            this.UpdateStateP3(k, j, jk);
            this.UpdateStateP4(k, j, jk);
            this.UpdateStatePop(k);

            // Death rate subsector
            //
            this.UpdateFpu(k);
            this.UpdateLmp(k);
            this.UpdateLmf(k);
            this.UpdateCmi(k);
            this.UpdateHsapc(k);

            // inside Population sector
            //
            this.UpdateEhspc(k);
            this.UpdateLmhs(k);
            this.UpdateLmc(k);
            this.UpdateLe(k);
            //
            this.UpdateM1(k);
            this.UpdateM2(k);
            this.UpdateM3(k);
            this.UpdateM4(k);
            //
            this.UpdateMat1(k, kl);
            this.UpdateMat2(k, kl);
            this.UpdateMat3(k, kl);
            //
            this.UpdateD1(k, kl);
            this.UpdateD2(k, kl);
            this.UpdateD3(k, kl);
            this.UpdateD4(k, kl);
            this.UpdateD(k, jk); // replace (0, -1) by (0, 0) at init
            this.UpdateCdr(k);
            //
            // Birth rate subsector
            //
            //  connect World3 sectors to Population
            // Industrial Output > Population
            this.UpdateAiopc(k);
            this.UpdateDiopc(k);
            this.UpdateFie(k);
            //
            // inside Population sector
            //
            this.UpdatesSfsn(k);
            this.UpdateFrsn(k);
            this.UpdateDcfs(k);
            this.UpdatePle(k);
            this.UpdateCmple(k);
            this.UpdateDtf(k);
            //
            this.UpdateFm(k);
            this.UpdateMtf(k);
            this.UpdateNfc(k);
            // 
            this.UpdateFsafc(k);
            this.UpdateFcapc(k);
            this.UpdateFcfpc(k);
            this.UpdateFce(k);
            //
            this.UpdateTf(k);
            this.UpdateCbr(k, jk);
            this.UpdateB(k, kl);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    private void SetDelayFunctions()
    {
        // "HSAPC", "IOPC"
        var smoothHsapc = new Smooth(this.Hsapc, this.Dt, this.Time);
        this.World.Smooths.Add(nameof(this.Hsapc), smoothHsapc);

        // Defined in Capital Sector ??? 
        // var smoothIopc = new Smooth(this.Iopc, this.Dt, this.Time);

        // "LE", "IOPC", "FCAPC"
        foreach (List<double> delay in new List<List<double>> { this.Le, this.Fcapc })
        {
            var delay3 = new DelayInformationThree(delay, this.Dt, this.Time);
            this.World.DelayInfThrees.Add(nameof(delay), delay3);
        }
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

    private void InitializeLists(int size, double value)
    {
        var type = this.GetType();
        var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var propertyInfo in properties)
        {
            if (propertyInfo is null)
            {
                continue;
            }

            var propertyType = propertyInfo.PropertyType;
            if (!IsListOfDouble(propertyType))
            {
                continue;
            }

            var setter = propertyInfo.GetSetMethod(nonPublic: true);
            if (setter is not null)
            {
                double[] array = new double[size];
                Array.Fill(array, value);
                var list = new List<double>(array);
                setter.Invoke(this, [list]);
            }
        }
    }

    private static bool IsListOfDouble(Type type)
    {
        // Check if the type is a generic type and if its generic type definition is List<>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            // Get the generic arguments (the types inside the List<>)
            Type[] genericArguments = type.GetGenericArguments();

            // Check if there is exactly one generic argument and if it's of type double
            if (genericArguments.Length == 1 && genericArguments[0] == typeof(double))
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateStateP1(int k, int j, int jk)
        => this.P1[k] = this.P1[j] + this.Dt * (this.B[jk] - this.D1[jk] - this.Mat1[jk]);

    private void UpdateStateP2(int k, int j, int jk)
        => this.P2[k] = this.P2[j] + this.Dt * (this.Mat1[jk] - this.D2[jk] - this.Mat2[jk]);

    private void UpdateStateP3(int k, int j, int jk)
        => this.P3[k] = this.P3[j] + this.Dt * (this.Mat2[jk] - this.D3[jk] - this.Mat3[jk]);

    private void UpdateStateP4(int k, int j, int jk)
        => this.P4[k] = this.P4[j] + this.Dt * (this.Mat3[jk] - this.D4[jk]);

    private void UpdateStatePop(int k)
        => this.P1[k] = this.P1[k] + this.P2[k] + this.P3[k] + this.P4[k];

    // From step k requires: POP
    private void UpdateFpu(int k) => this.Fpu[k] = (nameof(this.Fpu)).Interpolate(this.Pop[k]);

    // From step k requires: PPOLX ( in Pollution sector ) 
    private void UpdateLmp(int k) => this.Lmp[k] = (nameof(this.Lmp)).Interpolate(this.Pollution.Ppolx[k]);

    // From step k requires: FPC ( in Agri. sector ) 
    private void UpdateLmf(int k)
        => this.Lmf[k] =
            (nameof(this.Lmf)).Interpolate(this.Agriculture.Fpc[k] / this.Agriculture.Sfpc);

    // From step k requires: IOPC ( In Capital sector ) 
    private void UpdateCmi(int k) => (nameof(this.Cmi)).Interpolate(this.Capital.Iopc[k]);

    // From step k requires: SOPC ( in Service sector ) 
    private void UpdateHsapc(int k) => (nameof(this.Hsapc)).Interpolate(this.Capital.Sopc[k]);


    // From step k=0 requires: HSAPC, else nothing
    private void UpdateEhspc(int k)
         => this.Ehspc[k] = this.Smooth((nameof(this.Hsapc)), k, this.Hsid);

    // From step k requires: EHSPC
    private void UpdateLmhs(int k)
    {
        this.Lmhs1[k] = (nameof(this.Lmhs1)).Interpolate(this.Ehspc[k]);
        this.Lmhs2[k] = (nameof(this.Lmhs2)).Interpolate(this.Ehspc[k]);
        this.Lmhs[k] = Clip(this.Lmhs2[k], this.Lmhs1[k], this.Time[k], this.Iphst);
    }

    // From step k requires: CMI FPU
    private void UpdateLmc(int k) => this.Lmc[k] = 1.0 - this.Cmi[k] * this.Fpu[k];

    // From step k requires: LMF LMHS LMP LMC
    private void UpdateLe(int k)
        => this.Le[k] = this.Len * this.Lmf[k] * this.Lmhs[k] * this.Lmp[k] * this.Lmc[k];

    // From step k requires: LE
    private void UpdateM1(int k)
        => this.M1[k] = (nameof(this.M1)).Interpolate(this.Le[k]);

    // From step k requires: LE
    private void UpdateM2(int k)
        => this.M2[k] = (nameof(this.M2)).Interpolate(this.Le[k]);

    // From step k requires: LE
    private void UpdateM3(int k)
        => this.M3[k] = (nameof(this.M3)).Interpolate(this.Le[k]);

    // From step k requires: LE
    private void UpdateM4(int k)
        => this.M4[k] = (nameof(this.M4)).Interpolate(this.Le[k]);

    // From step k requires: P1 M1
    private void UpdateMat1(int k, int kl)
        => this.Mat1[kl] = this.P1[k] * (1.0 - this.M1[k]) / 15.0;

    // From step k requires: P2 M2
    private void UpdateMat2(int k, int kl)
        => this.Mat2[kl] = this.P2[k] * (1.0 - this.M2[k]) / 30.0;

    // From step k requires: P3 M3
    private void UpdateMat3(int k, int kl)
        => this.Mat3[kl] = this.P3[k] * (1.0 - this.M3[k]) / 20.0;

    // From step k requires: P1 M1
    private void UpdateD1(int k, int kl)
        => this.D1[kl] = this.P1[k] * this.M1[k];

    // From step k requires: P2 M2
    private void UpdateD2(int k, int kl)
        => this.D2[kl] = this.P2[k] * this.M2[k];

    // From step k requires: P3 M3
    private void UpdateD3(int k, int kl)
        => this.D3[kl] = this.P3[k] * this.M3[k];

    // From step k requires: P4 M4
    private void UpdateD4(int k, int kl)
        => this.D4[kl] = this.P4[k] * this.M4[k];

    // From step k requires: nothing
    private void UpdateD(int k, int jk)
        => this.D[k] = this.D1[jk] + this.D2[jk] + this.D3[jk] + this.D4[jk];

    // From step k requires: D POP 
    private void UpdateCdr(int k)
        => this.Cdr[k] = 1000.0 * this.D[k] / this.Pop[k];

    // From step k requires: From step k=0 requires: IOPC, else nothing
    private void UpdateAiopc(int k)
        => this.Aiopc[k] = this.Smooth("iopc", k, this.Ieat);

    // From step k=0 requires: IOPC, else nothing
    private void UpdateDiopc(int k)
        => this.Diopc[k] = this.DelayInfThree("iopc", k, this.Sad);

    // From step k requires: IOPC AIOPC
    private void UpdateFie(int k)
        => this.Fie[k] = (this.Capital.Iopc[k] - this.Aiopc[k]) / this.Aiopc[k];

    // From step k requires: DIOPC
    private void UpdatesSfsn(int k)
        => this.Sfsn[k] = (nameof(this.Sfsn)).Interpolate(this.Diopc[k]);

    // From step k requires: FIE
    private void UpdateFrsn(int k)
        => this.Frsn[k] = (nameof(this.Frsn)).Interpolate(this.Fie[k]);

    // From step k requires: FRSN SFSN
    private void UpdateDcfs(int k)
        => this.Dcfs[k] =
            Clip(2.0, this.Dcfsn * this.Frsn[k] * this.Sfsn[k], this.Time[k], this.Zpgt);

    // From step k=0 requires: LE, else nothing
    private void UpdatePle(int k)
        => this.Ple[k] = this.DelayInfThree(nameof(this.Le), k, this.Lpd);

    // From step k requires: PLE
    private void UpdateCmple(int k)
        => this.Cmple[k] = (nameof(this.Cmple)).Interpolate(this.Ple[k]);

    // From step k requires: DCFS CMPLE
    private void UpdateDtf(int k)
        => this.Dtf[k] = this.Dcfs[k] * this.Cmple[k];

    // From step k requires: LE
    private void UpdateFm(int k)
        => this.Fm[k] = (nameof(this.Fm)).Interpolate(this.Le[k]);

    // From step k requires: FM
    private void UpdateMtf(int k)
        => this.Mtf[k] = this.Mtfn * this.Fm[k];

    // From step k requires: MTF DTF
    private void UpdateNfc(int k)
        => this.Nfc[k] = this.Mtf[k] / this.Dtf[k] - 1.0;


    // From step k requires: NFC
    private void UpdateFsafc(int k)
        => this.Fsafc[k] = (nameof(this.Fsafc)).Interpolate(this.Nfc[k]);

    // From step k requires: FSAFC SOPC
    private void UpdateFcapc(int k)
        => this.Fcapc[k] = this.Fsafc[k] * this.Capital.Sopc[k]; //  from Capital: Service Output

    // From step k=0 requires: FCAPC, else nothing
    private void UpdateFcfpc(int k)
        => this.Fcfpc[k] = this.DelayInfThree(nameof(this.Fcapc), k, this.Hsid);

    // From step k requires: FCFPC
    private void UpdateFce(int k)
    {
        double clippedFce = "Fce_ToClip".Interpolate(this.Fcfpc[k]);
        this.Fce[k] = Clip(1.0, clippedFce, this.Time[k], this.Fcest);
    }

    // From step k requires: MTF FCE DTF
    private void UpdateTf(int k)
        => this.Tf[k] = 
            Math.Min (this.Mtf[k], (this.Mtf[k] * (1 - this.Fce[k]) + this.Dtf[k] * this.Fce[k]));

    // From step k requires: POP
    private void UpdateCbr(int k, int jk)
        => this.Cbr[k] = 1000 * this.B[jk] / this.Pop[k];

    // From step k requires: D P2 TF
    private void UpdateB(int k, int kl)
        => this.B[kl] = 
            Clip(this.D[k], this.Tf[k] * this.P2[k] * 0.5 / this.Rlt, this.Time[k], this.Pet);
}
