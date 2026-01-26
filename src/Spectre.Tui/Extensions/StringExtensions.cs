using Wcwidth;

namespace Spectre.Tui;

internal static class StringExtensions
{
    extension(string? text)
    {
        public string[] SplitLines()
        {
            return NormalizeNewLines(text)
                .Split(['\n'], StringSplitOptions.None);
        }

        internal string[] SplitWords()
        {
            var result = new List<string>();

            using (var reader = new StringBuffer(text))
            {
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    result.Add(char.IsWhiteSpace(current)
                        ? Read(reader, char.IsWhiteSpace)
                        : Read(reader, c => !char.IsWhiteSpace(c)));
                }
            }

            return [.. result];

            static string Read(StringBuffer reader, Func<char, bool> criteria)
            {
                var buffer = new StringBuilder();
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    if (!criteria(current))
                    {
                        break;
                    }

                    buffer.Append(reader.Read());
                }

                return buffer.ToString();
            }
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

    private static string NormalizeNewLines(string? text)
    {
        return text == null
            ? string.Empty
            : text.Replace("\r\n", "\n", StringComparison.Ordinal);
    }
}