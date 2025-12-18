namespace Spectre.Tui.Tests;

public sealed class RendererTests
{
    private sealed class TestTextWidget(int x, int y, string text) : IWidget
    {
        public void Render(RenderContext context)
        {
            for (var i = 0; i < text.Length; i++)
            {
                context.SetSymbol(x + i, y, text[i]);
            }
        }
    }

    [Fact]
    public void Should_Render_Buffer()
    {
        // Given
        var fixture = new TuiFixture(
            new Size(11, 5));

        // When
        var result = fixture.Render(frame =>
        {
            frame.Render(new TestTextWidget(3, 2, "Hello"));
        });

        // Then
        result.ShouldBe(
            """
            •••••••••••
            •••••••••••
            •••Hello•••
            •••••••••••
            •••••••••••
            """);
    }

    [Fact]
    public void Should_Only_Render_Diff_Between_Frames()
    {
        // Given
        var fixture = new TuiFixture(new Size(11, 5));
        fixture.Render(new TestTextWidget(3, 1, "Hello"));

        // When
        var result = fixture.Render(frame =>
        {
            frame.Render(new TestTextWidget(3, 1, "Hello"));
            frame.Render(new TestTextWidget(3, 2, "World"));
        });

        // Then
        result.ShouldBe(
            """
            •••••••••••
            •••••••••••
            •••World•••
            •••••••••••
            •••••••••••
            """);
    }
}