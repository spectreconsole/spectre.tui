using Spectre.Tui.App;

namespace Spectre.Tui;

public sealed class TabsKeyMap<T> : IKeyMap
    where T : ITabWidgetItem
{
    private readonly TabsWidget<T> _widget;

    public KeyBinding MoveLeft { get; set; } =
        KeyBinding.For(KeyPress.For(Key.Tab).WithShift()).WithHelp("Previous tab");

    public KeyBinding MoveRight { get; set; } = KeyBinding.For(Key.Tab).WithHelp("Next tab");

    public TabsKeyMap(TabsWidget<T> widget)
    {
        _widget = widget ?? throw new ArgumentNullException(nameof(widget));
    }

    public void HandleKey(IKeyInfo message)
    {
        if (_widget.KeyMap.MoveLeft.Matches(message))
        {
            _widget.MoveLeft();
        }

        if (_widget.KeyMap.MoveRight.Matches(message))
        {
            _widget.MoveRight();
        }
    }

    public IEnumerable<KeyBinding> Help()
    {
        yield return MoveRight;
        yield return MoveLeft;
    }
}