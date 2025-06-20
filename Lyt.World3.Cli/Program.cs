namespace Lyt.World3.Cli;

using Lyt.World3.Model;

internal class Program
{
    static void Main(string[] _)
    {
        Console.WriteLine("World 3");
        var world = new World();
        Console.WriteLine("World : Created");
        world.Initialize();
        Console.WriteLine("World : Initialized");
        Console.WriteLine("");

        Console.WriteLine("Press <enter> to close.");
        Console.ReadLine();
    }
}
