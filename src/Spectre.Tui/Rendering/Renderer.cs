namespace Spectre.Tui;

[PublicAPI]
public sealed class Renderer
{
    private readonly ITerminal _terminal;
    private readonly Stopwatch _stopwatch;
    private readonly TargetFps _targetFps;
    private readonly SwapChain _swapChain;

    private TimeSpan _lastUpdate;
    private TimeSpan _lastRender;
    private Rectangle _viewport;
    private int _lastCellsChanged;

    public Renderer(ITerminal terminal)
    {
        _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _stopwatch = new Stopwatch();
        _targetFps = new TargetFps();
        _viewport = _terminal.GetSize().ToRectangle();
        _swapChain = new SwapChain(_viewport);
        _lastUpdate = TimeSpan.Zero;
        _lastCellsChanged = 0;

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

    public void NoTargetFps()
    {
        _targetFps.Target = null;
    }

    /// <summary>
    /// Returns the wall-clock time remaining until the next call to <see cref="Draw"/>
    /// would actually produce a render. Returns <see cref="TimeSpan.Zero"/> when a
    /// render is already due (or when no FPS target is set). Useful for callers that
    /// want to pace their loop precisely without busy-waiting.
    /// </summary>
    public TimeSpan TimeUntilNextRender()
    {
        if (_targetFps.Target == null)
        {
            return TimeSpan.Zero;
        }

        var elapsed = _stopwatch.Elapsed - _lastUpdate;
        var pendingAccumulated = _targetFps.Accumulated + elapsed;
        if (pendingAccumulated >= _targetFps.Target.Value)
        {
            return TimeSpan.Zero;
        }

        return _targetFps.Target.Value - pendingAccumulated;
    }

    public void Draw(Action<RenderContext, FrameInfo> callback)
    {
        // Calculate the time since last update
        var elapsed = _stopwatch.Elapsed - _lastUpdate;
        _lastUpdate = _stopwatch.Elapsed;

        var wasResized = ResizeIfNeeded();

        // Should we skip rendering?
        if (!_targetFps.ShouldRedraw(elapsed) && !wasResized)
        {
            return;
        }

        // Calculate the time since last render
        var elapsedSinceLastRender = _stopwatch.Elapsed - _lastRender;
        _lastRender = _stopwatch.Elapsed;

        // Fill out the current frame
        var frame = new RenderContext(_swapChain.Current, _viewport);
        callback(frame, new FrameInfo(elapsedSinceLastRender, _stopwatch.Elapsed, _lastCellsChanged));

        // Calculate the diff between the back and front buffer
        // and render it to the terminal buffer
        var lastPosition = default(Position?);
        var cellsChanged = 0;
        foreach (var (x, y, cell) in _swapChain.Diff())
        {
            cellsChanged++;

            // Do we need to move within the buffer?
            var movedForward = lastPosition != null && x == lastPosition.Value.X + 1 && y == lastPosition.Value.Y;
            if (!movedForward)
            {
                _terminal.MoveTo(x, y);
            }

            lastPosition = new Position(x, y);
            _terminal.Write(cell);
        }

        // Update the cell diff
        _lastCellsChanged = cellsChanged;

        // Show/Hide cursor
        if (frame.CursorPosition == null)
        {
            _terminal.HideCursor();
        }
        else
        {
            _terminal.ShowCursor(frame.CursorPosition.Value);
        }

        _terminal.Flush();
        _swapChain.Swap();
    }

    private bool ResizeIfNeeded()
    {
        var area = _terminal.GetSize().ToRectangle();
        if (area.Equals(_viewport))
        {
            return false;
        }

        _swapChain.Resize(area);
        _viewport = area;

        // Clear the terminal
        _terminal.Clear();

        // We resized
        return true;
    }
}