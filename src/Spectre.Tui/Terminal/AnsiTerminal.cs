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

        // TODO: Remove
        // _writer
        //     .EnterAltScreen()
        //     .CursorHome()
        //     .HideCursor();

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
            // TODO: Remove
            // _writer
            //     .ExitAltScreen()
            //     .ShowCursor();

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
        // TODO: Remove
        // _writer.EraseInDisplay(2);

        Mode.Clear(_writer);
    }

    public abstract Size GetSize();

    public void MoveTo(int x, int y)
    {
        // TODO: Remove
        // _writer.CursorPosition(y + 1, x + 1);

        Mode.MoveTo(x, y, _writer);
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
        private Appearance? _current;
        private Appearance? _previous;

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
