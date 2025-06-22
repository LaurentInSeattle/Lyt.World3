namespace Lyt.World3.Model.Sectors;

/// <summary>
///     Agriculture sector. The initial code is defined p.362.
/// </summary>
public sealed class Agriculture : Sector
{
    #region Documentation 

    /*
        ali { get; private set ; } 
            arable land initial[hectares]. The default is 0.9e9.
        pali { get; private set ; } 
            potentially arable land initial[hectares]. The default is 2.3e9.
        lfh { get; private set ; } 
            land fraction harvested[]. The default is 0.7.
        palt { get; private set ; } 
            potentially arable land total[hectares]. The default is 3.2e9.
        pl { get; private set ; } 
            processing loss[]. The default is 0.1.
        alai1 { get; private set ; } 
            alai, value before time = pyear[years].The default is 2.
        alai2 { get; private set ; } 
            alai, value after time = pyear[years].The default is 2.
        io70 { get; private set ; } 
            industrial output in 1970 [dollars/year]. The default is 7.9e11.
        lyf1 { get; private set ; } 
            lyf, value before time = pyear[].The default is 1.
        lyf2 { get; private set ; } 
            lyf, value after time = pyear[].The default is 1.
        sd { get; private set ; } 
            social discount[1 / year]. The default is 0.07.
        uili { get; private set ; } 
            urban-industrial land initial[hectares]. The default is 8.2e6.
        alln { get; private set ; } 
            average life of land normal[years]. The default is 6000.
        uildt { get; private set ; } 
            urban-industrial land development time[years]. The default is 10.
        lferti { get; private set ; } 
            land fertility initial[vegetable - equivalent kilograms / hectare - year].
            The default is 600.
        ilf { get; private set ; } 
            inherent land fertility[vegetable - equivalent kilograms / hectare - year].
            The default is 600.
        fspd { get; private set ; } 
            food shortage perception delay[years]. The default is 2.
        sfpc { get; private set ; } 
            subsistence food per capita
            [vegetable - equivalent kilograms / person - year]. The default is 230.

        **Loop 1 - food from investment in land development**

        al { get; private set ; } = [];
            arable land[hectares].
        pal { get; private set ; } = [];
            potentially arable land[hectares].
        dcph { get; private set ; } = [];
            development cost per hectare[dollars / hectare].
        f { get; private set ; } = [];
            food[vegetable - equivalent kilograms / year].
        fpc { get; private set ; } = [];
            food per capita[vegetable - equivalent kilograms / person - year].
        fioaa { get; private set ; } = [];
            fraction of industrial output allocated to agriculture[].
        fioaa1 { get; private set ; } = [];
            fioaa, value before time = pyear[].
        fioaa2 { get; private set ; } = [];
            fioaa, value after time = pyear[].
        ifpc { get; private set ; } = [];
            indicated food per capita[vegetable - equivalent kilograms / person - year].
        ifpc1 { get; private set ; } = [];
            ifpc, value before time = pyear
            [vegetable - equivalent kilograms / person - year].
        ifpc2 { get; private set ; } = [];
            ifpc, value after time = pyear
            [vegetable - equivalent kilograms / person - year].
        ldr { get; private set ; } = [];
            land development rate[hectares / year].
        lfc { get; private set ; } = [];
            land fraction cultivated[].
        tai { get; private set ; } = [];
            total agricultural investment[dollars / year].

        **Loop 2 - food from investment in agricultural inputs**

        ai { get; private set ; } = [];
            agricultural inputs[dollars / year].
        aiph { get; private set ; } = [];
            agricultural inputs per hectare[dollars / hectare - year].
        alai { get; private set ; } = [];
            average lifetime of agricultural inputs[years].
        cai { get; private set ; } = [];
            current agricultural inputs[dollars / year].
        ly { get; private set ; } = [];
            land yield[vegetable - equivalent kilograms / hectare - year].
        lyf { get; private set ; } = [];
            land yield factor[].
        lymap { get; private set ; } = [];
            land yield multiplier from air pollution[].
        lymap1 { get; private set ; } = [];
            lymap, value before time = pyear[].
        lymap2 { get; private set ; } = [];
            lymap, value after time = pyear[].
        lymc { get; private set ; } = [];
            land yield multiplier from capital[].

        **Loop 1 & 2 - the investment allocation decision*

        fiald { get; private set ; } = [];
            fraction of inputs allocated to land development[].
        mlymc { get; private set ; } = [];
            marginal land yield multiplier from capital[hectares / dollar].
        mpai { get; private set ; } = [];
            marginal productivity of agricultural inputs
            [vegetable equivalent kilograms / dollar].
        mpld { get; private set ; } = [];
            marginal productivity of land development
            [vegetable - equivalent kilograms / dollar].

        **Loop 3 -land erosion and urban-industrial use**

        uil { get; private set ; } = [];
            urban-industrial land[hectares].
        all { get; private set ; } = [];
            average life of land[years].
        llmy { get; private set ; } = [];
            land life multiplier from yield[].
        llmy1 { get; private set ; } = [];
            llmy, value before time = pyear[].
        llmy2 { get; private set ; } = [];
            llmy, value after time = pyear[].
        ler { get; private set ; } = [];
            land erosion rate[hectares / year].
        lrui { get; private set ; } = [];
            land removal for urban-industrial use[hectares / year].
        uilpc { get; private set ; } = [];
            urban-industrial land per capita[hectares / person].
        uilr { get; private set ; } = [];
            urban-industrial land required [hectares].

        **Loop 4 - land fertility degradation**

        lfert { get; private set ; } = [];
            land fertility[vegetable - equivalent kilograms / hectare - year].
        lfd { get; private set ; } = [];
            land fertility degradation
            [vegetable - equivalent kilograms / hectare - year - year].
        lfdr { get; private set ; } = [];
            land fertility degradation rate[1 / year].

        **Loop 5 - land fertility regeneration**

        lfr { get; private set ; } = [];
            land fertility regeneration
            [vegetable - equivalent kilograms / hectare - year - year].
        lfrt { get; private set ; } = [];
            land fertility regeneration time[years].

        **Loop 6 - discontinuing land maintenance**

        falm { get; private set ; } = [];
            fraction of inputs allocated to land maintenance[dimensionless].
        fr { get; private set ; } = [];
            food ratio[].
        pfr { get; private set ; } = [];
            perceived food ratio[].
    */
    #endregion Documentation 

