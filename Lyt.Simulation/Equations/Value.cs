namespace Lyt.Simulation.Equations;

public class Value(Simulator model, string name, int number, string units)
{
    public readonly Simulator Simulator = model;

    public readonly string Name = name;

    public readonly int Number = number;

    public readonly string Units = units;

    private double k;

    public double K
    {
        get => this.k;
        set
        {
            this.CheckValue(value);
            this.k = value;
        }
    }

    public bool CannotBeZero { get; set; }

    public bool CannotBeNegative { get; set; }

    public string Sector { get; set; } = string.Empty;

    public string SubSector { get; set; } = string.Empty;

    public string FriendlyName => string.Format("{0} ({1})", this.Name.Capitalize().Wordify(), this.Number);

    public string FriendlyUnits => this.Units.Capitalize().Wordify();

    #region Debug Conditional code 

    [Conditional("DEBUG")]
    public void CheckValue(double value )
    {
        if (this.CannotBeZero)
        {
            this.CheckForZero(value);
        }

        this.CheckForNaNAndInfinityValue(value);
        if (this.CannotBeNegative)
        {
            this.CheckForNegative(value);
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNaNAndInfinity()
    {
        if (double.IsNaN(this.k) || double.IsInfinity(this.k))
        {
            Debug.WriteLine(this.FriendlyName + " is 'NaN' or infinite. ~ " + this.Name);
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNaNAndInfinity(string equationName )
    {
        var equation = this.Simulator.EquationFromName(equationName);
        if (double.IsNaN(k) || double.IsInfinity(k))
        {
            Debug.WriteLine(equation.FriendlyName + " is 'NaN' or infinite. ~ " + equation.Name);
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNaNAndInfinityValue(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            Debug.WriteLine("Value is 'NaN' or infinite." );
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNegative(string equationName)
    {
        var equation = this.Simulator.EquationFromName(equationName);
        if (equation.K < 0.0 && this.Simulator.TickCount > 1)
        {
            Debug.WriteLine(equation.FriendlyName + " is negative. ~ " + equation.Name);
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNegative()
    {
        if (this.k < 0.0 && this.Simulator.TickCount > 1)
        {
            Debug.WriteLine(this.FriendlyName + " is negative. ~ " + this.Name);
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    [Conditional("DEBUG")]
    public void CheckForNegative(double value)
    {
        if (value < 0.0 &&  this.Simulator.TickCount > 1 )
        {
            Debug.WriteLine("Value is negative. ");
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    public static bool IsAlmostZero(double value)
    {
        const double epsilon = 0.000_000_000_1;
        return Math.Abs(value) < epsilon ; 
    }

    [Conditional("DEBUG")]
    public void CheckForZero(double value)
    {
        const double epsilon = 0.000_000_000_1; 
        if (Math.Abs(value) < epsilon && this.Simulator.TickCount > 1)
        {
            Debug.WriteLine("Value is zero. ");
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    #endregion Debug Conditional code 
}
