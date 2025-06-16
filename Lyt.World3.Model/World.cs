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

        this.Tables = World.LoadTables("functions_table_world3");
        this.Agriculture = new Agriculture(yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Capital = new Capital(yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Pollution = new Pollution(yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Population = new Population(yearMin, yearMax, dt, policyYear, iphst, isVerbose);
        this.Resource = new Resource(yearMin, yearMax, dt, policyYear, iphst, isVerbose);
    }

    public List<Table> Tables { get; private set; }

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

    public static List<Table> LoadTables(string resourceFileName)
    {
        try
        {
            resourceFileName += ".json";
            string serialized = SerializationUtilities.LoadEmbeddedTextResource(resourceFileName, out string? resourceFullName);
            return SerializationUtilities.Deserialize<List<Table>>(serialized);
        }
        catch
        {
            return [];
        }
    }
}