    public Agriculture(World world) : base(world)
        => InitializeLists(this, this.N, double.NaN);

    #region Constants 
    // arable land initial[hectares]. The default is 0.9e9.
    public double Ali { get; private set; }

    // potentially arable land initial[hectares]. The default is 2.3e9.
    public double Pali { get; private set; }

    // land fraction harvested[]. The default is 0.7.
    public double Lfh { get; private set; }

    // potentially arable land total[hectares]. The default is 3.2e9.
    public double Palt { get; private set; }

    // processing loss[]. The default is 0.1.
    public double Pl { get; private set; }

    // alai, value before time = pyear[years].The default is 2.
    public double Alai1 { get; private set; }

    // alai, value after time = pyear[years].The default is 2.
    public double Alai2 { get; private set; }

    // industrial output in 1970 [dollars/year]. The default is 7.9e11.
    public double Io70 { get; private set; }

    // lyf, value before time = pyear[].The default is 1.
    public double Lyf1 { get; private set; }

    // lyf, value after time = pyear[].The default is 1.
    public double Lyf2 { get; private set; }

    // social discount[1 / year]. The default is 0.07.
    public double Sd { get; private set; }

    // urban-industrial land initial[hectares]. The default is 8.2e6.
    public double Uili { get; private set; }

    // average life of land normal[years]. The default is 6000.
    public double Alln { get; private set; }

    // urban-industrial land development time[years]. The default is 10.
    public double Uildt { get; private set; }

    // land fertility initial[vegetable - equivalent kilograms / hectare - year]. The default is 600.    
    public double Lferti { get; private set; }

    // inherent land fertility[vegetable - equivalent kilograms / hectare - year]. The default is 600.
    public double Ilf { get; private set; }

    // food shortage perception delay[years]. The default is 2.
    public double Fspd { get; private set; }

    // subsistence food per capita [vegetable - equivalent kilograms / person - year]. The default is 230.
    public double Sfpc { get; private set; }

    #endregion Constants 

