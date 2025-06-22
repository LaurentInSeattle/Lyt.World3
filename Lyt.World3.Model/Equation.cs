// #define VERBOSE_Equation

namespace Lyt.World3.Model;

public sealed class Equation
{
    public Equation(Sector sector, MethodInfo updateMethodInfo)
    {
        this.Sector = sector;
        this.UpdateMethodInfo = updateMethodInfo;
        this.EvaluationOrder = int.MaxValue;

        string valueName = updateMethodInfo.Name.Replace("Update", "");
        this.PropertyName = valueName.ToUpper();
        this.Dependencies = [];
        var attributes = updateMethodInfo.GetCustomAttributes();
        foreach (var attribute in attributes)
        {
            if (attribute is DependsOnAttribute dependsOnAttribute)
            {
                this.Dependencies.Add(dependsOnAttribute.Property);
            }
        }

        Type returnType = updateMethodInfo.ReturnType;
        if (returnType != typeof(void))
        {
            throw new Exception("Invalid return type for " + this.UpdateMethodInfo.Name);
        }

        this.ParameterCount = updateMethodInfo.GetParameters().Length;
        if ((this.ParameterCount != 1) && (this.ParameterCount != 2))
        {
            throw new Exception("Invalid parameter count  for " + this.UpdateMethodInfo.Name);
        }

        PropertyInfo? propertyInfo =
            this.Sector.GetType().GetProperty(valueName, BindingFlags.Instance | BindingFlags.Public);
        if (propertyInfo is not null)
        {
            var getter = propertyInfo.GetGetMethod();
            if (getter is not null)
            {
                object? list = getter.Invoke(this.Sector, null);
                if (list is not null && Model.Sector.IsListOfDouble(list.GetType()))
                {
                    this.Values = (List<double>)list;
                }
                else
                {
                    throw new Exception("No actual data  for " + this.UpdateMethodInfo.Name);
                }
            }
            else
            {
                throw new Exception("No list getter  for " + this.UpdateMethodInfo.Name);
            }
        }
        else
        {
            throw new Exception("No matching data  for " + this.UpdateMethodInfo.Name);
        }

        this.IsResolved = this.Dependencies.Count == 0;
    }

    public Sector Sector { get; private set; }

    public MethodInfo UpdateMethodInfo { get; private set; }

    public string PropertyName { get; private set; }

    public List<double> Values { get; private set; }

    public List<string> Dependencies { get; private set; }

    public int EvaluationOrder { get; set; }

    public int ParameterCount { get; private set; }

    public bool IsResolved { get; private set; }

    public void Evaluate(int k)
    {
        int j = k == 0 ? 0 : k - 1;
        double before = this.Values[j]; 
        if (this.ParameterCount == 1)
        {
            this.UpdateMethodInfo.Invoke(this.Sector, [k]);
        }
        else if (this.ParameterCount == 2)
        {
            this.UpdateMethodInfo.Invoke(this.Sector, [k, j]);
        }
        else
        {
            throw new Exception("Invalid parameter count  for " + this.UpdateMethodInfo.Name);
        }

#if VERBOSE_Equation
        double after = this.Values[k];
        Debug.WriteLine(
            "Step: " + k + " " + this.Sector.Name + "  " + this.PropertyName + 
            "  before: " + before + " after: " + after );
        if (k == 0)
        {
            if (double.IsNaN(after))
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            }
        }
        else
        {
            if (double.IsNaN(before) || double.IsNaN(after))
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            }
        }
#endif // VERBOSE_Equation
    }

    // If all dependencies are resolved this equation is also resolved 
    public bool TryResolve(HashSet<string> resolvedProperties)
    {
        foreach (string property in this.Dependencies)
        {
            if (!resolvedProperties.Contains(property))
            {
                return false;
            }
        }

        this.IsResolved = true;
        return true;
    }

    public string ToDebugString(HashSet<string>? resolvedProperties = null)
    {
        StringBuilder sb = new();
        sb.Append(this.Sector.Name);
        sb.Append(":  ");
        sb.Append(this.PropertyName);
        sb.Append(":  ");
        foreach (string dependsOn in this.Dependencies)
        {
            sb.Append(dependsOn);
            sb.Append("  ");
        }

        if (resolvedProperties is not null)
        {
            sb.Append("Missing:  ");
            foreach (string dependsOn in this.Dependencies)
            {
                if (!resolvedProperties.Contains(dependsOn))
                {
                    sb.Append(dependsOn);
                    sb.Append("  ");
                }
            }

        }

        return sb.ToString();
    }
}
