namespace Spectre.Tui.App;

internal sealed class ScreenStack : IEnumerable<Screen>
{
    private readonly List<Screen> _screens = [];

    public int Count => _screens.Count;

    public void Push(ApplicationContext context, Screen screen)
    {
        _screens.Add(screen);
        screen.OnEnter(context);
    }

    public Screen Pop(ApplicationContext context)
    {
        if (_screens.Count == 0)
        {
            ThrowForEmptyStack();
        }

        var last = _screens[^1];
        _screens.Remove(last);

        last.OnLeave(context);

        return last;
    }

    public void Render(RenderContext context, FrameInfo frame)
    {
        var firstOpaque = _screens.Count - 1;
        for (var i = _screens.Count - 1; i >= 0; i--)
        {
            if (_screens[i].IsTransparent)
            {
                continue;
            }

            firstOpaque = i;
            break;
        }

        for (var i = firstOpaque; i < _screens.Count; i++)
        {
            _screens[i].Update(frame, context);
            context.Render(_screens[i]);
        }
    }

    public Screen Peek()
    {
        if (_screens.Count == 0)
        {
            ThrowForEmptyStack();
        }

        return _screens[^1];
    }

    public void Unwind(ApplicationContext context)
    {
        while (Count > 0)
        {
            Pop(context).OnLeave(context);
        }
    }

    public IEnumerator<Screen> GetEnumerator()
    {
        var screens = new List<Screen>(_screens);
        return ((IEnumerable<Screen>)screens).Reverse().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void ThrowForEmptyStack()
    {
        Debug.Assert(_screens.Count == 0);
        throw new InvalidOperationException("Screen stack is empty");
    }
}