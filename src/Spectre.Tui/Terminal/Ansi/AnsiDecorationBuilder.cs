namespace Spectre.Tui.Ansi;

internal static class AnsiDecorationBuilder
{
    public static void GetSgr(Decoration decoration, ref List<byte> result)
    {
        if ((decoration & Decoration.Bold) != 0)
        {
            result.Add(1);
        }

        if ((decoration & Decoration.Dim) != 0)
        {
            result.Add(2);
        }

        if ((decoration & Decoration.Italic) != 0)
        {
            result.Add(3);
        }

        if ((decoration & Decoration.Underlined) != 0)
        {
            result.Add(4);
        }

        if ((decoration & Decoration.SlowBlink) != 0)
        {
            result.Add(5);
        }

        if ((decoration & Decoration.RapidBlink) != 0)
        {
            result.Add(6);
        }

        if ((decoration & Decoration.Invert) != 0)
        {
            result.Add(7);
        }

        if ((decoration & Decoration.Conceal) != 0)
        {
            result.Add(8);
        }

        if ((decoration & Decoration.Strikethrough) != 0)
        {
            result.Add(9);
        }
    }
}