namespace Spectre.Tui;

[PublicAPI]
public record TextSpan
{
    public string Text { get; init; }
    public Style? Style { get; init; }
    public bool IsLineBreak { get; }
    public bool IsWhiteSpace { get; }
    public bool IsAnsi { get; }

    public static TextSpan TextBreak { get; } = new TextSpan(Environment.NewLine, null, true, false);
    public static TextSpan Empty { get; } = new TextSpan(string.Empty, null, false, false);

    public TextSpan(string text, Style? style = null)
        : this(text, style, false, false)
    {
    }

    private TextSpan(string text, Style? style, bool lineBreak, bool control)
    {
        Text = string.Concat(text.SplitLines());
        Style = style;
        IsLineBreak = lineBreak;
        IsWhiteSpace = string.IsNullOrWhiteSpace(text);
        IsAnsi = control;
    }

    public static TextSpan Ansi(string control)
    {
        return new TextSpan(control, default, false, true);
    }

    public int GetWidth()
    {
        return Text.GetCellWidth();
    }

    public static implicit operator TextSpan(string text)
    {
        return new TextSpan(text);
    }
}