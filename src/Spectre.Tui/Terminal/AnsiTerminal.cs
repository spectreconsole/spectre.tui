namespace Spectre.Tui.Ansi;

[PublicAPI]
public abstract class AnsiTerminal : ITerminal
{
    private readonly StringBuilder _buffer;
    private readonly AnsiWriter _writer;
    private readonly AnsiState _state;

    public AnsiCapabilities Capabilities { get; }
    public ColorSystem ColorSystem { get; protected set; }
    protected ITerminalMode Mode { get; }

    protected AnsiTerminal(AnsiCapabilities capabilities, ITerminalMode mode)
    {
        Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));

        _buffer = new StringBuilder();
        _writer = new AnsiWriter(new StringWriter(_buffer), capabilities);
        _state = new AnsiState(_writer);

        Mode = mode ?? throw new ArgumentNullException(nameof(mode));
        Mode.OnAttach(_writer);
        Flush();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Flush(string buffer);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Mode.OnDetach(_writer);
            Flush();
        }
    }

    public void Flush()
    {
        try
        {
            Flush(_buffer.ToString());
        }
        finally
        {
            _buffer.Clear();
        }
    }

    public void Clear()
    {
        Mode.Clear(_writer);
    }

    public abstract Size GetSize();

    public void MoveTo(int x, int y)
    {
        Mode.MoveTo(x, y, _writer);

        // Invalidate tracked SGR state. InlineMode.MoveTo uses \e[u
        // (restore cursor) which some terminals (especially Windows)
        // interpret as DECRC, resetting SGR attributes to defaults.
        // Without this, AnsiState._previous would be stale and the
        // next Write could skip re-emitting style codes.
        _state.Reset();
    }

    public void Write(Cell cell)
    {
        if (!_state.Update(cell))
        {
            // State did not change
            _writer.Write(cell.Symbol);
            return;
        }

        // Reset SGR attributes
        _writer.ResetStyle();

        // Write the cell appearance
        _state.Write();

        // Write the cell symbol
        _writer.Write(cell.Symbol);

        // Swap the states
        _state.Swap();
    }

    private sealed class AnsiState(AnsiWriter writer)
    {
        private Style? _current;
        private Style? _previous;

        public bool Update(Cell cell)
        {
            _current = cell.Style;

            // First time we run?
            if (_previous == null)
            {
                return true;
            }

            return _current != _previous;
        }

        /// <summary>
        /// Invalidates tracked state so the next <see cref="Update"/>
        /// unconditionally signals a style change, forcing full SGR
        /// re-emission.
        /// </summary>
        public void Reset()
        {
            _previous = null;
            _current = null;
        }

        public void Swap()
        {
            _previous = _current;
            _current = null;
        }

        public void Write()
        {
            if (!_current.HasValue)
            {
                throw new InvalidOperationException("State has not been updated");
            }

            // Decoration
            writer.Decoration(_current.Value.Decoration);

            // Foreground
            if (_current.Value.Foreground != Color.Default)
            {
                writer.Foreground(_current.Value.Foreground);
            }

            // Background
            if (_current.Value.Background != Color.Default)
            {
                writer.Background(_current.Value.Background);
            }
        }
    }
}
