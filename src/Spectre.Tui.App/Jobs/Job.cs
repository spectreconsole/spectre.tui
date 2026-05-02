namespace Spectre.Tui.App;

internal sealed class Job : IJobHandle
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public Task Completion { get; }

    public Job(Application app, Func<IJobContext, Task> work, CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var context = new JobContext(app, _cancellationTokenSource.Token);

        Completion = Task.Run(
            async () =>
            {
                try
                {
                    await work(context).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (_cancellationTokenSource.IsCancellationRequested)
                {
                    // Cooperative cancellation — not an error.
                }
                catch (Exception ex)
                {
                    app.Broadcast(new JobFailedMessage(this, ex));
                }
                finally
                {
                    _cancellationTokenSource.Dispose();
                }
            },
            _cancellationTokenSource.Token);
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }
}
