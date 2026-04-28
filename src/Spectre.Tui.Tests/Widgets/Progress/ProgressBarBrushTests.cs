using Spectre.Console;

namespace Spectre.Tui.Tests;

public sealed class ProgressBarBrushTests
{
    public sealed class SolidBrush
    {
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(9)]
        public void Should_Return_Same_Style_For_Every_Cell(int cellIndex)
        {
            // Given
            var style = new Style(foreground: new Color(10, 20, 30));
            var brush = ProgressBarBrush.Solid(style);

            // When
            var result = brush.GetStyle(cellIndex, 10, TimeSpan.Zero);

            // Then
            result.ShouldBe(style);
        }
    }

    public sealed class GradientBrush
    {
        [Fact]
        public void Should_Return_From_At_First_Cell()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(0, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Return_To_At_Last_Cell()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(9, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(to);
        }

        [Fact]
        public void Should_Interpolate_Linearly_At_Midpoint()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(5, 11, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(new Color(100, 50, 25));
        }

        [Fact]
        public void Should_Return_From_With_Single_Cell()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(0, 1, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Clamp_Negative_Cell_Index_To_From()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(-5, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Clamp_Excessive_Cell_Index_To_To()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Gradient(from, to);

            // When
            var result = brush.GetStyle(99, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(to);
        }
    }

    public sealed class PulsateBrush
    {
        [Fact]
        public void Should_Return_From_At_Zero()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Pulsate(from, to, TimeSpan.FromSeconds(2));

            // When
            var result = brush.GetStyle(0, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Return_To_At_Half_Period()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var period = TimeSpan.FromSeconds(2);
            var brush = ProgressBarBrush.Pulsate(from, to, period);

            // When
            var result = brush.GetStyle(0, 10, period / 2).Foreground;

            // Then
            result.ShouldBe(to);
        }

        [Fact]
        public void Should_Return_From_At_Full_Period()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var period = TimeSpan.FromSeconds(2);
            var brush = ProgressBarBrush.Pulsate(from, to, period);

            // When
            var result = brush.GetStyle(0, 10, period).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Return_Same_Color_For_All_Cells()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Pulsate(from, to, TimeSpan.FromSeconds(2));
            var elapsed = TimeSpan.FromSeconds(0.7);

            // When
            var first = brush.GetStyle(0, 10, elapsed).Foreground;
            var last = brush.GetStyle(9, 10, elapsed).Foreground;

            // Then
            last.ShouldBe(first);
        }

        [Fact]
        public void Should_Use_Default_Period_When_Null()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Pulsate(from, to);

            // When
            var result = brush.GetStyle(0, 10, TimeSpan.FromSeconds(0.75)).Foreground;

            // Then
            result.ShouldBe(to);
        }
    }

    public sealed class WaveBrush
    {
        [Fact]
        public void Should_Return_From_At_First_Cell_At_Zero_Time()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Wave(from, to, TimeSpan.FromSeconds(2));

            // When
            var result = brush.GetStyle(0, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Return_To_At_Half_Bar_At_Zero_Time()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Wave(from, to, TimeSpan.FromSeconds(2));

            // When
            var result = brush.GetStyle(5, 10, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(to);
        }

        [Fact]
        public void Should_Return_To_At_First_Cell_At_Half_Period()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var period = TimeSpan.FromSeconds(2);
            var brush = ProgressBarBrush.Wave(from, to, period);

            // When
            var result = brush.GetStyle(0, 10, period / 2).Foreground;

            // Then
            result.ShouldBe(to);
        }

        [Fact]
        public void Should_Return_From_At_Half_Bar_At_Half_Period()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var period = TimeSpan.FromSeconds(2);
            var brush = ProgressBarBrush.Wave(from, to, period);

            // When
            var result = brush.GetStyle(5, 10, period / 2).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Return_From_With_Zero_Total_Cells()
        {
            // Given
            var from = new Color(10, 20, 30);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Wave(from, to, TimeSpan.FromSeconds(2));

            // When
            var result = brush.GetStyle(0, 0, TimeSpan.Zero).Foreground;

            // Then
            result.ShouldBe(from);
        }

        [Fact]
        public void Should_Use_Default_Period_When_Null()
        {
            // Given
            var from = new Color(0, 0, 0);
            var to = new Color(200, 100, 50);
            var brush = ProgressBarBrush.Wave(from, to);

            // When
            var result = brush.GetStyle(0, 10, TimeSpan.FromSeconds(1)).Foreground;

            // Then
            result.ShouldBe(to);
        }
    }
}
