namespace Spectre.Tui;

[PublicAPI]
public sealed class PopupWidget : IWidget
{
    public Size Size { get; set; }

    public BackdropWidget? Backdrop { get; set; }
    public IWidget? Content { get; set; }

    public PopupWidget(Size size)
    {
        Size = size;
        Backdrop = new BackdropWidget();
    }

    public void Render(RenderContext ctx)
    {
        var area = ctx.Screen.Center(Size);

        if (Backdrop != null)
        {
            ctx.Render(Backdrop.Exclusion(area));
        }

        ctx.Render(new ClearWidget(), area);

        if (Content != null)
        {
            ctx.Render(Content, area);
        }
    }
}

[PublicAPI]
public static class PopupWidgetExtensions
{
    extension(PopupWidget popup)
    {
        public PopupWidget Backdrop(BackdropWidget? backdrop)
        {
            popup.Backdrop = backdrop;
            return popup;
        }

        public PopupWidget Content(IWidget? content)
        {
            popup.Content = content;
            return popup;
        }
    }
}