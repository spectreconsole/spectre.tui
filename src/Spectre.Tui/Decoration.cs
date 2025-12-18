namespace Spectre.Tui;

[Flags]
[PublicAPI]
public enum Decoration : ushort
{
    None = 0,
    Bold = 1 << 1,
    Dim = 1 << 2,
    Italic = 1 << 3,
    Underlined = 1 << 4,
    SlowBlink = 1 << 5,
    RapidBlink = 1 << 6,
    Invert = 1 << 7,
    Conceal = 1 << 8,
    Strikethrough = 1 << 9,
}