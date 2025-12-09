namespace Spectre.Tui;

internal static class AnsiBuilder
{
    private const string Esc = "\e";
    private const string Csi = Esc + "[";

    public static string GetAnsi(Cell cell)
    {
        return $"{Sgr(GetAnsiCodes(cell.Decoration))}{cell.Rune}{Sgr(0)}";
    }

    private static string Sgr(params IEnumerable<byte> codes)
    {
        var joinedCodes = string.Join(";", codes.Select(c => c.ToString()));
        return $"{Csi}{joinedCodes}m";
    }

    private static IEnumerable<byte> GetAnsiCodes(Decoration decoration)
    {
        if ((decoration & Decoration.Bold) != 0)
        {
            yield return 1;
        }

        if ((decoration & Decoration.Dim) != 0)
        {
            yield return 2;
        }

        if ((decoration & Decoration.Italic) != 0)
        {
            yield return 3;
        }

        if ((decoration & Decoration.Underlined) != 0)
        {
            yield return 4;
        }

        if ((decoration & Decoration.SlowBlink) != 0)
        {
            yield return 5;
        }

        if ((decoration & Decoration.RapidBlink) != 0)
        {
            yield return 6;
        }

        if ((decoration & Decoration.Invert) != 0)
        {
            yield return 7;
        }

        if ((decoration & Decoration.Conceal) != 0)
        {
            yield return 8;
        }

        if ((decoration & Decoration.Strikethrough) != 0)
        {
            yield return 9;
        }
    }
}