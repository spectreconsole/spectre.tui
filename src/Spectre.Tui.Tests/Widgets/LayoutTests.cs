namespace Spectre.Tui.Tests;

public sealed class LayoutTests
{
    [Fact]
    public void Should_Set_Default_Property_Values()
    {
        // Given, When
        var layout = new Layout();

        // Then
        layout.Name.ShouldBeNull();
        layout.Ratio.ShouldBe(1);
        layout.Size.ShouldBeNull();
        layout.IsVisible.ShouldBeTrue();
    }

    [Fact]
    public void Should_Set_Name_From_Constructor()
    {
        // Given, When
        var layout = new Layout("Foo");

        // Then
        layout.Name.ShouldBe("Foo");
    }

    [Fact]
    public void Should_Allow_Null_Name_In_Constructor()
    {
        // Given, When
        var layout = new Layout(null);

        // Then
        layout.Name.ShouldBeNull();
    }

    public sealed class TheRatioProperty
    {
        [Fact]
        public void Should_Throw_When_Set_Below_One()
        {
            // Given
            var layout = new Layout();

            // When
            var result = Record.Exception(() => layout.Ratio = 0);

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }

        [Fact]
        public void Should_Allow_Setting_To_One_Or_Greater()
        {
            // Given
            var layout = new Layout();

            // When
            layout.Ratio = 5;

            // Then
            layout.Ratio.ShouldBe(5);
        }
    }

    public sealed class TheSizeProperty
    {
        [Fact]
        public void Should_Throw_When_Set_Below_One()
        {
            // Given
            var layout = new Layout();

            // When
            var result = Record.Exception(() => layout.Size = 0);

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }

        [Fact]
        public void Should_Allow_Null()
        {
            // Given
            var layout = new Layout
            {
                Size = 5,
            };

            // When
            layout.Size = null;

            // Then
            layout.Size.ShouldBeNull();
        }

        [Fact]
        public void Should_Allow_Setting_To_One_Or_Greater()
        {
            // Given
            var layout = new Layout();

            // When
            layout.Size = 7;

            // Then
            layout.Size.ShouldBe(7);
        }
    }

    public sealed class TheMinimumSizeProperty
    {
        [Fact]
        public void Should_Throw_When_Set_Below_One()
        {
            // Given
            var layout = new Layout();

            // When
            var result = Record.Exception(() => layout.MinimumSize = 0);

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }

        [Fact]
        public void Should_Allow_Setting_To_One_Or_Greater()
        {
            // Given
            var layout = new Layout();

            // When
            layout.MinimumSize = 3;

            // Then
            layout.MinimumSize.ShouldBe(3);
        }
    }

    public sealed class TheSplitColumnsMethod
    {
        [Fact]
        public void Should_Return_Same_Instance_For_Chaining()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.SplitColumns(
                new Layout("Left"),
                new Layout("Right"));

            // Then
            result.ShouldBeSameAs(layout);
        }

