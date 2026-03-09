namespace Spectre.Tui;

internal interface IRatioResolvable
{
    int Ratio { get; }
    int? Size { get; }
    int MinimumSize { get; }
}

internal static class RatioResolver
{
    public static List<int> Resolve(int total, IReadOnlyList<IRatioResolvable> edges)
    {
        var sizes = edges.Select(GetEdgeWidth).ToArray();

        while (sizes.Any(s => s == null))
        {
            // Get all edges and map them back to their index.
            // Ignore edges which have a explicit size.
            var flexibleEdges = sizes.Zip(edges, (a, b) => (Size: a, Edge: b))
                .Enumerate()
                .Select(x => (x.Index, x.Item.Size, x.Item.Edge))
                .Where(x => x.Size == null)
                .ToList();

            // Get the remaining space
            var remaining = total - sizes.Sum(size => size ?? 0);
            if (remaining <= 0)
            {
                // No more room for flexible edges.
                return sizes
                    .Zip(edges, (size, edge) => (Size: size, Edge: edge))
                    .Select(zip => zip.Size ?? zip.Edge.MinimumSize)
                    .Select(size => size > 0 ? size : 1)
                    .ToList();
            }

            var portion = (float)remaining / flexibleEdges
                .Sum(x => Math.Max(1, x.Edge.Ratio));

            var invalidate = false;
            foreach (var (index, size, edge) in flexibleEdges)
            {
                if (portion * edge.Ratio <= edge.MinimumSize)
                {
                    sizes[index] = edge.MinimumSize;

                    // New fixed size will invalidate calculations,
                    // so we need to repeat the process
                    invalidate = true;
                    break;
                }
            }

            if (!invalidate)
            {
                var remainder = 0f;
                foreach (var flexibleEdge in flexibleEdges)
                {
                    var (div, mod) = DivMod((portion * flexibleEdge.Edge.Ratio) + remainder, 1);
                    remainder = mod;
                    sizes[flexibleEdge.Index] = div;
                }
            }
        }

        return sizes.Select(x => x ?? 1).ToList();

        static (int Div, float Mod) DivMod(float x, float y)
        {
            var (div, mod) = ((int)(x / y), x % y);
            if (!(mod > 0.9999))
            {
                return (div, mod);
            }

            div++;
            mod = 0;
            return (div, mod);
        }

        static int? GetEdgeWidth(IRatioResolvable edge)
        {
            if (edge.Size != null && edge.Size < edge.MinimumSize)
            {
                return edge.MinimumSize;
            }

            return edge.Size;
        }
    }
}