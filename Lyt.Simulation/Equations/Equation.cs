namespace Lyt.Simulation.Equations;

public class Equation : Value
{
    public delegate double UpdateDelegate();

    public Equation(Simulator model, string name, int number, string units = "dimensionless")
        : base(model, name, number, units)
    {
        this.Log = [];
        model.OnNewEquation(this);
    } 

    public UpdateDelegate? UpdateFunction { get; set; }

    public double J { get; set; }

    public double Minimum { get; private set; }

    public double Maximum { get; private set; }

    public double NormalizedValue => (this.K - this.Minimum) / (this.Maximum - this.Minimum);

    public double NormalizedLoggedValue(int index)
    {
        if ( this.Maximum == double.MinValue)
        {
            return 0.0;
        }

        if ( this.Minimum == double.MaxValue)
        {
            return 0.0;
        }

        if (IsAlmostZero(this.Maximum - this.Minimum))
        {
            return 0.0;
        }

        return (this.Log[index] - this.Minimum) / (this.Maximum - this.Minimum);
    }

    public List<double> Log { get; set; } = [];

    public virtual void Reset()
    {
        this.Log = new(512);
        this.Maximum = double.MinValue;
        this.Minimum = double.MaxValue;
    }

    public virtual void Initialize() { }

    public virtual void Update()
    {
        if (this.UpdateFunction is not null)
        {
            this.K = this.UpdateFunction.Invoke();
            this.CheckForNaNAndInfinity();

            if (this.K > this.Maximum)
            {
                this.Maximum = this.K;
            }

            if (this.K < this.Minimum)
            {
                this.Minimum = this.K;
            }
        }
    }

    public virtual void Tick()
    {
        if (this.Log != null)
        {
            this.Log.Add(this.K);
        }
        else
        {
            Debug.WriteLine(this.FriendlyName + " has not logging support. ~ " + this.Name);
            if (Debugger.IsAttached) { Debugger.Break(); }
        }

        this.J = this.K;
    }
}
