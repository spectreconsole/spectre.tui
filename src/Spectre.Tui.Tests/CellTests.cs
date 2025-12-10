using Shouldly;

namespace Spectre.Tui.Tests;

public sealed class CellTests
{
    public sealed class EmptyConstructor
    {
        [Fact]
        public void Should_Have_Default_Rune()
        {
            // Given, When
            var cell = new Cell();

            // Then
            cell.Rune.ShouldBe(default);
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