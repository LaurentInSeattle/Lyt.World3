namespace Lyt.World3.Model.Utilities;

public sealed class Table
{
    public Table() { /* Required for serialization */ }

    [JsonPropertyName("sector")]
    public string Sector { get; set; } = string.Empty;

    [JsonPropertyName("x.name")]
    public string XName { get; set; } = string.Empty;

    [JsonPropertyName("x.values")]
    public List<double> XValues { get; set; } = [];

    [JsonPropertyName("y.name")]
    public string YName { get; set; } = string.Empty;

    [JsonPropertyName("y.values")]
    public List<double> YValues { get; set; } = [];
}
