namespace Sandbox;

public class SelectionPopup : Screen
{
    private readonly string _name;
    private readonly KeyBinding _quit = KeyBinding.For(Key.Escape);

    public SelectionPopup(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        switch (message)
        {
            case KeyMessage key when _quit.Matches(key):
                context.Pop();
                return;
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(
            Paragraph.FromMarkup(
                    $"""
                     You selected:
                     [yellow]{_name}[/]

                     Press [blue]ESC[/] to close
                     """)
                .Centered()
                .AlignedMiddle());
    }
}