namespace Spectre.Tui;

public class CompositeWidget : IWidget
{
    private readonly List<IWidget> _widgets;

    public CompositeWidget(params List<IWidget> widgets)
    {
        _widgets = widgets ?? throw new ArgumentNullException(nameof(widgets));
    }

    void IWidget.Render(RenderContext context)
    {
        foreach (var widget in _widgets)
        {
            context.Render(widget);
        }
    }
}