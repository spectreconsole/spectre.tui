namespace Spectre.Tui.App;

internal static class InputPump
{
    public static async Task Run(
        ApplicationContext application,
        IInputReader reader,
        CancellationToken cancellationToken)
    {
        reader.Initialize(application);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var e = await reader.Read(cancellationToken);
                if (e is not null)
                {
                    application.Send(e);
                }
                else
                {
                    await Task.Delay(10, cancellationToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected on shutdown
        }
        catch (ChannelClosedException)
        {
            // Channel closed during shutdown
        }
    }
}