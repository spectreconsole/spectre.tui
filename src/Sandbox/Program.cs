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

        var layout = new Layout("Root")
            .SplitRows(
                new Layout("Top").Size(1),
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
                        .Inner(
                            new CompositeWidget(
                                new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                                new PaddingWidget(new Size(-1, -1), todo),
                                new ScrollbarWidget()
                                    .VerticalRight()
                                    .Position(todo.Position).Length(todo.Length)
                                    .ViewportLength(1)
                                    .Style(Color.Gray)
                                    .ThumbStyle(Color.Yellow))),
                    middle.Inflate(new Size(-12, -5)));

                // Help
                ctx.Render(
                    Paragraph.FromMarkup("[bold][[Q]][/]:Quit  [bold][[↑↓]][/]:Move  [bold][[Space]][/]:Select")
                        .Style(new Style(Color.Gray))
                        .Centered(), bottom);

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
                        todo.MoveDown();
                        break;
                    case ConsoleKey.UpArrow:
                        todo.MoveUp();
                        break;
                    case ConsoleKey.Spacebar:
                        todo.Toggle();
                        break;
                }
            }
        }
    }
}