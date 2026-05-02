namespace Spectre.Tui.App;

[PublicAPI]
public abstract class Screen
{
    public virtual bool IsTransparent => false;

    public virtual void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
    }

    public virtual void OnEnter(ApplicationContext context)
    {
    }

    public virtual void OnLeave(ApplicationContext context)
    {
    }

    public abstract void Render(RenderContext context, FrameInfo frame);

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }
}