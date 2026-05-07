namespace Spectre.Tui.App;

[PublicAPI]
public record KeyMessage : ApplicationMessage, IKeyInfo
{
    public Key Key { get; init; }
    public char? Character { get; init; }
    public KeyModifier Modifiers { get; init; }
}