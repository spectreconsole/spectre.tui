using Spectre.Tui;

namespace Sandbox;

public sealed class ClearWidget(
    char? symbol = null,
    Decoration decoration = Decoration.None,
    Color? foreground = null,
    Color? background = null) : IWidget
{
    public void Render(RenderContext context)
    {
        for (var x = 0; x < context.Viewport.Width; x++)
        {
            for (var y = 0; y < context.Viewport.Height; y++)
            {
                context.GetCell(x, y)?
                    .SetSymbol(symbol ?? ' ')
                    .SetDecoration(decoration)
                    .SetForeground(foreground)
                    .SetBackground(background);
            }
        }
    }
}