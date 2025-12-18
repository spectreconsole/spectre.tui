namespace Spectre.Tui;

public interface IRenderContext
{
    Rectangle Viewport { get; }

    void Render(IWidget widget, Rectangle area);

    /// <summary>
    /// Sets the cell at the specified (viewport) coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    Cell? GetCell(int x, int y);
}

public static class IRenderContextExtensions
{
    extension(IRenderContext context)
    {
        public void Render(IWidget widget)
        {
            context.Render(widget, context.Viewport);
        }

        public void SetRune(int x, int y, char rune)
        {
            context.SetRune(x, y, new Rune(rune));
        }

        public void SetRune(int x, int y, Rune rune)
        {
            context.GetCell(x, y)?.SetRune(rune);
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

internal sealed record RenderContext : IRenderContext
{
    public Buffer Buffer { get; }
    public Rectangle Screen { get; private init; }
    public Rectangle Viewport { get; private init; }

    public RenderContext(Buffer buffer, Rectangle screen, Rectangle viewport)
    {
        Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        Screen = screen;
        Viewport = viewport;
    }

    public void Render(IWidget widget, Rectangle area)
    {
        if (area.Width == 0 || area.Height == 0)
        {
            return;
        }

        var screen = Screen.Intersect(
            new Rectangle(
                Screen.X + area.X, Screen.Y + area.Y,
                area.Width, area.Height));

        var viewport = new Rectangle(0, 0, screen.Width, screen.Height);
        widget.Render(this with { Screen = screen, Viewport = viewport });
    }

    public Cell? GetCell(int x, int y)
    {
        return Buffer.GetCell(Screen.X + x, Screen.Y + y);
    }
}