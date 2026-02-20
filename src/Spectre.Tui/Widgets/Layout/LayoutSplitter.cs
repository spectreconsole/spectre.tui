namespace Spectre.Tui;

internal abstract class LayoutSplitter
{
    public static LayoutSplitter Column { get; } = new ColumnSplitter();
    public static LayoutSplitter Row { get; } = new RowSplitter();

    public IEnumerable<(Layout Child, Rectangle Region)> Divide(Rectangle region, IEnumerable<Layout> layouts)
    {
        return Divide(region, layouts.ToList());
    }

    protected abstract IEnumerable<(Layout Child, Rectangle Region)> Divide(Rectangle region, List<Layout> children);

    private sealed class ColumnSplitter : LayoutSplitter
    {
        protected override IEnumerable<(Layout Child, Rectangle Region)> Divide(Rectangle region, List<Layout> children)
        {
            var widths = RatioResolver.Resolve(region.Width, children);
            var offset = 0;

            foreach (var (child, childWidth) in children.Zip(widths, (child, width) => (child, width)))
            {
                yield return (child, new Rectangle(region.X + offset, region.Y, childWidth, region.Height));
                offset += childWidth;
            }
        }
    }

    private sealed class RowSplitter : LayoutSplitter
    {
        protected override IEnumerable<(Layout Child, Rectangle Region)> Divide(Rectangle region, List<Layout> children)
        {
            var heights = RatioResolver.Resolve(region.Height, children);
            var offset = 0;

            foreach (var (child, childHeight) in children.Zip(heights, (child, height) => (child, height)))
            {
                yield return (child, new Rectangle(region.X, region.Y + offset, region.Width, childHeight));
                offset += childHeight;
            }
        }
    }
}