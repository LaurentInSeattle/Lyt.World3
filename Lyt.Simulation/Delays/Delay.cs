namespace Lyt.Simulation.Delays;

public sealed class Delay(
    Simulator model, string name, int number, string units, double delay, string inputEquationName) : 
    DelayedAuxiliary(model, name, number, units, delay, inputEquationName)
{
    // Mutable and readonly => Cannot make it a struct 
    private class Stage
    {
        public double J;
        public double K;
    }

    private readonly double delayPerStage = delay / 3.0;
    private readonly Stage alpha = new Stage();
    private readonly Stage beta = new Stage();
    private readonly Stage gamma = new Stage();

    public override void Initialize()
    {
        if (this.input is null)
        {
            throw new Exception("No input equation");
        }

        this.J = this.K = this.input.K;
        this.alpha.J = this.alpha.K = this.input.J;
        this.beta.J = this.beta.K = this.input.J;
        this.gamma.J = this.gamma.K = this.input.J;
    }

    public override void Update()
    {
        if (this.input is null)
        {
            throw new Exception("No input equation");
        }

        if (this.firstCall)
        {
            this.J = this.K = this.input.K;
            this.alpha.J = this.alpha.K = this.K;
            this.beta.J = this.beta.K = this.K;
            this.gamma.J = this.gamma.K = this.K;
            this.firstCall = false;
        }
        else
        {
            double dt = this.Simulator.DeltaTime;
            this.alpha.K = this.alpha.J + dt * (this.input.J - this.alpha.J) / this.delayPerStage;
            this.beta.K = this.beta.J + dt * (this.alpha.J - this.beta.J) / this.delayPerStage;
            this.gamma.K = this.gamma.J + dt * (this.beta.J - this.gamma.J) / this.delayPerStage;
            this.alpha.J = this.alpha.K;
            this.beta.J = this.beta.K;
            this.gamma.J = this.gamma.K;
            this.K = this.gamma.K;
        }
    }
}
