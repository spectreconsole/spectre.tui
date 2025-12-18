namespace Spectre.Tui.Tests;

public sealed class TextLineTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Set_Text()
        {
            // Given, When
            var line = TextLine.FromString("Hello World", Color.Red);

            // Then
            line.Spans.Count.ShouldBe(1);
            line.Spans[0].Text.ShouldBe("Hello World");
        }

        [Fact]
        public void Should_Set_Style()
        {
            // Given, When
            var line = TextLine.FromString("Hello\nWorld", Color.Red);

            // Then
            line.Style.ShouldBe(new Style
            {
                Foreground = Color.Red,
            });
        }

        [Fact]
        public void Should_Join_Lines_To_Single_One()
        {
            // Given
            var line = TextLine.FromString("Hello\nWorld", Color.Red);

            // Then
            line.Spans.Count.ShouldBe(1);
            line.Spans[0].Text.ShouldBe("HelloWorld");
        }

        [Fact]
        public void Should_Not_Set_Style_If_No_Style_Was_Provided()
        {
            // Given
            var line = TextLine.FromString("Hello World");

            // Then
            line.Style.ShouldBe(default);
        }
    }

    public sealed class TheGetWidthMethod
    {
        [Fact]
        public void Should_Return_Cell_Width()
        {
            // Given
            var segment = TextLine.FromString("Hello World");

            // When
            var result = segment.GetWidth();

            // Then
            result.ShouldBe(11);
        }
    }
}