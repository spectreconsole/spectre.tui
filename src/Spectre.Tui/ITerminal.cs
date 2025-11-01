using System.Runtime.InteropServices;
using System.Text;

namespace Spectre.Tui.Terminal;

public interface ITerminal : IDisposable
{
    void Write(char text);
    void Write(ReadOnlySpan<char> text);
    void Flush();
    Size GetSize();
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
    }

    public void Write(char text)
    {
        _buffer.Append(text);
    }

    public void Write(ReadOnlySpan<char> text)
    {
        _buffer.Append(text.ToArray());
    }

    public void Flush()
    {
        byte[] utf8 = Encoding.UTF8.GetBytes(_buffer.ToString());
        write(1, utf8, utf8.Length);
        _buffer.Clear();
    }

    public Size GetSize()
    {
        // TODO: Use ioctl with TIOCGWINSZ
        return new Size(Console.WindowWidth, Console.WindowHeight);
    }

    private void WriteAndFlush(ReadOnlySpan<char> text)
    {
        Write(text);
        Flush();
    }
}