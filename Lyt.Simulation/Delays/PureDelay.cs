namespace Lyt.Simulation.Delays; 

public sealed class PureDelay : DelayedAuxiliary
{
    private readonly int stageCount;
    private readonly List<double> stages;

    public PureDelay(
        Simulator model, string name, int number, string units, 
        double delay, string inputEquationName)
        : base(model, name, number, units, delay, inputEquationName)
    {
        this.stageCount = (int)(delay / model.DeltaTime);
        this.stages = new List<double>(stageCount);
    }

    public override void Initialize()
    {
        if (this.input is null)
        {
            throw new Exception("No input equation");
        }

        this.J = this.K = this.input.K;
        for (int i = 0; i < this.stageCount; ++i)
        {
            this.stages.Add(this.input.K); 
        }
    }

    public override void Update()
    {
        if (this.input is null)
        {
            throw new Exception("No input equation");
        }

        this.J = this.K = this.stages[this.stageCount -1 ];
        this.stages.Insert(0,this.input.K);
        this.stages.RemoveAt(this.stageCount);
    }
}
