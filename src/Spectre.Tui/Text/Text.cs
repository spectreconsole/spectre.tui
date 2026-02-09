namespace Spectre.Tui;

[PublicAPI]
public sealed record Text : IWidget
{
    public Style? Style { get; set; }
    public List<TextLine> Lines { get; } = [];

    public Text()
        : this([])
    {
    }

    public Text(TextLine line)
        : this([line])
    {
    }

    public Text(List<TextLine> lines)
    {
        Lines = lines ?? throw new ArgumentNullException(nameof(lines));
    }

    public int GetWidth()
    {
        return Lines.Max(line => line.GetWidth());
    }

    public int GetHeight()
    {
        return Lines.Count;
    }

    public void Append(string text, Style? style)
    {
        foreach (var (_, first, _, part) in text.SplitLines().Enumerate())
        {
            if (first)
            {
                var line = Lines.LastOrDefault();

                // No lines?
                if (line == null)
                {
                    Lines.Add(new TextLine());
                    line = Lines[^1];
                }

                // Empty part?
                if (string.IsNullOrEmpty(part))
                {
                    line.Spans.Add(TextSpan.Empty);
                }
                else
                {
                    foreach (var span in part.SplitWords())
                    {
                        line.Spans.Add(new TextSpan(span, style ?? Spectre.Console.Style.Plain));
                    }
                }
            }
            else
            {
                var line = new TextLine();

                // Empty part?
                if (string.IsNullOrEmpty(part))
                {
                    line.Spans.Add(TextSpan.Empty);
                }
                else
                {
                    foreach (var span in part.SplitWords())
                    {
                        line.Spans.Add(new TextSpan(span, style ?? Spectre.Console.Style.Plain));
                    }
                }


                Lines.Add(line);
            }
        }
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
        public static Text FromMarkup(string text, Style? style = null)
        {
            var result = new Text();
            foreach (var line in AnsiMarkup.Parse(text, style))
            {
                result.Append(line.Text, line.Style);
            }

            return result;
        }

        public static Text FromString(string text, Style? appearance = null)
        {
            List<TextLine> lines = [.. text.SplitLines().Select(line => TextLine.FromString(line, appearance))];
            return new Text(lines)
            {
                Style = appearance
            };
        }
    }
}