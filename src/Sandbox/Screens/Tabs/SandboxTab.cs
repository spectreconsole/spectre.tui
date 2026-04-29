namespace Sandbox;

public abstract class SandboxTab
{
    public abstract string TabLabel { get; }
    public abstract string HelpMarkup { get; }

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }

    public virtual void OnEvent(ApplicationEvent evt, ApplicationContext ctx)
    {
    }

    public abstract void Render(RenderContext ctx, Rectangle area, FrameInfo frame);
}