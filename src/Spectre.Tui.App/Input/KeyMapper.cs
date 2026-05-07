namespace Spectre.Tui.App;

internal static class KeyMapper
{
    public static KeyMessage Map(ConsoleKeyInfo info)
    {
        var character = info.KeyChar != '\0' ? (char?)info.KeyChar : null;
        var key = character != null && !char.IsControl(character.Value) && !char.IsWhiteSpace(character.Value)
            ? Key.Character
            : MapKey(info);

        return new KeyMessage
        {
            Key = key,
            Character = character,
            Modifiers = (KeyModifier)info.Modifiers,
        };
    }

    private static Key MapKey(ConsoleKeyInfo info)
    {
        return info.Key switch
        {
            ConsoleKey.Backspace => Key.Backspace,
            ConsoleKey.Tab => Key.Tab,
            ConsoleKey.Enter => Key.Enter,
            ConsoleKey.Escape => Key.Escape,
            ConsoleKey.Spacebar => Key.Space,
            ConsoleKey.PageUp => Key.PageUp,
            ConsoleKey.PageDown => Key.PageDown,
            ConsoleKey.End => Key.End,
            ConsoleKey.Home => Key.Home,
            ConsoleKey.LeftArrow => Key.Left,
            ConsoleKey.UpArrow => Key.Up,
            ConsoleKey.RightArrow => Key.Right,
            ConsoleKey.DownArrow => Key.Down,
            ConsoleKey.Insert => Key.Insert,
            ConsoleKey.Delete => Key.Delete,
            ConsoleKey.F1 => Key.F1,
            ConsoleKey.F2 => Key.F2,
            ConsoleKey.F3 => Key.F3,
            ConsoleKey.F4 => Key.F4,
            ConsoleKey.F5 => Key.F5,
            ConsoleKey.F6 => Key.F6,
            ConsoleKey.F7 => Key.F7,
            ConsoleKey.F8 => Key.F8,
            ConsoleKey.F9 => Key.F9,
            ConsoleKey.F10 => Key.F10,
            ConsoleKey.F11 => Key.F11,
            ConsoleKey.F12 => Key.F12,
            ConsoleKey.F13 => Key.F13,
            ConsoleKey.F14 => Key.F14,
            ConsoleKey.F15 => Key.F15,
            ConsoleKey.F16 => Key.F16,
            ConsoleKey.F17 => Key.F17,
            ConsoleKey.F18 => Key.F18,
            ConsoleKey.F19 => Key.F19,
            ConsoleKey.F20 => Key.F20,
            ConsoleKey.F21 => Key.F21,
            ConsoleKey.F22 => Key.F22,
            ConsoleKey.F23 => Key.F23,
            ConsoleKey.F24 => Key.F24,
            _ => Key.None,
        };
    }
}