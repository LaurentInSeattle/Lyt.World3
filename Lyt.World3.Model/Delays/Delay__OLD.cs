namespace Lyt.World3.Model.Delays;

/// <summary>
///     Delay function of the 3rd order. 
///     Returns a class that is callable as a function(see Call ) at a given step k.
///     Computes the ouptput delayed vector out_arr from the input, at the step k.
/// </summary>
public class DelayThree 
{
    private struct Stage
    {
        public double J;
        public double K;
    }

    protected readonly double dt; // Time step 
    protected readonly List<double> input; // input vector of the delay function.

    private double delayPerStage;
    private Stage alpha;
    private Stage beta;
    private Stage gamma;

    public DelayThree(List<double> input, double dt, double[] _)
    {
        this.input = input;
        this.dt = dt;
        this.alpha = new Stage();
        this.beta = new Stage();
        this.gamma = new Stage();
    }

    protected virtual void InitializeOutput(double delay)
    {
        //def _init_out_arr(self, delay):
        //  self.out_arr[0, :] = self.in_arr[0] * 3 / delay
        //foreach (double[] array in this.output)
        //{
        //    array[0] = this.input[0] * 3.0 / delay;
        //}
    }

    public double Call(int k, double delay)
    {
        if (k == 0)
        {
            this.Initialize(delay);
            return this.input[0];
        }

        double inputJ = input[k - 1]; 
        this.alpha.K = this.alpha.J + dt * ( inputJ - this.alpha.J) / this.delayPerStage;
        this.beta.K = this.beta.J + dt * (this.alpha.J - this.beta.J) / this.delayPerStage;
        this.gamma.K = this.gamma.J + dt * (this.beta.J - this.gamma.J) / this.delayPerStage;
        this.alpha.J = this.alpha.K;
        this.beta.J = this.beta.K;
        this.gamma.J = this.gamma.K;
        return this.gamma.K;
    }

    public void Initialize(double delay)
    {
        this.delayPerStage = delay / 3.0;
        this.InitializeOutput(delay);
        double initInput = this.input[0];
        this.alpha.J = this.alpha.K = initInput;
        this.beta.J = this.beta.K = initInput;
        this.gamma.J = this.gamma.K = initInput;
    }

    /* 
    
    public Delay(Simulator model, string name, int number, string units, double delay, string inputEquationName)
        : base(model, name, number, units)
    {
        this.firstCall = true;
        this.delayPerStage = delay / 3.0;
        this.inputEquationName = inputEquationName;
    }

    public Delay(string name, string units, double delay, string inputEquationName)
        : base(Simulator.Instance, name, 0, units)
    {
        this.firstCall = true;
        this.delayPerStage = delay / 3.0;
        this.inputEquationName = inputEquationName;
    }

    public override void Reset()
    {
        this.firstCall = true;
        this.Input = this.Simulator.EquationFromName(this.inputEquationName);
        this.alpha = new Stage();
        this.beta = new Stage();
        this.gamma = new Stage();
        base.Reset();
    }

    public override void Initialize()
    {
        this.J = this.K = this.Input.K;
        this.alpha.J = this.alpha.K = this.Input.J;
        this.beta.J = this.beta.K = this.Input.J;
        this.gamma.J = this.gamma.K = this.Input.J;
    }

    public override void Update()
    {
        if (this.firstCall)
        {
            this.J = this.K = this.Input.K;
            this.alpha.J = this.alpha.K = this.K;
            this.beta.J = this.beta.K = this.K;
            this.gamma.J = this.gamma.K = this.K;
            this.firstCall = false;
        }
        else
        {
            double dt = this.Simulator.DeltaTime;
            this.alpha.K = this.alpha.J + dt * (this.Input.J - this.alpha.J) / this.delayPerStage;
            this.beta.K = this.beta.J + dt * (this.alpha.J - this.beta.J) / this.delayPerStage;
            this.gamma.K = this.gamma.J + dt * (this.beta.J - this.gamma.J) / this.delayPerStage;
            this.alpha.J = this.alpha.K;
            this.beta.J = this.beta.K;
            this.gamma.J = this.gamma.K;
            this.K = this.gamma.K;
        }
    }
    */
}
