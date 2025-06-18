namespace Lyt.World3.Model;

public class Sector
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

        // self.time = np.arange(self.year_min, self.year_max, self.dt)
    }

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

    // Print information for debugging.The default is False.
    public bool IsVerbose { get; private set; } = false;

    public int Length { get; private set; }

    public double[] Time { get; private set; }

    public int N { get; private set; }

    public Agriculture Agriculture => this.World.Agriculture;

    public Capital Capital => this.World.Capital;

    public Pollution Pollution => this.World.Pollution;

    public Population Population => this.World.Population;

    public Resource Resource => this.World.Resource;

    public double Smooth(string key, int k, double delay)
        => this.World.Smooth(key, k, delay);

    public double DelayInfThree(string key, int k, double delay)
        => this.World.DelayInfThree(key, k, delay);

    public double DelayThree(string key, int k, double delay)
        => this.World.DelayThree(key, k, delay);
}
