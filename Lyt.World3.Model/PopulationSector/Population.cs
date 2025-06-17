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
        // Initialize the state and rate variables of the population sector
        this.InitializeLists(this.N, double.NaN);
    }

    public void InitializeConstants (
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
        this.Dcfsn = dcfsn; this.Fcest = fcest; 
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

    public void Update (int k, int j, int jk)
    {
        this.UpdateStateP1(k, j, jk);
        this.UpdateStateP2(k, j, jk);
        this.UpdateStateP3(k, j, jk);
        this.UpdateStateP4(k, j, jk);
        this.UpdateStatePop(k);
    }

    public void SetDelayFunctions ()
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
    public List<double> Frsm { get; private set; } = [];

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

    public static bool IsListOfDouble(Type type)
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
        => this.P4[k] = this.P4[j] + this.Dt * (this.Mat3[jk] - this.D4[jk] );

    private void UpdateStatePop(int k)
        => this.P1[k] = this.P1[k] + this.P2[k] + this.P3[k] + this.P4[k] ;
}

/*  
 *  
*/
