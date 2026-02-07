using Spectre.Console;
using Spectre.Tui;

namespace Sandbox;

public sealed class FpsWidget : IWidget
{
    private readonly Text _text;

    public FpsWidget(
        TimeSpan elapsed,
        Color? foreground = null,
        Color? background = null)
    {
        var fps = TimeSpan.FromSeconds(1) / elapsed;

        _text = Text.FromMarkup(
            $"[yellow]FPS:[/] {fps:0.000}",
            new Appearance
            {
                Foreground = foreground ?? Color.Default,
                Background = background ?? Color.Default,
            });
    }

    public void Render(RenderContext context)
    {
        var width = _text.GetWidth();

        context.Render(
            _text,
            new Rectangle(
                (context.Viewport.Width - width) / 2,
                context.Viewport.Height / 2,
                width, 1));
    }
}