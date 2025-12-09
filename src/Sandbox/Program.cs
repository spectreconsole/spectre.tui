using System.Text;
using Spectre.Tui;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = Terminal.Create();
        var renderer = new Renderer(terminal);

        while (running)
        {
            renderer.Draw((ctx, elapsed) =>
            {
                // Outer box
                ctx.Render(new BoxWidget());
                ctx.Render(new ClearWidget(new Rune('.')), ctx.Viewport.Inflate(-1, -1));

                // Inner box
                var inner = ctx.Viewport.Inflate(new Size(-10, -5));
                ctx.Render(new BoxWidget(), inner);
                ctx.Render(
                    new ClearWidget(new Rune('O'), Decoration.Bold),
                    inner.Inflate(-1, -1));

                // FPS
                ctx.Render(
                    new TextWidget(((int)(1.0f / elapsed.TotalSeconds)).ToString()),
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