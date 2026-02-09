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
            .HighlightSymbol("→")
            .WrapAround()
            .SelectedIndex(0);

        while (running)
        {
            renderer.Draw((ctx, elapsed) =>
            {
                // Outer box
                ctx.Render(new BoxWidget(Color.Red)
                {
                    Border = Border.Double
                });
                ctx.Render(new ClearWidget('·'), ctx.Viewport.Inflate(-1, -1));

                // Ball
                ball.Update(elapsed, ctx.Viewport.Inflate(-1, -1));
                ctx.Render(new BallWidget(), ball);

                // FPS
                ctx.Render(
                    new FpsWidget(elapsed, foreground: Color.Green),
                    new Rectangle(0, ctx.Screen.Bottom - 1, ctx.Screen.Width, 1));

                // Inner box
                var inner = ctx.Viewport.Inflate(new Size(-12, -5));
                ctx.Render(new BoxWidget(Color.Green), inner);
                ctx.Render(
                    new ClearWidget(' ', new Style(decoration: Decoration.Bold)),
                    inner.Inflate(-1, -1));

                // To-Do list
                ctx.Render(todo, inner.Inflate(-1, -1));
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
                        todo.SelectedItem?.Toggle();
                        break;
                }
            }
        }
    }
}