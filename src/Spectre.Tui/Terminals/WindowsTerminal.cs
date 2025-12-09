namespace Spectre.Tui;

internal sealed class WindowsTerminal : ITerminal
{
    private readonly StringBuilder _buffer;

    public WindowsTerminal()
    {
        _buffer = new StringBuilder();

        WriteAndFlush("\e[?1049h\e[H");
        WriteAndFlush("\e[?25l");
    }

    public void Dispose()
    {
        WriteAndFlush("\e[?1049l");
        WriteAndFlush("\e[?25h");
    }

    public void Write(IEnumerable<(int x, int y, Cell cell)> updates)
    {
        var lastPosition = default(Position?);

        foreach (var (x, y, cell) in updates)
        {
            if (lastPosition == null || !(x == lastPosition.Value.X + 1 && y == lastPosition.Value.Y))
            {
                MoveTo(x, y);
            }

            lastPosition = new Position(x, y);
            Write(AnsiBuilder.GetAnsi(cell));
        }
    }

    public void Flush()
    {
        try
        {
            Console.Write(_buffer.ToString());
        }
        finally
        {
            _buffer.Clear();
        }
    }

    public void Clear()
    {
        Write("\e[2J");
    }

    public Size GetSize()
    {
        // TODO: Use ioctl with TIOCGWINSZ
        return new Size(Console.WindowWidth, Console.WindowHeight);
    }

    private void MoveTo(int x, int y)
    {
        Write($"\e[{y + 1};{x + 1}H");
    }

    private void WriteAndFlush(ReadOnlySpan<char> text)
    {
        Write(text);
        Flush();
    }

    private void Write(ReadOnlySpan<char> text)
    {
        _buffer.Append(text.ToArray());
    }
}
