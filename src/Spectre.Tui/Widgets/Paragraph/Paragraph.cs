namespace Spectre.Tui;

[PublicAPI]
public sealed record Paragraph : IWidget
{
    public Style? Style { get; set; }
    public Justify Alignment { get; set; } = Justify.Left;
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
    public Overflow Overflow { get; set; } = Overflow.Fold;
    public List<TextLine> Lines { get; } = [];

    public Paragraph()
        : this([])
    {
    }

    public Paragraph(TextLine line)
        : this([line])
    {
    }

    public Paragraph(List<TextLine> lines)
    {
        Lines = lines ?? throw new ArgumentNullException(nameof(lines));
    }

    public int GetWidth()
    {
        return Lines.Count == 0 ? 0 : Lines.Max(line => line.GetWidth());
    }

    public int GetHeight()
    {
        return Lines.Count;
    }

    public int GetWrappedHeight(int width)
    {
        return Overflow == Overflow.Fold
            ? TextLineWrapper.Wrap(Lines, width).Count()
            : Lines.Count;
    }

    public void Append(string text, Style? style)
    {
        foreach (var (_, first, _, part) in text.SplitLines().Enumerate())
        {
            if (first)
            {
                var line = Lines.LastOrDefault();

                if (line == null)
                {
                    Lines.Add(new TextLine());
                    line = Lines[^1];
                }

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

    void IWidget.Render(RenderContext context)
    {
        var maxWidth = context.Viewport.Width;
        var height = context.Viewport.Height;

        if (maxWidth <= 0 || height <= 0)
        {
            return;
        }

        var lines = EnumerateLines(maxWidth).ToList();
        if (lines.Count == 0)
        {
            return;
        }

        int offsetY;
        int startIndex;
        if (lines.Count <= height)
        {
            startIndex = 0;
            offsetY = VerticalAlignment switch
            {
                VerticalAlignment.Middle => (height - lines.Count) / 2,
                VerticalAlignment.Bottom => height - lines.Count,
                _ => 0,
            };
        }
        else
        {
            offsetY = 0;
            startIndex = VerticalAlignment == VerticalAlignment.Bottom
                ? lines.Count - height
                : 0;
        }

        var y = offsetY;
        for (var i = startIndex; i < lines.Count; i++)
        {
            if (y >= height)
            {
                return;
            }

            var line = lines[i];
            var lineWidth = Math.Min(line.GetWidth(), maxWidth);
            var x = Alignment switch
            {
                Justify.Center => Math.Max(0, (maxWidth - lineWidth) / 2),
                Justify.Right => Math.Max(0, maxWidth - lineWidth),
                _ => 0,
            };

            context.SetLine(x, y, line, maxWidth - x, Style);
            y++;
        }
    }

    private IEnumerable<TextLine> EnumerateLines(int maxWidth)
    {
        switch (Overflow)
        {
            case Overflow.Crop:
                return Lines;
            case Overflow.Ellipsis:
                return Lines.Select(line => TextLineWrapper.TruncateWithEllipsis(line, maxWidth));
            default:
                return TextLineWrapper.Wrap(Lines, maxWidth);
        }
    }
}

[PublicAPI]
public static class ParagraphExtensions
{
    extension(Paragraph)
    {
        public static Paragraph From(string text, Style? appearance = null)
        {
            return Paragraph.FromString(text, appearance);
        }

        public static Paragraph FromString(string text, Style? appearance = null)
        {
            var paragraph = new Paragraph
            {
                Style = appearance,
            };
            paragraph.Append(text, appearance);
            return paragraph;
        }

        public static Paragraph FromMarkup(string text, Style? style = null)
        {
            var result = new Paragraph
            {
                Style = style,
            };

            foreach (var line in AnsiMarkup.Parse(text, style))
            {
                result.Append(line.Text, line.Style);
            }

            return result;
        }
    }

    extension(Paragraph paragraph)
    {
        public Paragraph Style(Style? style)
        {
            paragraph.Style = style;
            return paragraph;
        }

        public Paragraph Alignment(Justify alignment)
        {
            paragraph.Alignment = alignment;
            return paragraph;
        }

        public Paragraph LeftAligned()
        {
            return paragraph.Alignment(Justify.Left);
        }

        public Paragraph Centered()
        {
            return paragraph.Alignment(Justify.Center);
        }

        public Paragraph RightAligned()
        {
            return paragraph.Alignment(Justify.Right);
        }

        public Paragraph VerticalAlignment(VerticalAlignment alignment)
        {
            paragraph.VerticalAlignment = alignment;
            return paragraph;
        }

        public Paragraph AlignedTop()
        {
            return paragraph.VerticalAlignment(Tui.VerticalAlignment.Top);
        }

        public Paragraph AlignedMiddle()
        {
            return paragraph.VerticalAlignment(Tui.VerticalAlignment.Middle);
        }

        public Paragraph AlignedBottom()
        {
            return paragraph.VerticalAlignment(Tui.VerticalAlignment.Bottom);
        }

        public Paragraph Overflow(Overflow overflow)
        {
            paragraph.Overflow = overflow;
            return paragraph;
        }

        public Paragraph Cropped()
        {
            paragraph.Overflow = Tui.Overflow.Crop;
            return paragraph;
        }

        public Paragraph Ellipsis()
        {
            paragraph.Overflow = Tui.Overflow.Ellipsis;
            return paragraph;
        }

        public Paragraph Folded()
        {
            paragraph.Overflow = Tui.Overflow.Fold;
            return paragraph;
        }
    }
}
