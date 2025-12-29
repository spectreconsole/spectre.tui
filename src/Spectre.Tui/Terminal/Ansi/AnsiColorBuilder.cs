namespace Spectre.Tui.Ansi;

internal static class AnsiColorBuilder
{
    public static void GetSgr(
        ColorSystem system, Color color, bool foreground,
        ref List<byte> result)
    {
        switch (system)
        {
            case ColorSystem.NoColors:
                break;
            case ColorSystem.Legacy:
                AddThreeBitCodes(color, foreground, ref result);
                break;
            case ColorSystem.Standard:
                AddFourBitCodes(color, foreground, ref result);
                break;
            case ColorSystem.EightBit:
                AddEightBitCodes(color, foreground, ref result);
                break;
            case ColorSystem.TrueColor:
                AddTrueColorCodes(color, foreground, ref result);
                break;
        }
    }

    private static void AddThreeBitCodes(
        Color color, bool foreground,
        ref List<byte> result)
    {
        var number = color.Number;
        if (number == null || color.Number >= 8)
        {
            number = ColorPalette.ExactOrClosest(ColorSystem.Legacy, color).Number;
        }

        Debug.Assert(number >= 0 && number < 8, "Invalid range for 4-bit color");

        var mod = foreground ? 30 : 40;
        result.Add((byte)(number.Value + mod));
    }

    private static void AddFourBitCodes(
        Color color, bool foreground,
        ref List<byte> result)
    {
        var number = color.Number;
        if (number == null || color.Number >= 16)
        {
            number = ColorPalette.ExactOrClosest(ColorSystem.Standard, color).Number;
        }

        Debug.Assert(number >= 0 && number < 16, "Invalid range for 4-bit color");

        var mod = number < 8 ? (foreground ? 30 : 40) : (foreground ? 82 : 92);
        result.Add((byte)(number.Value + mod));
    }

    private static void AddEightBitCodes(
        Color color, bool foreground,
        ref List<byte> result)
    {
        var number = color.Number ?? ColorPalette.ExactOrClosest(ColorSystem.EightBit, color).Number;
        Debug.Assert(number >= 0 && number <= 255, "Invalid range for 8-bit color");

        var mod = foreground ? (byte)38 : (byte)48;
        result.AddRange([mod, 5, (byte)number]);
    }

    private static void AddTrueColorCodes(
        Color color, bool foreground,
        ref List<byte> result)
    {
        if (color.Number != null)
        {
            AddEightBitCodes(color, foreground, ref result);
            return;
        }

        var mod = foreground ? (byte)38 : (byte)48;
        result.AddRange([mod, 2, color.R, color.G, color.B]);
    }
}