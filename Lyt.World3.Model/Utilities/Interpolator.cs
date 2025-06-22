// #define VERBOSE_Interpolate

namespace Lyt.World3.Model.Utilities;

public static class Interpolator
{
    private const double BigEpsilon = double.Epsilon * 1e24; 

    private static Dictionary<string, Table> TableDictionary { get; set; }

    static Interpolator()
    {
        List<Table> tables = Interpolator.LoadTables("functions_table_world3");
        TableDictionary = 
            tables.ToDictionary(table => table.YName.ToUpper(), table => table);
    }

    public static double Interpolate(this string function, double x)
    {
        if (double.IsNaN(x))
        {
            if ( Debugger.IsAttached ) {  Debugger.Break(); }
            throw new Exception("Cannot interpolate NaN - Function: " + function);
        }

        string key = function.ToUpper();
        if (!TableDictionary.TryGetValue(key, out Table? table) || table is null)
        {
            throw new Exception("Missing table for function: " + function);
        }

        List<double> xArray = table.XValues;
        List<double> yArray = table.YValues;
        int count = xArray.Count;
        int maxIndex = count - 1;
        if (x <= xArray[0])
        {
            return yArray[0];
        }

        if (x >= xArray[maxIndex])
        {
            return yArray[maxIndex];
        }

        int slot = 0;
        while ((slot < count) && (x > xArray[1 + slot]))
        {
            ++slot;
        }

        if (slot >= maxIndex)
        {
            // Should never happen 
            if (Debugger.IsAttached) { Debugger.Break(); }
        }

        double x1 = xArray[slot];
        double x2 = xArray[1 + slot];
        if (Math.Abs(x2 - x1) < Interpolator.BigEpsilon)
        {
            throw new Exception("Error in data table for function: " + function);
        }

        double y1 = yArray[slot];
        double y2 = yArray[1 + slot];
        double value =  y1 + (x - x1) * (y2 - y1) / (x2 - x1);

#if VERBOSE_Interpolate
        Debug.WriteLine("Interpolating " + function + " for x= " + x.ToString());
        Debug.WriteLine("     between x1 " + x1.ToString() + " and  x2 " + x2.ToString());
        Debug.WriteLine("     with y1 " + y1.ToString() + " and  y2 " + y2.ToString());
        if (double.IsNaN(value))
        {
            Debug.WriteLine("     value is NaN");
            if (Debugger.IsAttached) { Debugger.Break(); }
        } 
        else
        {
            Debug.WriteLine("     returning  " + value.ToString());
        } 
#endif // VERBOSE_Interpolate

        return value; 
    }

    private static List<Table> LoadTables(string resourceFileName)
    {
        try
        {
            resourceFileName += ".json";
            string serialized = SerializationUtilities.LoadEmbeddedTextResource(resourceFileName, out string? resourceFullName);
            return SerializationUtilities.Deserialize<List<Table>>(serialized);
        }
        catch
        {
            return [];
        }
    }
}
