namespace Lyt.World3.Model.Utilities;

public static class MathUtilities
{
    #region PY
    /*
    def switch(var1, var2, boolean_switch):
    """
    Logical function returning var1 if boolean_switch is False, else var2.
    Parameters
    ----------
    var1 : any
    var2 : any
    boolean_switch : bool
    Returns
    -------
    var1 or var2
    """
    if np.isnan(var1) or np.isnan(var2):
        return np.nan
    else:
        if bool (boolean_switch) is False:
            return var1
        else:
            return var2
    */
    #endregion PY

    /// <summary> Logical function returning x if doSwitch is False, else y. </summary>
    public static double Switch(double x, double y, bool doSwitch)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.NaN;
        }

        return !doSwitch ? x : y;
    }

    #region PY
    /*
    def clip(func2, func1, t, t_switch) :
        """
        Logical function used as time switch to change parameter value.
        Parameters
        ----------
        func2 : any
        func1 : any
        t : float           current time value.
        t_switch : float    time threshold.

        Returns
        -------
        func2 if t>t_switch, else func1.
        """
        if np.isnan(func1) or np.isnan(func2):
            return np.nan
        else:
            if t <= t_switch:
                return func1
            else:
                return func2
*/
    #endregion PY

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

    #region PY
    /*
    def ramp(slope, t_offset, t) :
        """
        Affine function with provided slope, clipped at 0 for t < t_offset.

        Parameters
        ----------
        slope : float
            ramp slope.
        t_offset : float
            time when ramps begins.
        t : float
            current time value.

        Returns
        -------
        slope * (t - t_offset) if t >= t_offset, else 0

        """
        if t<t_offset:
            return 0
        else:
            return slope* (t - t_offset)
*/
    #endregion PY

    /// <summary> 
    /// Affine function with provided slope, clipped at 0 for t < t_offset 
    /// slope: ramp slope, t_offset: time when ramps begins, t: current time value.
    /// </summary> 
    /// <returns> slope* (t - t_offset) if t >= t_offset, else 0 </returns>
    // Returns:  
    public static double Ramp(double slope, double t_offset, double t)
        => t < t_offset ? 0.0 : slope * (t - t_offset);

    #region PY
    /*
# linear systems of order 1 or 3 for delay and smoothing
    def func_delay1(out_, t_, in_, del_):
        """
        Computes the derivative of out_ at time t_, for the 1st order delay. Used in integration by odeint.

        """
        return (in_ - out_) / del_

    */
    #endregion PY

    /// <summary>
    /// Computes the derivative of out_ at time t_, for the 1st order delay. Used in integration by odeint.
    /// </summary>
#pragma warning disable IDE0060 // Remove unused parameter
    public static double DelayOne(double output, double t, double input, double delay)
            => (input - output) / delay;
#pragma warning restore IDE0060 
}
