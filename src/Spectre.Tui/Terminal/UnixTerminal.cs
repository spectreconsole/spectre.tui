namespace Spectre.Tui.Ansi;

internal sealed class UnixTerminal(AnsiCapabilities capabilities)
    : AnsiTerminal(capabilities)
{
    [DllImport("libc")]
    private static extern int write(int fd, byte[] buf, int n);

    public override Size GetSize()
    {
        // TODO: Use ioctl with TIOCGWINSZ
        return new Size(System.Console.WindowWidth, System.Console.WindowHeight);
    }

    protected override void Flush(string buffer)
    {
        var bytes = Encoding.UTF8.GetBytes(buffer);
        var _ = write(1, bytes, bytes.Length);
    }
}