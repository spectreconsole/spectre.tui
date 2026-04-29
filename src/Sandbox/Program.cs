using System.Text;

namespace Sandbox;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = "Spectre.Tui.App Sandbox";
        Console.OutputEncoding = Encoding.Unicode;

        await Application
            .Create()
            .RunAsync(new SandboxScreen());
    }
}
