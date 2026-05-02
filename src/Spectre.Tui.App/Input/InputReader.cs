namespace Spectre.Tui.App;

[PublicAPI]
public interface IInputReader : IDisposable
{
    void Initialize(ApplicationContext application);
    ValueTask<ApplicationMessage?> Read(CancellationToken cancellationToken);
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

    public ValueTask<ApplicationMessage?> Read(CancellationToken cancellationToken)
    {
        return !System.Console.KeyAvailable
            ? new ValueTask<ApplicationMessage?>((ApplicationMessage?)null)
            : new ValueTask<ApplicationMessage?>(new KeyMessage(System.Console.ReadKey(true)));
    }
}