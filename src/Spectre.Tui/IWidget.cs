namespace Spectre.Tui;

[PublicAPI]
public interface IWidget
{
    void Render(RenderContext context);
}

[PublicAPI]
public interface IStatefulWidget<in TState>
{
    void Render(RenderContext context, TState state);
}