namespace Spectre.Tui;

[PublicAPI]
public sealed class BoxWidget(Style? style = null) : IWidget
{
    public Style? Style { get; set; } = style;
    public Border Border { get; set; } = Border.Rounded;
    public IWidget? Inner { get; set; }
    public List<BoxTitle> Titles { get; } = [];
    public int TitlePadding { get; set; }

    void IWidget.Render(RenderContext context)
    {
        var area = context.Viewport;

        if (area.Height == 0)
        {
            return;
        }

        if (area.Height == 1)
        {
            for (var x = 0; x < area.Width; x++)
            {
                if (x == 0)
                {
                    context.SetSymbol(x, 0, Border.VerticalLeft);
                }
                else if (x == area.Width - 1)
                {
                    context.SetSymbol(x, 0, Border.VerticalRight);
                }
                else
                {
                    context.SetSymbol(x, 0, Border.HorizontalTop);
                }

                context.SetStyle(x, 0, Style);
            }

            RenderTitles(context, TitlePosition.Top, 0);
            return;
        }

        // Top/Bottom
        for (var x = 0; x < area.Width; x++)
        {
            if (x == 0)
            {
                context.SetSymbol(x, 0, Border.TopLeft);
                context.SetStyle(x, 0, Style);
                context.SetSymbol(x, area.Height - 1, Border.BottomLeft);
                context.SetStyle(x, area.Height - 1, Style);
            }
            else if (x == area.Width - 1)
            {
                context.SetSymbol(x, 0, Border.TopRight);
                context.SetStyle(x, 0, Style);
                context.SetSymbol(x, area.Height - 1, Border.BottomRight);
                context.SetStyle(x, area.Height - 1, Style);
            }
            else
            {
                context.SetSymbol(x, 0, Border.HorizontalTop);
                context.SetStyle(x, 0, Style);
                context.SetSymbol(x, area.Height - 1, Border.HorizontalBottom);
                context.SetStyle(x, area.Height - 1, Style);
            }
        }

        // Sides
        for (var y = 1; y < area.Height - 1; y++)
        {
            context.SetSymbol(0, y, Border.VerticalLeft);
            context.SetStyle(0, y, Style);
            context.SetSymbol(area.Width - 1, y, Border.VerticalRight);
            context.SetStyle(area.Width - 1, y, Style);
        }

        RenderTitles(context, TitlePosition.Top, 0);
        RenderTitles(context, TitlePosition.Bottom, area.Height - 1);

        if (Inner != null)
        {
            context.Render(Inner, context.Viewport.Inflate(-1, -1));
        }
    }

    private void RenderTitles(RenderContext context, TitlePosition position, int y)
    {
        if (Titles.Count == 0)
        {
            return;
        }

        var area = context.Viewport;
        var padding = Math.Max(0, TitlePadding);

        var leftEdge = 1 + padding;
        var rightEdge = (area.Width - 1) - padding;
        if (leftEdge >= rightEdge)
        {
            return;
        }

        var avail = rightEdge - leftEdge;

        // Stable order: Left, Right, Center
        foreach (var alignment in new[] { Justify.Left, Justify.Right, Justify.Center })
        {
            var group = Titles
                .Where(t => t.Position == position && t.Alignment == alignment)
                .ToList();

            if (group.Count == 0)
            {
                continue;
            }

            var line = JoinTitles(group);
            var lineWidth = line.GetWidth();

            var x = alignment switch
            {
                Justify.Left => leftEdge,
                Justify.Right => Math.Max(leftEdge, rightEdge - lineWidth),
                Justify.Center => Math.Max(leftEdge, leftEdge + ((avail - lineWidth) / 2)),
                _ => leftEdge,
            };

            var maxWidth = rightEdge - x;
            if (maxWidth <= 0)
            {
                continue;
            }

            context.SetLine(x, y, line, maxWidth, Style);
        }
    }

    private static TextLine JoinTitles(IReadOnlyList<BoxTitle> titles)
    {
        if (titles.Count == 1)
        {
            return titles[0].Text;
        }

        var spans = new List<TextSpan>();
        for (var i = 0; i < titles.Count; i++)
        {
            if (i > 0)
            {
                spans.Add(new TextSpan(" "));
            }

            // Bake each title's line-level style into its spans so it survives
            // the join (the merged TextLine has no single line-level style).
            var lineStyle = titles[i].Text.Style;
            foreach (var span in titles[i].Text.Spans)
            {
                spans.Add(lineStyle == null
                    ? span
                    : span with
                    {
                        Style = lineStyle.Value.Combine(span.Style)
                    });
            }
        }

        return new TextLine(spans);
    }
}

[PublicAPI]
public static class BoxWidgetExtensions
{
    extension(BoxWidget widget)
    {
        public BoxWidget Style(Style? style)
        {
            widget.Style = style;
            return widget;
        }

        public BoxWidget Border(Border border)
        {
            widget.Border = border;
            return widget;
        }

        public BoxWidget TitlePadding(int padding)
        {
            widget.TitlePadding = padding;
            return widget;
        }

        public BoxWidget Inner(IWidget inner)
        {
            widget.Inner = inner;
            return widget;
        }

        public BoxWidget Title(TextLine text)
        {
            return widget.Title(text, TitlePosition.Top, Justify.Left);
        }

        public BoxWidget Title(TextLine text, TitlePosition position)
        {
            return widget.Title(text, position, Justify.Left);
        }

        public BoxWidget Title(TextLine text, TitlePosition position, Justify alignment)
        {
            widget.Titles.Add(new BoxTitle(text, position, alignment));
            return widget;
        }

        public BoxWidget MarkupTitle(string markup)
        {
            return widget.Title(TextLine.FromMarkup(markup), TitlePosition.Top, Justify.Left);
        }

        public BoxWidget MarkupTitle(string markup, TitlePosition position)
        {
            return widget.Title(TextLine.FromMarkup(markup), position, Justify.Left);
        }

        public BoxWidget MarkupTitle(string markup, TitlePosition position, Justify alignment)
        {
            return widget.Title(TextLine.FromMarkup(markup), position, alignment);
        }
    }
}