    #region States and Rates  

    // Loop 1 - food from investment in land development

    // arable land[hectares].
    public List<double> Al { get; private set; } = [];

    // potentially arable land[hectares].
    public List<double> Pal { get; private set; } = [];

    // development cost per hectare[dollars / hectare].
    public List<double> Dcph { get; private set; } = [];

    // food [vegetable - equivalent kilograms / year].
    public List<double> F { get; private set; } = [];

    // food per capita[vegetable - equivalent kilograms / person - year].
    public List<double> Fpc { get; private set; } = [];

    // fraction of industrial output allocated to agriculture[].
    public List<double> Fioaa { get; private set; } = [];

    // fioaa, value before time = pyear[].
    public List<double> Fioaa1 { get; private set; } = [];

    // fioaa, value after time = pyear[].
    public List<double> Fioaa2 { get; private set; } = [];

    // indicated food per capita[vegetable - equivalent kilograms / person - year].
    public List<double> Ifpc { get; private set; } = [];

    // ifpc, value before time = pyear [vegetable - equivalent kilograms / person - year].
    public List<double> Ifpc1 { get; private set; } = [];

    // ifpc, value after time = pyear  [vegetable - equivalent kilograms / person - year].
    public List<double> Ifpc2 { get; private set; } = [];

    // land development rate[hectares / year].
    public List<double> Ldr { get; private set; } = [];

    // land fraction cultivated[].
    public List<double> Lfc { get; private set; } = [];

    // total agricultural investment[dollars / year].
    public List<double> Tai { get; private set; } = [];

    // Loop 2 - food from investment in agricultural inputs

    // Agricultural inputs[dollars / year].
    public List<double> Ai { get; private set; } = [];

    // Agricultural inputs per hectare[dollars / hectare - year].
    public List<double> Aiph { get; private set; } = [];

    // Average lifetime of agricultural inputs[years].
    public List<double> Alai { get; private set; } = [];

    //  Current agricultural inputs[dollars / year].
    public List<double> Cai { get; private set; } = [];

    //  Land yield[vegetable - equivalent kilograms / hectare - year].
    public List<double> Ly { get; private set; } = [];

    // Land yield factor[].
    public List<double> Lyf { get; private set; } = [];

    // Land yield multiplier from air pollution[].
    public List<double> Lymap { get; private set; } = [];

    // Lymap, value before time = pyear[].
    public List<double> Lymap1 { get; private set; } = [];

    // Lymap, value after time = pyear[].
    public List<double> Lymap2 { get; private set; } = [];

    // Land yield multiplier from capital[].
    public List<double> Lymc { get; private set; } = [];

    // Loop 1 & 2 - the investment allocation decision

    // fraction of inputs allocated to land development[].
    public List<double> Fiald { get; private set; } = [];

    // marginal land yield multiplier from capital[hectares / dollar].
    public List<double> Mlymc { get; private set; } = [];

    // marginal productivity of agricultural inputs [vegetable equivalent kilograms / dollar]. 
    public List<double> Mpai { get; private set; } = [];

    // marginal productivity of land development [vegetable - equivalent kilograms / dollar].
    public List<double> Mpld { get; private set; } = [];

    // Loop 3 -land erosion and urban-industrial use

    // Urban-industrial land[hectares].
    public List<double> Uil { get; private set; } = [];

    // Average life of land[years].
    public List<double> All { get; private set; } = [];

    // Land life multiplier from yield[].
    public List<double> Llmy { get; private set; } = [];

    // Llmy, value before time = pyear[].
    public List<double> Llmy1 { get; private set; } = [];

    // Llmy, value after time = pyear[].
    public List<double> Llmy2 { get; private set; } = [];

    // Land erosion rate[hectares / year].    
    public List<double> Ler { get; private set; } = [];

    // Land removal for urban-industrial use[hectares / year].
    public List<double> Lrui { get; private set; } = [];

    // Urban-industrial land per capita[hectares / person].
    public List<double> Uilpc { get; private set; } = [];

    // Urban-industrial land required [hectares].
    public List<double> Uilr { get; private set; } = [];

    // Loop 4 - land fertility degradation

    // Land fertility[vegetable - equivalent kilograms / hectare - year].
    public List<double> Lfert { get; private set; } = [];

