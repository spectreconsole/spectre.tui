namespace Spectre.Tui;

// TODO: Move this into Spectre.Console.Ansi
internal static class StyleExtensions
{
    public static Style Combine(this Style style, Style? other)
    {
        return other == null
            ? style
            : style.Combine(other.Value);
    }
}