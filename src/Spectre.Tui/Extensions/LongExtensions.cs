namespace Spectre.Tui;

internal static class LongExtensions
{
    extension(long n)
    {
        public int GetDigitCount()
        {
            if (n == 0)
            {
                return 0;
            }

            return (int)Math.Floor(Math.Log10(n) + 1);
        }
    }
}