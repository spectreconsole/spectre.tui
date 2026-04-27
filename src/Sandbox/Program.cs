using System.Text;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = Terminal.Create();
        var renderer = new Renderer(terminal);
        renderer.SetTargetFps(60);

        Console.Title = "Spectre.Tui Sandbox";
        Console.OutputEncoding = Encoding.Unicode;
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            running = false;
        };

        var ball = new BallState();
        var spinner = new SpinnerWidget().Kind(SpinnerKind.Dots);
        var cities = new CityTableWidget(
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

        var todo = new TodoWidget(
        [
            new ToDoItem("नमस्ते [red]Happy Holidays[/] 🎅 Happy Holidays: [u]Happy Holidays[/]"),
            new ToDoItem("Another list item"),
            new ToDoItem("An [italic]initially[/] completed list item", true),
            new ToDoItem("A list item "),
            new ToDoItem("Another list item "),
            new ToDoItem("Believe it or not, a list item"),
            new ToDoItem("A list item (wow)"),
            new ToDoItem("A list item... you know"),
            new ToDoItem("A list item "),
            new ToDoItem("Another list item "),
            new ToDoItem("Believe it or not, a list item"),
            new ToDoItem("A list item (wow)")
        ]);

        var tabs = new MyTabsWidget([
            "Largest Cities",
            "To-Do",
        ]);

        var layout = new Layout("Root")
            .SplitRows(
                new Layout("Top").Size(1),
                new Layout("Tabs").Size(1),
                new Layout("Middle"),
                new Layout("Bottom")
                    .Size(1)
                    .SplitColumns(
                        new Layout("BottomLeft"),
                        new Layout("BottomRight").Size(1)));

        while (running)
        {
            renderer.Draw((ctx, info) =>
            {
                // Perform frame dependent updates
                spinner.Update(info);

                var top = layout.GetArea(ctx, "Top");
                var middle = layout.GetArea(ctx, "Middle");
                var bottom = layout.GetArea(ctx, "BottomLeft");
                var bottomRight = layout.GetArea(ctx, "BottomRight");

                // FPS
                ctx.Render(
                    new FpsWidget(info.Fps, foreground: Color.Green),
                    top);

                // Tabs
                ctx.Render(tabs, layout.GetArea(ctx, "Tabs"));

                // Outer box
                ctx.Render(new BoxWidget(Color.Red)
                {
                    Border = Border.Double,
                }, middle);
                ctx.Render(new ClearWidget('╱', Color.Gray), middle.Inflate(-1, -1));

                // Ball
                ball.Update(info.FrameTime, middle.Inflate(-1, -1));
                ctx.Render(new BallWidget(), ball);

                // Inner box
                ctx.Render(
                    new BoxWidget()
                        .Style(Color.Green)
                        .Border(Border.Rounded)
                        .TitlePadding(1)
                        .MarkupTitle(
                            tabs.SelectedIndex == 0
                                ? "[yellow]Largest Cities[/]"
                                : "[yellow]To-Do[/]")
                        .Inner(
                            new CompositeWidget(
                                new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                                new PaddingWidget(new Padding(1, 0, 2, 0),
                                    tabs.SelectedIndex == 0 ? cities : todo),
                                new ScrollbarWidget()
                                    .VerticalRight()
                                    .Position(tabs.SelectedIndex == 0 ? cities.Position : todo.Position)
                                    .Length(tabs.SelectedIndex == 0 ? cities.Length : todo.Length)
                                    .ViewportLength(1)
                                    .Style(Color.Gray)
                                    .ThumbStyle(Color.Green))),
                    middle.Inflate(new Size(-10, -4)));

                // Help
                if (tabs.SelectedIndex == 0)
                {
                    ctx.Render(
                        Paragraph.FromMarkup("[bold][[Q]][/]:Quit  [bold][[↑↓]][/]:Move")
                            .Style(new Style(Color.Gray))
                            .Centered(), bottom);
                }
                else
                {
                    ctx.Render(
                        Paragraph.FromMarkup("[bold][[Q]][/]:Quit  [bold][[↑↓]][/]:Move  [bold][[SPACE]][/]:Select")
                            .Style(new Style(Color.Gray))
                            .Centered(), bottom);
                }

                ctx.Render(spinner, bottomRight);
            });

            // Handle input
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Q:
                        running = false;
                        break;
                    case ConsoleKey.DownArrow:
                        if (tabs.SelectedIndex == 0)
                        {
                            cities.MoveDown();
                        }
                        else
                        {
                            todo.MoveDown();
                        }

                        break;
                    case ConsoleKey.UpArrow:
                        if (tabs.SelectedIndex == 0)
                        {
                            cities.MoveUp();
                        }
                        else
                        {
                            todo.MoveUp();
                        }

                        break;
                    case ConsoleKey.Spacebar:
                        if (tabs.SelectedIndex != 0)
                        {
                            todo.Toggle();
                        }

                        break;
                    case ConsoleKey.Tab:
                        tabs.MoveNext();
                        break;
                }
            }
        }
    }
}