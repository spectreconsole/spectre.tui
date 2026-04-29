namespace Sandbox;

public sealed class InfoPopup : Popup
{
    private int _lastTick;

    private readonly ScrollViewWidget _scroller =
        new ScrollViewWidget().HorizontalScroll(ScrollMode.Disabled);

    public override void OnEvent(ApplicationContext context, ApplicationEvent evt)
    {
        switch (evt)
        {
            case TickEvent tick:
                _lastTick = tick.Count;
                break;
            case KeyEvent { Key.Key: ConsoleKey.B or ConsoleKey.Escape }:
                context.Pop();
                break;
            case KeyEvent { Key.Key: ConsoleKey.UpArrow }:
                _scroller.ScrollUp();
                break;
            case KeyEvent { Key.Key: ConsoleKey.DownArrow }:
                _scroller.ScrollDown();
                break;
        }
    }

    public override void Render(RenderContext context, FrameInfo frame)
    {
        // Update the scroller content
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
                     """).Centered());

        // Render the popup
        context.Render(
            new PopupWidget(new Size(50, 12))
                .Content(
                    new BoxWidget()
                        .Title("Popup", TitlePosition.Top, Justify.Center)
                        .Style(Color.Yellow)
                        .Inner(_scroller)));
    }
}