using Spectre.Tui.App;

namespace Spectre.Tui;

[PublicAPI]
public interface IKeyInfo
{
    Key Key { get; }
    char? Character { get; }
    KeyModifier Modifiers { get; }
}

public static class IKeyInfoExtensions
{
    extension(IKeyInfo key)
    {
        public bool Ctrl => (key.Modifiers & KeyModifier.Ctrl) != 0;
        public bool Alt => (key.Modifiers & KeyModifier.Alt) != 0;
        public bool Shift => (key.Modifiers & KeyModifier.Shift) != 0;
    }
}