    // Land fertility degradation [vegetable - equivalent kilograms / hectare - year - year].
    public List<double> Lfd { get; private set; } = [];

    // Land fertility degradation rate[1 / year].
    public List<double> Lfdr { get; private set; } = [];

    // Loop 5 - land fertility regeneration 

    // Land fertility regeneration  [vegetable - equivalent kilograms / hectare - year - year].
    public List<double> Lfr { get; private set; } = [];

    // Land fertility regeneration time[years].
    public List<double> Lfrt { get; private set; } = [];

    // Loop 6 - discontinuing land maintenance 

    // Fraction of inputs allocated to land maintenance[dimensionless].
    public List<double> Falm { get; private set; } = [];

    // Food ratio[].
    public List<double> Fr { get; private set; } = [];

    // Perceived food ratio[].
    public List<double> Pfr { get; private set; } = [];

    #endregion States and Rates 

    // Delays of the Agriculture Sector 
    public override void SetDelayFunctions()
    {
        this.CreateSmooth(new(this.Cai));
        this.CreateSmooth(new(this.Fr));
    }

    public void InitializeConstants(
        double ali = 0.9e9,
        double pali = 2.3e9,
        double lfh = 0.7,
        double palt = 3.2e9,
        double pl = 0.1,
        double alai1 = 2,
        double alai2 = 2,
        double io70 = 7.9e11,
        double lyf1 = 1,
        double lyf2 = 1,
        double sd = 0.07,
        double uili = 8.2e6,
        double alln = 6000,
        double uildt = 10,
        double lferti = 600,
        double ilf = 600,
        double fspd = 2,
        double sfpc = 230
        )
    {
        // loop 1 - food from investment in land development
        this.Ali = ali;
        this.Pali = pali;
        this.Lfh = lfh;
        this.Palt = palt;
        this.Pl = pl;

        // loop 2 - food from investment in agricultural inputs
        this.Alai1 = alai1;
        this.Alai2 = alai2;
        this.Io70 = io70;
        this.Lyf1 = lyf1;
        this.Lyf2 = lyf2;

        // loop 1 & 2 - the investment allocation decision
        this.Sd = sd;

        // loop 3 -land erosion and urban-industrial use
        this.Uili = uili;
        this.Alln = alln;
        this.Uildt = uildt;

        // loop 4 - land fertility degradation
        this.Lferti = lferti;

        // loop 5 - land fertility regeneration
        this.Ilf = ilf;

        // loop 6 - discontinuing land maintenance
        this.Fspd = fspd;
        this.Sfpc = sfpc;
    }

    // State variable, requires previous step only
    private void UpdateAl(int k, int j)
    {
        if (k == 0)
        {
            this.Al[0] = this.Ali;
        }
        else
        {
            this.Al[k] = this.Al[j] + this.Dt * (this.Ldr[j] - this.Ler[j] - this.Lrui[j]);
        }
    }

    // State variable, requires previous step only
    private void UpdatePal(int k, int j)
    {
        if (k == 0)
        {
            this.Pal[0] = this.Pali;
        }
        else
        {
            this.Pal[k] = this.Pal[j] - this.Dt * this.Ldr[j];
        }
    }

    // State variable, requires previous step only
    private void UpdateUil(int k, int j)
    {
        if (k == 0)
        {
            this.Uil[0] = this.Uili;
        }
        else
        {
            this.Uil[k] = this.Uil[j] + this.Dt * this.Lrui[j];
        }
    }

    // State variable, requires previous step only
    private void UpdateLfert(int k, int j)
    {
        if (k == 0)
        {
            this.Lfert[0] = this.Lferti;
        }
        else
        {
            this.Lfert[k] = this.Lfert[j] + this.Dt * (this.Lfr[j] - this.Lfd[j]);
        }
    }

    // From step k requires: AL
    [DependsOn("AL")]
    private void UpdateLfc(int k) => this.Lfc[k] = this.Al[k] / this.Palt;

    // From step k requires: LY AL
    [DependsOn("LY"), DependsOn("AL")]
    private void UpdateF(int k)
        => this.F[k] = this.Ly[k] * this.Al[k] * this.Lfh * (1 - this.Pl);

