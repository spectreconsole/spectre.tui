namespace Spectre.Tui;

public sealed class PaddingWidget(Padding padding, IWidget widget) : IWidget
{
    private readonly IWidget _widget = widget ?? throw new ArgumentNullException(nameof(widget));

    public void Render(RenderContext context)
    {
        context.Render(_widget, context.Viewport.Pad(padding));
    }
}