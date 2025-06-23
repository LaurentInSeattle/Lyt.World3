namespace Lyt.Simulation.Delays;

public class DelayedAuxiliary : Auxiliary
{
    protected readonly string inputEquationName;
    protected readonly double delay;

    // Equation object is possibly and most likely not existing yet at construction time 
    // therefore we only pass a name and get the input object later, at Reset time 
    protected Equation? input;

    protected bool firstCall;

    public DelayedAuxiliary(
        Simulator model, string name, int number, string units, double delay, string inputEquationName) 
        :  base(model, name, number, units)
    {
        this.inputEquationName = inputEquationName;
        this.delay = delay;
    }

    public override void Reset()
    {
        this.firstCall = true;
        this.input = this.Simulator.EquationFromName(this.inputEquationName);
        if (this.input is null)
        {
            throw new Exception("No input equation");
        }

        base.Reset();
    }
}
