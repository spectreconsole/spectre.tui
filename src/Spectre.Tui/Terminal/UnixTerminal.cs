namespace Spectre.Tui.Ansi;

internal sealed class UnixTerminal : AnsiTerminal
{
    [DllImport("libc")]
    private static extern int write(int fd, byte[] buf, int n);

    public UnixTerminal(ColorSystem colors)
        : base(colors)
    {
    }

    public override Size GetSize()
    {
        // TODO: Use ioctl with TIOCGWINSZ
        return new Size(Console.WindowWidth, Console.WindowHeight);
    }

    protected override void Flush(string buffer)
    {
        var bytes = Encoding.UTF8.GetBytes(buffer);
        var _ = write(1, bytes, bytes.Length);
    }
}