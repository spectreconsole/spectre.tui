namespace Sandbox;

public sealed class FpsWidget : IWidget
{
    private const double SmoothingAlpha = 0.1;

    private readonly Color? _foreground;
    private readonly Color? _background;
    private double _smoothedFps;

    public FpsWidget(Color? foreground = null, Color? background = null)
    {
        _foreground = foreground;
        _background = background;
    }

    public void Update(FrameInfo info)
    {
        var fps = info.Fps;
        if (double.IsInfinity(fps) || double.IsNaN(fps))
        {
            return;
        }

        _smoothedFps = _smoothedFps == 0d
            ? fps
            : _smoothedFps + (SmoothingAlpha * (fps - _smoothedFps));
    }

    void IWidget.Render(RenderContext context)
    {
        var paragraph = Paragraph.FromMarkup(
            $"[yellow]FPS:[/] {_smoothedFps:0.000}",
            new Style
            {
                Foreground = _foreground ?? Color.Default,
                Background = _background ?? Color.Default,
            }).Alignment(Justify.Center);

        context.Render(paragraph);
    }
}
