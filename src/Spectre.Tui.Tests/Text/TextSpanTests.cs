namespace Spectre.Tui.Tests;

public sealed class TextSpanTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Set_Text()
        {
            // Given
            var span = new TextSpan("Hello World", Color.Red);

            // Then
            span.Text.ShouldBe("Hello World");
        }

        [Fact]
        public void Should_Set_Style()
        {
            // Given
            var span = new TextSpan("Hello World", Color.Red);

            // Then
            span.Style.ShouldBe(new Style
            {
                Foreground = Color.Red
            });
        }

        [Fact]
        public void Should_Join_Lines_To_Single_One()
        {
            // Given
            var span = new TextSpan("Hello\nWorld", Color.Red);

            // Then
            span.Text.ShouldBe("HelloWorld");
        }

        [Fact]
        public void Should_Set_Style_To_Null_If_No_Style_Was_Provided()
        {
            // Given
            var span = new TextSpan("Hello World");

            // Then
            span.Style.ShouldBeNull();
        }
    }

    public sealed class TheGetWidthMethod
    {
        [Fact]
        public void Should_Return_Cell_Width()
        {
            // Given
            var span = new TextSpan("Hello World");

            // When
            var result = span.GetWidth();

            // Then
            result.ShouldBe(11);
        }
    }
}