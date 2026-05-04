namespace Sandbox;

public sealed class SandboxScreen : Screen
{
    private const double ProgressSpeed = 20d;

    private readonly BallState _ball = new();
    private readonly SpinnerWidget _spinner = new SpinnerWidget().Kind(SpinnerKind.Default);
    private readonly FpsWidget _fps = new(foreground: Color.Green);
    private readonly TabsWidget _tabs;
    private readonly ProgressBarWidget _progress;
    private readonly Layout _layout;
    private readonly SandboxTab[] _tabComponents;

    private double _progressDirection = 1d;
    private IJobHandle? _tickJob;
    private int _tickCount;

    private SandboxTab ActiveTab => _tabComponents[_tabs.SelectedIndex];

    public SandboxScreen()
    {
        _tabComponents =
        [
            new CitiesTab(),
            new TodoTab(),
            new ScrollTab(),
            new TextBoxTab(),
            new SparklineTab(),
        ];

        _tabs = new TabsWidget(_tabComponents.Select(t => t.TabLabel).ToList());

        _progress = new ProgressBarWidget()
            .Value(0).Max(100)
            .Foreground(ProgressBarBrush.Wave(new Color(177, 79, 255), new Color(0, 255, 163)))
            .HideLabel()
            .Smooth();

        _layout = new Layout("Root")
            .SplitRows(
                new Layout("Top").Size(1),
                new Layout("Tabs").Size(1),
                new Layout("Middle"),
                new Layout("Progress").Size(2),
                new Layout("Bottom")
                    .Size(1)
                    .SplitColumns(
                        new Layout("BottomLeft"),
                        new Layout("BottomRight").Size(1)));
    }

    public override void OnEnter(ApplicationContext context)
    {
        _tickJob ??= context.StartJob(async job =>
        {
            var count = 0;
            while (!job.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), job.CancellationToken);
                count++;
                job.Broadcast(new TickMessage(count));
            }
        });
    }

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        if (message is TickMessage tick)
        {
            _tickCount = tick.Count;
            return;
        }

        if (message is not KeyMessage k)
        {
            return;
        }

        switch (k.Info.Key)
        {
            case ConsoleKey.Q:
                context.Quit();
                return;
            case ConsoleKey.B:
                context.Push(new Popup(new Size(50, 12), "Popup", new InfoPopup()));
                return;
            case ConsoleKey.Tab:
                _tabs.MoveNext();
                return;
        }

        _tabComponents[_tabs.SelectedIndex]
            .OnMessage(context, message);
    }

    public override void Update(FrameInfo frame, IRenderBounds bounds)
    {
        ActiveTab.Update(frame, bounds);

        _spinner.Update(frame);
        _progress.Update(frame);
        _fps.Update(frame);

        _ball.Update(frame.FrameTime, _layout.GetArea(bounds, "Middle").Inflate(-1, -1));

        _progress.Value += _progressDirection * ProgressSpeed * frame.FrameTime.TotalSeconds;
        if (_progress.Value >= _progress.Max)
        {
            _progress.Value = _progress.Max;
            _progressDirection = -1d;
        }
        else if (_progress.Value <= 0d)
        {
            _progress.Value = 0d;
            _progressDirection = 1d;
        }
    }

    public override void Render(RenderContext context)
    {
        var top = _layout.GetArea(context, "Top");
        var middle = _layout.GetArea(context, "Middle");
        var bottom = _layout.GetArea(context, "BottomLeft");
        var bottomRight = _layout.GetArea(context, "BottomRight");

        // FPS
        context.Render(_fps, top);

        // Tabs
        context.Render(_tabs, _layout.GetArea(context, "Tabs"));

        // Outer box
        context.Render(new BoxWidget(Color.Red).Border(Border.Double), middle);
        context.Render(new ClearWidget('╱', Color.Gray), middle.Inflate(-1, -1));

        // Ball
        context.Render(new BallWidget(), _ball);

        // Active tab content
        context.Render(ActiveTab, middle.Inflate(new Size(-10, -4)));

        // Progress
        context.Render(_progress, _layout.GetArea(context, "Progress"));

        // Help
        var helpMarkup = $"[bold][[Q]][/]:Quit  [bold][[B]][/]:Popup  {ActiveTab.HelpMarkup}  [grey](ticks: {_tickCount})[/]";
        context.Render(
            Paragraph.FromMarkup(helpMarkup)
                .Style(new Style(Color.Gray))
                .Centered(), bottom);

        // Spinner
        context.Render(_spinner, bottomRight);
    }
}
