using System.Text;
using Spectre.Tui;

namespace Sandbox;

public sealed class BoxWidget : IWidget
{
    public void Render(IRendererContext context)
    {
        var area = context.Viewport;

        // Top
        for (var x = 0; x < area.Width; x++)
        {
            if (x == 0)
            {
                context.SetRune(x, 0, '╭');
            }
            else if (x == area.Width - 1)
            {
                context.SetRune(x, 0, '╮');
            }
            else
            {
                context.SetRune(x, 0, '─');
            }
        }

        // Sides
        for (var y = 1; y < area.Height - 1; y++)
        {
            context.SetRune(0, y, '│');
            context.SetRune(area.Width - 1, y, '│');
        }

        // Bottom
        for (var x = 0; x < area.Width; x++)
        {
            if (x == 0)
            {
                context.SetRune(x, area.Height - 1, '╰');
            }
            else if (x == area.X + area.Width - 1)
            {
                context.SetRune(x, area.Height - 1, '╯');
            }
            else
            {
                context.SetRune(x, area.Height - 1, '─');
            }
        }

        // Clear the inside of the box
        var innerArea = area.Inflate(new Size(-1, -1));
        context.Render(new ClearWidget(new Rune(' ')), innerArea);
    }
}