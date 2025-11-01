using Shouldly;

namespace Spectre.Tui.Tests;

public sealed class SizeTests
{
    [Fact]
    public void Should_Assign_Properties_Correctly()
    {
        // Given, When
        var size = new Size(2, 3);

        // Then
        size.Width.ShouldBe(2);
        size.Height.ShouldBe(3);
        size.Area.ShouldBe(6);
    }
}