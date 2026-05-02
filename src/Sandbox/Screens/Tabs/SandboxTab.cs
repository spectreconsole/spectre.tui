namespace Sandbox;

public abstract class SandboxTab : IWidget
{
    public abstract string TabLabel { get; }
    public abstract string HelpMarkup { get; }

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }

    public virtual void OnEvent(ApplicationContext context, ApplicationMessage e)
    {
    }

    public abstract void Render(RenderContext context);
}