using Spectre.Tui.Ansi;

namespace Spectre.Tui.Testing;

public sealed class AnsiTestTerminal : AnsiTerminal, ITestTerminal
{
    private readonly Size _size;


    public string Output { get; private set; } = "[Terminal buffer not flushed]";

    public AnsiTestTerminal(ColorSystem colors, Size? size = null)
        : base(colors)
    {
        _size = size ?? new Size(80, 25);
    }

    public override Size GetSize()
    {
        return _size;
    }

    protected override void Flush(string buffer)
    {
        Output = buffer;
    }
}