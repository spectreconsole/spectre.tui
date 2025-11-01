using Shouldly;

namespace Spectre.Tui.Tests;

public sealed class RegionTests
{
    [Fact]
    public void Should_Assign_Properties_Correctly()
    {
        // Given, When
        var rect = new Region(1, 2, 3, 4);

        // Then
        rect.X.ShouldBe(1);
        rect.Y.ShouldBe(2);
        rect.Width.ShouldBe(3);
        rect.Height.ShouldBe(4);
        rect.Top.ShouldBe(2);
        rect.Bottom.ShouldBe(6);
        rect.Left.ShouldBe(1);
        rect.Right.ShouldBe(4);
    }
}