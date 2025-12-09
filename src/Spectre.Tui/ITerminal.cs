using System.Runtime.InteropServices;

namespace Spectre.Tui;

public interface ITerminal : IDisposable
{
    void Clear();
    Size GetSize();
    void Write(IEnumerable<(int x, int y, Cell cell)> updates);
    void Flush();
}

public sealed class Terminal : ITerminal
{
    private readonly StringBuilder _buffer;

    [DllImport("libc")]
    private static extern int write(int fd, byte[] buf, int n);

    public Terminal()
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

file static class AnsiBuilder
{
    private const string Esc = "\e";
    private const string Csi = Esc + "[";

    public static string GetAnsi(Cell cell)
    {
        return $"{Sgr(GetAnsiCodes(cell.Decoration))}{cell.Rune}{Sgr(0)}";
    }

    private static string Sgr(params IEnumerable<byte> codes)
    {
        var joinedCodes = string.Join(";", codes.Select(c => c.ToString()));
        return $"{Csi}{joinedCodes}m";
    }

    private static IEnumerable<byte> GetAnsiCodes(Decoration decoration)
    {
        if ((decoration & Decoration.Bold) != 0)
        {
            yield return 1;
        }

        if ((decoration & Decoration.Dim) != 0)
        {
            yield return 2;
        }

        if ((decoration & Decoration.Italic) != 0)
        {
            yield return 3;
        }

        if ((decoration & Decoration.Underlined) != 0)
        {
            yield return 4;
        }

        if ((decoration & Decoration.SlowBlink) != 0)
        {
            yield return 5;
        }

        if ((decoration & Decoration.RapidBlink) != 0)
        {
            yield return 6;
        }

        if ((decoration & Decoration.Invert) != 0)
        {
            yield return 7;
        }

        if ((decoration & Decoration.Conceal) != 0)
        {
            yield return 8;
        }

        if ((decoration & Decoration.Strikethrough) != 0)
        {
            yield return 9;
        }
    }
}