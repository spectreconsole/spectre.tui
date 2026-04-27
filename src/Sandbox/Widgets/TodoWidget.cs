namespace Sandbox;

public sealed class ToDoItem(string todo, bool completed = false) : ListWidgetItem
{
    public string Todo { get; init; } = todo;
    public bool Completed { get; set; } = completed;

    public void Toggle()
    {
        Completed = !Completed;
    }

    protected override Text CreateText(bool isSelected)
    {
        var symbol = Completed ? "✓" : " ";
        var decoration = isSelected
            ? "yellow"
            : (Completed ? "green" : "grey");

        return Text.FromMarkup(
            Completed
                ? $"[{decoration}]{symbol} {Todo}[/]"
                : $"[{decoration}]{symbol} {Todo.RemoveMarkup()}[/]");
    }
}

/// <summary>
/// A custom widget that wraps a list in a JustInTimeWidget so that
/// we're only recalculating stuff if something changes.
/// </summary>
public sealed class TodoWidget : JustInTimeWidget
{
    private readonly ListWidget<ToDoItem> _widget;

    public int Position => _widget.SelectedIndex ?? 0;
    public int Length => _widget.Items.Count;

    public TodoWidget(List<ToDoItem> items)
    {
        _widget = new ListWidget<ToDoItem>(items)
            .HighlightSymbol("→ ")
            .WrapAround()
            .SelectedIndex(0);
    }

    public void MoveUp()
    {
        _widget.MoveUp();
        MarkAsDirty();
    }

    public void MoveDown()
    {
        _widget.MoveDown();
        MarkAsDirty();
    }

    public void Toggle()
    {
        _widget.SelectedItem?.Toggle();
        MarkAsDirty();
    }

    protected override void RenderDirty(RenderContext context)
    {
        context.Render(_widget);
    }
}