namespace Lyt.World3.Model;

// World could maybe become a Sector ? 
public sealed class World
{
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

        this.Agriculture = new Agriculture(this, yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Capital = new Capital(this, yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Pollution = new Pollution(this, yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Population = new Population(this, yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Resource = new Resource(this, yearMin, yearMax, dt, policyYear, iphst, isVerbose);
    }

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
