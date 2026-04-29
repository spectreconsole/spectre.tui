namespace Spectre.Tui.App;

[PublicAPI]
public abstract class Screen
{
    public virtual bool IsTransparent => false;

    public virtual void OnEvent(ApplicationContext context, ApplicationEvent evt)
    {
    }

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }

    public abstract void Render(RenderContext context, FrameInfo frame);

    public virtual void OnEnter(ApplicationContext context)
    {
    }

    public virtual void OnLeave(ApplicationContext ctx)
    {
    }
}