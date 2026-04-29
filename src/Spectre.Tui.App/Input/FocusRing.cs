namespace Spectre.Tui.App;

[PublicAPI]
public sealed class FocusRing
{
    private readonly IFocusable[] _items;
    private int _current;

    public IFocusable Focused => _items[_current];

    public FocusRing(params IFocusable[] items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Length == 0)
        {
            throw new ArgumentException("FocusRing requires at least one focusable.", nameof(items));
        }

        _items = items;

        for (var i = 0; i < _items.Length; i++)
        {
            _items[i].IsFocused = i == 0;
        }
    }

    public bool HandleInput(ApplicationEvent evt)
    {
        if (evt is not KeyEvent k || k.Key.Key != ConsoleKey.Tab)
        {
            return false;
        }

        var direction = (k.Key.Modifiers & ConsoleModifiers.Shift) != 0 ? -1 : 1;
        Move(direction);
        return true;
    }

    public void Focus(IFocusable item)
    {
        var index = Array.IndexOf(_items, item);
        if (index < 0)
        {
            throw new ArgumentException("The given focusable is not part of this ring.", nameof(item));
        }

        SetFocus(index);
    }

    private void Move(int delta)
    {
        var next = ((_current + delta) % _items.Length + _items.Length) % _items.Length;
        SetFocus(next);
    }

    private void SetFocus(int index)
    {
        if (index == _current)
        {
            return;
        }

        _items[_current].IsFocused = false;
        _items[index].IsFocused = true;
        _current = index;
    }
}