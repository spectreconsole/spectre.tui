namespace Spectre.Tui;

[PublicAPI]
public sealed class Renderer
{
    private readonly ITerminal _terminal;
    private readonly Stopwatch _stopwatch;
    private readonly Buffer[] _buffers;
    private TimeSpan _lastUpdate;
    private TimeSpan _lastRender;
    private int _bufferIndex;
    private Rectangle _viewport;
    private readonly TargetFps _targetFps;

    public Renderer(ITerminal terminal)
    {
        _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _lastUpdate = TimeSpan.Zero;
        _viewport = _terminal.GetSize().ToRectangle();
        _targetFps = new TargetFps();
        _buffers =
        [
            Buffer.Empty(_viewport),
            Buffer.Empty(_viewport),
        ];

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    public void SetTargetFps(int fps)
    {
        if (fps <= 0)
        {
            throw new ArgumentException("Target FPS cannot be equal to or less than zero");
        }

        _targetFps.Target = TimeSpan.FromSeconds(1) / fps;
    }

    public void Draw(Action<RenderContext, TimeSpan> callback)
    {
        // Calculate the time since last update
        var elapsed = _stopwatch.Elapsed - _lastUpdate;
        _lastUpdate = _stopwatch.Elapsed;

        var wasResized = ResizeIfNeeded();

        // Should we skip rendering?
        if (!_targetFps.ShouldRedraw(elapsed))
        {
            if (!wasResized)
            {
                return;
            }
        }

        // Calculate the time since last render
        var elapsedSinceLastRender = _stopwatch.Elapsed - _lastRender;
        _lastRender = _stopwatch.Elapsed;

        // Fill out the current frame
        var frame = new RenderContext(_buffers[_bufferIndex], _viewport, _viewport);
        callback(frame, elapsedSinceLastRender);

        // Calculate the diff between the back and front buffer
        var prev = _buffers[1 - _bufferIndex];
        var curr = _buffers[_bufferIndex];
        var diff = prev.Diff(curr);

        // Render the current frame
        var lastPosition = default(Position?);
        foreach (var (x, y, cell) in diff)
        {
            // Do we need to move within the buffer?
            var movedForward = lastPosition != null && x == lastPosition.Value.X + 1 && y == lastPosition.Value.Y;
            if (!movedForward)
            {
                _terminal.MoveTo(x, y);
            }

            lastPosition = new Position(x, y);
            _terminal.Write(cell);
        }

        // Swap the buffers
        SwapBuffers();

        // Flush the backend
        _terminal.Flush();
    }

    private bool ResizeIfNeeded()
    {
        var area = _terminal.GetSize().ToRectangle();
        if (area.Equals(_viewport))
        {
            return false;
        }

        // Reset buffer
        _buffers[_bufferIndex].Resize(area);
        _buffers[1 - _bufferIndex].Resize(area);
        _viewport = area;

        // Clear the terminal
        _terminal.Clear();

        // Reset the back buffer
        _buffers[1 - _bufferIndex].Reset();

        // We resized
        return true;
    }

    private void SwapBuffers()
    {
        _buffers[1 - _bufferIndex].Reset();
        _bufferIndex = 1 - _bufferIndex;
    }
}

internal sealed class TargetFps
{
    public TimeSpan Accumulated { get; private set; }

    public TimeSpan? Target
    {
        get;
        set
        {
            Accumulated = TimeSpan.Zero;
            field = value;
        }
    }

    public bool ShouldRedraw(TimeSpan delta)
    {
        if (Target == null)
        {
            // Uncapped FPS
            return true;
        }

        Accumulated += delta;

        if (Accumulated >= Target)
        {
            Accumulated = TimeSpan.Zero;
            return true;
        }

        return false;
    }
}