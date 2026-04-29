namespace Spectre.Tui.App;

public sealed class ApplicationContext
{
    private readonly Application _app;
    private readonly ScreenStack _stack;

    internal ApplicationContext(
        Application app,
        ScreenStack stack)
    {
        _app = app ?? throw new ArgumentNullException(nameof(app));
        _stack = stack ?? throw new ArgumentNullException(nameof(stack));
    }

    public void Push(Screen screen)
    {
        _stack.Push(this, screen);
    }

    public void Pop()
    {
        _stack.Pop(this);
    }

    public void Quit()
    {
        _app.Quit();
    }

    public void Send<T>(T data)
        where T : ApplicationEvent
    {
        _app.Send(data);
    }

    public void Broadcast<T>(T data)
        where T : ApplicationEvent
    {
        _app.Broadcast(data);
    }

    public IJobHandle StartJob(Func<IJobContext, Task> work)
    {
        return _app.StartJob(work);
    }
}