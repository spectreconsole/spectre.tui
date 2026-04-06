using Spectre.Console;
using Spectre.Tui.Ansi;

namespace Spectre.Tui.Testing;

public sealed class AnsiTestTerminal : AnsiTerminal, ITestTerminal
{
    private readonly Size _size;

    public string Output { get; private set; } = "[Terminal buffer not flushed]";

    public AnsiTestTerminal(
        ColorSystem colors = ColorSystem.TrueColor,
        Size? size = null,
        ITerminalMode? mode = null)
            : base(new AnsiCapabilities
            {
                Ansi = true,
                ColorSystem = colors,
                Links = true,
                AlternateBuffer = true,
            }, mode ?? new FullscreenMode())
    {
        _size = size ?? new Size(80, 25);
    }

    public override void HideCursor()
    {
        // Do not emit cursor logic (for now)
    }

    public override Size GetSize()
    {
        return Mode.GetSize(_size.Width, _size.Height);
    }

    protected override void Flush(string buffer)
    {
        Output = buffer;
    }
}
