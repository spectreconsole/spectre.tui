namespace Spectre.Tui.App;

internal sealed class JobContext : IJobContext
{
    private readonly Application _app;

    public CancellationToken CancellationToken { get; }

    public JobContext(Application app, CancellationToken cancellationToken)
    {
        _app = app;
        CancellationToken = cancellationToken;
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
}
