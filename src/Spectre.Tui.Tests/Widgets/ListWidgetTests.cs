namespace Spectre.Tui.Tests;

public sealed class ListWidgetTests
{
    private sealed class TestItem(string text) : IListWidgetItem
    {
        Text IListWidgetItem.CreateText(bool isSelected)
        {
            return Text.FromString(text);
        }
    }

    private sealed class JitWrapper(IWidget inner) : JustInTimeWidget
    {
        protected override void RenderDirty(RenderContext context)
        {
            context.Render(inner);
        }
    }

    [Fact]
    public void Should_Scroll_When_Selection_Is_Past_Visible_Area()
    {
        // Given
        var list = new ListWidget<TestItem>(
        [
            new TestItem("A"),
            new TestItem("B"),
            new TestItem("C"),
            new TestItem("D"),
            new TestItem("E"),
        ]).SelectedIndex(4);

        var fixture = new TuiFixture(new Size(3, 3));

        // When
        var result = fixture.Render(list);

        // Then
        result.ShouldBe(
            """
            C••
            D••
            E••
            """);
    }

    [Fact]
    public void Should_Scroll_When_Wrapped_In_JustInTimeWidget()
    {
        // Given
        var list = new ListWidget<TestItem>(
        [
            new TestItem("A"),
            new TestItem("B"),
            new TestItem("C"),
            new TestItem("D"),
            new TestItem("E"),
        ]).SelectedIndex(4);

        var fixture = new TuiFixture(new Size(3, 3));

        // When
        var result = fixture.Render(new JitWrapper(list));

        // Then
        result.ShouldBe(
            """
            C••
            D••
            E••
            """);
    }
}
