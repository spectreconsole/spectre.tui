namespace Spectre.Tui;

[PublicAPI]
public sealed record RenderContext : IRenderBounds
{
    private readonly IBuffer _buffer;

    public Rectangle Screen { get; internal init; }
    public Rectangle Viewport { get; internal init; }

    internal RenderContext? Parent { get; init; }

    internal Position? CursorPosition
    {
        get
        {
            return Parent != null ? Parent.CursorPosition : field;
        }
        set
        {
            if (Parent != null)
            {
                Parent.CursorPosition = value;
                return;
            }

            field = value;
        }
    }

    internal RenderContext(IBuffer buffer, Rectangle screen)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

        Parent = null;
        Screen = screen;
        Viewport = screen;
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
            widget.Render(context);
        }

        public void Render(IWidget widget, Rectangle area)
        {
            if (context.CreateAreaRenderContext(area, out var areaContext))
            {
                widget.Render(areaContext);
            }
        }

        public void Render<TState>(IStatefulWidget<TState> widget, TState state)
        {
            widget.Render(context, state);
        }

        public void Render<TState>(IStatefulWidget<TState> widget, Rectangle area, TState state)
        {
            if (context.CreateAreaRenderContext(area, out var areaContext))
            {
                widget.Render(areaContext, state);
            }
        }

        public Position SetString(Position position, string text, Style? style = null, int? maxWidth = null)
        {
            return context.SetString(position.X, position.Y, text, style, maxWidth);
        }

        public Position SetString(int x, int y, string text, Style? style = null, int? maxWidth = null)
        {
            var remainingWidth = Math.Min(
                context.Viewport.Right - x,
                maxWidth ?? context.Viewport.Right);

            foreach (var symbol in text.Graphemes())
            {
                var width = symbol.GetCellWidth();

                // Skip zero-width characters
                if (width == 0)
                {
                    continue;
                }

                // Stop when no more horizontal space is available
                remainingWidth -= width;
                if (remainingWidth < 0)
                {
                    break;
                }

                context.GetCell(x, y)?
                    .SetSymbol(symbol)
                    .SetStyle(style);

                var nextSymbolPosition = x + width;
                x++;

                // Reset the next following cells
                while (x < nextSymbolPosition)
                {
                    context.GetCell(x, y)?.SetSymbol("");
                    x++;
                }
            }

            return new Position(x, y);
        }

        public Position SetLine(Position position, TextLine line, int maxWidth, Style? baseStyle = null)
        {
            return context.SetLine(position.X, position.Y, line, maxWidth, baseStyle);
        }

        public Position SetLine(int x, int y, TextLine line, int maxWidth, Style? baseStyle = null)
        {
            var lineBaseStyle = baseStyle?.Combine(line.Style) ?? line.Style;

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
                    lineBaseStyle?.Combine(span.Style) ?? span.Style,
                    remainingWidth);

                var w = pos.X - x;
                x = pos.X;
                remainingWidth -= w;
            }

            return new Position(x, y);
        }

        public Position SetSpan(Position position, TextSpan span, int maxWidth)
        {
            return context.SetSpan(position.X, position.Y, span, maxWidth);
        }

        public Position SetSpan(int x, int y, TextSpan span, int maxWidth)
        {
            return context.SetString(x, y, span.Text, span.Style, maxWidth);
        }

        public void SetSymbol(Position position, char symbol)
        {
            context.SetSymbol(position.X, position.Y, symbol);
        }

        public void SetSymbol(int x, int y, char symbol)
        {
            context.GetCell(x, y)?.SetSymbol(symbol);
        }

        public void SetSymbol(Position position, Rune symbol)
        {
            context.SetSymbol(position.X, position.Y, symbol);
        }

        public void SetSymbol(int x, int y, Rune symbol)
        {
            context.GetCell(x, y)?.SetSymbol(symbol);
        }

        public void SetStyle(Position position, Style? style)
        {
            context.SetStyle(position.X, position.Y, style);
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
            for (var y = intersected.Y; y < intersected.Y + intersected.Height; y++)
            {
                for (var x = intersected.X; x < intersected.X + intersected.Width; x++)
                {
                    context.GetCell(x, y)?.SetStyle(style);
                }
            }
        }

        public void SetForeground(Position position, Color? color)
        {
            context.SetForeground(position.X, position.Y, color);
        }

        public void SetForeground(int x, int y, Color? color)
        {
            context.GetCell(x, y)?.SetForeground(color);
        }

        public void SetBackground(Position position, Color? color)
        {
            context.SetBackground(position.X, position.Y, color);
        }

        public void SetBackground(int x, int y, Color? color)
        {
            context.GetCell(x, y)?.SetBackground(color);
        }

        public void SetCursorPosition(Position position)
        {
            context.SetCursorPosition(position.X, position.Y);
        }

        public void SetCursorPosition(int x, int y)
        {
            context.CursorPosition = new Position(x, y);
        }

        public void Blit(Position position, RenderSurface surface)
        {
            context.Blit(position.X, position.Y, surface);
        }

        public void Blit(int x, int y, RenderSurface surface)
        {
            context.Blit(x, y, surface, new Rectangle(0, 0, surface.Width, surface.Height));
        }

        public void Blit(Position position, RenderSurface surface, Rectangle source)
        {
            context.Blit(position.X, position.Y, surface, source);
        }

        public void Blit(int x, int y, RenderSurface surface, Rectangle source)
        {
            if (source.IsEmpty)
            {
                return;
            }

            foreach (var (position, cell) in surface.GetCells())
            {
                if (cell.Symbol.Length == 0)
                {
                    continue;
                }

                var symbolWidth = cell.Symbol.GetCellWidth();

                // Clip against the source rectangle
                if (position.X < source.X || position.Y < source.Y ||
                    position.X + symbolWidth > source.Right ||
                    position.Y >= source.Bottom)
                {
                    continue;
                }

                var destinationX = x + position.X - source.X;
                var destinationY = y + position.Y - source.Y;

                // Clip against the destination viewport
                if (destinationX < 0 || destinationY < 0 ||
                    destinationX + symbolWidth > context.Viewport.Width ||
                    destinationY >= context.Viewport.Height)
                {
                    continue;
                }

                context.GetCell(destinationX, destinationY)?
                    .SetSymbol(cell.Symbol)
                    .SetStyle(cell.Style);

                // Write continuation cells for wide symbols
                for (var i = 1; i < symbolWidth; i++)
                {
                    context.GetCell(destinationX + i, destinationY)
                        ?.SetSymbol("");
                }
            }
        }

        private bool CreateAreaRenderContext(Rectangle area, [NotNullWhen(true)] out RenderContext? result)
        {
            if (area.IsEmpty)
            {
                result = null;
                return false;
            }

            var screen = context.Screen.Intersect(
                new Rectangle(
                    context.Screen.X + area.X, context.Screen.Y + area.Y,
                    area.Width, area.Height));

            // The provided screen does not intersect with
            // the area. We can't render anything in this rectangle.
            if (screen.IsEmpty)
            {
                result = null;
                return false;
            }

            result = context with
            {
                Parent = context,
                Screen = screen,
                Viewport = new Rectangle(0, 0, screen.Width, screen.Height)
            };

            return true;
        }
    }
}