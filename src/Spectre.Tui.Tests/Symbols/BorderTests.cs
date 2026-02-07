namespace Spectre.Tui.Tests.Symbols;

public sealed class BorderTests
{
    [Fact]
    public void Plain()
    {
        // Given, When
        var result = RenderBox(Border.Plain);

        // Then
        result.ShouldBe(
            """
            ┌──┐
            │  │
            │  │
            └──┘
            """);
    }

    [Fact]
    public void Rounded()
    {
        // Given, When
        var result = RenderBox(Border.Rounded);

        // Then
        result.ShouldBe(
            """
            ╭──╮
            │  │
            │  │
            ╰──╯
            """);
    }

    [Fact]
    public void Double()
    {
        // Given, When
        var result = RenderBox(Border.Double);

        // Then
        result.ShouldBe(
            """
            ╔══╗
            ║  ║
            ║  ║
            ╚══╝
            """);
    }

    [Fact]
    public void Bold()
    {
        // Given, When
        var result = RenderBox(Border.Bold);

        // Then
        result.ShouldBe(
            """
            ┏━━┓
            ┃  ┃
            ┃  ┃
            ┗━━┛
            """);
    }

    private static string RenderBox(Border border)
    {
        return string.Format(
            """
            {0}{1}{2}{3}
            {4}  {5}
            {6}  {7}
            {8}{9}{10}{11}
            """,
            border.TopLeft, border.HorizontalTop,
            border.HorizontalTop, border.TopRight,
            border.VerticalLeft, border.VerticalRight,
            border.VerticalLeft, border.VerticalRight,
            border.BottomLeft, border.HorizontalBottom,
            border.HorizontalBottom, border.BottomRight);
    }
}