        [Fact]
        public void Should_Throw_When_Splitting_Twice()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = Record.Exception(() =>
                layout.SplitColumns(
                    new Layout("Foo"),
                    new Layout("Bar")));

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }
    }

    public sealed class TheSplitRowsMethod
    {
        [Fact]
        public void Should_Return_Same_Instance_For_Chaining()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.SplitRows(
                new Layout("Top"),
                new Layout("Bottom"));

            // Then
            result.ShouldBeSameAs(layout);
        }

        [Fact]
        public void Should_Throw_When_Splitting_Twice()
        {
            // Given
            var layout = new Layout()
                .SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom"));

            // When
            var result = Record.Exception(() =>
                layout.SplitRows(
                    new Layout("Foo"),
                    new Layout("Bar")));

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }
    }

    public sealed class TheGetLayoutMethod
    {
        [Fact]
        public void Should_Find_Layout_By_Name()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = layout.GetLayout("Right");

            // Then
            result.Name.ShouldBe("Right");
        }

        [Fact]
        public void Should_Find_Nested_Layout()
        {
            // Given
            var layout = new Layout()
                .SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom").SplitColumns(
                        new Layout("Left"),
                        new Layout("Right")));

            // When
            var result = layout.GetLayout("Right");

            // Then
            result.Name.ShouldBe("Right");
        }

        [Fact]
        public void Should_Be_Case_Insensitive()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = layout.GetLayout("LEFT");

            // Then
            result.Name.ShouldBe("Left");
        }

        [Fact]
        public void Should_Be_Able_To_Retrieve_Hidden_Layouts()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left").Hidden(),
                    new Layout("Right"));

            // When
            var result = layout.GetLayout("Left");

            // Then
            result.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Throw_When_Name_Is_Null()
        {
            // Given
            var layout = new Layout();

            // When
            var result = Record.Exception(() => layout.GetLayout(null!));

            // Then
            result.ShouldBeOfType<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Name_Is_Empty()
        {
            // Given
            var layout = new Layout();

            // When
            var result = Record.Exception(() => layout.GetLayout(string.Empty));

            // Then
            result.ShouldBeOfType<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Layout_Not_Found()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = Record.Exception(() => layout.GetLayout("Missing"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }
    }

    public sealed class TheGetAreaMethod
    {
        [Fact]
        public void Should_Return_Full_Area_When_Self_Is_Named()
        {
            // Given
            var layout = new Layout("Root");

            // When
            var result = layout.GetArea(new Rectangle(0, 0, 10, 5), "Root");

            // Then
            result.ShouldBe(new Rectangle(0, 0, 10, 5));
        }

        [Fact]
        public void Should_Compute_Area_For_Column_Split()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var left = layout.GetArea(new Rectangle(0, 0, 10, 5), "Left");
            var right = layout.GetArea(new Rectangle(0, 0, 10, 5), "Right");

            // Then
            left.ShouldBe(new Rectangle(0, 0, 5, 5));
            right.ShouldBe(new Rectangle(5, 0, 5, 5));
        }

        [Fact]
        public void Should_Compute_Area_For_Row_Split()
        {
            // Given
            var layout = new Layout()
                .SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom"));

            // When
            var top = layout.GetArea(new Rectangle(0, 0, 10, 4), "Top");
            var bottom = layout.GetArea(new Rectangle(0, 0, 10, 4), "Bottom");

            // Then
            top.ShouldBe(new Rectangle(0, 0, 10, 2));
            bottom.ShouldBe(new Rectangle(0, 2, 10, 2));
        }

        [Fact]
        public void Should_Honor_Fixed_Size()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Fixed").Size(3),
                    new Layout("Flex"));

            // When
            var fixedArea = layout.GetArea(new Rectangle(0, 0, 10, 5), "Fixed");
            var flexArea = layout.GetArea(new Rectangle(0, 0, 10, 5), "Flex");

            // Then
            fixedArea.ShouldBe(new Rectangle(0, 0, 3, 5));
            flexArea.ShouldBe(new Rectangle(3, 0, 7, 5));
        }

        [Fact]
        public void Should_Compute_Area_For_Nested_Layout()
        {
            // Given
            var layout = new Layout()
                .SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom").SplitColumns(
                        new Layout("Left"),
                        new Layout("Right")));

            // When
            var result = layout.GetArea(new Rectangle(0, 0, 10, 4), "Right");

            // Then
            result.ShouldBe(new Rectangle(5, 2, 5, 2));
        }

        [Fact]
        public void Should_Skip_Hidden_Children()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Hidden").Hidden(),
                    new Layout("Visible"));

            // When
            var result = layout.GetArea(new Rectangle(0, 0, 10, 5), "Visible");

            // Then
            result.ShouldBe(new Rectangle(0, 0, 10, 5));
        }

        [Fact]
        public void Should_Be_Case_Insensitive()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = layout.GetArea(new Rectangle(0, 0, 10, 5), "RIGHT");

            // Then
            result.ShouldBe(new Rectangle(5, 0, 5, 5));
        }

        [Fact]
        public void Should_Throw_When_Layout_Not_Found()
        {
            // Given
            var layout = new Layout()
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            // When
            var result = Record.Exception(() =>
                layout.GetArea(new Rectangle(0, 0, 10, 5), "Missing"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
        }
    }

    public sealed class TheExtensionMethods
    {
        [Fact]
        public void Ratio_Should_Set_Value_And_Return_Same_Instance()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.Ratio(3);

            // Then
            result.ShouldBeSameAs(layout);
            layout.Ratio.ShouldBe(3);
        }

        [Fact]
        public void Size_Should_Set_Value_And_Return_Same_Instance()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.Size(7);

            // Then
            result.ShouldBeSameAs(layout);
            layout.Size.ShouldBe(7);
        }

        [Fact]
        public void MinimumSize_Should_Set_Value_And_Return_Same_Instance()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.MinimumSize(2);

            // Then
            result.ShouldBeSameAs(layout);
            layout.MinimumSize.ShouldBe(2);
        }

        [Fact]
        public void Visible_Should_Set_IsVisible_And_Return_Same_Instance()
        {
            // Given
            var layout = new Layout();
            layout.Visible(false);

            // When
            var result = layout.Visible();

            // Then
            result.ShouldBeSameAs(layout);
            layout.IsVisible.ShouldBeTrue();
        }

        [Fact]
        public void Hidden_Should_Set_IsVisible_To_False_And_Return_Same_Instance()
        {
            // Given
            var layout = new Layout();

            // When
            var result = layout.Hidden();

            // Then
            result.ShouldBeSameAs(layout);
            layout.IsVisible.ShouldBeFalse();
        }

        [Fact]
        public void Should_Throw_When_Layout_Is_Null()
        {
            // Given
            Layout? layout = null;

            // When
            var result = Record.Exception(() => layout!.Ratio(1));

            // Then
            result.ShouldBeOfType<ArgumentNullException>();
        }
    }
}
