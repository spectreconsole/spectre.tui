using Spectre.Tui.App;

namespace Spectre.Tui.Tests;

public sealed class KeyBindingTests
{
    private sealed record TestKey(
        Key Key = Key.None,
        char? Character = null,
        KeyModifier Modifiers = KeyModifier.None) : IKeyInfo;

    public sealed class TheMatchesMethod
    {
        [Fact]
        public void Should_Match_When_Key_Equals_Incoming_Key()
        {
            // Given
            var binding = KeyBinding.For(Key.Enter);

            // When
            var result = binding.Matches(new TestKey(Key.Enter));

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Match_When_Key_Differs()
        {
            // Given
            var binding = KeyBinding.For(Key.Enter);

            // When
            var result = binding.Matches(new TestKey(Key.Escape));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Match_When_Character_Equals_Incoming_Character()
        {
            // Given
            var binding = KeyBinding.For('q');

            // When
            var result = binding.Matches(new TestKey(Character: 'q'));

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Match_When_Character_Differs()
        {
            // Given
            var binding = KeyBinding.For('q');

            // When
            var result = binding.Matches(new TestKey(Character: 'Q'));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Match_When_Any_Of_Multiple_Keys_Match()
        {
            // Given
            var binding = KeyBinding.For(Key.Up, Key.Down);

            // When
            var result = binding.Matches(new TestKey(Key.Down));

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Match_When_Modifiers_Match_Exactly()
        {
            // Given
            var binding = KeyBinding.For(KeyPress.For('q').WithCtrl());

            // When
            var result = binding.Matches(new TestKey(Character: 'q', Modifiers: KeyModifier.Ctrl));

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Match_When_Required_Modifier_Missing()
        {
            // Given
            var binding = KeyBinding.For(KeyPress.For('q').WithCtrl());

            // When
            var result = binding.Matches(new TestKey(Character: 'q'));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Match_When_Extra_Modifier_Present()
        {
            // Given
            var binding = KeyBinding.For('q');

            // When
            var result = binding.Matches(new TestKey(Character: 'q', Modifiers: KeyModifier.Ctrl));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Match_When_Modifier_Superset()
        {
            // Given
            var binding = KeyBinding.For(KeyPress.For('q').WithCtrl());

            // When
            var result = binding.Matches(
                new TestKey(Character: 'q', Modifiers: KeyModifier.Ctrl | KeyModifier.Shift));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Match_When_Disabled()
        {
            // Given
            var binding = KeyBinding.For(KeyPress.For(Key.Enter)).Disabled();

            // When
            var result = binding.Matches(new TestKey(Key.Enter));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Match_When_Keys_Empty()
        {
            // Given
            var binding = new KeyBinding { Keys = [] };

            // When
            var result = binding.Matches(new TestKey(Key.Enter));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Match_When_KeyPress_Has_Neither_Key_Nor_Character()
        {
            // Given
            var binding = KeyBinding.For(new KeyPress { Modifiers = KeyModifier.Ctrl });

            // When
            var result = binding.Matches(new TestKey(Modifiers: KeyModifier.Ctrl));

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Match_When_Both_Key_And_Character_Match_On_Same_KeyPress()
        {
            // Given
            var binding = KeyBinding.For(new KeyPress { Key = Key.Enter, Character = '\n' });

            // When
            var result = binding.Matches(new TestKey(Key.Enter, Character: '\n'));

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Match_When_Both_Set_But_Character_Differs()
        {
            // Given
            var binding = KeyBinding.For(new KeyPress { Key = Key.Enter, Character = '\r' });

            // When
            var result = binding.Matches(new TestKey(Key.Enter, Character: '\n'));

            // Then
            result.ShouldBeFalse();
        }
    }

    public sealed class TheCombineMethod
    {
        [Fact]
        public void Should_Union_Keys()
        {
            // Given
            var up = KeyBinding.For(Key.Up);
            var down = KeyBinding.For(Key.Down);

            // When
            var combined = KeyBinding.Combine(up, down);

            // Then
            combined.Keys.Count.ShouldBe(2);
            combined.Keys[0].Key.ShouldBe(Key.Up);
            combined.Keys[1].Key.ShouldBe(Key.Down);
        }

        [Fact]
        public void Should_Default_To_Empty_Help()
        {
            // Given
            var up = KeyBinding.For(Key.Up).WithHelp("Move up");
            var down = KeyBinding.For(Key.Down).WithHelp("Move down");

            // When
            var combined = KeyBinding.Combine(up, down);

            // Then
            combined.Help.ShouldBe(string.Empty);
        }

        [Fact]
        public void Should_Apply_WithHelp_To_Combined_Binding()
        {
            // Given
            var up = KeyBinding.For(Key.Up);
            var down = KeyBinding.For(Key.Down);

            // When
            var combined = KeyBinding.Combine(up, down).WithHelp("Move");

            // Then
            combined.Help.ShouldBe("Move");
        }

        [Fact]
        public void Should_Be_Enabled_When_All_Inputs_Enabled()
        {
            // Given
            var up = KeyBinding.For(Key.Up);
            var down = KeyBinding.For(Key.Down);

            // When
            var combined = KeyBinding.Combine(up, down);

            // Then
            combined.Enabled.ShouldBeTrue();
        }

        [Fact]
        public void Should_Be_Disabled_When_Any_Input_Disabled()
        {
            // Given
            var up = KeyBinding.For(Key.Up).Disabled();
            var down = KeyBinding.For(Key.Down);

            // When
            var combined = KeyBinding.Combine(up, down);

            // Then
            combined.Enabled.ShouldBeFalse();
        }

        [Fact]
        public void Should_Match_Any_Constituent_Key()
        {
            // Given
            var combined = KeyBinding.Combine(
                KeyBinding.For(Key.Up),
                KeyBinding.For(Key.Down));

            // When
            var matchesUp = combined.Matches(new TestKey(Key.Up));
            var matchesDown = combined.Matches(new TestKey(Key.Down));
            var matchesLeft = combined.Matches(new TestKey(Key.Left));

            // Then
            matchesUp.ShouldBeTrue();
            matchesDown.ShouldBeTrue();
            matchesLeft.ShouldBeFalse();
        }

        [Fact]
        public void Should_Allow_Empty_Input()
        {
            // Given, When
            var combined = KeyBinding.Combine();

            // Then
            combined.Keys.ShouldBeEmpty();
            combined.Enabled.ShouldBeTrue();
        }
    }

    public sealed class TheOrderProperty
    {
        [Fact]
        public void Should_Default_To_Zero()
        {
            // Given, When
            var binding = KeyBinding.For(Key.Up);

            // Then
            binding.Order.ShouldBe(0);
        }

        [Fact]
        public void Should_Be_Set_By_Order_Extension()
        {
            // Given
            var binding = KeyBinding.For(Key.Up);

            // When
            var ordered = binding.Order(5);

            // Then
            ordered.Order.ShouldBe(5);
        }

        [Fact]
        public void Should_Allow_Negative_Order()
        {
            // Given
            var binding = KeyBinding.For(Key.Up);

            // When
            var ordered = binding.Order(-3);

            // Then
            ordered.Order.ShouldBe(-3);
        }
    }
}
