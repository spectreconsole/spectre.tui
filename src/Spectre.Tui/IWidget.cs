namespace Spectre.Tui;

[PublicAPI]
public interface IWidget
{
    void Render(RenderContext context);
}