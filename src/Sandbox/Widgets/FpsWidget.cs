using Spectre.Tui;

namespace Sandbox;

public sealed class FpsWidget : IWidget
{
    private readonly string _text;
    private readonly Style _style;

    public FpsWidget(string text,
        Color? foreground = null,
        Color? background = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        _text = $" FPS: {text} ";
        _style = new Style
        {
            Foreground = foreground ?? Color.Default,
            Background = background ?? Color.Default,
        };
    }

    public void Render(RenderContext context)
    {
        context.Render(
            Text.FromString(_text, _style),
            new Rectangle(
                (context.Viewport.Width - _text.Length) / 2,
                context.Viewport.Height / 2,
                _text.Length, 1));
    }
}