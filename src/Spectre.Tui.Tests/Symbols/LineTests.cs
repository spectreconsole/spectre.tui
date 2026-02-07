namespace Spectre.Tui.Tests.Symbols;

public sealed class LineTests
{
    [Fact]
    public void Plain()
    {
        // Given, When
        var result = RenderBox(Line.Plain);

        // Then
        result.ShouldBe(
            """
            ┌─┬┐
            │ ││
            ├─┼┤
            └─┴┘
            """);
    }

    [Fact]
    public void Rounded()
    {
        // Given, When
        var result = RenderBox(Line.Rounded);

        // Then
        result.ShouldBe(
            """
            ╭─┬╮
            │ ││
            ├─┼┤
            ╰─┴╯
            """);
    }

    [Fact]
    public void Double()
    {
        // Given, When
        var result = RenderBox(Line.Double);

        // Then
        result.ShouldBe(
            """
            ╔═╦╗
            ║ ║║
            ╠═╬╣
            ╚═╩╝
            """);
    }

    [Fact]
    public void Bold()
    {
        // Given, When
        var result = RenderBox(Line.Bold);

        // Then
        result.ShouldBe(
            """
            ┏━┳┓
            ┃ ┃┃
            ┣━╋┫
            ┗━┻┛
            """);
    }

    private static string RenderBox(Line line)
    {
        return string.Format(
            """
            {0}{1}{2}{3}
            {4}{5}{6}{7}
            {8}{9}{10}{11}
            {12}{13}{14}{15}
            """,
            line.TopLeft, line.Horizontal,
            line.HorizontalDown, line.TopRight,
            line.Vertical, " ",
            line.Vertical, line.Vertical,
            line.VerticalRight, line.Horizontal,
            line.Cross, line.VerticalLeft,
            line.BottomLeft, line.Horizontal,
            line.HorizontalUp, line.BottomRight);
    }
}
