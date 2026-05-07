namespace Sandbox;

public sealed class TextBoxTab : SandboxTab
{
    private readonly KeyBinding _open = KeyBinding.For(Key.Enter).WithHelp("Open");

    public override string TabLabel => "TextBox";

    public override IEnumerable<KeyBinding> Help()
    {
        yield return _open;
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        if (message is not KeyMessage k)
        {
            return;
        }

        if (_open.Matches(k))
        {
            context.Push(new PopupWidget(new Size(50, 15), "TextBox", new TextBoxPopup()));
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(
            Paragraph.FromMarkup(
                "Press [yellow]ENTER[/] to open text box popup")
                .Centered()
                .VerticalAlignment(VerticalAlignment.Middle));
    }
}
