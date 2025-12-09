using Spectre.Tui;

namespace Sandbox;

public sealed class TextWidget(string text) : IWidget
{
    public void Render(IRendererContext context)
    {
        text = $" FPS: {text} ";

        var x = (context.Viewport.Width - text.Length) / 2;
        var y = context.Viewport.Height / 2;

        foreach (var rune in text)
        {
            context.SetRune(x, y, rune);
            x++;
        }
    }
}
