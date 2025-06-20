namespace Lyt.World3.Model;

using static System.Runtime.InteropServices.JavaScript.JSType;

// World could maybe become a Sector ? 

/// <summary>
///     The World3 model as it is described in the technical book [ref 1]. 
///     World3 is structured in 5 main sectors and contains 12 state variables.
///     The figures in the first prints of the Limits to Growth [ref 2] result from an older 
///     version of the model, with slighly different numerical parameters and some missing 
///     dynamical phenomena.
///     References
///     ----------
///     [1] Meadows, Dennis L., William W.Behrens, Donella H. Meadows, Roger F.
///         Naill, Jørgen Randers, and Erich Zahn. * Dynamics of growth in a finite
///         world*. Cambridge, MA: Wright-Allen Press, 1974.
///
///     [2] Meadows, Donella H., Dennis L. Meadows, Jorgen Randers, and William
///        W.Behrens. * The limits to growth*. New York 102, no. 1972 (1972): 27.    
/// 
/// </summary>
public sealed class World
{
    #region Documentation 
    /*
        year_min : float, optional
            start year of the simulation[year]. The default is 1900.
        year_max : float, optional
            end year of the simulation[year]. The default is 2100.
        dt : float, optional
            time step of the simulation[year]. The default is 1.
        pyear : float, optional
            implementation date of new policies[year]. The default is 1975.
        iphst : float, optional
            implementation date of new policy on health service time[year]. The default is 1940.
    */
    #endregion Documentation 

    public World(
        double yearMin = 1900, double yearMax = 2100,
        double dt = 1,
        double policyYear = 1975, double iphst = 1940,
        bool isVerbose = false)
    {
        this.YearMin = yearMin;
        this.YearMax = yearMax;
        this.Dt = dt;
        this.PolicyYear = policyYear;
        this.Iphst = iphst;
        this.IsVerbose = isVerbose;

        // Initialize length, counts and time array 
        this.Length = (int)(yearMax - yearMin);
        this.N = (int)(this.Length / this.Dt);
        this.Time = new double[1 + this.Length];
        double currentTime = this.YearMin;
        for (int i = 0; i <= this.Length; ++i)
        {
            this.Time[i] = currentTime;
            currentTime += this.Dt;
        }

        this.Agriculture = new Agriculture(this);
        this.Capital = new Capital(this);
        this.Pollution = new Pollution(this);
        this.Population = new Population(this);
        this.Resource = new Resource(this);

        this.Sectors = [this.Agriculture, this.Capital, this.Pollution, this.Population, this.Resource];
    }

    // Initialize the sector ( == initial loop with k=0).
    public void Initialize()
    {
        foreach (var sector in this.Sectors)
        {
            // BUG 
            // FAils to initialize in delays three 
            // sector.Initialize();
        }
    }

    // Update one loop of the sector.
    public void Update(int k, int j, int jk, int kl)
    {
        foreach (var sector in this.Sectors)
        {
            // sector.Update(k, j, jk, kl);
        }
    }

    public int Length { get; private set; }

    public int N { get; private set; }

    public double[] Time { get; private set; }

    public List<Sector> Sectors { get; private set; }

    public Dictionary<string, Smooth> Smooths { get; private set; } = [];

    public Dictionary<string, DelayInformationThree> DelayInfThrees { get; private set; } = [];

    public Dictionary<string, DelayThree> DelayThrees { get; private set; } = [];

    public double Smooth(string key, int k, double delay)
    {
        if (!this.Smooths.TryGetValue(key, out Smooth? smooth))
        {
            throw new Exception("Missing smooth:  " + key);
        }

        return smooth.Call(k, delay);
    }

    public double DelayInfThree(string key, int k, double delay)
    {
        if (!this.DelayInfThrees.TryGetValue(key, out DelayInformationThree? delayInfThree))
        {
            throw new Exception("Missing DelayInfThree:  " + key);
        }

        return delayInfThree.Call(k, delay);
    }

    public double DelayThree(string key, int k, double delay)
    {
        if (!this.DelayThrees.TryGetValue(key, out DelayThree? delayThree))
        {
            throw new Exception("Missing DelayThree:  " + key);
        }

        return delayThree.Call(k, delay);
    }

    // The five sectors 
    public Agriculture Agriculture { get; private set; }

    public Capital Capital { get; private set; }

    public Pollution Pollution { get; private set; }

    public Population Population { get; private set; }

    public Resource Resource { get; private set; }

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

    // Print information for debugging.The default is False.
    public bool IsVerbose { get; private set; } = false;
}
