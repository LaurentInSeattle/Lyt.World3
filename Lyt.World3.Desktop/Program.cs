using System;
using Avalonia;

namespace Lyt.World3.Desktop;

using Lyt.World3; 

internal class Program
{
    private const long gigabyte = 1_024 * 1_024 * 1_024;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .With(new SkiaOptions()
            {
                 MaxGpuResourceSizeBytes = 2 * gigabyte,
            })
            .LogToTrace();
}
