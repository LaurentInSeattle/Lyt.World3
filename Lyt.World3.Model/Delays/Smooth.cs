﻿namespace Lyt.World3.Model.Delays;

/// <summary>
///     Delay information function of the 1st order for smoothing. Also named DLINF1 in Dynamo.
///     Returns a class that is callable as a function(see Call parameters) at a given step k.
///     Computes the smoothed vector out_arr from the input array, at the step k.
/// </summary>
public sealed class Smooth
{
    private readonly double dt; // Time step 
    private readonly List<double> input; // input vector of the delay function.
    private readonly double[] output;

    public Smooth(List<double> input, double dt, double[] t)
    {
        this.input = input;
        this.dt = dt;
        this.output = new double[t.Length];
        Array.Fill(this.output, 0);
    }

    // k : current loop index.
    // delay :  delay parameter. Higher delay increases smoothing.
    public double Call(int k, double delay)
    {
        if (k == 0)
        {
            double value = this.input[0];
            this.output[0] = value;
            return value;
        }

        // For now: Euler integration only  

        // dout = self.in_arr[k - 1] - self.out_arr[k - 1]
        // dout *= self.dt / delay
        // self.out_arr[k] = self.out_arr[k - 1] + dout

        double dout = this.input[k - 1] - this.output[k - 1];
        dout *= this.dt / delay;
        this.output[k] = this.output[k - 1] + dout;
        return this.output[k];
    }
}