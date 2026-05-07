namespace Sandbox;

public abstract class SandboxTab : IWidget, IKeyMap
{
    public abstract string TabLabel { get; }

    public virtual IEnumerable<KeyBinding> Help()
    {
        yield break;
    }

    public virtual void OnMessage(ApplicationContext context, ApplicationMessage message)
    {
    }

    public virtual void Update(FrameInfo frame, IRenderBounds bounds)
    {
    }

    public abstract void Render(RenderContext context);
}
