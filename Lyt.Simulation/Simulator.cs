namespace Lyt.Simulation;

public abstract class Simulator
{
    protected readonly List<Equation> EquationsList;
    protected readonly List<Level> Levels;
    protected readonly List<Rate> Rates;

    protected Dictionary<string, Auxiliary> Auxiliaries;
    protected List<Auxiliary> OrderedAuxiliaries;
    protected Dictionary<string, Equation> EquationsDictionary;

    protected string CurrentSector { get; set; }
    protected string CurrentSubSector { get; set; }

    private Action? customUpdate;
    private Action? customStart;

    public Simulator()
    {
        this.EquationsList = new(256);
        this.Levels = new (32);
        this.Rates = new (32);
        this.Auxiliaries = new (128);
        this.OrderedAuxiliaries = new (128);
        this.EquationsDictionary = new(256);
        this.Parameters = new([]);
        this.CurrentSector = string.Empty;
        this.CurrentSubSector = string.Empty;
    }

    public abstract void Parametrize ();

    public abstract bool SimulationEnded();

    public virtual double InitialTime() => 0.0;

    public virtual string TimeUnit => string.Empty;

    public virtual int PlotRows => 1;

    public virtual int PlotCols => 1;

    protected void FinalizeConstruction (
        IEnumerable<string> auxSequence, 
        Action? customUpdate = null, Action? customStart = null)
    {
        this.customUpdate = customUpdate;
        this.customStart = customStart;
        this.SortAuxiliaryEquations(auxSequence);
        this.EquationsDictionary = this.EquationsList.Where(equ => equ != null).ToDictionary(equ => equ.Name, equ => equ);
        this.Reset();
    }

    public Parameters Parameters { get; protected set; }

    public int TickCount { get; private set; }

    public double Time { get; private set; }

    public double DeltaTime { get; private set; }

    public void OnNewLevel(Level level) => this.Levels.Add(level);

    public void OnNewRate(Rate rate) => this.Rates.Add(rate);

    public void OnNewAuxiliary(Auxiliary auxiliary) 
        => this.Auxiliaries.Add(auxiliary.Name, auxiliary);

    public void OnNewEquation(Equation equation)
    {
        equation.Sector = this.CurrentSector;
        equation.SubSector = this.CurrentSubSector;
        this.EquationsList.Add(equation);
    }

    public Equation EquationFromName(string equationName)
    {
        if (this.EquationsDictionary.TryGetValue(equationName, out var equation))
        {
            return equation;
        }

        throw new Exception("Equation not found: " + equationName);
    }

    public void Tick()
    {
        ++this.TickCount;
        this.UpdateLevels();
        this.UpdateAuxiliaries();
        this.UpdateRates();
        this.customUpdate?.Invoke();
        this.TickEquations();
        this.CheckForNaNsAndInfinities();
        this.Time += this.DeltaTime;
    }

    public Dictionary<string, List<double>> GetLogs(IEnumerable<string> equationNames)
    {
        var allData = new Dictionary<string, List<double>>();
        foreach (string equationName in equationNames)
        {
            var equation = this.EquationFromName(equationName);
            var data = equation.Data;
            if (!data.IsNullOrEmpty())
            {
                allData.Add(equationName, data);
            }
            else
            {
                Debug.WriteLine("No data collected for: " + equationName);
                continue;
            }
        }

        return allData;
    }

    public List<double> GetData(string equationName)
    {
        var equation = this.EquationFromName(equationName);
        var data = equation.Data;
        if (!data.IsNullOrEmpty())
        {
            return data;
        }

        Debug.WriteLine("No data collected for: " + equationName);
        if (Debugger.IsAttached) { Debugger.Break(); }
        throw new Exception("No data collected for: " + equationName);
    }

    public void Start(double deltaTime)
    {
        this.DeltaTime = deltaTime;
        this.Reset();
        this.InitializeLevels();
        this.InitializeSmoothAndDelays();
        this.TickCount = 0;
        this.Time = this.InitialTime();

        for (int i = 1; i <= 3; ++i)
        {
            this.UpdateAuxiliaries();
            this.UpdateRates();
            this.customUpdate?.Invoke();
            this.TickEquations();
        }

        this.InitializeLevels();
        this.TickCount = 0;
        this.Time = this.InitialTime();
        this.customStart?.Invoke();
    }

    protected static double Clip(double a, double b, double x, double y) => x >= y ? a : b;

    protected static double Positive (double x) => x < 0.0 ? 0.0 : x;

    protected static double AsInt(double x) => Math.Round(x, MidpointRounding.AwayFromZero);

    private void SortAuxiliaryEquations(IEnumerable<string> auxSequence)
    {
        int orderIndex = 0;
        foreach (string auxiliaryName in auxSequence)
        {
            if (this.Auxiliaries.TryGetValue(auxiliaryName, out var auxiliary))
            {
                auxiliary.EvaluationOrder = orderIndex;
                ++orderIndex;
            }
        }

        this.OrderedAuxiliaries =
            [.. (from auxiliary in this.Auxiliaries.Values 
             orderby auxiliary.EvaluationOrder ascending 
             select auxiliary)];
        this.Auxiliaries = [];
    }

    private void InitializeSmoothAndDelays()
    {
        this.EquationsList.ForEach<Equation>(
            (equation) =>
            {
                if (equation is PureDelay pureDelay)
                {
                    pureDelay.Initialize();
                }

                if (equation is Smooth smooth)
                {
                    smooth.Initialize();
                }

                if (equation is Delay delay)
                {
                    delay.Initialize();
                }
            });
    }

    private void InitializeLevels() 
        => this.Levels.ForEach(level => level.Initialize());

    private void UpdateLevels() 
        => this.Levels.ForEach(level => level.Update());

    private void UpdateAuxiliaries() 
        => this.OrderedAuxiliaries.ForEach(aux => aux.Update());

    private void UpdateRates() 
        => this.Rates.ForEach(rate => rate.Update());

    private void Reset() 
        => this.EquationsList.ForEach(equation => equation.Reset());

    private void TickEquations() 
        => this.EquationsList.ForEach(equation => equation.Tick());

    [Conditional("DEBUG")]
    protected void CheckForNaNsAndInfinities()
        => this.EquationsDictionary.Values.ForEach(equation => equation.CheckForNaNAndInfinity());
}
