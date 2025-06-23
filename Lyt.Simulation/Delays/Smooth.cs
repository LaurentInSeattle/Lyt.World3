namespace Lyt.Simulation.Delays;

public sealed class Smooth(
    Simulator model, string name, int number, string units,
    double delay, string inputEquationName, double initialValue) 
    : DelayedAuxiliary(model, name, number, units, delay, inputEquationName)
{
    public delegate double InitializeDelegate();

    private readonly double initialValue = initialValue;

    public InitializeDelegate? InitializeFunction { get; set; }

    public override void Initialize()
    {
        double startValue; 
        if (this.InitializeFunction is not null)
        {
            startValue = this.InitializeFunction.Invoke(); 
        }
        else
        {
            startValue = this.initialValue;
        }

        this.J = this.K = startValue; 
    }

    public override void Update()
    {
        if (this.firstCall)
        {
            this.Initialize();
            this.firstCall = false;
        }
        else
        {
            if (this.input is null)
            {
                throw new Exception("No input equation");
            }

            this.K = this.J + this.Simulator.DeltaTime * (this.input.J - this.J) / this.delay;
        }
    }
}
