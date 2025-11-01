using System.Collections;

namespace Spectre.Tui;

public sealed class Buffer
{
    public Region Region { get; }
    public Cell[] Cells { get; }
    public int Length { get; }

    private Buffer(Region region, Cell[] cells)
    {
        Region = region;
        Cells = cells ?? throw new ArgumentNullException(nameof(cells));
        Length = region.Area;

        if (Length != Cells.Length)
        {
            throw new InvalidOperationException("Mismatch between buffer size and provided area");
        }
    }

    public static Buffer Empty(Size size)
    {
        return Empty(new Region(0, 0, size.Width, size.Height));
    }

    public static Buffer Empty(Region region)
    {
        return Filled(region, new Cell());
    }

    public static Buffer Filled(Size size, Cell cell)
    {
        return Filled(new Region(0, 0, size.Width, size.Height), cell);
    }

    public static Buffer Filled(Region area, Cell cell)
    {
        var cells = new Cell[area.Area];
        Array.Fill(cells, cell);
        return new Buffer(area, cells);
    }

    public Cell GetCell(int index)
    {
        if (index < 0 || index >= Length)
        {
            return new Cell();
        }

        return Cells[index];
    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Region.Width || y >= Region.Height)
        {
            return new Cell();
        }

        var index = (y * Region.Width) + x;
        if (index >= Length)
        {
            return new Cell();
        }

        return Cells[index];
    }

    public void SetCell(int x, int y, Cell cell)
    {
        if (x < 0 || y < 0 || x >= Region.Width || y >= Region.Height)
        {
            return;
        }

        var index = (y * Region.Width) + x;
        if (index >= Length)
        {
            return;
        }

        Cells[index] = cell;
    }

    public void Render<T>(T widget, Region area)
        where T : IWidget
    {
        widget.Render(area, this);
    }

    public void Render<T, TState>(T widget, TState state, Region area)
        where T : IWidget<TState>
    {
        widget.Render(area, state, this);
    }
}

file sealed class BufferEnumerator : IEnumerator<(int x, int y, Cell cell)>
{
    private readonly Buffer _buffer;
    private (int x, int y, Cell cell)? _current;
    private int _index = -1;

    object? IEnumerator.Current => _current!;
    (int x, int y, Cell cell) IEnumerator<(int x, int y, Cell cell)>.Current => _current!.Value;

    public BufferEnumerator(Buffer buffer)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
    }

    public bool MoveNext()
    {
        if (_index >= _buffer.Length - 1)
        {
            return false;
        }

        _index++;

        var x = _index % _buffer.Region.Width;
        var y = _index / _buffer.Region.Height;

        _current = (x, y, _buffer.GetCell(_index));
        return true;
    }

    public void Reset()
    {
        _index = 0;
    }

    public void Dispose()
    {
    }
}