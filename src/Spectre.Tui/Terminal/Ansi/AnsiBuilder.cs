namespace Spectre.Tui.Ansi;

internal sealed class AnsiBuilder
{
    private readonly State _state = new();
    private readonly StringBuilder _builder = new();

    private const string Esc = "\e";
    private const string Csi = Esc + "[";

    public string GetAnsi(Cell cell, ColorSystem colors)
    {
        if (!_state.Update(cell))
        {
            // State did not change
            return cell.Symbol;
        }

        _builder.Clear();
        _builder.Append(Sgr(0)); // Close any previous state
        _builder.Append(Sgr(_state.GetAnsi(colors)));
        _builder.Append(cell.Symbol);

        // Swap the states
        _state.Swap();

        // Return the result
        return _builder.ToString();
    }

    private static string Sgr(params IEnumerable<byte> codes)
    {
        return $"{Csi}{string.Join(";", codes.Select(c => c.ToString()))}m";
    }

    private sealed class State
    {
        private Style? _current;
        private Style? _previous;
        private List<byte> _result = [];

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

        public List<byte> GetAnsi(ColorSystem colors)
        {
            if (!_current.HasValue)
            {
                throw new InvalidOperationException("State has not been updated");
            }

            _result.Clear();

            // Decoration
            AnsiDecorationBuilder.GetSgr(_current.Value.Decoration, ref _result);

            // Foreground
            if (_current.Value.Foreground != Color.Default)
            {
                AnsiColorBuilder.GetSgr(
                    colors, _current.Value.Foreground,
                    foreground: true, ref _result);
            }

            // Background
            if (_current.Value.Background != Color.Default)
            {
                AnsiColorBuilder.GetSgr(
                    colors, _current.Value.Background,
                    foreground: false, ref _result);
            }

            return _result;
        }
    }
}