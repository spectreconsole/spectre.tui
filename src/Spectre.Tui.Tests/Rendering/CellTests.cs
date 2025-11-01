using System.Text;
using Shouldly;

namespace Spectre.Tui.Tests.Rendering;

public sealed class CellTests
{
    public sealed class EmptyConstructor
    {
        [Fact]
        public void Should_Have_Space_Rune()
        {
            // Given, When
            var cell = new Cell();

            // Then
            cell.Rune.ShouldBe(new Rune(' '));
        }

        [Fact]
        public void Should_Have_No_Decoration()
        {
            // Given, When
            var cell = new Cell();

            // Then
            cell.Decoration.ShouldBe(Decoration.None);
        }
    }
}