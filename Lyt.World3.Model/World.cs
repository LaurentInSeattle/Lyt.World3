namespace Lyt.World3.Model;

public sealed class World
{
    public World()
    {
        this.Tables = World.LoadTables("functions_table_world3");
        this.Agriculture = new Agriculture();
        this.Capital = new Capital();
        this.Pollution = new Pollution();
        this.Population = new Population();
        this.Resource = new Resource();
    }

    public List<Table> Tables { get; private set; }

    public Agriculture Agriculture { get; private set; }

    public Capital Capital { get; private set; }

    public Pollution Pollution { get; private set; }

    public Population Population { get; private set; }

    public Resource Resource { get; private set; }

    public static List<Table> LoadTables(string resourceFileName)
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
