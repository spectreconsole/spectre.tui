namespace Spectre.Tui.Ansi;

internal sealed class WindowsTerminal : AnsiTerminal
{
    public WindowsTerminal(ColorSystem colors)
        : base(colors)
    {
    }

    protected override void Flush(string buffer)
    {
        System.Console.Write(buffer);
    }

    public override Size GetSize()
    {
        // TODO: Use Win32 API to get console size
        return new Size(System.Console.WindowWidth, System.Console.WindowHeight);
    }
}