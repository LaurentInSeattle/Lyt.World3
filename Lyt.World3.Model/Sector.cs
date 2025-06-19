namespace Lyt.World3.Model;

public abstract class Sector
{
    public Sector(
        World world,
        double yearMin, double yearMax,
        double dt,
        double policyYear, double iphst,
        bool isVerbose = false)
    {
        this.World = world;
        this.YearMin = yearMin;
        this.YearMax = yearMax;
        this.Dt = dt;
        this.PolicyYear = policyYear;
        this.Iphst = iphst;
        this.IsVerbose = isVerbose;
        this.Length = (int)(yearMax - yearMin);
        this.N = (int)(this.Length / this.Dt);
        this.Time = new double[1 + this.Length];

        // MUST DO !!! 
        // self.time = np.arange(self.year_min, self.year_max, self.dt)
        this.SetDelayFunctions(); 
    }

    // Initialize the sector ( == initial loop with k=0).
    public abstract void Initialize();

    // Update one loop of the sector.
    public abstract void Update(int k, int j, int jk, int kl);

    // Setup the delay objects and functions for the sector.
    protected abstract void SetDelayFunctions(); 

    public World World { get; private set; }

    // start year of the simulation[year]. The default is 1900.    
    public double YearMin { get; private set; } = 1900;

    // end year of the simulation[year]. The default is 2100.
    public double YearMax { get; private set; } = 2100;

    // time step of the simulation[year]. The default is 1.
    public double Dt { get; private set; } = 1;

    // implementation date of new policies[year]. The default is 1975.
    public double PolicyYear { get; private set; } = 1975;

    // implementation date of new policy on health service time[year] The default is 1940.
    public double Iphst { get; private set; } = 1940;

    // Print information for debugging. The default is False.
    public bool IsVerbose { get; private set; } = false;

    public int Length { get; private set; }

    public double[] Time { get; private set; }

    public int N { get; private set; }

    public Agriculture Agriculture => this.World.Agriculture;

    public Capital Capital => this.World.Capital;

    public Pollution Pollution => this.World.Pollution;

    public Population Population => this.World.Population;

    public Resource Resource => this.World.Resource;

    protected double ClipPolicyYear(double x, double y, int k)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.NaN;
        }

        return MathUtilities.Clip( x, y, this.Time[k], this.PolicyYear);
    }

    protected void CreateSmooth(List<double> smoothedList)
    {
        var smooth = new Smooth(smoothedList, this.Dt, this.Time);
        this.World.Smooths.Add(nameof(smoothedList), smooth);
    }

    protected double Smooth(string key, int k, double delay)
        => this.World.Smooth(key, k, delay);

    protected void CreateDelayInfThree(List<double> delayedList)
    {
        var delay3 = new DelayInformationThree(delayedList, this.Dt, this.Time);
        this.World.DelayInfThrees.Add(nameof(delayedList), delay3);
    }

    protected double DelayInfThree(string key, int k, double delay)
        => this.World.DelayInfThree(key, k, delay);

    protected void CreateDelayThree(List<double> delayedList)
    {
        var delay3 = new DelayThree(delayedList, this.Dt, this.Time);
        this.World.DelayThrees.Add(nameof(delayedList), delay3);
    }

    protected double DelayThree(string key, int k, double delay)
        => this.World.DelayThree(key, k, delay);

    protected static void InitializeLists(Sector sector, int size, double value)
    {
        var type = sector.GetType();
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
                setter.Invoke(sector, [list]);
            }
        }
    }

    protected static bool IsListOfDouble(Type type) => IsListOf<double>(type); 

    // TODO: Move this method into some library 
    protected static bool IsListOf<T>(Type type)
    {
        // Check if the type is a generic type and if its generic type definition is List<>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            // Get the generic arguments (the types inside the List<>)
            Type[] genericArguments = type.GetGenericArguments();

            // Check if there is exactly one generic argument and if it is of type T
            if (genericArguments.Length == 1 && genericArguments[0] == typeof(T))
            {
                return true;
            }
        }

        return false;
    }
}
