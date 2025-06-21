namespace Lyt.World3.Model;

public sealed class Equation
{
    public Equation(Sector sector, MethodInfo methodInfo)
    {
        this.Sector = sector;
        this.MethodInfo = methodInfo;
        this.EvaluationOrder = int.MaxValue;

        this.PropertyName = methodInfo.Name.Replace("Update", "").ToUpper();
        this.Dependencies = [];
        var attributes = methodInfo.GetCustomAttributes();
        foreach (var attribute in attributes)
        {
            if (attribute is DependsOnAttribute dependsOnAttribute)
            {
                this.Dependencies.Add(dependsOnAttribute.Property);
            }
        }

        Type returnType = methodInfo.ReturnType;
        if (returnType != typeof(void))
        {
            throw new Exception("Invalid return type for " + this.MethodInfo.Name);
        }

        this.ParameterCount = methodInfo.GetParameters().Length;
        if ((this.ParameterCount != 1) && (this.ParameterCount != 2))
        {
            throw new Exception("Invalid parameter count  for " + this.MethodInfo.Name);
        }

        this.IsResolved = this.Dependencies.Count == 0;
    }

    public Sector Sector { get; private set; }

    public MethodInfo MethodInfo { get; private set; }

    public string PropertyName { get; private set; }

    public List<string> Dependencies { get; private set; }

    public int EvaluationOrder { get; set; }

    public int ParameterCount { get; private set; }

    public bool IsResolved { get; private set; }

    public void Evaluate(int k)
    {
        if (this.ParameterCount == 1)
        {
            this.MethodInfo.Invoke(this.Sector, [k]);
        }
        else if (this.ParameterCount == 2)
        {
            int j = k == 0 ? 0 : k - 1;
            this.MethodInfo.Invoke(this.Sector, [k, j]);
        }
        else
        {
            throw new Exception("Invalid parameter count  for " + this.MethodInfo.Name);
        }
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
