using System.Text;
using Spectre.Tui;
using Spectre.Tui.Terminal;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        using var terminal = new Terminal();
        var renderer = new Renderer(terminal);
        var state = new AppState();

        while (true)
        {
            renderer.Draw((frame, elapsed) =>
            {
                state.Update(elapsed);

                frame.Render(new Box(state.Invert));
                frame.Render(new CenteredText("FPS:" + state.Fps));
            });

            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }
    }

    private sealed class AppState
    {
        private readonly Fps _fps = new();
        private readonly Accumulator _accumulator = new(TimeSpan.FromSeconds(0.4));

        public double Fps => (int)_fps.Fps2;
        public bool Invert { get; private set; }

        public void Update(TimeSpan elapsed)
        {
            Invert = _accumulator.IsTime(elapsed) ? !Invert : Invert;
            _fps.Update(elapsed);
        }
    }
}

public sealed class Box(bool invert) : IWidget
{
    public void Render(Region area, Spectre.Tui.Buffer buffer)
    {
        for (var y = area.Top; y < area.Bottom; y++)
        {
            for (var x = area.Left; x < area.Right; x++)
            {
                buffer.SetCell(x, y, new Cell
                {
                    Rune = new Rune(GetRune(x, y)),
                });
            }
        }

        char GetRune(int x, int y)
        {
            if (x == 0 || x == area.Right - 1)
            {
                return invert ? '*' : '#';
            }

            if (y == 0 || y == area.Bottom - 1)
            {
                return invert ? '*' : '#';
            }

            return ' ';
        }
    }
}

public sealed class CenteredText(string text) : IWidget
{
    public void Render(Region area, Spectre.Tui.Buffer buffer)
    {
        var x = (area.Width - text.Length) / 2;
        var y = area.Height / 2;

        foreach (var (a, b) in text.Select((a, b) => (a, b)))
        {
            buffer.SetCell(x + b, y, new Cell
            {
                Rune = new Rune(a),
            });
        }
    }
}

public sealed class Accumulator(TimeSpan threshold)
{
    private TimeSpan _accumulator = TimeSpan.Zero;

    public bool IsTime(TimeSpan elapsed)
    {
        _accumulator += elapsed;
        if (_accumulator > threshold)
        {
            _accumulator = TimeSpan.Zero;
            return true;
        }

        return false;
    }
}

public sealed class Fps
{
    private TimeSpan _accumulator = TimeSpan.Zero;
    private int _frames;
    public int Last { get; private set; }

    public double Fps2 { get; private set; }

    public static TimeSpan Threshold = TimeSpan.FromSeconds(1);

    public void Update(TimeSpan elapsed)
    {
        Fps2 = 1f / elapsed.TotalSeconds;


        _frames++;
        _accumulator += elapsed;

        if (_accumulator.TotalSeconds > TimeSpan.FromSeconds(1).TotalSeconds)
        {
            Last = _frames;
            _frames = 0;
            _accumulator = TimeSpan.Zero;
        }
    }
}