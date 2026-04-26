namespace Spectre.Tui;

public sealed class Scrollbar
{
    public required char? Begin { get; init; }
    public required char? End { get; init; }
    public required char Track { get; init; }
    public required char Thumb { get; init; }

    public static Scrollbar Vertical { get; } = new()
    {
        Begin = '↑',
        End = '↓',
        Track = Line.Symbols.Vertical,
        Thumb = Block.Full,
    };

    public static Scrollbar Horizontal { get; } = new()
    {
        Begin = '←',
        End = '→',
        Track = Line.Symbols.Horizontal,
        Thumb = Block.Full,
    };

    public static Scrollbar DoubleVertical { get; } = new()
    {
        Begin = '▲',
        End = '▼',
        Track = Line.Symbols.DoubleVertical,
        Thumb = Block.Full,
    };

    public static Scrollbar DoubleHorizontal { get; } = new()
    {
        Begin = '◄',
        End = '►',
        Track = Line.Symbols.DoubleHorizontal,
        Thumb = Block.Full,
    };
}