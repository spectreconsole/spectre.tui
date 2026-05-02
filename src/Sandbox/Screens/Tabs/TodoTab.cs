namespace Sandbox;

public sealed class TodoTab : SandboxTab
{
    private readonly TodoWidget _todo;

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

    public override string TabLabel => "List";
    public override string HelpMarkup => "[bold][[↑↓]][/]:Move  [bold][[SPACE]][/]:Select";

    public override void OnEvent(ApplicationContext context, ApplicationMessage e)
    {
        if (e is not KeyMessage k)
        {
            return;
        }

        switch (k.Info.Key)
        {
            case ConsoleKey.UpArrow: _todo.MoveUp(); break;
            case ConsoleKey.DownArrow: _todo.MoveDown(); break;
            case ConsoleKey.Spacebar: _todo.Toggle(); break;
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
