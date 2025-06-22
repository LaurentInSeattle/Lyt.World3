namespace Lyt.World3.Model.Delays;

/// <summary>
/// Delay information function of the 3rd order for smoothing.
/// Returns a class that is callable as a function(see Call parameters) at a given step k.
/// Computes the smoothed vector out_arr from the input in_arr, at the step k.
/// </summary>
/// <remarks> Same as DelayThree except the initialisation step at k == 0. </remarks>
public sealed class DelayInformationThree : DelayThree
{
    public DelayInformationThree(List<double> input, double dt, double[] t) 
        : base(input, dt, t)
    {
    }

    protected override void InitializeOutput(double delay )
    {
        //self.out_arr[0, :] = self.in_arr[0]
        foreach (double[] array in this.output)
        {
            array[0] = this.input[0];
        }
    }
} 
