namespace Sandbox;

public sealed class ScrollTab : SandboxTab
{
    private readonly ScrollViewWidget _scroll;

    public ScrollTab()
    {
        _scroll = new ScrollViewWidget()
            .Inner(
                Paragraph.FromMarkup(
                    """
                    [yellow]Scrollable paragraph demo[/]

                    Use [bold]↑↓[/] to scroll one row, [bold]←→[/] to scroll one column.
                    [bold]PgUp/PgDn[/] move by a page; [bold]Home/End[/] jump to the extremes.

                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur porttitor scelerisque lorem, vel mattis neque vulputate pellentesque. Nunc hendrerit est quis auctor vulputate. Sed molestie nisl eros, rutrum ornare enim feugiat at.
                    Aliquam mollis sit amet nisi eu vestibulum. Nam pharetra hendrerit nisl sit amet luctus.
                    Donec rhoncus efficitur neque sed vulputate. Phasellus fringilla feugiat orci, vel tempus dolor ullamcorper id.
                    Pellentesque accumsan ligula a nibh efficitur ullamcorper.

                    Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Aenean condimentum mauris sit amet libero ornare.
                    Mauris vehicula urna a libero suscipit, vel sodales quam mattis. Sed gravida fermentum justo, et aliquam diam venenatis sed.
                    Cras euismod, lectus eu vehicula bibendum, augue ipsum vehicula sapien.

                    Quisque eget porta est. Curabitur ut hendrerit lacus. Pellentesque commodo, est at gravida porta, magna ipsum vehicula urna, vel ornare velit ipsum sed augue.
                    Etiam ut accumsan elit. Suspendisse potenti. Aenean nec lectus a velit fringilla aliquet.

                    Praesent id mi at velit suscipit fermentum. Cras gravida ipsum a velit imperdiet, vel ornare ipsum porta. Sed ut ante in dolor cursus malesuada.
                    Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.

                    [bold green]End of demo[/]
                    """)
                    .Folded())
            .HorizontalScroll(ScrollMode.Disabled)
            .ScrollbarStyle(Color.Gray)
            .ScrollbarThumbStyle(Color.Green);
    }

    public override string TabLabel => "Scroll";
    public override string HelpMarkup => "[bold][[↑↓←→ PgUp PgDn Home End]][/]:Scroll";

    public override void OnEvent(ApplicationEvent evt, ApplicationContext ctx)
    {
        if (evt is not KeyEvent k)
        {
            return;
        }

        switch (k.Key.Key)
        {
            case ConsoleKey.UpArrow: _scroll.ScrollUp(); break;
            case ConsoleKey.DownArrow: _scroll.ScrollDown(); break;
            case ConsoleKey.LeftArrow: _scroll.ScrollLeft(); break;
            case ConsoleKey.RightArrow: _scroll.ScrollRight(); break;
            case ConsoleKey.PageUp: _scroll.PageUp(); break;
            case ConsoleKey.PageDown: _scroll.PageDown(); break;
            case ConsoleKey.Home: _scroll.ScrollToTop(); break;
            case ConsoleKey.End: _scroll.ScrollToBottom(); break;
        }
    }

    public override void Render(RenderContext ctx, Rectangle area, FrameInfo frame)
    {
        ctx.Render(
            new BoxWidget()
                .Style(Color.Green)
                .Border(Border.Rounded)
                .TitlePadding(1)
                .MarkupTitle("[yellow]Scroll[/]")
                .Inner(new CompositeWidget(
                    new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                    _scroll)),
            area);
    }
}
