using Wcwidth;

namespace Spectre.Tui;

internal static class CharExtensions
{
    extension(char character)
    {
        public int GetCellWidth()
        {
            return UnicodeCalculator.GetWidth(character);
        }
    }
}