    // From step k requires: F POP
    [DependsOn("F"), DependsOn("POP")]
    private void UpdateFpc(int k)
        => this.Fpc[k] = this.F[k] / this.Population.Pop[k];

    // From step k requires: IOPC
    [DependsOn("IOPC")]
    private void UpdateIfpc(int k)
    {
        this.Ifpc1[k] = nameof(this.Ifpc1).Interpolate(this.Capital.Iopc[k]);
        this.Ifpc2[k] = nameof(this.Ifpc2).Interpolate(this.Capital.Iopc[k]);
        this.Ifpc[k] = this.ClipPolicyYear(this.Ifpc2[k], this.Ifpc1[k], k);
    }

    // From step k requires: FPC IFPC
    [DependsOn("FPC"), DependsOn("IFPC")]
    private void UpdateFioaa(int k)
    {
        this.Fioaa1[k] = nameof(this.Fioaa1).Interpolate(this.Fpc[k] / this.Ifpc[k]);
        this.Fioaa2[k] = nameof(this.Fioaa2).Interpolate(this.Fpc[k] / this.Ifpc[k]);
        this.Fioaa[k] = this.ClipPolicyYear(this.Fioaa2[k], this.Fioaa1[k], k);
    }

    // From step k requires: IO FIOAA
    [DependsOn("IO"), DependsOn("FIOAA")]
    private void UpdateTai(int k) => this.Tai[k] = this.Capital.Io[k] * this.Fioaa[k];

    // From step k requires: PAL
    [DependsOn("PAL")]
    private void UpdateDcph(int k)
        => this.Dcph[k] = nameof(this.Dcph).Interpolate(this.Pal[k] / this.Palt);

    // From step k requires: AIPH
    [DependsOn("AIPH")]
    private void UpdateMlymc(int k)
        => this.Mlymc[k] = nameof(this.Mlymc).Interpolate(this.Aiph[k]);

    // From step k requires: ALAI LY MLYMC LYMC
    [DependsOn("ALAI"), DependsOn("LY"), DependsOn("MLYMC"), DependsOn("LYMC")]
    private void UpdateMpai(int k)
        => this.Mpai[k] = this.Alai[k] * this.Ly[k] * this.Mlymc[k] / this.Lymc[k];

    // From step k requires: LY DCPH
    [DependsOn("LY"), DependsOn("DCPH")]
    private void UpdateMpld(int k)
        => this.Mpld[k] = this.Ly[k] / (this.Dcph[k] * this.Sd);

    // From step k requires: MPLD MPAI
    [DependsOn("MPLD"), DependsOn("MPAI")]
    private void UpdateFiald(int k)
        => this.Fiald[k] = nameof(this.Fiald).Interpolate(this.Mpld[k] / this.Mpai[k]);

    // From step k requires: TAI FIALD DCPH
    [DependsOn("TAI"), DependsOn("FIALD"), DependsOn("DCPH")]
    private void UpdateLdr(int k) => this.Ldr[k] = this.Tai[k] * this.Fiald[k] / this.Dcph[k];

    // From step k requires: TAI FIALD
    [DependsOn("TAI"), DependsOn("FIALD")]
    private void UpdateCai(int k) => this.Cai[k] = this.Tai[k] * (1 - this.Fiald[k]);

    // From step k requires: nothing
    private void UpdateAlai(int k)
        => this.Alai[k] = this.ClipPolicyYear(this.Alai2, this.Alai1, k);

    // From step k=0 requires: CAI, else nothing
    // [DependsOn("CAI")]
    private void UpdateAi(int k)
    {
        if (k == 0)
        {
            this.Ai[0] = 5e9;
        }
        else
        {
            this.Ai[k] = this.Smooth(nameof(this.Cai), k, this.Alai[k]);
        }
    }

    // From step k=0 requires: FR, else nothing
    // [DependsOn("FR")]
    private void UpdatePfr(int k)
    {
        if (k == 0)
        {
            this.Pfr[0] = 1;
        }
        else
        {
            this.Pfr[k] = this.Smooth(nameof(this.Fr), k, this.Fspd);
        }
    }
    
