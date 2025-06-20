namespace Lyt.World3.Model.Utilities;

public sealed class Named 
{
    public Named(
        List<double> payload, 
        [CallerArgumentExpression(nameof(payload))] string listName = "")
    {
        this.Name = listName.Replace( "this.", "");
        this.Payload = payload;
    }

    public string Name { get; private set; }

    public List<double> Payload { get; private set; }
}
