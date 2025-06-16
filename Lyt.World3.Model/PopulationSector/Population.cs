namespace Lyt.World3.Model.PopulationSector;

/// <summary>
/// Population sector with four age levels. Can be run independantly from other sectors with 
/// exogenous inputs. The initial code is defined p.170.
/// </summary>
public sealed class Population : Sector
{
    public Population(
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false) : base(yearMin, yearMax, dt, policyYear, iphst, isVerbose)
    {

    }

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

    // Rates 

    // maturation rate, age 14-15 [persons/year].
    public List<double> Mat1 { get; private set; } = [];

    // maturation rate, age 44-45 [persons/year].
    public List<double> Mat2 { get; private set; } = [];

    // maturation rate, age 64-65 [persons/year].
    public List<double> Mat3 { get; private set; } = [];

    // Death Rate Subsector 

    /*
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

     */

    // Birth Rate Subsector 
    /*
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

}

/*  
 *  
*/
