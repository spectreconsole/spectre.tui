namespace Spectre.Tui;

[PublicAPI]
public sealed record RenderContext
{
    private readonly Buffer _buffer;

    public Rectangle Screen { get; internal init; }
    public Rectangle Viewport { get; internal init; }

    internal RenderContext(Buffer buffer, Rectangle screen, Rectangle viewport)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        Screen = screen;
        Viewport = viewport;
    }

    public Cell? GetCell(int x, int y)
    {
        return _buffer.GetCell(Screen.X + x, Screen.Y + y);
    }
}

[PublicAPI]
public static class RenderContextExtensions
{
    extension(RenderContext context)
    {
        public void Render(IWidget widget)
        {
            context.Render(widget, context.Viewport);
        }

        public void Render(IWidget widget, Rectangle area)
        {
            if (area.Width == 0 || area.Height == 0)
            {
                return;
            }

            var screen = context.Screen.Intersect(
                new Rectangle(
                    context.Screen.X + area.X, context.Screen.Y + area.Y,
                    area.Width, area.Height));

            var viewport = new Rectangle(0, 0, screen.Width, screen.Height);
            widget.Render(context with
            {
                Screen = screen,
                Viewport = viewport
            });
        }

        public Position SetString(int x, int y, string text, Style? style, int? maxWidth = null)
        {
            var remainingWidth = Math.Min(context.Viewport.Right - x, maxWidth ?? context.Viewport.Right);

            var graphemes = text.Graphemes()
                .Select(c => (String: c, Width: c.GetCellWidth()))
                .Where(c => c.Width > 0)
                .TakeWhile((a) =>
                {
                    remainingWidth -= a.Width;
                    return remainingWidth >= 0;
                });

            foreach (var (symbol, width) in graphemes)
            {
                context.GetCell(x, y)?.SetSymbol(symbol).SetStyle(style);
                var next = x + width;
                x++;

                // Reset the next following cells
                while (x < next)
                {
                    context.GetCell(x, y)?.Reset();
                    x++;
                }
            }

            return new Position(x, y);
        }

        public Position SetLine(int x, int y, TextLine line, int maxWidth)
        {
            var remainingWidth = maxWidth;
            foreach (var span in line.Spans)
            {
                if (remainingWidth == 0)
                {
                    break;
                }

                var pos = context.SetString(
                    x, y,
                    span.Text,
                    line.Style?.Combine(span.Style) ?? span.Style,
                    remainingWidth);

                var w = pos.X - x;
                x = pos.X;
                remainingWidth -= w;
            }

            return new Position(x, y);
        }

        public Position SetSpan(int x, int y, TextSpan span, int maxWidth)
        {
            return context.SetString(x, y, span.Text, span.Style, maxWidth);
        }

        public void SetSymbol(int x, int y, char symbol)
        {
            context.GetCell(x, y)?.SetSymbol(symbol);
        }

        public void SetSymbol(int x, int y, Rune symbol)
        {
            context.GetCell(x, y)?.SetSymbol(symbol);
        }

        public void SetStyle(int x, int y, Style? style)
        {
            context.GetCell(x, y)?.SetStyle(style);
        }

        public void SetStyle(Rectangle area, Style? style)
        {
            if (style == null)
            {
                return;
            }

            var intersected = context.Viewport.Intersect(area);
            for (var y = 0; y < intersected.Height; y++)
            {
                for (var x = 0; x < intersected.Width; x++)
                {
                    context.GetCell(x, y)?.SetStyle(style);
                }
            }
        }

        public void SetForeground(int x, int y, Color? color)
        {
            context.GetCell(x, y)?.SetForeground(color);
        }

        public void SetBackground(int x, int y, Color? color)
        {
            context.GetCell(x, y)?.SetBackground(color);
        }
    }
}