using System.Text;

namespace Spectre.Tui.Testing;

public sealed class TestTerminal : ITerminal
{
    private readonly Size _size;
    private readonly string[] _buffer;
    private int _position;

    public bool SupportsAnsi { get; set; }
    public string Output { get; private set; } = "[Terminal buffer not flushed]";

    public TestTerminal(Size? size)
    {
        _size = size ?? new Size(80, 25);
        _buffer = new string[_size.Area];
        _position = 0;

        Array.Fill(_buffer, "•");
    }

    public void Dispose()
    {
    }

    public void Clear()
    {
        Array.Fill(_buffer, "•");
        _position = 0;
    }

    public Size GetSize()
    {
        return _size;
    }

    public void MoveTo(int x, int y)
    {
        _position = (y * _size.Width) + x;
    }

    public void Write(Cell cell)
    {
        _buffer[_position] = cell.Symbol;
        _position++;
    }

    public void Flush()
    {
        Output = Render();
        Clear();
    }

    private string Render()
    {
        var output = new StringBuilder();

        for (var y = 0; y < _size.Height; y++)
        {
            for (var x = 0; x < _size.Width; x++)
            {
                output.Append(_buffer[(y * _size.Width) + x]);
            }

            if (y != _size.Height - 1)
            {
                output.AppendLine();
            }
        }

        return output.ToString();
    }
}