using Spectre.Console;

namespace Spectre.Tui.Tests;

public sealed class SparklineWidgetTests
{
    [Fact]
    public void Should_Render_Blank_When_Data_Is_Empty()
    {
        // Given
        var fixture = new TuiFixture(new Size(5, 1));
        var widget = new SparklineWidget();

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("•••••");
    }

    [Fact]
    public void Should_Render_Eighths_Progression_In_Single_Row()
    {
        // Given
        var fixture = new TuiFixture(new Size(9, 1));
        var widget = new SparklineWidget()
            .Data(0ul, 1ul, 2ul, 3ul, 4ul, 5ul, 6ul, 7ul, 8ul)
            .Max(8);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("•▁▂▃▄▅▆▇█");
    }

    [Fact]
    public void Should_Stack_Bars_Across_Multiple_Rows()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 2));
        var widget = new SparklineWidget()
            .Data(0ul, 8ul, 16ul)
            .Max(16);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe(
            """
            ••█
            •██
            """);
    }

    [Fact]
    public void Should_Auto_Compute_Max_From_Data()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 1));
        var widget = new SparklineWidget()
            .Data(0ul, 5ul, 10ul);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("•▄█");
    }

    [Fact]
    public void Should_Render_Blank_When_All_Data_Is_Zero()
    {
        // Given
        var fixture = new TuiFixture(new Size(4, 1));
        var widget = new SparklineWidget()
            .Data(0ul, 0ul, 0ul, 0ul);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("••••");
    }

    [Fact]
    public void Should_Render_Blank_When_Max_Is_Explicitly_Zero()
    {
        // Given
        var fixture = new TuiFixture(new Size(4, 1));
        var widget = new SparklineWidget()
            .Data(5ul, 10ul, 15ul, 20ul)
            .Max(0);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("••••");
    }

    [Fact]
    public void Should_Clip_When_Data_Exceeds_Viewport_Width()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 1));
        var widget = new SparklineWidget()
            .Data(1ul, 2ul, 3ul, 4ul, 5ul, 6ul, 7ul, 8ul)
            .Max(8);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("▁▂▃");
    }

    [Fact]
    public void Should_Pin_Newest_Sample_To_Right_Edge_When_RightToLeft()
    {
        // Given
        var fixture = new TuiFixture(new Size(5, 1));
        var widget = new SparklineWidget()
            .Data(1ul, 2ul, 3ul)
            .Max(3)
            .Direction(SparklineDirection.RightToLeft);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("••▃▅█");
    }

    [Fact]
    public void Should_Drop_Oldest_Samples_When_RightToLeft_Overflows_Viewport()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 1));
        var widget = new SparklineWidget()
            .Data(1ul, 2ul, 3ul, 4ul, 5ul, 6ul, 7ul, 8ul)
            .Max(8)
            .Direction(SparklineDirection.RightToLeft);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("▆▇█");
    }

    [Fact]
    public void Should_Apply_Per_Segment_Style_Over_Widget_Style()
    {
        // Given
        var fixture = new TuiFixture(new AnsiTestTerminal(size: new Size(3, 1)));
        var widget = new SparklineWidget()
            .Style(new Style(Color.Yellow))
            .Data(
                new SparklineSegment(8, new Style(Color.Red)),
                new SparklineSegment(8),
                new SparklineSegment(8))
            .Max(8);

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldContain("\e[38;5;9m█");
        result.ShouldContain("\e[38;5;11m█");
    }

    [Fact]
    public void Should_Render_Absent_Value_With_Configured_Symbol()
    {
        // Given
        var fixture = new TuiFixture(new Size(3, 1));
        var widget = new SparklineWidget()
            .Data(
                new SparklineSegment(1),
                new SparklineSegment(null),
                new SparklineSegment(3))
            .Max(3)
            .AbsentValueSymbol('?');

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe("▃?█");
    }

    [Fact]
    public void Should_Fill_Every_Row_For_Absent_Value()
    {
        // Given
        var fixture = new TuiFixture(new Size(1, 3));
        var widget = new SparklineWidget()
            .Data(new SparklineSegment(null))
            .AbsentValueSymbol('?');

        // When
        var result = fixture.Render(widget);

        // Then
        result.ShouldBe(
            """
            ?
            ?
            ?
            """);
    }

    [Fact]
    public void Should_Implicitly_Convert_Ulong_To_Segment()
    {
        // Given, When
        SparklineSegment segment = 42ul;

        // Then
        segment.Value.ShouldBe(42ul);
        segment.Style.ShouldBeNull();
    }
}
