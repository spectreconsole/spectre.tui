namespace Sandbox;

public sealed class CitiesTab : SandboxTab
{
    private readonly CityTableWidget _cities;

    public CitiesTab()
    {
        _cities = new CityTableWidget(
        [
            new City(1, "Tokyo", "Japan", 37400068),
            new City(2, "Delhi", "India", 32941308),
            new City(3, "Shanghai", "China", 28516904),
            new City(4, "Dhaka", "Bangladesh", 23210000),
            new City(5, "São Paulo", "Brazil", 22806704),
            new City(6, "Cairo", "Egypt", 22183200),
            new City(7, "Mexico City", "Mexico", 22085140),
            new City(8, "Beijing", "China", 21766214),
            new City(9, "Mumbai", "India", 21296517),
            new City(10, "Osaka", "Japan", 19222665),
            new City(11, "Chongqing", "China", 17341000),
            new City(12, "Karachi", "Pakistan", 17236000),
            new City(13, "Kinshasa", "DR Congo", 16315534),
            new City(14, "Lagos", "Nigeria", 15945912),
            new City(15, "Istanbul", "Turkey", 15848000),
            new City(16, "Buenos Aires", "Argentina", 15369919),
            new City(17, "Kolkata", "India", 15133888),
            new City(18, "Manila", "Philippines", 14406059),
            new City(19, "Guangzhou", "China", 13964637),
            new City(20, "Tianjin", "China", 13794450),
        ]);
    }

    public override string TabLabel => "Table";
    public override string HelpMarkup => "[bold][[↑↓]][/]:Move";

    public override void OnEvent(ApplicationEvent evt, ApplicationContext ctx)
    {
        if (evt is not KeyEvent k)
        {
            return;
        }

        switch (k.Key.Key)
        {
            case ConsoleKey.UpArrow: _cities.MoveUp(); break;
            case ConsoleKey.DownArrow: _cities.MoveDown(); break;
        }
    }

    public override void Render(RenderContext ctx, Rectangle area, FrameInfo frame)
    {
        ctx.Render(
            new BoxWidget()
                .Style(Color.Green)
                .Border(Border.Rounded)
                .TitlePadding(1)
                .MarkupTitle("[yellow]Largest Cities[/]")
                .Inner(new CompositeWidget(
                    new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                    new PaddingWidget(new Padding(1, 0, 2, 0), _cities),
                    new ScrollbarWidget()
                        .VerticalRight()
                        .Position(_cities.Position)
                        .Length(_cities.Length)
                        .ViewportLength(1)
                        .Style(Color.Gray)
                        .ThumbStyle(Color.Green))),
            area);
    }
}
