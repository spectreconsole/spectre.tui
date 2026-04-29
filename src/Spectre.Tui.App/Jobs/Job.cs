namespace Spectre.Tui.App;

internal sealed class Job : IJobHandle
{
    private readonly CancellationTokenSource _cts;

    public Task Completion { get; }

    public Job(Application app, Func<IJobContext, Task> work, CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var ctx = new JobContext(app, _cts.Token);

        Completion = Task.Run(
            async () =>
            {
                try
                {
                    await work(ctx).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (_cts.IsCancellationRequested)
                {
                    // Cooperative cancellation — not an error.
                }
                catch (Exception ex)
                {
                    app.Broadcast(new JobFailed(this, ex));
                }
                finally
                {
                    _cts.Dispose();
                }
            },
            _cts.Token);
    }

    public void Cancel()
    {
        _cts.Cancel();
    }
}
