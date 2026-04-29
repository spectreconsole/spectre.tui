namespace Spectre.Tui;

[PublicAPI]
public sealed class BackdropWidget : IWidget
{
    public Color BaseColor { get; set; } = Color.Default;
    public Color? Foreground { get; set; }
    public Rectangle? Exclusion { get; set; }

    public void Render(RenderContext context)
    {
        var area = context.Viewport;
        var fg = GetForeground();

        for (var y = area.Top; y < area.Bottom; y++)
        {
            for (var x = area.Left; x < area.Right; x++)
            {
                if (Exclusion != null)
                {
                    if (x >= Exclusion.Value.Left && x < Exclusion.Value.Right &&
                        y >= Exclusion.Value.Top && y < Exclusion.Value.Bottom)
                    {
                        continue;
                    }
                }

                context
                    .GetCell(x, y)?
                    .SetForeground(fg)
                    .SetBackground(BaseColor);

            }
        }
    }

    private Color GetForeground()
    {
        if (Foreground != null)
        {
            return Foreground.Value;
        }

        var (r, g, b) = (BaseColor.R, BaseColor.G, BaseColor.B);
        return new Color(
            (byte)(r / 2 + 64),
            (byte)(g / 2 + 64),
            (byte)(b / 2 + 64));
    }
}

[PublicAPI]
public static class BackdropWidgetExtensions
{
    extension(BackdropWidget backdrop)
    {
        public BackdropWidget Exclusion(Rectangle? area)
        {
            backdrop.Exclusion = area;
            return backdrop;
        }

        public BackdropWidget BaseColor(Color color)
        {
            backdrop.BaseColor = color;
            return backdrop;
        }

        public BackdropWidget Foreground(Color? color)
        {
            backdrop.Foreground = color;
            return backdrop;
        }
    }
}