namespace Lyt.World3.Model.Utilities;

public static class MathUtilities
{
    /// <summary> Logical function returning x if doSwitch is False, else y. </summary>
    public static double Switch(double x, double y, bool doSwitch)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.NaN;
        }

        return !doSwitch ? x : y;
    }

    /// <summary> Logical function used as time switch to change parameter value. </summary> 
    /// <returns> y if t > t_switch, else x.</returns>
    public static double Clip(double x, double y, double t, double t_switch)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.NaN;
        }

        return t <= t_switch ? x : y;
    }

    /// <summary> 
    /// Affine function with provided slope, clipped at 0 for t < t_offset 
    /// slope: ramp slope, t_offset: time when ramps begins, t: current time value.
    /// </summary> 
    /// <returns> slope* (t - t_offset) if t >= t_offset, else 0 </returns>
    // Returns:  
    public static double Ramp(double slope, double t_offset, double t)
        => t < t_offset ? 0.0 : slope * (t - t_offset);

    // If we stick with Euler integration, there is no need for this function 
    //
    //    /// <summary>
    //    /// Computes the derivative of out_ at time t_, for the 1st order delay. 
    //    /// Used in integration by odeint.
    //    /// </summary>
    //#pragma warning disable IDE0060 // Remove unused parameter
    //    public static double DelayOne(double output, double t, double input, double delay)
    //            => (input - output) / delay;
    //#pragma warning restore IDE0060 
}
