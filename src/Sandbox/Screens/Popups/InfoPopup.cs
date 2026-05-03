namespace Sandbox;

public sealed class InfoPopup : Screen
{
    private int _lastTick;

    private readonly ScrollViewWidget _scroller =
        new ScrollViewWidget().HorizontalScroll(ScrollMode.Disabled);

    public override void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
        switch (message)
        {
            case TickMessage tick:
                _lastTick = tick.Count;
                break;
            case KeyMessage { Info.Key: ConsoleKey.B or ConsoleKey.Escape }:
                context.Pop();
                break;
            case KeyMessage { Info.Key: ConsoleKey.UpArrow }:
                _scroller.ScrollUp();
                break;
            case KeyMessage { Info.Key: ConsoleKey.DownArrow }:
                _scroller.ScrollDown();
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