    // From step k requires: PFR
    [DependsOn("PFR")]
    private void UpdateFalm(int k)
        => this.Falm[k] = nameof(this.Falm).Interpolate(this.Pfr[k]);

    // From step k requires: FPC
    [DependsOn("FPC")]
    private void UpdateFr(int k) => this.Fr[k] = this.Fpc[k] / this.Sfpc;

    // From step k requires: AI FALM AL
    [DependsOn("AI"), DependsOn("FALM"), DependsOn("AL")]
    private void UpdateAiph(int k)
        => this.Aiph[k] = this.Ai[k] * (1 - this.Falm[k]) / this.Al[k];

    // From step k requires: AIPH
    [DependsOn("AIPH")]
    private void UpdateLymc(int k)
        => this.Lymc[k] = nameof(this.Lymc).Interpolate(this.Aiph[k]);

    // From step k requires: nothing
    private void UpdateLyf(int k)
        => this.Lyf[k] = this.ClipPolicyYear(this.Lyf2, this.Lyf1, k);

    // From step k requires: IO
    [DependsOn("IO")]
    private void UpdateLymap(int k)
    {
        double temp = this.Capital.Io[k] / this.Io70;
        this.Lymap1[k] = nameof(this.Lymap1).Interpolate(temp);
        this.Lymap2[k] = nameof(this.Lymap2).Interpolate(temp);
        this.Lymap[k] = this.ClipPolicyYear(this.Lymap2[k], this.Lymap1[k], k);
    }

    // From step k requires: PPOLX
    [DependsOn("PPOLX")]
    private void UpdateLfdr(int k)
        => this.Lfdr[k] = nameof(this.Lfdr).Interpolate(this.Pollution.Ppolx[k]);

    // From step k requires: LFERT LFDR
    [DependsOn("LFERT"), DependsOn("LFDR")]
    private void UpdateLfd(int k) => this.Lfd[k] = this.Lfert[k] * this.Lfdr[k];

    // From step k requires: LYF LFERT LYMC LYMAP
    [DependsOn("LYF"), DependsOn("LFERT"), DependsOn("LYMC"), DependsOn("LYMAP")]
    private void UpdateLy(int k)
        => this.Ly[k] = this.Lyf[k] * this.Lfert[k] * this.Lymc[k] * this.Lymap[k];

    // From step k requires: LLMY
    [DependsOn("LLMY")]
    private void UpdateAll(int k) => this.All[k] = this.Alln * this.Llmy[k];

    // From step k requires: LY
    [DependsOn("LY")]
    private void UpdateLlmy(int k)
    {
        double temp = this.Ly[k] / this.Ilf;
        this.Llmy1[k] = nameof(this.Llmy1).Interpolate(temp);
        this.Llmy2[k] = nameof(this.Llmy2).Interpolate(temp);
        this.Llmy[k] = this.ClipPolicyYear(this.Llmy2[k], this.Llmy1[k], k);
    }

    // From step k requires: AL ALL
    [DependsOn("AL"), DependsOn("ALL")]
    private void UpdateLer(int k) => this.Ler[k] = this.Al[k] / this.All[k];

    // From step k requires: IOPC
    [DependsOn("IOPC")]
    private void UpdateUilpc(int k)
        => this.Uilpc[k] = nameof(this.Uilpc).Interpolate(this.Capital.Iopc[k]);

    // From step k requires: UILPC POP
    [DependsOn("UILPC"), DependsOn("POP")]
    private void UpdateUilr(int k) => this.Uilr[k] = this.Uilpc[k] * this.Population.Pop[k];

    // From step k requires: UILR UIL
    [DependsOn("UILR"), DependsOn("UIL")]
    private void UpdateLrui(int k)
        => this.Lrui[k] = Math.Max(0, (this.Uilr[k] - this.Uil[k]) / this.Uildt);

    // From step k requires: LFERT LFRT
    [DependsOn("LFERT"), DependsOn("LFRT")]
    private void UpdateLfr(int k)
        => this.Lfr[k] = (this.Ilf - this.Lfert[k]) / this.Lfrt[k];

    // From step k requires: FALM
    [DependsOn("FALM")]
    private void UpdateLfrt(int k)
        => this.Lfrt[k] = nameof(this.Lfrt).Interpolate(this.Falm[k]);
}
