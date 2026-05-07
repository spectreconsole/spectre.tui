namespace Sandbox;

public sealed class PopupWidget : Screen
{
    public Size Size { get; }
    public string Title { get; }
    public Screen Inner { get; }

    public sealed override bool IsTransparent => true;

    public PopupWidget(Size size, string title, Screen inner)
    {
        Size = size;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public override void OnEnter(ApplicationContext context)
    {
        Inner.OnEnter(context);
    }

    public override void OnLeave(ApplicationContext context)
    {
        Inner.OnLeave(context);
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        Inner.OnMessage(context, message);
    }

    public override void Update(FrameInfo frame, IRenderBounds bounds)
    {
        Inner.Update(frame, bounds);
    }

    public override void Render(RenderContext context)
    {
        // Render the popup
        context.Render(
            new Spectre.Tui.PopupWidget(Size)
                .Content(
                    new BoxWidget()
                        .Title(Title, TitlePosition.Top, Justify.Center)
                        .Style(Color.Yellow)
                        .Inner(Inner)));
    }
}