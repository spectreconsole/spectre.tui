namespace Spectre.Tui.App;

[PublicAPI]
public interface IInputReader : IDisposable
{
    void Initialize(ApplicationContext application);
    ValueTask<ApplicationEvent?> Read(CancellationToken cancellationToken);
}

internal sealed class InputReader : IInputReader
{
    private ConsoleCancelEventHandler? _cancelHandler;

    public void Initialize(ApplicationContext application)
    {
        _cancelHandler = (_, e) =>
        {
            e.Cancel = true;
            application.Quit();
        };

        System.Console.CancelKeyPress += _cancelHandler;
    }

    public void Dispose()
    {
        System.Console.CancelKeyPress -= _cancelHandler;
    }

    public ValueTask<ApplicationEvent?> Read(CancellationToken cancellationToken)
    {
        return !System.Console.KeyAvailable
            ? new ValueTask<ApplicationEvent?>((ApplicationEvent?)null)
            : new ValueTask<ApplicationEvent?>(new KeyEvent(System.Console.ReadKey(true)));
    }
}