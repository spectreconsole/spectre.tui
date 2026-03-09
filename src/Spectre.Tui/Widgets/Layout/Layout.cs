namespace Spectre.Tui;

/// <summary>
/// Represents a layout
/// </summary>
[PublicAPI]
public sealed class Layout : IRatioResolvable
{
    private Layout[] _children;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ratio.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>1</c>.
    /// Must be greater than <c>0</c>.
    /// </remarks>
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

    /// <summary>
    /// Gets or sets the minimum width.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>1</c>.
    /// Must be greater than <c>0</c>.
    /// </remarks>
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

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// Must be greater than <c>0</c>.
    /// </remarks>
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

    /// <summary>
    /// Gets or sets a value indicating whether or not the layout should
    /// be visible or not.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool IsVisible { get; set; } = true;

    private LayoutSplitter? Splitter { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Layout"/> class.
    /// </summary>
    /// <param name="name">The layout name.</param>
    public Layout(string? name = null)
    {
        _children = [];

        Splitter = null;
        Ratio = 1;
        Size = null;

        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Rectangle GetArea(RenderContext ctx, string name)
    {
        var stack = new Stack<(Layout Layout, Rectangle Region)>();
        stack.Push((this, ctx.Viewport));

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
                             .Divide(current.Region, current.Layout.GetChildren()))
                {
                    stack.Push(childAndRegion);
                }
            }
        }

        throw new InvalidOperationException($"Could not find layout '{name}'");
    }

    /// <summary>
    /// Gets a child layout by its name.
    /// </summary>
    /// <param name="name">The layout name.</param>
    /// <returns>The specified child <see cref="Layout"/>.</returns>
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

            foreach (var layout in current.GetChildren())
            {
                stack.Push(layout);
            }
        }

        throw new InvalidOperationException($"Could not find layout '{name}'");
    }

    /// <summary>
    /// Splits the layout into rows.
    /// </summary>
    /// <param name="children">The layout to split into rows.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Layout SplitRows(params Layout[] children)
    {
        PerformSplit(LayoutSplitter.Row, children);
        return this;
    }

    /// <summary>
    /// Splits the layout into columns.
    /// </summary>
    /// <param name="children">The layout to split into columns.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Layout SplitColumns(params Layout[] children)
    {
        PerformSplit(LayoutSplitter.Column, children);
        return this;
    }

    private IEnumerable<Layout> GetChildren()
    {
        return _children.Where(c => c.IsVisible);
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

/// <summary>
/// Contains extension methods for <see cref="Layout"/>.
/// </summary>
[PublicAPI]
public static class LayoutExtensions
{
    /// <summary>
    /// Sets the ratio of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="ratio">The ratio.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Ratio(this Layout layout, int ratio)
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Ratio = ratio;
        return layout;
    }

    /// <summary>
    /// Sets the size of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="size">The size.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Size(this Layout layout, int size)
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Size = size;
        return layout;
    }

    /// <summary>
    /// Sets the minimum width of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="size">The size.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout MinimumSize(this Layout layout, int size)
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.MinimumSize = size;
        return layout;
    }
}