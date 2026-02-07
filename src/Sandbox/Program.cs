using System.Text;
using Spectre.Console;
using Spectre.Tui;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = Terminal.Create();
        var renderer = new Renderer(terminal);
        renderer.SetTargetFps(60);

        Console.Title = "Spectre.Tui Sandbox";
        Console.OutputEncoding = Encoding.Unicode;

        while (running)
        {
            renderer.Draw((ctx, elapsed) =>
            {
                // Outer box
                ctx.Render(new BoxWidget(Color.Red) { Border = Border.Double });
                ctx.Render(new ClearWidget('O'), ctx.Viewport.Inflate(-1, -1));

                // Inner box
                var inner = ctx.Viewport.Inflate(new Size(-10, -5));
                ctx.Render(new BoxWidget(Color.Green), inner);
                ctx.Render(
                    new ClearWidget('.', new Style(decoration: Decoration.Bold)),
                    inner.Inflate(-1, -1));

                // FPS
                ctx.Render(
                    new FpsWidget(elapsed, foreground: Color.Green),
                    inner.Inflate(-1, -1));

                // Some text
                ctx.Render(Text.FromMarkup(
                    $"""
                    नमस्ते [red]Happy Holidays[/] 🎅 Happy Holidays\n[u]Happy Holidays[/]
                    Happy Holidays [yellow bold]Happy Holidays[/] Happy Holidays Happy Holidays
                    {ctx.Viewport.Width} x {ctx.Viewport.Height}
                    """, Color.Green),
                    inner.Inflate(-1, -1));
            });

            // Time to quit?
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                running = false;
            }
        }
    }
}