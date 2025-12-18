namespace Spectre.Tui.Tests;

public sealed class RectangleTests
{
    [Fact]
    public void Should_Assign_Properties_Correctly()
    {
        // Given, When
        var rect = new Rectangle(1, 2, 3, 4);

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

    [Fact]
    public void Should_Deconstruct_As_Expected()
    {
        // Given
        var region = new Rectangle(1, 2, 5, 10);

        // When
        var (x, y, width, height) = region;

        // Then
        x.ShouldBe(1);
        y.ShouldBe(2);
        width.ShouldBe(5);
        height.ShouldBe(10);
    }

    public sealed class TheInflateMethod
    {
        [Fact]
        public void Should_Inflate_Region_With_Expected_Size()
        {
            // Given
            var region = new Rectangle(3, 5, 7, 10);

            // When
            var result = region.Inflate(2, 3);

            // Then
            result.ShouldBe(
                new Rectangle(1, 2, 11, 16));
        }

        [Fact]
        public void Should_Inflate_Region_With_Expected_Size_But_Clamp_Position()
        {
            // Given
            var region = new Rectangle(1, 2, 5, 10);

            // When
            var result = region.Inflate(2, 3);

            // Then
            result.ShouldBe(
                new Rectangle(0, 0, 9, 16));
        }

        [Fact]
        public void Should_Deflate_Region_With_Expected_Size_If_Size_Is_Negative()
        {
            // Given
            var region = new Rectangle(1, 2, 5, 10);

            // When
            var result = region.Inflate(-1, -2);

            // Then
            result.ShouldBe(
                new Rectangle(2, 4, 3, 6));
        }
    }

    public sealed class TheOffsetMethod
    {
        [Fact]
        public void Should_Offset_Region()
        {
            // Given
            var region = new Rectangle(1, 2, 5, 10);

            // When
            var result = region.Offset(2, 3);

            // Then
            result.ShouldBe(
                new Rectangle(3, 5, 5, 10));
        }
    }

    public sealed class TheUnionMethod
    {
        [Fact]
        public void Should_Offset_Region()
        {
            // Given
            var region = new Rectangle(1, 2, 5, 10);

            // When
            var result = region.Union(
                new Rectangle(3, 4, 15, 20));

            // Then
            result.ShouldBe(
                new Rectangle(1, 2, 17, 22));
        }
    }

    public sealed class TheIntersectMethod
    {
        [Fact]
        public void Should_Throw_If_No_Intersection_Could_Be_Made()
        {
            // Given
            var r1 = new Rectangle(0, 0, 10, 10);
            var r2 = new Rectangle(11, 11, 10, 10);

            // When
            var result = Record.Exception(() => r1.Intersect(r2));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("The two rectangles do not intersect");
        }

        [Fact]
        public void Should_Return_Intersection()
        {
            // Given
            var r1 = new Rectangle(0, 0, 15, 12);
            var r2 = new Rectangle(5, 5, 10, 10);

            // When
            var result = r1.Intersect(r2);

            // Then
            result.ShouldBe(
                new Rectangle(5, 5, 10, 7));
        }
    }

    [Fact]
    public void Should_Consider_Two_Equal_Rectangles_Equal()
    {
        // Given
        var first = new Rectangle(1, 2, 3, 5);
        var second = new Rectangle(1, 2, 3, 5);

        // When
        var result = first == second;

        // Then
        result.ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Consider_Two_Inequal_Rectangles_Equal()
    {
        // Given
        var first = new Rectangle(1, 2, 3, 5);
        var second = new Rectangle(1, 2, 3, 7);

        // When
        var result = first == second;

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Return_Same_HashCode_For_Two_Equal_Rectangles()
    {
        // Given, When
        var first = new Rectangle(1, 2, 3, 5);
        var second = new Rectangle(1, 2, 3, 5);

        // Then
        first.ShouldBe(second);
    }
}