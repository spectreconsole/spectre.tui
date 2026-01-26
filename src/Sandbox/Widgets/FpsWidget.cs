using Spectre.Console;
using Spectre.Tui;

namespace Sandbox;

public sealed class FpsWidget : IWidget
{
    private readonly string _text;
    private readonly Appearance _style;

    public FpsWidget(
        TimeSpan elapsed,
        Color? foreground = null,
        Color? background = null)
    {
        var fps = TimeSpan.FromSeconds(1) / elapsed;

        _text = $"[yellow]FPS:[/] {fps:0.000}";
        _style = new Appearance
        {
            Foreground = foreground ?? Color.Default,
            Background = background ?? Color.Default,
        };
    }

    public void Render(RenderContext context)
    {
        var text = Text.FromMarkup(_text, _style);
        var width = text.GetWidth();

        context.Render(
            text,
            new Rectangle(
                (context.Viewport.Width - width) / 2,
                context.Viewport.Height / 2,
                width, 1));
    }
}