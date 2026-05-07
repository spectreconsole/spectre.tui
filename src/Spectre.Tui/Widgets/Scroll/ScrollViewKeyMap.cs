using Spectre.Tui.App;

namespace Spectre.Tui;

public sealed class ScrollViewKeyMap : IKeyMap
{
    private readonly ScrollViewWidget _widget;

    public KeyBinding ScrollUp { get; set; } = KeyBinding.For(Key.Up).WithHelp("Scroll up");
    public KeyBinding ScrollDown { get; set; } = KeyBinding.For(Key.Down).WithHelp("Scroll down");
    public KeyBinding ScrollLeft { get; set; } = KeyBinding.For(Key.Left).WithHelp("Scroll left");
    public KeyBinding ScrollRight { get; set; } = KeyBinding.For(Key.Right).WithHelp("Scroll right");
    public KeyBinding PageUp { get; set; } = KeyBinding.For(Key.PageUp).WithHelp("Page up");
    public KeyBinding PageDown { get; set; } = KeyBinding.For(Key.PageDown).WithHelp("Page down");
    public KeyBinding ScrollHome { get; set; } = KeyBinding.For(Key.Home).WithHelp("Top");
    public KeyBinding ScrollToEnd { get; set; } = KeyBinding.For(Key.End).WithHelp("Bottom");

    public ScrollViewKeyMap(ScrollViewWidget widget)
    {
        _widget = widget ?? throw new ArgumentNullException(nameof(widget));
    }

    public void HandleKey(IKeyInfo message)
    {
        if (_widget.KeyMap.ScrollUp.Matches(message))
        {
            _widget.ScrollUp();
        }

        if (_widget.KeyMap.ScrollDown.Matches(message))
        {
            _widget.ScrollDown();
        }

        if (_widget.KeyMap.ScrollLeft.Matches(message))
        {
            _widget.ScrollLeft();
        }

        if (_widget.KeyMap.ScrollRight.Matches(message))
        {
            _widget.ScrollRight();
        }

        if (_widget.KeyMap.PageUp.Matches(message))
        {
            _widget.PageUp();
        }

        if (_widget.KeyMap.PageDown.Matches(message))
        {
            _widget.PageDown();
        }

        if (_widget.KeyMap.ScrollHome.Matches(message))
        {
            _widget.ScrollToHome();
        }

        if (_widget.KeyMap.ScrollToEnd.Matches(message))
        {
            _widget.ScrollToEnd();
        }
    }

    public IEnumerable<KeyBinding> Help()
    {
        yield return KeyBinding.Combine(ScrollUp, ScrollDown, ScrollLeft, ScrollRight).WithHelp("Scroll");
    }
}