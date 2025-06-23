// #define VERBOSE_Dependencies

namespace Lyt.World3.Model;

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
        double policyYear = 1975, double iphst = 1940)
    {
        this.YearMin = yearMin;
        this.YearMax = yearMax;
        this.Dt = dt;
        this.PolicyYear = policyYear;
        this.Iphst = iphst;

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

        // The ordering of sectors is important! 
        this.Sectors =
        [
            this.Population,
            this.Capital,
            this.Agriculture,
            this.Pollution,
            this.Resource
        ];

        foreach (var sector in this.Sectors)
        {
            sector.SetDelayFunctions();
        }

        this.Equations = new(256);
        foreach (var sector in this.Sectors)
        {
            var methods = sector.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            if (methods.Length == 0)
            {
                throw new Exception("No methods");
            }

            foreach (var method in methods)
            {
                if (!method.Name.StartsWith("Update"))
                {
                    continue;
                }

                var equation = new Equation(sector, method);
                this.Equations.Add(equation);
            }
        }

        this.ResolveDependencies();
    }

    public int Length { get; private set; }

    public int N { get; private set; }

    public double[] Time { get; private set; }

    public List<Sector> Sectors { get; private set; }

    public List<Equation> Equations { get; private set; }

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

    public Dictionary<string, Smooth> Smooths { get; private set; } = [];

    public Dictionary<string, DelayInformationThree> DelayInfThrees { get; private set; } = [];

    public Dictionary<string, DelayThree> DelayThrees { get; private set; } = [];

    // Initialize all sectors
    // Initialze constants and run an initial loop with k=0
    public void Initialize()
    {
        this.Population.InitializeConstants();
        this.Capital.InitializeConstants();
        this.Agriculture.InitializeConstants();
        this.Pollution.InitializeConstants();
        this.Resource.InitializeConstants();

        foreach (var equation in this.Equations)
        {
            equation.Evaluate(0);
            equation.CheckNan(0);
        }

        foreach (var equation in this.Equations)
        {
            equation.Evaluate(0);
        }
    }

    // Update one loop for all sectors.
    public void Update(int k)
    {
        foreach (var equation in this.Equations)
        {
            equation.Evaluate(k);
            equation.CheckNan(k);
        }
    }

    private void ResolveDependencies()
    {
        Debug.WriteLine("Resolving dependencies... " + this.Equations.Count + " Equations");

        bool done = false;
        int evaluationOrder = 0;
        HashSet<string> resolvedProperties = new(256);
        var resolvedEquations =
            (from equation in this.Equations
             where equation.IsResolved
             select equation);
        foreach (var resolvedEquation in resolvedEquations)
        {
            resolvedEquation.EvaluationOrder = evaluationOrder;
            resolvedProperties.Add(resolvedEquation.PropertyName);
#if VERBOSE_Dependencies
            Debug.WriteLine(" Independant Equation: " + resolvedEquation.PropertyName);
#endif // VERBOSE_Dependencies
        }

        Debug.WriteLine(resolvedProperties.Count + " Independant Equations");

        ++evaluationOrder;
        while (!done)
        {
            int resolvedEquationsCount = 0;
            var unresolvedEquations =
                (from equation in this.Equations
                 where !equation.IsResolved
                 select equation);
            foreach (var unresolvedEquation in unresolvedEquations)
            {
                bool resolved = unresolvedEquation.TryResolve(resolvedProperties);
                if (resolved)
                {
                    unresolvedEquation.EvaluationOrder = evaluationOrder;
                    resolvedProperties.Add(unresolvedEquation.PropertyName);
#if VERBOSE_Dependencies
                    Debug.WriteLine("Loop: Resolved: " + unresolvedEquation.PropertyName);
#endif // VERBOSE_Dependencies
                    ++resolvedEquationsCount;
                    break;
                }
            }

            int unresolvedEquationsCount =
                (from equation in this.Equations
                 where !equation.IsResolved
                 select equation).Count();

#if VERBOSE_Dependencies
            if (resolvedEquationsCount == 0)
            {
                Debug.WriteLine("Unresolved Equations Count: " + unresolvedEquationsCount);
                foreach (var unresolvedEquation in unresolvedEquations)
                {
                    Debug.WriteLine(unresolvedEquation.ToDebugString(resolvedProperties));
                }

                if (Debugger.IsAttached) { Debugger.Break(); }
            }
            else
            {
                Debug.WriteLine("Loop: " + evaluationOrder);
                Debug.WriteLine("    Resolved Equations Count: " + resolvedEquationsCount);
                Debug.WriteLine("    Unresolved Equations Count: " + unresolvedEquationsCount);
            }
#else
            if (resolvedEquationsCount == 0)
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            }
#endif // VERBOSE_Dependencies

            ++evaluationOrder;
            if (unresolvedEquationsCount == 0)
            {
                Debug.WriteLine("All dependencies resolved, No Unresolved Equations");
                Debug.WriteLine(this.Equations.Count + " Equations");
                done = true;
            }
        }

        var sortedEquations =
            (from equation in this.Equations
             orderby equation.EvaluationOrder ascending
             select equation).ToList();
        this.Equations = sortedEquations;
    }
}
