using Spectre.Console;

namespace Spectre.Tui.Tests;

public sealed class ProgressBarWidgetTests
{
    private sealed class ElapsedCapturingBrush : ProgressBarBrush
    {
        public TimeSpan? Captured { get; private set; }

        public override Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed)
        {
            Captured = elapsed;
            return Style.Plain;
        }
    }

    [Fact]
    public void Should_Accumulate_Elapsed_Time_Across_Update_Calls()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var brush = new ElapsedCapturingBrush();
        var widget = new ProgressBarWidget()
            .Foreground(brush)
            .Value(50).Max(100)
            .HideLabel();

        // When
        widget.Update(new FrameInfo(TimeSpan.FromSeconds(0.5), TimeSpan.Zero, 0));
        widget.Update(new FrameInfo(TimeSpan.FromSeconds(1.5), TimeSpan.Zero, 0));
        fixture.Render(widget);

        // Then
        brush.Captured.ShouldBe(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Should_Render_Empty_Bar()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(0)
            .Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒•••0%");
    }

    [Fact]
    public void Should_Render_Full_Bar()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(100)
            .Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("███████████████•100%");
    }

    [Fact]
    public void Should_Render_Partial_Bar_With_Percentage()
    {
        // Given
        var fixture = new TuiFixture(new Size(24, 1));
        var bar = new ProgressBarWidget()
            .Value(30)
            .Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("██████▒▒▒▒▒▒▒▒▒▒▒▒▒••30%");
    }

    [Fact]
    public void Should_Hide_Label_When_Disabled()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(50)
            .Max(100)
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("█████▒▒▒▒▒");
    }

    [Fact]
    public void Should_Render_Fraction_Label()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(130)
            .Max(1200)
            .Fraction();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("█▒▒▒▒▒▒▒▒▒••130/1200");
    }

    [Fact]
    public void Should_Render_Custom_Label()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(42)
            .Max(100)
            .Label(ProgressBarLabel.Custom((v, _) => $"{v}!"));

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("███████▒▒▒▒▒▒▒▒▒•42!");
    }

    [Fact]
    public void Should_Use_Custom_Symbols()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(50)
            .Max(100)
            .Symbols('#', '-')
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("#####-----");
    }

    [Fact]
    public void Should_Render_Smooth_Sub_Cell_Boundary()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(55)
            .Max(100)
            .Smooth()
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("█████▌••••");
    }

    [Fact]
    public void Should_Render_Smooth_Eighth_Increment()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 1));
        var bar = new ProgressBarWidget()
            .Value(10)
            .Max(100)
            .Smooth()
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("▊•••••••");
    }

    [Fact]
    public void Should_Render_Smooth_Empty_Bar()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 1));
        var bar = new ProgressBarWidget()
            .Value(0)
            .Max(100)
            .Smooth()
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("••••••••");
    }

    [Fact]
    public void Should_Render_Smooth_Full_Bar()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 1));
        var bar = new ProgressBarWidget()
            .Value(100)
            .Max(100)
            .Smooth()
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("████████");
    }

    [Fact]
    public void Should_Render_Smooth_With_Label()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(30)
            .Max(100)
            .Smooth();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("████▌••••••••••••30%");
    }

    [Fact]
    public void Should_Clamp_Value_Above_Max()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(200)
            .Max(100)
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("██████████");
    }

    [Fact]
    public void Should_Clamp_Negative_Value()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(-50)
            .Max(100)
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("▒▒▒▒▒▒▒▒▒▒");
    }

    [Fact]
    public void Should_Render_Empty_When_Max_Is_Zero()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var bar = new ProgressBarWidget()
            .Value(50)
            .Max(0)
            .HideLabel();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("▒▒▒▒▒▒▒▒▒▒");
    }

    [Fact]
    public void Should_Drop_Label_When_Bar_Would_Be_Too_Narrow()
    {
        // Given
        var fixture = new TuiFixture(new Size(5, 1));
        var bar = new ProgressBarWidget()
            .Value(50)
            .Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("███▒▒");
    }

    [Fact]
    public void Should_Keep_Label_When_Bar_Has_Exactly_Three_Cells()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 1));
        var bar = new ProgressBarWidget()
            .Value(30)
            .Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe("█▒▒••30%");
    }

    [Theory]
    [InlineData(5, "█▒▒▒▒▒▒▒▒▒▒▒▒▒▒•••5%")]
    [InlineData(99, "███████████████••99%")]
    [InlineData(100, "███████████████•100%")]
    public void Should_Reserve_Max_Width_For_Percentage(int value, string expected)
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(value).Max(100);

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(5, "▒▒▒▒▒▒▒▒▒▒••••5/1200")]
    [InlineData(1200, "██████████•1200/1200")]
    public void Should_Reserve_Max_Width_For_Fraction(int value, string expected)
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(value).Max(1200)
            .Fraction();

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(5, "█▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒•5!")]
    [InlineData(50, "████████▒▒▒▒▒▒▒▒•50!")]
    public void Should_Allow_Custom_Label_Width_To_Vary(int value, string expected)
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var bar = new ProgressBarWidget()
            .Value(value).Max(100)
            .Label(ProgressBarLabel.Custom((v, _) => $"{v}!"));

        // When
        var result = fixture.Render(bar);

        // Then
        result.ShouldBe(expected);
    }
}
