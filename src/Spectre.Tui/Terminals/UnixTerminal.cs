using System.Runtime.InteropServices;

namespace Spectre.Tui;

internal sealed class UnixTerminal : ITerminal
{
    private readonly StringBuilder _buffer;

    [DllImport("libc")]
    private static extern int write(int fd, byte[] buf, int n);

    public UnixTerminal()
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
            var bytes = Encoding.UTF8.GetBytes(_buffer.ToString());
            var _ = write(1, bytes, bytes.Length);
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

    private void Write(char text)
    {
        _buffer.Append(text);
    }

    private void Write(ReadOnlySpan<char> text)
    {
        _buffer.Append(text.ToArray());
    }
}
