namespace Spectre.Tui;

internal static class AnsiBuilder
{
    private const string Esc = "\e";
    private const string Csi = Esc + "[";

    public static string GetAnsi(ref Cell cell, ColorSystem colors)
    {
        return $"{Sgr(GetAnsiCodes(colors, ref cell))}{cell.Symbol}{Sgr(0)}";
    }

    private static IEnumerable<byte> GetAnsiCodes(ColorSystem colors, ref Cell cell)
    {
        var codes = AnsiDecorationBuilder.GetAnsiCodes(cell.Decoration);

        // Got foreground?
        if (cell.Foreground != Color.Default)
        {
            codes = codes.Concat(
                AnsiColorBuilder.GetAnsiCodes(
                    colors,
                    cell.Foreground,
                    true));
        }

        // Got background?
        if (cell.Background != Color.Default)
        {
            codes = codes.Concat(
                AnsiColorBuilder.GetAnsiCodes(
                    colors,
                    cell.Background,
                    false));
        }

        return codes;
    }

    private static string Sgr(params IEnumerable<byte> codes)
    {
        var joinedCodes = string.Join(";", codes.Select(c => c.ToString()));
        return $"{Csi}{joinedCodes}m";
    }
}