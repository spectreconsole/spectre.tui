namespace Sandbox;

public sealed class TodoTab : SandboxTab
{
    private readonly TodoWidget _todo;
    private readonly KeyBinding _toggle = KeyBinding.For(Key.Space).WithHelp("Toggle");

    public override string TabLabel => "List";

    public override IEnumerable<KeyBinding> Help()
    {
        foreach (var binding in _todo.KeyMap.Help())
        {
            yield return binding;
        }

        yield return _toggle;
    }

    public TodoTab()
    {
        _todo = new TodoWidget(
        [
            new ToDoItem("नमस्ते [red]Happy Holidays[/] 🎅 Happy Holidays: [u]Happy Holidays[/]"),
            new ToDoItem("Another list item"),
            new ToDoItem("An [italic]initially[/] completed list item", true),
            new ToDoItem("A list item "),
            new ToDoItem("Another list item "),
            new ToDoItem("Believe it or not, a list item"),
            new ToDoItem("A list item (wow)"),
            new ToDoItem("A list item... you know"),
            new ToDoItem("A list item "),
            new ToDoItem("Another list item "),
            new ToDoItem("Believe it or not, a list item"),
            new ToDoItem("A list item (wow)"),
        ]);
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        if (message is KeyMessage key)
        {
            if (_toggle.Matches(key))
            {
                _todo.Toggle();
                return;
            }

            _todo.HandleKey(key);
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(
            new BoxWidget()
                .Style(Color.Green)
                .Border(Border.Rounded)
                .TitlePadding(1)
                .MarkupTitle("[yellow]To-Do[/]")
                .Inner(new CompositeWidget(
                    new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                    new PaddingWidget(new Padding(1, 0, 2, 0), _todo),
                    new ScrollbarWidget()
                        .VerticalRight()
                        .Position(_todo.Position)
                        .Length(_todo.Length)
                        .ViewportLength(1)
                        .Style(Color.Gray)
                        .ThumbStyle(Color.Green))));
    }
}