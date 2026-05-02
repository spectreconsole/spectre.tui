namespace Spectre.Tui.App;

[PublicAPI]
public sealed class Application
{
    private static readonly TimeSpan _spinThreshold = TimeSpan.FromMilliseconds(2);

    private readonly ApplicationSettings _settings;
    private readonly ApplicationContext _context;
    private readonly Channel<QueuedEvent> _events;
    private readonly ScreenStack _stack;
    private readonly CancellationTokenSource _shutdownSource;
    private bool _running;

    private abstract record QueuedEvent
    {
        internal sealed record Quit : QueuedEvent;

        internal sealed record Send(ApplicationMessage Message) : QueuedEvent;

        internal sealed record Broadcast(ApplicationMessage Message) : QueuedEvent;
    }

    private Application(ApplicationSettings? settings = null)
    {
        _settings = settings ?? new ApplicationSettings();
        _stack = new ScreenStack();
        _context = new ApplicationContext(this, _stack);
        _shutdownSource = new CancellationTokenSource();
        _events = Channel.CreateUnbounded<QueuedEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
        });
    }

    public static Application Create(ApplicationSettings? settings = null)
    {
        return new Application(settings);
    }

    public async Task RunAsync(Screen initial)
    {
        ArgumentNullException.ThrowIfNull(initial);

        using var terminal = _settings.Terminal ?? Terminal.Create();
        var renderer = new Renderer(terminal);
        renderer.SetTargetFps(_settings.TargetFps);

        _stack.Push(_context, initial);
        _running = true;

        var reader = _settings.InputReader ?? new InputReader();
        var input = InputPump.Run(_context, reader, _shutdownSource.Token);

        try
        {
            while (_running)
            {
                while (_events.Reader.TryRead(out var queued))
                {
                    Dispatch(queued);

                    if (!_running)
                    {
                        // TODO: Should we process all remaining messages?
                        break;
                    }
                }

                if (!_running || _stack.Count == 0)
                {
                    break;
                }

                renderer.Draw(_stack.Render);

                // Pace the loop using the renderer's own accumulator. Sleep when
                // there's plenty of budget left, busy-poll the last ~2 ms so the
                // render fires within microseconds of the deadline. Keeps CPU off
                // 100% (avoids thermal throttling) while preserving precise timing.
                if (renderer.TimeUntilNextRender() > _spinThreshold)
                {
                    await Task.Delay(1);
                }
            }
        }
        catch (OperationCanceledException) when (_shutdownSource.Token.IsCancellationRequested)
        {
            // Expected on shutdown
        }
        finally
        {
            // Shut down
            await _shutdownSource.CancelAsync();

            // Wait for the input thread to cancel
            await input;

            // Unwind the screen stack
            _stack.Unwind(_context);
        }
    }

    internal void Quit()
    {
        _events.Writer.TryWrite(new QueuedEvent.Quit());
    }

    internal void Send<T>(T data)
        where T : ApplicationMessage
    {
        _events.Writer.TryWrite(new QueuedEvent.Send(data));
    }

    internal void Broadcast<T>(T data)
        where T : ApplicationMessage
    {
        _events.Writer.TryWrite(new QueuedEvent.Broadcast(data));
    }

    internal IJobHandle StartJob(Func<IJobContext, Task> work)
    {
        ArgumentNullException.ThrowIfNull(work);
        return new Job(this, work, _shutdownSource.Token);
    }

    private void Dispatch(QueuedEvent queued)
    {
        switch (queued)
        {
            case QueuedEvent.Quit:
                _running = false;
                break;
            case QueuedEvent.Send e when _stack.Count > 0:
                _stack.Peek().OnMessage(_context, e.Message);
                break;
            case QueuedEvent.Broadcast e:
                foreach (var screen in _stack)
                {
                    screen.OnMessage(_context, e.Message);
                }

                break;
        }
    }
}