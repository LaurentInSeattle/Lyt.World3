﻿namespace Lyt.World3.Model;

public abstract class Sector
{
    public Sector(World world) => this.World = world;

    // Initialize the sector ( == initial loop with k=0).
    public abstract void Initialize();

    // Update one loop of the sector.
    public abstract void Update(int k);

    // Setup the delay objects and functions for the sector.
    public abstract void SetDelayFunctions(); 

    public World World { get; private set; }

    public string Name => this.GetType().Name;

    #region Convenience Properties to access world properties 

    // start year of the simulation[year]. The default is 1900.    
    public double YearMin => this.World.YearMin;

    // end year of the simulation[year]. The default is 2100.
    public double YearMax => this.World.YearMax;

    // implementation date of new policies[year]. The default is 1975.
    public double PolicyYear => this.World.PolicyYear;

    // implementation date of new policy on health service time[year] The default is 1940.
    public double Iphst => this.World.Iphst;

    protected double[] Time => this.World.Time;

    // time step of the simulation[year]. The default is 1.
    public double Dt => this.World.Dt;

    protected int Length => this.World.Length;

    protected int N => this.World.N;

    protected Agriculture Agriculture => this.World.Agriculture;

    protected Capital Capital => this.World.Capital;

    protected Pollution Pollution => this.World.Pollution;

    protected Population Population => this.World.Population;

    protected Resource Resource => this.World.Resource;

    #endregion Convenience Properties to access world properties 

    protected double ClipPolicyYear(double x, double y, int k)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.NaN;
        }

        return MathUtilities.Clip( x, y, this.Time[k], this.PolicyYear);
    }

    protected void CreateSmooth(Named smoothedList)
    {
        var smooth = new Smooth(smoothedList.Payload, this.Dt, this.Time);
        this.World.Smooths.Add(smoothedList.Name, smooth);
    }

    protected double Smooth(string key, int k, double delay)
        => this.World.Smooth(key, k, delay);

    protected void CreateDelayInfThree(Named delayedList)
    {
        var delay3 = new DelayInformationThree(delayedList.Payload, this.Dt, this.Time);
        this.World.DelayInfThrees.Add(delayedList.Name, delay3);
    }

    protected double DelayInfThree(string key, int k, double delay)
        => this.World.DelayInfThree(key, k, delay);

    protected void CreateDelayThree(Named delayedList)
    {
        var delay3 = new DelayThree(delayedList.Payload, this.Dt, this.Time);
        this.World.DelayThrees.Add(delayedList.Name, delay3);
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
