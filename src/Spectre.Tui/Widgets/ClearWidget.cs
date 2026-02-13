namespace Spectre.Tui;

[PublicAPI]
public sealed class ClearWidget(
    char? symbol = null,
    Style? style = null) : IWidget
{
    public void Render(RenderContext context)
    {
        for (var x = 0; x < context.Viewport.Width; x++)
        {
            for (var y = 0; y < context.Viewport.Height; y++)
            {
                context.GetCell(x, y)?
                    .SetSymbol(symbol ?? ' ')
                    .SetStyle(style);
            }
        }
    }
}