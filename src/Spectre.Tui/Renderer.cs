using System.Diagnostics;
using Spectre.Tui.Terminal;

namespace Spectre.Tui;

public class Renderer
{
    private readonly ITerminal _terminal;
    private readonly Buffer _buffer;
    private readonly Stopwatch _stopwatch;
    private TimeSpan _lastUpdate;

    public Renderer(ITerminal terminal)
    {
        _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _buffer = Buffer.Empty(_terminal.GetSize());
        _stopwatch = new Stopwatch();
        _lastUpdate = TimeSpan.Zero;

        _stopwatch.Start();
    }

    public void Draw(Action<Frame, TimeSpan> callback)
    {
        var elapsed = _stopwatch.Elapsed - _lastUpdate;
        _lastUpdate = _stopwatch.Elapsed;

        // Set the cursor position
        _terminal.Write("\e[H");

        // Fill out the current frame
        var frame = new Frame(_buffer);
        callback(frame, elapsed);

        // Render the current frame
        var x = 0;
        var y = 0;
        foreach (var cell in frame.Buffer.Cells)
        {
            _terminal.Write((char)cell.Rune.Value);

            x++;
            if (x != frame.ViewPort.Right)
            {
                continue;
            }

            y++;
            x = 0;

            if (y != frame.ViewPort.Bottom)
            {
                _terminal.Write("\eE");
            }
        }

        // Flush the backend
        _terminal.Flush();
    }
}