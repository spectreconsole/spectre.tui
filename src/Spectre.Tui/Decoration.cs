namespace Spectre.Tui;

public enum Decoration : ushort
{
    None = 0,
    Bold = 1 << 1,
    Dim = 1 << 2,
    Italic = 1 << 3,
    Underlined = 1 << 4,
    SlowBlink = 1 << 5,
    RapidBlink = 1 << 6,
    Reversed = 1 << 7,
    Hidden = 1 << 8,
    CrossedOut = 1 << 9,
}