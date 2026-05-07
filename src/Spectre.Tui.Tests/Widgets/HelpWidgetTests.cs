using Spectre.Tui.App;

namespace Spectre.Tui.Tests;

public sealed class HelpWidgetTests
{
    private sealed class TestKeyMap(params KeyBinding[] bindings) : IKeyMap
    {
        public IEnumerable<KeyBinding> Help() => bindings;
    }

    public sealed class TheRenderMethod
    {
        [Fact]
        public void Should_Render_Single_Binding_With_Help()
        {
            // Given
            var keyMap = new TestKeyMap(KeyBinding.For(Key.Up).WithHelp("Move up"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Move up•••••••••");
        }

        [Fact]
        public void Should_Render_Key_Only_When_Help_Empty()
        {
            // Given
            var keyMap = new TestKeyMap(KeyBinding.For(Key.Up));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(10, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]•••••••");
        }

        [Fact]
        public void Should_Join_Multiple_Bindings_With_Double_Space()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up"),
                KeyBinding.For(Key.Down).WithHelp("Down"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Up  [↓]:Down••••");
        }

        [Fact]
        public void Should_Join_Alternative_Keys_With_Slash()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up, Key.Down).WithHelp("Navigate"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑/↓]:Navigate••••••");
        }

        [Fact]
        public void Should_Render_Character_Keys_Uppercased()
        {
            // Given
            var keyMap = new TestKeyMap(KeyBinding.For('q').WithHelp("Quit"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(15, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Q]:Quit•••••••");
        }

        [Fact]
        public void Should_Render_Modifier_Prefix()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(KeyPress.For('s').WithCtrl()).WithHelp("Save"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Ctrl+S]:Save•••••••");
        }

        [Fact]
        public void Should_Render_Multiple_Modifiers_In_Order()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(KeyPress.For(Key.Tab).WithCtrl().WithShift()).WithHelp("Switch"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(25, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Ctrl+Shift+Tab]:Switch••");
        }

        [Fact]
        public void Should_Render_Named_Keys()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.PageUp).WithHelp("Up"),
                KeyBinding.For(Key.PageDown).WithHelp("Dn"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(25, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[PgUp]:Up  [PgDn]:Dn•••••");
        }

        [Fact]
        public void Should_Skip_Disabled_Bindings()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up").Disabled(),
                KeyBinding.For(Key.Down).WithHelp("Down"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(15, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↓]:Down•••••••");
        }

        [Fact]
        public void Should_Merge_Multiple_KeyMaps()
        {
            // Given
            var first = new TestKeyMap(KeyBinding.For('q').WithHelp("Quit"));
            var second = new TestKeyMap(KeyBinding.For(Key.Up).WithHelp("Up"));
            var widget = new HelpWidget(first, second).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Q]:Quit  [↑]:Up••••");
        }

        [Fact]
        public void Should_Render_Nothing_When_No_KeyMaps()
        {
            // Given
            var widget = new HelpWidget();
            var fixture = new TuiFixture(new Size(8, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("••••••••");
        }

        [Fact]
        public void Should_Render_Nothing_When_All_Bindings_Disabled()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up").Disabled(),
                KeyBinding.For(Key.Down).WithHelp("Down").Disabled());
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(8, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("••••••••");
        }

        [Fact]
        public void Should_Honor_Custom_Separator()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up"),
                KeyBinding.For(Key.Down).WithHelp("Dn"));
            var widget = new HelpWidget(keyMap).LeftAligned().Separator(" | ");
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Up | [↓]:Dn•••••");
        }

        [Fact]
        public void Should_Center_When_Centered()
        {
            // Given
            var keyMap = new TestKeyMap(KeyBinding.For(Key.Up).WithHelp("Up"));
            var widget = new HelpWidget(keyMap).Centered();
            var fixture = new TuiFixture(new Size(13, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("•••[↑]:Up••••");
        }

        [Fact]
        public void Should_Truncate_With_Ellipsis_When_Output_Exceeds_Width()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For('q').WithHelp("Quit"),
                KeyBinding.For('b').WithHelp("Popup"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(10, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Q]:Quit …");
        }

        [Fact]
        public void Should_Not_Truncate_When_Output_Fits()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For('q').WithHelp("Quit"),
                KeyBinding.For('b').WithHelp("Popup"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[Q]:Quit  [B]:Popup•");
        }

        [Fact]
        public void Should_Render_Combined_Binding_As_Single_Entry()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.Combine(
                    KeyBinding.For(Key.Up),
                    KeyBinding.For(Key.Down)).WithHelp("Move"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(15, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑/↓]:Move•••••");
        }

        [Fact]
        public void Should_Sort_Bindings_By_Order_Ascending()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For('q').WithHelp("Quit").Order(10),
                KeyBinding.For(Key.Up).WithHelp("Up").Order(0));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Up  [Q]:Quit••••");
        }

        [Fact]
        public void Should_Preserve_Yield_Order_For_Equal_Order_Values()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up"),
                KeyBinding.For(Key.Down).WithHelp("Down"),
                KeyBinding.For(Key.Left).WithHelp("Left"));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(30, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Up  [↓]:Down  [←]:Left••••");
        }

        [Fact]
        public void Should_Sort_Across_Multiple_KeyMaps()
        {
            // Given
            var first = new TestKeyMap(KeyBinding.For('q').WithHelp("Quit").Order(99));
            var second = new TestKeyMap(KeyBinding.For(Key.Up).WithHelp("Up"));
            var widget = new HelpWidget(first, second).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[↑]:Up  [Q]:Quit••••");
        }

        [Fact]
        public void Should_Sort_Negative_Order_To_Front()
        {
            // Given
            var keyMap = new TestKeyMap(
                KeyBinding.For(Key.Up).WithHelp("Up"),
                KeyBinding.For('h').WithHelp("Help").Order(-1));
            var widget = new HelpWidget(keyMap).LeftAligned();
            var fixture = new TuiFixture(new Size(20, 1));

            // When
            var result = fixture.Render(widget);

            // Then
            result.ShouldBe("[H]:Help  [↑]:Up••••");
        }
    }
}
