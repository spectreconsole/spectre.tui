namespace Spectre.Tui.Tests;

public sealed class ScrollbarWidgetTests
{
    [Fact]
    public void Should_Render_Vertical_Thumb_At_Top()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(0);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            █
            █
            █
            ║
            ║
            ║
            ║
            ║
            ▼
            """);
    }

    [Fact]
    public void Should_Render_Vertical_Thumb_In_Middle()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            ║
            ║
            █
            █
            █
            ║
            ║
            ║
            ▼
            """);
    }

    [Fact]
    public void Should_Render_Vertical_Thumb_At_Bottom()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(7);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            ║
            ║
            ║
            ║
            ║
            █
            █
            █
            ▼
            """);
    }

    [Fact]
    public void Should_Fill_Track_When_Viewport_Equals_Length()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(10)
            .Position(0);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            █
            █
            █
            █
            █
            █
            █
            █
            ▼
            """);
    }

    [Fact]
    public void Should_Render_Without_Crashing_When_Length_Is_Zero()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(0)
            .ViewportLength(0)
            .Position(0);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            █
            █
            █
            █
            █
            █
            █
            █
            ▼
            """);
    }

    [Fact]
    public void Should_Render_Single_Thumb_Cell_When_Size_Is_One()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 1));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe("█");
    }

    [Fact]
    public void Should_Render_Caps_Only_When_Size_Is_Two()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 2));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            ▼
            """);
    }

    [Fact]
    public void Should_Clamp_Negative_Position_To_Top()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(-5);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            █
            █
            █
            ║
            ║
            ║
            ║
            ║
            ▼
            """);
    }

    [Fact]
    public void Should_Clamp_Overshoot_Position_To_Bottom()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(999);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            ║
            ║
            ║
            ║
            ║
            █
            █
            █
            ▼
            """);
    }

    [Fact]
    public void Should_Render_Horizontal_Thumb_In_Middle()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var scrollbar = new ScrollbarWidget()
            .HorizontalTop()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe("◄══███═══►");
    }

    [Fact]
    public void Should_Position_VerticalLeft_In_Wider_Viewport()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 5));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲••
            ║••
            █••
            ║••
            ▼••
            """);
    }

    [Fact]
    public void Should_Position_VerticalRight_In_Wider_Viewport()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 5));
        var scrollbar = new ScrollbarWidget()
            .VerticalRight()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ••▲
            ••║
            ••█
            ••║
            ••▼
            """);
    }

    [Fact]
    public void Should_Position_HorizontalTop_In_Taller_Viewport()
    {
        // Given
        var fixture = new TuiFixture(new Size(5, 3));
        var scrollbar = new ScrollbarWidget()
            .HorizontalTop()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ◄═█═►
            •••••
            •••••
            """);
    }

    [Fact]
    public void Should_Position_HorizontalBottom_In_Taller_Viewport()
    {
        // Given
        var fixture = new TuiFixture(new Size(5, 3));
        var scrollbar = new ScrollbarWidget()
            .HorizontalBottom()
            .Length(10)
            .ViewportLength(3)
            .Position(4);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            •••••
            •••••
            ◄═█═►
            """);
    }

    [Fact]
    public void Should_Handle_Length_Near_Int_MaxValue_Without_Overflow()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 10));
        var scrollbar = new ScrollbarWidget()
            .VerticalLeft()
            .Length(int.MaxValue)
            .ViewportLength(1)
            .Position(int.MaxValue - 1);

        // When
        var result = fixture.Render(scrollbar);

        // Then
        result.ShouldBe(
            """
            ▲
            ║
            ║
            ║
            ║
            ║
            ║
            ║
            █
            ▼
            """);
    }
}
