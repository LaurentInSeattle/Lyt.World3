namespace Lyt.Simulation.Interpolation; 

public sealed class Table : Auxiliary
{
    public readonly double[] TableData;
    public readonly double Min;
    public readonly double Max;
    public readonly double Delta;
    public readonly List<double> Indices;

    public Table(
        Simulator model, 
        string name, int number, string units, 
        double[] tableData, double min, double max, double delta)
        : base(model, name, number, units)
    {
        this.TableData = tableData;
        this.Min = min;
        this.Max = max;
        this.Delta = delta;
        this.Indices = [];
        for (double i = this.Min; i <= this.Max; i += this.Delta)
        {
            this.Indices.Add(i);
        }
    }

    public override void Update()
    {
        if (this.UpdateFunction != null)
        {
            double source = this.UpdateFunction.Invoke();
            this.CheckForNaNAndInfinityValue(source); 
            double value = this.Lookup(source);
            this.CheckRange(value); 

            this.K = value;
        }
    }

    private double Lookup(double source)
    {
        if (source <= this.Min)
        {
            return this.TableData[0];
        }
        else if (source >= this.Max)
        {
            return this.TableData[this.TableData.Length - 1];
        }
        else
        {
            int j = 0;
            for (double i = this.Min; i <= this.Max; i += this.Delta, j++)
            {
                if (i >= source)
                {
                    double lowerVal = this.TableData[j - 1];
                    double upperVal = this.TableData[j];
                    double fraction = (source - (i - this.Delta)) / this.Delta;
                    this.CheckInterpolate(fraction, source);
                    return lowerVal + fraction * (upperVal - lowerVal);
                }
            }
        }

        throw new Exception("Table lookup failed to find a value: " + this.Name + " Index: " + source.ToString("D"));
    }

    [Conditional("DEBUG")]
    private void CheckRange(double value)
    {
        double first = this.TableData[0];
        double last = this.TableData[this.TableData.Length - 1];
        double min = Math.Min(first, last);
        double max = Math.Max(first, last);
        if (value < min && value > max)
        {
            throw new Exception("Table lookup out of range: " + this.Name + " Source: " + value.ToString("F"));
        }
    }

    [Conditional("DEBUG")]
    private void CheckInterpolate(double fraction, double source)
    {
        if (fraction < 0.0 || fraction > 1.0)
        {
            throw new Exception("Table lookup failed to interpolate: " + this.Name + " Source: " + source.ToString("D"));
        }
    }
}
