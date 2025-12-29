namespace Spectre.Tui;

internal static class ColorSystemDetector
{
    // Adapted from https://github.com/willmcgugan/rich/blob/f0c29052c22d1e49579956a9207324d9072beed7/rich/console.py#L391
    public static ColorSystem Detect()
    {
        // No colors?
        if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
        {
            return ColorSystem.NoColors;
        }

        // Windows?
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows 10.0.15063 and above support true color,
            // and we can probably assume that the next major
            // version of Windows will support true color as well.
            if (GetWindowsVersionInformation(out var major, out var build))
            {
                if ((major == 10 && build >= 15063) || major > 10)
                {
                    return ColorSystem.TrueColor;
                }
            }

            return ColorSystem.EightBit;
        }

        var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");
        if (!string.IsNullOrWhiteSpace(colorTerm))
        {
            if (colorTerm.Equals("truecolor", StringComparison.OrdinalIgnoreCase) ||
                colorTerm.Equals("24bit", StringComparison.OrdinalIgnoreCase))
            {
                return ColorSystem.TrueColor;
            }
        }

        // Should we default to eight-bit colors?
        return ColorSystem.EightBit;
    }

    private static bool GetWindowsVersionInformation(out int major, out int build)
    {
        major = 0;
        build = 0;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        // The reason we're not always using this, is because it will return wrong values on other runtimes than .NET 6+
        // See https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/environment-osversion-returns-correct-version
        var version = Environment.OSVersion.Version;
        major = version.Major;
        build = version.Build;
        return true;
    }
}