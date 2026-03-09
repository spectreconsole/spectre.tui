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