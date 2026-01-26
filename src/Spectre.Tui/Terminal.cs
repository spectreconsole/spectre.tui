namespace Spectre.Tui;

[PublicAPI]
public static class Terminal
{
    public static ITerminal Create()
    {
        var caps = AnsiCapabilities.Create(System.Console.Out);
        if (!caps.Ansi)
        {
            throw new NotSupportedException("Your terminal does not support ANSI");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal(caps);
        }

        return new UnixTerminal(caps);
    }
}