namespace Spectre.Tui;

[PublicAPI]
public sealed class Layout : IRatioResolvable
{
    private Layout[] _children;

    public string? Name { get; set; }

    public int Ratio
    {
        get;
        set
        {
            if (value < 1)
            {
                throw new InvalidOperationException("Ratio must be equal to or greater than 1");
            }

            field = value;
        }
    }

    public int MinimumSize
    {
        get;
        set
        {
            if (value < 1)
            {
                throw new InvalidOperationException("Minimum size must be equal to or greater than 1");
            }

            field = value;
        }
    }

    public int? Size
    {
        get => field;
        set
        {
            if (value < 1)
            {
                throw new InvalidOperationException("Size must be equal to or greater than 1");
            }

            field = value;
        }
    }

    public bool IsVisible { get; set; } = true;

    private LayoutSplitter? Splitter { get; set; }

    public Layout(string? name = null)
    {
        _children = [];

        Splitter = null;
        Ratio = 1;
        Size = null;

        Name = name;
    }

    public Rectangle GetArea(IRenderBounds context, string name)
    {
        return GetArea(context.Viewport, name);
    }

    public Rectangle GetArea(Rectangle area, string name)
    {
        var stack = new Stack<(Layout Layout, Rectangle Region)>();
        stack.Push((this, area));

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current.Layout.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false)
            {
                return current.Region;
            }

            if (current.Layout.HasChildren() && current.Layout.Splitter != null)
            {
                foreach (var childAndRegion in current.Layout.Splitter
                             .Divide(current.Region, current.Layout.GetChildren(includeInvisible: false)))
                {
                    stack.Push(childAndRegion);
                }
            }
        }

        throw new InvalidOperationException($"Could not find layout '{name}'");
    }

    public Layout GetLayout(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        var stack = new Stack<Layout>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (name.Equals(current.Name, StringComparison.OrdinalIgnoreCase))
            {
                return current;
            }

            foreach (var layout in current.GetChildren(includeInvisible: true))
            {
                stack.Push(layout);
            }
        }

        throw new InvalidOperationException($"Could not find layout '{name}'");
    }

    public Layout SplitRows(params Layout[] children)
    {
        PerformSplit(LayoutSplitter.Row, children);
        return this;
    }

    public Layout SplitColumns(params Layout[] children)
    {
        PerformSplit(LayoutSplitter.Column, children);
        return this;
    }

    private IEnumerable<Layout> GetChildren(bool includeInvisible)
    {
        if (!includeInvisible)
        {
            return _children.Where(c => c.IsVisible);
        }

        return _children;
    }

    private bool HasChildren()
    {
        return _children.Any(c => c.IsVisible);
    }

    private void PerformSplit(LayoutSplitter splitter, Layout[] layouts)
    {
        if (_children.Length > 0)
        {
            throw new InvalidOperationException("Cannot split the same layout twice");
        }

        Splitter = splitter ?? throw new ArgumentNullException(nameof(splitter));
        _children = layouts ?? throw new ArgumentNullException(nameof(layouts));
    }
}

[PublicAPI]
public static class LayoutExtensions
{
    extension(Layout layout)
    {
        public Layout Ratio(int ratio)
        {
            ArgumentNullException.ThrowIfNull(layout);

            layout.Ratio = ratio;
            return layout;
        }

        public Layout Size(int size)
        {
            ArgumentNullException.ThrowIfNull(layout);

            layout.Size = size;
            return layout;
        }

        public Layout MinimumSize(int size)
        {
            ArgumentNullException.ThrowIfNull(layout);

            layout.MinimumSize = size;
            return layout;
        }

        public Layout Visible(bool visible = true)
        {
            ArgumentNullException.ThrowIfNull(layout);

            layout.IsVisible = visible;
            return layout;
        }

        public Layout Hidden()
        {
            return layout.Visible(false);
        }
    }
}