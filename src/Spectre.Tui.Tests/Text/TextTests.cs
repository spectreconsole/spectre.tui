namespace Spectre.Tui.Tests;

public sealed class TextTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Set_Text()
        {
            // Given, When
            var text = Text.FromString("Hello World", Color.Red);

            // Then
            text.Lines.Count.ShouldBe(1);
        }

        [Fact]
        public void Should_Set_Style()
        {
            // Given, When
            var text = Text.FromString("Hello World", Color.Red);

            // Then
            text.Lines.Count.ShouldBe(1);
            text.Style.ShouldBe(new Style
            {
                Foreground = Color.Red,
            });
        }

        [Fact]
        public void Should_Split_Lines()
        {
            // Given
            var text = Text.FromString("Hello\nWorld", Color.Red);

            // Then
            text.Lines.Count.ShouldBe(2);
            text.Lines[0].Spans[0].Text.ShouldBe("Hello");
            text.Lines[1].Spans[0].Text.ShouldBe("World");
        }

        [Fact]
        public void Should_Not_Set_Style_If_No_Style_Was_Provided()
        {
            // Given
            var text = Text.FromString("Hello World");

            // Then
            text.Style.ShouldBeNull();
        }
    }

    public sealed class TheGetWidthMethod
    {
        [Fact]
        public void Should_Return_Maximum_Line_Width()
        {
            // Given
            var segment = Text.FromString("Hello\nWorld!");

            // When
            var result = segment.GetWidth();

            // Then
            result.ShouldBe(6);
        }
    }
}