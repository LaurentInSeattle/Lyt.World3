namespace Lyt.World3.Model.AgricultureSector;

using static MathUtilities;

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
        => Sector.InitializeLists(this, this.N, double.NaN);

    #region Constants, States and Rates 

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

    #endregion Constants, State and Rates 

    // Delays of the Agriculture Sector 
    protected override void SetDelayFunctions()
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

    // Initialize the Capital sector ( == initial loop with k=0).
    public override void Initialize()
    {
        try
        {
            // Set initial conditions
            this.Al[0] = this.Ali;
            this.Pal[0] = this.Pali;
            this.Uil[0] = this.Uili;
            this.Lfert[0] = this.Lferti;

            // ??? 
            this.Ai[0] = 5e9;
            this.Pfr[0] = 1;

            //Loop 1                                       
            this.UpdateLfc(0);
            this.UpdateF(0);
            this.UpdateFpc(0);
            this.UpdateIfpc(0);
            this.UpdateFioaa(0);
            this.UpdateTai(0);
            this.UpdateDcph(0);

            // loop 1&2                                
            this.UpdateMlymc(0);
            this.UpdateMpai(0);
            this.UpdateMpld(0);
            this.UpdateFiald(0);

            //  back to loop 1                             
            this.UpdateLdr(0, 0);

            //  loop 2                                     
            this.UpdateCai(0);
            this.UpdateAlai(0);

            //  loop 6                                     
            this.UpdateFalm(0);
            this.UpdateFr(0);

            //  back to loop 2                             
            this.UpdateAiph(0);
            this.UpdateLymc(0);
            this.UpdateLyf(0);
            this.UpdateLymap(0);

            //  loop 4                                     
            this.UpdateLfdr(0);

            // back to loop 2                             
            this.UpdateLfd(0, 0);
            this.UpdateLy(0);

            //  loop 3                                   
            this.UpdateAll(0);
            this.UpdateLlmy(0);
            this.UpdateLer(0, 0);
            this.UpdateUilpc(0);
            this.UpdateUilr(0);
            this.UpdateLrui(0, 0);

            //  loop 5                                     
            this.UpdateLfr(0, 0);
            this.UpdateLfrt(0);

            // recompute supplementary initial conditions
            this.UpdateAi(0);
            this.UpdatePfr(0);
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
            //  Update state variables
            this.UpdateAl(k, j, jk);
            this.UpdatePal(k, j, jk);
            this.UpdateUil(k, j, jk);
            this.UpdateLfert(k, j, jk);

            //  loop 1
            this.UpdateLfc(k);
            this.UpdateF(k);
            this.UpdateFpc(k);
            this.UpdateIfpc(k);
            this.UpdateFioaa(k);
            this.UpdateTai(k);
            this.UpdateDcph(k);

            // loop 1 & 2
            this.UpdateMlymc(k);
            this.UpdateMpai(k);
            this.UpdateMpld(k);
            this.UpdateFiald(k);

            //  back to loop 1
            this.UpdateLdr(k, kl);

            // loop 2
            this.UpdateCai(k);
            this.UpdateAlai(k);
            this.UpdateAi(k); //  !!! checks cai for all k but useless if >=1

            // loop 6
            this.UpdatePfr(k);
            this.UpdateFalm(k);
            this.UpdateFr(k);

            // back to loop 2
            this.UpdateAiph(k);
            this.UpdateLymc(k);
            this.UpdateLyf(k);
            this.UpdateLymap(k);

            // loop 4
            this.UpdateLfdr(k);

            // back to loop 2
            this.UpdateLfd(k, kl);
            this.UpdateLy(k);

            // loop 3
            this.UpdateAll(k);
            this.UpdateLlmy(k);
            this.UpdateLer(k, kl);
            this.UpdateUilpc(k);
            this.UpdateUilr(k);
            this.UpdateLrui(k, kl);

            //  loop 5
            this.UpdateLfr(k, kl);
            this.UpdateLfrt(k);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    private void UpdateAl(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdatePal(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdateUil(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdateLfert(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdateLfc(int k) => throw new NotImplementedException();
    private void UpdateF(int k) => throw new NotImplementedException();
    private void UpdateFpc(int k) => throw new NotImplementedException();
    private void UpdateIfpc(int k) => throw new NotImplementedException();
    private void UpdateFioaa(int k) => throw new NotImplementedException();
    private void UpdateTai(int k) => throw new NotImplementedException();
    private void UpdateDcph(int k) => throw new NotImplementedException();
    private void UpdateMlymc(int k) => throw new NotImplementedException();
    private void UpdateMpai(int k) => throw new NotImplementedException();
    private void UpdateMpld(int k) => throw new NotImplementedException();
    private void UpdateFiald(int k) => throw new NotImplementedException();
    private void UpdateLdr(int k, int kl) => throw new NotImplementedException();
    private void UpdateCai(int k) => throw new NotImplementedException();
    private void UpdateAlai(int k) => throw new NotImplementedException();
    private void UpdateAi(int k) => throw new NotImplementedException();
    private void UpdatePfr(int k) => throw new NotImplementedException();
    private void UpdateFalm(int k) => throw new NotImplementedException();
    private void UpdateFr(int k) => throw new NotImplementedException();
    private void UpdateAiph(int k) => throw new NotImplementedException();
    private void UpdateLymc(int k) => throw new NotImplementedException();
    private void UpdateLyf(int k) => throw new NotImplementedException();
    private void UpdateLymap(int k) => throw new NotImplementedException();
    private void UpdateLfdr(int k) => throw new NotImplementedException();
    private void UpdateLfd(int k, int kl) => throw new NotImplementedException();
    private void UpdateLy(int k) => throw new NotImplementedException();
    private void UpdateAll(int k) => throw new NotImplementedException();
    private void UpdateLlmy(int k) => throw new NotImplementedException();
    private void UpdateLer(int k, int kl) => throw new NotImplementedException();
    private void UpdateUilpc(int k) => throw new NotImplementedException();
    private void UpdateUilr(int k) => throw new NotImplementedException();
    private void UpdateLrui(int k, int kl) => throw new NotImplementedException();
    private void UpdateLfr(int k, int kl) => throw new NotImplementedException();
    private void UpdateLfrt(int k) => throw new NotImplementedException();
}

/*
    @requires(["lfc"], ["al"])
    def _update_lfc(self, k):
        """
        From step k requires: AL
        """
        self.lfc[k] = self.al[k] / self.palt

    @requires(["al"])
    def _update_state_al(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        self.al[k] = self.al[j] + self.dt * (self.ldr[jk] - self.ler[jk] -
                                             self.lrui[jk])

    @requires(["pal"])
    def _update_state_pal(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        self.pal[k] = self.pal[j] - self.dt * self.ldr[jk]

    @requires(["f"], ["ly", "al"])
    def _update_f(self, k):
        """
        From step k requires: LY AL
        """
        self.f[k] = self.ly[k] * self.al[k] * self.lfh * (1 - self.pl)

    @requires(["fpc"], ["f", "pop"])
    def _update_fpc(self, k):
        """
        From step k requires: F POP
        """
        self.fpc[k] = self.f[k] / self.pop[k]

    @requires(["ifpc1", "ifpc2", "ifpc"], ["iopc"])
    def _update_ifpc(self, k):
        """
        From step k requires: IOPC
        """
        self.ifpc1[k] = self.ifpc1_f(self.iopc[k])
        self.ifpc2[k] = self.ifpc2_f(self.iopc[k])
        self.ifpc[k] = clip(self.ifpc2[k], self.ifpc1[k], self.time[k],
                            self.pyear)

    @requires(["tai"], ["io", "fioaa"])
    def _update_tai(self, k):
        """
        From step k requires: IO FIOAA
        """
        self.tai[k] = self.io[k] * self.fioaa[k]

    @requires(["fioaa1", "fioaa2", "fioaa"], ["fpc", "ifpc"])
    def _update_fioaa(self, k):
        """
        From step k requires: FPC IFPC
        """
        self.fioaa1[k] = self.fioaa1_f(self.fpc[k] / self.ifpc[k])
        self.fioaa2[k] = self.fioaa2_f(self.fpc[k] / self.ifpc[k])
        self.fioaa[k] = clip(self.fioaa2[k], self.fioaa1[k], self.time[k],
                             self.pyear)

    @requires(["ldr"], ["tai", "fiald", "dcph"])
    def _update_ldr(self, k, kl):
        """
        From step k requires: TAI FIALD DCPH
        """
        self.ldr[kl] = self.tai[k] * self.fiald[k] / self.dcph[k]

    @requires(["dcph"], ["pal"])
    def _update_dcph(self, k):
        """
        From step k requires: PAL
        """
        self.dcph[k] = self.dcph_f(self.pal[k] / self.palt)

    @requires(["cai"], ["tai", "fiald"])
    def _update_cai(self, k):
        """
        From step k requires: TAI FIALD
        """
        self.cai[k] = self.tai[k] * (1 - self.fiald[k])

    # OPTIMIZE checks more than necessary (cai[k] for k>=1)
    @requires(["ai"], ["cai", "alai"])
    def _update_ai(self, k):
        """
        From step k=0 requires: CAI, else nothing
        """
        self.ai[k] = self.smooth_cai(k, self.alai[k])

    @requires(["alai"])
    def _update_alai(self, k):
        """
        From step k requires: nothing
        """
        self.alai[k] = clip(self.alai2, self.alai1, self.time[k],
                            self.pyear)

    @requires(["aiph"], ["ai", "falm", "al"])
    def _update_aiph(self, k):
        """
        From step k requires: AI FALM AL
        """
        self.aiph[k] = self.ai[k] * (1 - self.falm[k]) / self.al[k]

    @requires(["lymc"], ["aiph"])
    def _update_lymc(self, k):
        """
        From step k requires: AIPH
        """
        self.lymc[k] = self.lymc_f(self.aiph[k])

    @requires(["ly"], ["lyf", "lfert", "lymc", "lymap"])
    def _update_ly(self, k):
        """
        From step k requires: LYF LFERT LYMC LYMAP
        """
        self.ly[k] = self.lyf[k] * self.lfert[k] * self.lymc[k] * self.lymap[k]

    @requires(["lyf"])
    def _update_lyf(self, k):
        """
        From step k requires: nothing
        """
        self.lyf[k] = clip(self.lyf2, self.lyf1, self.time[k],
                           self.pyear)

    @requires(["lymap1", "lymap2", "lymap"], ["io"])
    def _update_lymap(self, k):
        """
        From step k requires: IO
        """
        self.lymap1[k] = self.lymap1_f(self.io[k] / self.io70)
        self.lymap2[k] = self.lymap2_f(self.io[k] / self.io70)
        self.lymap[k] = clip(self.lymap2[k], self.lymap1[k], self.time[k],
                             self.pyear)

    @requires(["fiald"], ["mpld", "mpai"])
    def _update_fiald(self, k):
        """
        From step k requires: MPLD MPAI
        """
        self.fiald[k] = self.fiald_f(self.mpld[k] / self.mpai[k])

    @requires(["mpld"], ["ly", "dcph"])
    def _update_mpld(self, k):
        """
        From step k requires: LY DCPH
        """
        self.mpld[k] = self.ly[k] / (self.dcph[k] * self.sd)

    @requires(["mpai"], ["alai", "ly", "mlymc", "lymc"])
    def _update_mpai(self, k):
        """
        From step k requires: ALAI LY MLYMC LYMC
        """
        self.mpai[k] = self.alai[k] * self.ly[k] * self.mlymc[k] / self.lymc[k]

    @requires(["mlymc"], ["aiph"])
    def _update_mlymc(self, k):
        """
        From step k requires: AIPH
        """
        self.mlymc[k] = self.mlymc_f(self.aiph[k])

    @requires(["all"], ["llmy"])
    def _update_all(self, k):
        """
        From step k requires: LLMY
        """
        self.all[k] = self.alln * self.llmy[k]

    @requires(["llmy1", "llmy2", "llmy"], ["ly"])
    def _update_llmy(self, k):
        """
        From step k requires: LY
        """
        self.llmy1[k] = self.llmy1_f(self.ly[k] / self.ilf)
        self.llmy2[k] = self.llmy2_f(self.ly[k] / self.ilf)
        self.llmy[k] = clip(self.llmy2[k], self.llmy1[k], self.time[k],
                            self.pyear)

    @requires(["ler"], ["al", "all"])
    def _update_ler(self, k, kl):
        """
        From step k requires: AL ALL
        """
        self.ler[kl] = self.al[k] / self.all[k]

    @requires(["uilpc"], ["iopc"])
    def _update_uilpc(self, k):
        """
        From step k requires: IOPC
        """
        self.uilpc[k] = self.uilpc_f(self.iopc[k])

    @requires(["uilr"], ["uilpc", "pop"])
    def _update_uilr(self, k):
        """
        From step k requires: UILPC POP
        """
        self.uilr[k] = self.uilpc[k] * self.pop[k]

    @requires(["lrui"], ["uilr", "uil"])
    def _update_lrui(self, k, kl):
        """
        From step k requires: UILR UIL
        """
        self.lrui[kl] = np.maximum(0,
                                   (self.uilr[k] - self.uil[k]) / self.uildt)

    @requires(["uil"])
    def _update_state_uil(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        self.uil[k] = self.uil[j] + self.dt * self.lrui[jk]

    @requires(["lfert"])
    def _update_state_lfert(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        self.lfert[k] = self.lfert[j] + self.dt * (self.lfr[jk] - self.lfd[jk])

    @requires(["lfdr"], ["ppolx"])
    def _update_lfdr(self, k):
        """
        From step k requires: PPOLX
        """
        self.lfdr[k] = self.lfdr_f(self.ppolx[k])

    @requires(["lfd"], ["lfert", "lfdr"])
    def _update_lfd(self, k, kl):
        """
        From step k requires: LFERT LFDR
        """
        self.lfd[kl] = self.lfert[k] * self.lfdr[k]

    @requires(["lfr"], ["lfert", "lfrt"])
    def _update_lfr(self, k, kl):
        """
        From step k requires: LFERT LFRT
        """
        self.lfr[kl] = (self.ilf - self.lfert[k]) / self.lfrt[k]

    @requires(["lfrt"], ["falm"])
    def _update_lfrt(self, k):
        """
        From step k requires: FALM
        """
        self.lfrt[k] = self.lfrt_f(self.falm[k])

    @requires(["falm"], ["pfr"])
    def _update_falm(self, k):
        """
        From step k requires: PFR
        """
        self.falm[k] = self.falm_f(self.pfr[k])

    @requires(["fr"], ["fpc"])
    def _update_fr(self, k):
        """
        From step k requires: FPC
        """
        self.fr[k] = self.fpc[k] / self.sfpc

    @requires(["pfr"], ["fr"], check_after_init=False)
    def _update_pfr(self, k):
        """
        From step k=0 requires: FR, else nothing
        """
        self.pfr[k] = self.smooth_fr(k, self.fspd)
*/