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

        this.IsResolved = this.Dependencies.Count == 0;
    }

    public Sector Sector { get; private set; }

    public MethodInfo MethodInfo { get; private set; }

    public string PropertyName { get; private set; }

    public List<string> Dependencies { get; private set; }

    public int EvaluationOrder { get; set; }

    public bool IsResolved { get; private set; }

    public double Evaluate(int k)
        => this.MethodInfo.Invoke(this.Sector, [k]) is double value ? 
                value : 
                throw new Exception("Invalid return type for " + this.MethodInfo.Name );

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
