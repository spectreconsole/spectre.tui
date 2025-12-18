namespace Spectre.Tui.Tests;

public sealed class SizeTests
{
    [Fact]
    public void Should_Assign_Width_And_Height_Correctly()
    {
        // Given, When
        var size = new Size(2, 3);

        // Then
        size.Width.ShouldBe(2);
        size.Height.ShouldBe(3);
    }

    [Fact]
    public void Should_Calculate_Area_Correctly()
    {
        // Given
        var size = new Size(7, 3);

        // When
        var result = size.Area;

        // Then
        result.ShouldBe(21);
    }

    [Fact]
    public void Should_Consider_Two_Equal_Sizes_Equal()
    {
        // Given
        var first = new Size(2, 3);
        var second = new Size(2, 3);

        // When
        var result = first == second;

        // Then
        result.ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Consider_Two_Inequal_Sizes_Equal()
    {
        // Given
        var first = new Size(2, 3);
        var second = new Size(3, 3);

        // When
        var result = first == second;

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Return_Same_HashCode_For_Two_Equal_Sizes()
    {
        // Given, When
        var first = new Size(2, 3).GetHashCode();
        var second = new Size(2, 3).GetHashCode();

        // Then
        first.ShouldBe(second);
    }

    public sealed class TheToRectangleMethod
    {
        [Fact]
        public void Should_Return_Rectangle_With_Expected_Width_And_Height()
        {
            // Given
            var size = new Size(7, 12);

            // When
            var result = size.ToRectangle();

            // Then
            result.X.ShouldBe(0);
            result.Y.ShouldBe(0);
            result.Width.ShouldBe(7);
            result.Height.ShouldBe(12);
        }
    }
}