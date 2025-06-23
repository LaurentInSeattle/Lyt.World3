namespace Lyt.Simulation;

public sealed class Parameters(IEnumerable<Parameter> parameters)
{
    readonly Dictionary<string, Parameter> parameters 
        = parameters.ToDictionary(parameter => parameter.Name, parameter => parameter);

    public Dictionary<string, Parameter> All => this.parameters; 

    public double Get(string parameterName) => this.FromName(parameterName).CurrentValue;

    public Parameter FromName(string parameterName)
    {
        if (this.parameters.TryGetValue(parameterName, out var parameter))
        {
            return parameter;
        }
        else
        {
            throw new Exception("Parameter not found: " + parameterName);
        }
    }

    public void ToDefaults() 
        => this.parameters.Values.ForEach<Parameter>((parameter) => parameter.Default());

    public void CommitEdits() 
        => this.parameters.Values.ForEach<Parameter>((parameter) => parameter.CommitEdits());

    public void CancelEdits() 
        => this.parameters.Values.ForEach<Parameter>((parameter) => parameter.CancelEdits());
}
