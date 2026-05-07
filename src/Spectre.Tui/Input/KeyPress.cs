namespace Spectre.Tui.App;

[PublicAPI]
public record KeyPress : IKeyInfo
{
    public Key Key { get; init; }
    public char? Character { get; init; }
    public KeyModifier Modifiers { get; init; }

    public static KeyPress For(Key key)
    {
        return new KeyPress
        {
            Key = key,
        };
    }

    public static KeyPress For(char character)
    {
        return new KeyPress
        {
            Character = character,
        };
    }
}

[PublicAPI]
public static class KeyPressExtensions
{
    extension(KeyPress binding)
    {
        public KeyPress WithCtrl(bool value = true)
        {
            return binding with
            {
                Modifiers = binding.Modifiers | KeyModifier.Ctrl,
            };
        }

        public KeyPress WithAlt(bool value = true)
        {
            return binding with
            {
                Modifiers = binding.Modifiers | KeyModifier.Alt,
            };
        }

        public KeyPress WithShift(bool value = true)
        {
            return binding with
            {
                Modifiers = binding.Modifiers | KeyModifier.Shift,
            };
        }

        public KeyPress WithoutModifiers()
        {
            return binding with
            {
                Modifiers = KeyModifier.None,
            };
        }
    }
}