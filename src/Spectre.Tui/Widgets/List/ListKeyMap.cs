using Spectre.Tui.App;

namespace Spectre.Tui;

public sealed class ListKeyMap<TRow> : IKeyMap
    where TRow : IListWidgetItem
{
    private readonly ListWidget<TRow> _widget;

    public KeyBinding MoveUp { get; set; } = KeyBinding.For(Key.Up).WithHelp("Move up");
    public KeyBinding MoveDown { get; set; } = KeyBinding.For(Key.Down).WithHelp("Move down");

    public ListKeyMap(ListWidget<TRow> widget)
    {
        _widget = widget ?? throw new ArgumentNullException(nameof(widget));
    }

    public void HandleKey(IKeyInfo message)
    {
        if (_widget.KeyMap.MoveUp.Matches(message))
        {
            _widget.MoveUp();
        }

        if (_widget.KeyMap.MoveDown.Matches(message))
        {
            _widget.MoveDown();
        }
    }

    public IEnumerable<KeyBinding> Help()
    {
        yield return KeyBinding.Combine(MoveUp, MoveDown).WithHelp("Move");
    }
}