namespace Sandbox;

public class TextBoxPopup : Screen
{
    private readonly Layout _layout;
    private readonly TextBoxWidget _name;
    private readonly TextBoxWidget _password;
    private readonly TextBoxWidget _notes;
    private readonly FocusRing _focusRing;

    public TextBoxPopup()
    {
        _name = new TextBoxWidget()
            .AsSingleLine()
            .Placeholder("Enter your name…");

        _password = new TextBoxWidget()
            .AsSingleLine()
            .Password()
            .Placeholder("Password");

        _notes = new TextBoxWidget()
            .AsMultiLine()
            .Placeholder("Notes (multi-line, horizontal scroll)…");

        _focusRing = new FocusRing(_name, _password, _notes);
        _layout = new Layout("Root")
            .SplitRows(
                new Layout("Name").Size(3),
                new Layout("Password").Size(3),
                new Layout("Notes"));
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage e)
    {
        if (e is KeyMessage key)
        {
            switch (key.Key)
            {
                case Key.Escape:
                    context.Pop();
                    break;
                case Key.Tab:
                    CycleFocus(forward: !key.Shift);
                    return;
            }

            if (_focusRing.Focused is TextBoxWidget textbox)
            {
                textbox.KeyMap.HandleKey(key);
            }
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(new ClearWidget());

        context.Render(BuildBox("Name", _name, Color.Aqua), _layout.GetArea(context, "Name"));
        context.Render(BuildBox("Password", _password, Color.Yellow), _layout.GetArea(context, "Password"));
        context.Render(BuildBox("Notes", _notes, Color.Green), _layout.GetArea(context, "Notes"));
    }

    private void CycleFocus(bool forward)
    {
        // TODO: Clean this up by adding a cycle method
        var items = new IFocusable[] { _name, _password, _notes };
        var currentIdx = Array.IndexOf(items, _focusRing.Focused);
        var step = forward ? 1 : -1;
        var next = (currentIdx + step + items.Length) % items.Length;
        _focusRing.Focus(items[next]);
    }

    private static BoxWidget BuildBox(string title, TextBoxWidget inner, Color border)
    {
        return new BoxWidget()
            .Border(Border.Rounded)
            .Style(border)
            .TitlePadding(1)
            .MarkupTitle($"[bold]{title}[/]")
            .Inner(new PaddingWidget(new Padding(1, 0), inner));
    }
}