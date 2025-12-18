using Spectre.Tui;

namespace Sandbox;

public sealed class BoxWidget(Color? color = null) : IWidget
{
    public void Render(RenderContext context)
    {
        var area = context.Viewport;

        // Top/Bottom
        for (var x = 0; x < area.Width; x++)
        {
            if (x == 0)
            {
                context.SetSymbol(x, 0, '╭');
                context.SetForeground(x, 0, color);
                context.SetSymbol(x, area.Height - 1, '╰');
                context.SetForeground(x, area.Height - 1, color);
            }
            else if (x == area.Width - 1)
            {
                context.SetSymbol(x, 0, '╮');
                context.SetForeground(x, 0, color);
                context.SetSymbol(x, area.Height - 1, '╯');
                context.SetForeground(x, area.Height - 1, color);
            }
            else
            {
                context.SetSymbol(x, 0, '─');
                context.SetForeground(x, 0, color);
                context.SetSymbol(x, area.Height - 1, '─');
                context.SetForeground(x, area.Height - 1, color);
            }
        }

        // Sides
        for (var y = 1; y < area.Height - 1; y++)
        {
            context.SetSymbol(0, y, '│');
            context.SetSymbol(area.Width - 1, y, '│');
            context.SetForeground(0, y, color);
            context.SetForeground(area.Width - 1, y, color);
        }
    }
}