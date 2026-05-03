namespace Sandbox;

public sealed class TextBoxTab : SandboxTab
{
    public override string TabLabel => "TextBox";
    public override string HelpMarkup => "[bold][[Enter]][/]:Open";

    public override void OnMessage(ApplicationContext context, ApplicationMessage e)
    {
        if (e is not KeyMessage k)
        {
            return;
        }

        if (k.Info.Key == ConsoleKey.Enter)
        {
            context.Push(new Popup(new Size(50, 15), "TextBox", new TextBoxPopup()));
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
