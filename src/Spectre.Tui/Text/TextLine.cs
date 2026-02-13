namespace Spectre.Tui;

[PublicAPI]
public sealed record TextLine
{
    public Style? Style { get; set; }
    public List<TextSpan> Spans { get; init; } = [];

    public TextLine()
        : this([])
    {
    }

    public TextLine(TextSpan span)
        : this([span])
    {
    }

    public TextLine(List<TextSpan> spans)
    {
        Spans = spans;
    }

    public TextLine(IEnumerable<TextSpan> spans)
    {
        Spans = [.. spans];
    }

    public int GetWidth()
    {
        return Spans.Sum(segment => segment.GetWidth());
    }

    public static implicit operator TextLine(string text)
    {
        return new TextLine(new TextSpan(text));
    }
}

[PublicAPI]
public static class LineExtensions
{
    extension(TextLine)
    {
        public static TextLine FromMarkup(string text, Style? style = null)
        {
            return new TextLine(
                AnsiMarkup
                    .Parse(text)
                    .Select(x => new TextSpan(x.Text, x.Style)))
                {
                    Style = style,
                };
        }

        public static TextLine FromString(string text, Style? style = null)
        {
            return new TextLine(new TextSpan(text))
            {
                Style = style,
            };
        }
    }
}