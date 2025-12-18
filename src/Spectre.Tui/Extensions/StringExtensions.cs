using Wcwidth;

namespace Spectre.Tui;

internal static class StringExtensions
{
    extension(string? text)
    {
        public string[] SplitLines()
        {
            return NormalizeNewLines(text)
                .Split(['\n'], StringSplitOptions.None) ?? [];
        }

        private string NormalizeNewLines()
        {
            return text == null
                ? string.Empty
                : text.Replace("\r\n", "\n", StringComparison.Ordinal);
        }

        public int GetCellWidth()
        {
            return text == null ? 0 : UnicodeCalculator.GetWidth(text);
        }

        public IEnumerable<string> Graphemes()
        {
            if (text == null)
            {
                yield break;
            }

            var graphemes = StringInfo.GetTextElementEnumerator(text);
            while (graphemes.MoveNext())
            {
                yield return graphemes.GetTextElement();
            }
        }
    }
}