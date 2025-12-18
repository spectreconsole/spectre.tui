namespace Spectre.Tui.Tests;

public sealed class PositionTests
{
    [Fact]
    public void Should_Assign_X_And_Y_Correctly()
    {
        // Given, When
        var size = new Position(2, 3);

        // Then
        size.X.ShouldBe(2);
        size.Y.ShouldBe(3);
    }

    [Fact]
    public void Should_Consider_Two_Equal_Sizes_Equal()
    {
        // Given
        var first = new Position(2, 3);
        var second = new Position(2, 3);

        // When
        var result = first == second;

        // Then
        result.ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Consider_Two_Inequal_Sizes_Equal()
    {
        // Given
        var first = new Position(2, 3);
        var second = new Position(3, 3);

        // When
        var result = first == second;

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Return_Same_HashCode_For_Two_Equal_Sizes()
    {
        // Given, When
        var first = new Position(2, 3).GetHashCode();
        var second = new Position(2, 3).GetHashCode();

        // Then
        first.ShouldBe(second);
    }
}