namespace Spectre.Tui;

internal static class Int32Extensions
{
    extension(int value)
    {
        public int EnsurePositive()
        {
            return Math.Max(0, value);
        }
    }
}