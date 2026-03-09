using Spectre.Console;
using Spectre.Tui;
using System.Text;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var running = true;

        using var terminal = Terminal.Create();
        var renderer = new Renderer(terminal);
        renderer.SetTargetFps(144);
        Console.Title = "Spectre.Tui Sandbox";

        var widgets = new List<(bool, WriteWidget)>
        {
            (true, new WriteWidget(Color.Aqua)),
            (false, new WriteWidget(Color.Purple))
        };

        var inputWidget = new WriteWidget(Color.Aqua);

        while (running)
        {

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                var activePair = widgets.FirstOrDefault(pair => pair.Item1);
                var activeWidget = activePair.Item2;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        var active = widgets.IndexOf(activePair);
                        widgets = [.. widgets.Select((pair, index) => (index == (active + 1) % widgets.Count, pair.Item2))];
                        break;
                    case ConsoleKey.RightArrow:
                        running = false;
                        break;
                    case ConsoleKey.Q:
                        running = false;
                        break;
                    case ConsoleKey.Backspace:
                        activeWidget.DeleteChar();
                        break;
                    case ConsoleKey.Spacebar:
                        activeWidget.AppendChar(' ');
                        break;
                    case ConsoleKey.Escape:
                        activeWidget.Clear();
                        break;
                    case ConsoleKey.Tab:
                        activeWidget.Reverse();
                        break;
                    default:
                        activeWidget.AppendChar(key.ToString()[0]);
                    /*
        Console.OutputEncoding = Encoding.Unicode;
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            running = false;
        };

        var ball = new BallState();
        var todo = new ListWidget<ToDoItem>(
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
            ])
            .HighlightSymbol("→ ")
            .WrapAround()
            .SelectedIndex(0);

        while (running)
        {
            renderer.Draw((ctx, elapsed) =>
            {
                var layout = new Layout("Root")
                    .SplitRows(
                        new Layout("Top").Size(1),
                        new Layout("Middle"),
                        new Layout("Bottom").Size(1));

                var top = layout.GetArea(ctx, "Top");
                var middle = layout.GetArea(ctx, "Middle");
                var bottom = layout.GetArea(ctx, "Bottom");

                // FPS
                ctx.Render(
                    new FpsWidget(elapsed, foreground: Color.Green),
                    top);

                // Outer box
                ctx.Render(new BoxWidget(Color.Red)
                {
                    Border = Border.Double,
                }, middle);
                ctx.Render(new ClearWidget('╱', Color.Gray), middle.Inflate(-1, -1));

                // Ball
                ball.Update(elapsed, middle.Inflate(-1, -1));
                ctx.Render(new BallWidget(), ball);

                // Inner box
                var inner = middle.Inflate(new Size(-12, -5));
                ctx.Render(new BoxWidget(Color.Green)
                {
                    Border = Border.McGuganTall,
                }, inner);
                ctx.Render(
                    new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                    inner.Inflate(-1, -1));

                // To-Do list
                ctx.Render(todo, inner.Inflate(-2, -2));

                // Help
                ctx.Render(Text.FromMarkup("[bold][[Q]][/]:Quit  [bold][[↑↓]][/]:Move  [bold][[Space]][/]:Select", new Style(Color.Gray)), bottom);
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
                        todo.SelectedItem?.Toggle(); */
                        break;
                }
            }
            renderer.Draw((ctx, elapsed) =>
            {
                var vp = ctx.Viewport;
                var x = (int)Math.Floor(vp.Width * 0.4);
                var left = new Rectangle(0, 0, x, vp.Height);
                var right = new Rectangle(x + 1, 0, vp.Width - x - 1, vp.Height);

                ctx.Render(new BoxWidget(Color.Red), left);
                ctx.Render(new BoxWidget(Color.Gray), right);


                ctx.Render(widgets.First().Item2, Inner(left));
                ctx.Render(widgets.Last().Item2, Inner(right));
            });
        }
    }

    private static Rectangle Inner(Rectangle r) => r.Inflate(-1, -1);
}