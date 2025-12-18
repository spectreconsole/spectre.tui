using Spectre.Tui;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = Terminal.Create();
        var renderer = new Renderer(terminal);

        Console.Title = "Spectre.Tui Sandbox";

        while (running)
        {
            renderer.Draw((ctx, elapsed) =>
            {
                // Outer box
                ctx.Render(new BoxWidget(Color.Red));
                ctx.Render(new ClearWidget('O'), ctx.Viewport.Inflate(-1, -1));

                // Inner box
                var inner = ctx.Viewport.Inflate(new Size(-10, -5));
                ctx.Render(new BoxWidget(Color.Green), inner);
                ctx.Render(
                    new ClearWidget('.', Decoration.Bold),
                    inner.Inflate(-1, -1));

                // FPS
                ctx.Render(
                    new FpsWidget(((int)(1.0f / elapsed.TotalSeconds)).ToString(), foreground: Color.Green),
                    inner.Inflate(-1, -1));

                // Some text
                ctx.Render(Text.FromString("नमस्ते Happy Holidays 🎅 Happy Holidays\nHappy Holidays Happy Holidays Happy Holidays Happy Holidays Happy Holidays ", Color.Green),
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