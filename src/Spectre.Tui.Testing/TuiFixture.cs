namespace Spectre.Tui.Testing;

public sealed class TuiFixture
{
    private readonly TestTerminal _terminal;
    private readonly Renderer _renderer;

    public TuiFixture(Size? size = null)
    {
        _terminal = new TestTerminal(size ?? new Size(80, 25));
        _renderer = new Renderer(_terminal);
    }

    public string Render(IWidget widget)
    {
        _renderer.Draw((frame, _) =>
        {
            frame.Render(widget);
        });

        return _terminal.Output;
    }

    public string Render(Action<RenderContext> action)
    {
        _renderer.Draw((frame, _) =>
        {
            action(frame);
        });

        return _terminal.Output;
    }
}