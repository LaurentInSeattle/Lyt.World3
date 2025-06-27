namespace Lyt.Simulation.World3;

public sealed partial class WorldModel : Simulator
{
    public const int StartYear = 1900;
    public const int PolicyYear = 1975; // eqn 150.1

    private Parameter[] parameters =
    [
        new Parameter("Simulation Duration", 220, 180, 420, 20, Widget.Slider, "", Format.Integer),
        new Parameter("Delta Time", 1.0, 0.2, 1.0, 0.1),
        new Parameter("Resources Multiplier", 1.0, 0.5, 2.5, 0.1),
        new Parameter("Output Consumed", 0.43, 0.31, 0.53, 0.02),
    ];

    public override void Parametrize()
    {
        this.SetInitialResources(this.Parameters.Get("Resources Multiplier"));
        this.SetOutputConsumed(this.Parameters.Get("Output Consumed"));
    }
}
