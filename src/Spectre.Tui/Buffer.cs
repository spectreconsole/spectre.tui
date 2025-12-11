namespace Spectre.Tui;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
internal sealed class Buffer
{
    private Rectangle _screen;
    private Cell[] _cells;
    private int _length;

    internal Buffer(Rectangle screen, Cell[] cells)
    {
        _screen = screen;
        _cells = cells ?? throw new ArgumentNullException(nameof(cells));
        _length = screen.CalculateArea();

        if (_length != _cells.Length)
        {
            throw new InvalidOperationException("Mismatch between buffer size and provided area");
        }
    }

    public Cell? GetCell(int index)
    {
        return index < 0 || index >= _length
            ? null
            : _cells[index];
    }

    public Cell? GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _screen.Width || y >= _screen.Height)
        {
            return null;
        }

        return _cells[(y * _screen.Width) + x];
    }

    public void Reset()
    {
        for (var index = 0; index < _cells.Length; index++)
        {
            _cells[index].Reset();
        }
    }

    public void Resize(Rectangle area)
    {
        var cells = new Cell[area.CalculateArea()];
        for (var index = 0; index < cells.Length; index++)
        {
            cells[index] = new Cell();
        }

        _cells = cells;
        _screen = area;
        _length = _screen.CalculateArea();
    }

    public IEnumerable<(int x, int y, Cell)> Diff(Buffer other)
    {
        foreach (var (index, (current, previous)) in other._cells.Zip(_cells).Index())
        {
            if (current.Equals(previous))
            {
                continue;
            }

            var x = (index % _screen.Width) + _screen.X;
            var y = (index / _screen.Width) + _screen.Y;
            yield return (x, y, current);
        }
    }

    private string DebuggerDisplay()
    {
        return $"{_screen.X},{_screen.Y},{_screen.Width},{_screen.Height} ({_length} cells)";
    }
}

internal static class BufferExtensions
{
    extension(Buffer)
    {
        public static Buffer Empty(Rectangle region)
        {
            return Filled(region, new Cell());
        }

        public static Buffer Filled(Rectangle area, Cell prototype)
        {
            var cells = new Cell[area.CalculateArea()];
            for (var index = 0; index < cells.Length; index++)
            {
                cells[index] = prototype.Clone();
            }

            return new Buffer(area, cells);
        }
    }
}