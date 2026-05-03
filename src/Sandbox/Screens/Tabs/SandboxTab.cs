namespace Sandbox;

public abstract class SandboxTab : IWidget
{
    public abstract string TabLabel { get; }
    public abstract string HelpMarkup { get; }

    public virtual void OnMessage(ApplicationContext context, ApplicationMessage e)
    {
    }

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }

    public abstract void Render(RenderContext context);
}