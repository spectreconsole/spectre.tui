namespace Sandbox;

public sealed class SparklineTab : SandboxTab
{
    private const int StreamCapacity = 256;
    private static readonly TimeSpan _streamInterval = TimeSpan.FromMilliseconds(50);

    private readonly SparklineWidget _stream;
    private TimeSpan _streamElapsed = TimeSpan.Zero;

    public override string TabLabel => "Sparkline";
    public override string HelpMarkup => "[bold][[Tab]][/]:Next tab";

    public SparklineTab()
    {
        _stream = new SparklineWidget()
            .Direction(SparklineDirection.RightToLeft);
    }

    public override void Update(FrameInfo frame, IRenderBounds bounds)
    {
        _streamElapsed += frame.FrameTime;
        while (_streamElapsed >= _streamInterval)
        {
            _streamElapsed -= _streamInterval;
            _stream.Data.Add((ulong)Random.Shared.Next(0, 100));
            if (_stream.Data.Count > StreamCapacity)
            {
                _stream.Data.RemoveAt(0);
            }
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(
            new BoxWidget()
                .Style(Color.Green)
                .Border(Border.Rounded)
                .TitlePadding(1)
                .MarkupTitle("[yellow]Streaming[/]")
                .Inner(
                    new CompositeWidget(
                        new ClearWidget(),
                        new PaddingWidget(new Padding(1, 0, 1, 0), _stream)))
        );
    }
}