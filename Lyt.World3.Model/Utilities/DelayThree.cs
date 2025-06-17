namespace Lyt.World3.Model.Utilities;

// TODO: Refactor using Vectors and Matrices 

/// <summary>
///     Delay function of the 3rd order. 
///     Returns a class that is callable as a function(see Call parameters) at a given step k.
///     Computes the delayed vector out_arr from the input in_arr, at the step k.
/// </summary>
public class DelayThree
{
    protected readonly double dt; // Time step 
    protected readonly double[] input; // input vector of the delay function.
    protected readonly List<double[]> output;
    private readonly List<double[]> ANorm;
    private readonly double[] BNorm;

    public DelayThree(double[] input, double dt, double[] t)
    {
        this.input = input;
        this.dt = dt;
        this.output = [];
        for (int i = 0; i < 3; i++)
        {
            double[] array = new double[t.Length];
            Array.Fill<double>(array, 0);
            output.Add(array);
        }

        this.ANorm = [];
        double[] a1 = [-1.0, 0.0, 0.0];
        this.ANorm.Add(a1);
        double[] a2 = [1.0, -1.0, 0.0];
        this.ANorm.Add(a2);
        double[] a3 = [0.0, 1.0, -1.0];
        this.ANorm.Add(a3);

        this.BNorm = [1.0, 0.0, 0.0];
    }

    protected virtual void InitializeOutput(double delay)
    {
        //def _init_out_arr(self, delay):
        //  self.out_arr[0, :] = self.in_arr[0] * 3 / delay
        foreach (double[] array in this.output)
        {
            array[0] = this.input[0] * 3.0 / delay;
        }
    }

    // k : current loop index.
    // delay :  delay parameter. Higher delay increases smoothing.
    public double Call(int k, double delay)
    {
        if (k == 0)
        {
            this.InitializeOutput(delay);
            return this.output[2][k];
        }

        // For now: Euler integration only  

        // TEMP #1:     self.A_norm  @ self.out_arr[k - 1, :]
        // Matrix multiply by vector should return a vector  
        double[] aNorm = new double[3];
        for (int i = 0; i < 3; ++i)
        {
            double current = this.output[i][k - 1];
            aNorm[i] =
                this.ANorm[i][0] * current +
                this.ANorm[i][1] * current +
                this.ANorm[i][2] * current;
        }

        // TEMP #2:     self.B_norm * self.in_arr[k - 1]  : Vector multiplied by scalar 
        double[] bNorm = new double[3];
        for (int i = 0; i < 3; ++i)
        {
            bNorm[i] = this.BNorm[i] * this.input[k - 1];
        }

        // Add vectors aNorm and bNorm into dout 
        // dout = (self.A_norm  @ self.out_arr[k - 1, :] + self.B_norm * self.in_arr[k - 1])
        double[] dout = new double[3];
        for (int i = 0; i < 3; ++i)
        {
            dout[i] = aNorm[i] + bNorm[i];
        }


        // Scalar multiply vector of dout 
        // dout *= self.dt * 3 / delay
        for (int i = 0; i < 3; ++i)
        {
            dout[i] *= this.dt * 3 / delay;
        }

        // self.out_arr[k, :] = self.out_arr[k - 1, :] + dout
        int j = 0;
        foreach (double[] array in this.output)
        {
            array[k] = array[k - 1] + dout[j++];
        }

        return this.output[2][k];
    }
}

