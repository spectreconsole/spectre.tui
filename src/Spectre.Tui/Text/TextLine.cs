namespace Spectre.Tui;

[PublicAPI]
public sealed record TextLine
{
    public Appearance? Style { get; set; }
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

    public int GetWidth()
    {
        return Spans.Sum(segment => segment.GetWidth());
    }

    public static implicit operator TextLine(string text)
    {
        return new TextLine(new TextSpan(text));
    }
}

public static class LineExtensions
{
    extension(TextLine)
    {
        public static TextLine FromString(string text, Appearance? style = null)
        {
            return new TextLine(new TextSpan(text))
            {
                Style = style,
            };
        }
    }
}