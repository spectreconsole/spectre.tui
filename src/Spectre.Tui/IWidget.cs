namespace Spectre.Tui;

public interface IWidget
{
    void Render(Region area, Buffer buffer);
}

public interface IWidget<in TState>
{
    void Render(Region area, TState state, Buffer buffer);
}