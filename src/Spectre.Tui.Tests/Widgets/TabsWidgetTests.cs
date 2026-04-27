namespace Spectre.Tui.Tests;

public sealed class TabsWidgetTests
{
    private sealed class TestTabItem(string title) : ITabWidgetItem
    {
        public TextLine CreateTextLine(bool isSelected)
        {
            return title;
        }
    }

    private sealed class MarkedTabItem(string title) : ITabWidgetItem
    {
        public TextLine CreateTextLine(bool isSelected)
        {
            return isSelected ? $"<{title}>" : title;
        }
    }

    [Fact]
    public void Should_Skip_LeftPadding_On_First_Item()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var tabs = new TabsWidget<TestTabItem>(
            new TestTabItem("Foo"),
            new TestTabItem("Bar"),
            new TestTabItem("Baz"));

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•|•Bar•|•Baz•••••");
    }

    [Fact]
    public void Should_Render_Single_Item_Without_Separator()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var tabs = new TabsWidget<TestTabItem>(new TestTabItem("Foo"));

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•••••••");
    }

    [Fact]
    public void Should_Render_Nothing_When_Empty()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 1));
        var tabs = new TabsWidget<TestTabItem>();

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("••••••••••");
    }

    [Fact]
    public void Should_Truncate_With_Ellipsis_When_Title_Does_Not_Fit()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 1));
        var tabs = new TabsWidget<TestTabItem>(
            new TestTabItem("Foo"),
            new TestTabItem("Bar"),
            new TestTabItem("Baz"),
            new TestTabItem("Qux"));

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•|•B…");
    }

    [Fact]
    public void Should_Render_Ellipsis_When_Next_Tab_Cannot_Fit()
    {
        // Given
        var fixture = new TuiFixture(new Size(12, 1));
        var tabs = new TabsWidget<TestTabItem>(
            new TestTabItem("Foo"),
            new TestTabItem("Bar"),
            new TestTabItem("Baz"));

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•|•Bar•|…");
    }

    [Fact]
    public void Should_Render_Ellipsis_When_Last_Tab_Title_Cannot_Fit()
    {
        // Given
        var fixture = new TuiFixture(new Size(13, 1));
        var tabs = new TabsWidget<TestTabItem>(
            new TestTabItem("Foo"),
            new TestTabItem("Bar"),
            new TestTabItem("Baz"));

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•|•Bar•|•…");
    }

    [Fact]
    public void Should_Render_Custom_Separator_And_Empty_Padding()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var tabs = new TabsWidget<TestTabItem>(
                new TestTabItem("Foo"),
                new TestTabItem("Bar"),
                new TestTabItem("Baz"))
            .LeftPadding(string.Empty)
            .RightPadding(string.Empty)
            .Separator("/");

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo/Bar/Baz•••••••••");
    }

    [Fact]
    public void Should_Wrap_MoveRight_From_Last_To_First_When_WrapAround()
    {
        // Given
        var tabs = new TabsWidget<TestTabItem>(
                new TestTabItem("Foo"),
                new TestTabItem("Bar"),
                new TestTabItem("Baz"))
            .WrapAround()
            .SelectedIndex(2);

        // When
        tabs.MoveRight();

        // Then
        tabs.SelectedIndex.ShouldBe(0);
    }

    [Fact]
    public void Should_Clamp_MoveLeft_At_Start_When_Not_WrapAround()
    {
        // Given
        var tabs = new TabsWidget<TestTabItem>(
                new TestTabItem("Foo"),
                new TestTabItem("Bar"),
                new TestTabItem("Baz"))
            .SelectedIndex(0);

        // When
        tabs.MoveLeft();

        // Then
        tabs.SelectedIndex.ShouldBe(0);
    }

    [Fact]
    public void Should_Pass_IsSelected_Flag_To_Selected_Tab_Item()
    {
        // Given
        var fixture = new TuiFixture(new Size(20, 1));
        var tabs = new TabsWidget<MarkedTabItem>(
                new MarkedTabItem("Foo"),
                new MarkedTabItem("Bar"),
                new MarkedTabItem("Baz"))
            .SelectedIndex(1);

        // When
        var result = fixture.Render(tabs);

        // Then
        result.ShouldBe("Foo•|•<Bar>•|•Baz•••");
    }

    [Fact]
    public void Should_Throw_When_SelectedItem_Accessed_With_Empty_Items()
    {
        // Given
        var tabs = new TabsWidget<TestTabItem>();

        // When
        var result = Record.Exception(() => tabs.SelectedItem);

        // Then
        result.ShouldBeOfType<InvalidOperationException>();
    }
}
