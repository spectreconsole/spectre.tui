namespace Sandbox;

public sealed class InfoPopup : Screen
{
    private int _lastTick;
    private readonly KeyBinding _quit = KeyBinding.For(Key.Escape);
    private readonly ScrollViewWidget _scroller =
        new ScrollViewWidget().HorizontalScroll(ScrollMode.Disabled);

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        switch (message)
        {
            case KeyMessage key when _quit.Matches(key):
                context.Pop();
                return;
            case KeyMessage key:
                _scroller.KeyMap.HandleKey(key);
                break;
            case TickMessage tick:
                _lastTick = tick.Count;
                break;
        }
    }

    public override void Render(RenderContext context)
    {
        context.Render(
            _scroller
                .Inner(
                    Paragraph.FromMarkup(
                        $"""
                         This is a little popup that shows some word wrapped text, with some markup
                         [blue]colors[/] and [italic]styles.[/]

                         [grey]Last broadcast tick: {_lastTick}[/]

                         Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                         Curabitur porttitor scelerisque lorem, vel mattis neque vulputate pellentesque.
                         Nunc hendrerit est quis auctor vulputate. Sed molestie nisl eros, rutrum ornare
                         enim feugiat at. Aliquam mollis sit amet nisi eu vestibulum.
                         """).Centered()));
    }
}