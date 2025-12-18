namespace Spectre.Tui;

[PublicAPI]
public sealed record Text : IWidget
{
    public Style? Style { get; set; }
    public List<TextLine> Lines { get; } = [];

    public Text(List<TextLine> lines)
    {
        Lines = lines ?? throw new ArgumentNullException(nameof(lines));
    }

    public Text(TextLine textLine)
    {
        Lines = [textLine ?? throw new ArgumentNullException(nameof(textLine))];
    }

    public int GetWidth()
    {
        return Lines.Max(line => line.GetWidth());
    }

    public int GetHeight()
    {
        return Lines.Count;
    }

    public void Render(RenderContext context)
    {
        var maxWidth = context.Viewport.Width;
        var height = context.Viewport.Height;

        var y = 0;
        foreach (var line in Lines)
        {
            if (y >= height)
            {
                return;
            }

            context.SetLine(0, y, line, maxWidth);
            y++;
        }
    }
}

public static class TextExtensions
{
    extension(Text)
    {
        public static Text FromString(string text, Style? style = null)
        {
            List<TextLine> lines = [.. text.SplitLines().Select(line => TextLine.FromString(line, style))];
            return new Text(lines)
            {
                Style = style
            };
        }
    }
}