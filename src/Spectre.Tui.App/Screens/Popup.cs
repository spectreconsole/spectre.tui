namespace Spectre.Tui.App;

[PublicAPI]
public abstract class Popup : Screen
{
    public sealed override bool IsTransparent => true;
}