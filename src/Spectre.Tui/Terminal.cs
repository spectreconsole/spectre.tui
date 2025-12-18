using System.Runtime.InteropServices;

namespace Spectre.Tui;

[PublicAPI]
public static class Terminal
{
    public static ITerminal Create()
    {
        // Do we support ANSI?
        var (supportsAnsi, legacyWindowsTerm) = AnsiDetector.Detect(false, true);
        if (!supportsAnsi || legacyWindowsTerm)
        {
            throw new InvalidOperationException("The current terminal does not support VT codes");
        }

        // What colors do we support?
        var colors = ColorSystemDetector.Detect();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal(colors);
        }

        return new UnixTerminal(colors);
    }
}