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
