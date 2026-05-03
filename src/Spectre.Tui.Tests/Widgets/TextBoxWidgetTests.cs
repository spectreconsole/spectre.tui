namespace Spectre.Tui.Tests;

public sealed class TextBoxWidgetTests
{
    public sealed class TheInsertMethod
    {
        [Fact]
        public void Should_Append_Text_When_Cursor_At_End()
        {
            // Given
            var box = new TextBoxWidget();

            // When
            box.Insert("Hello");

            // Then
            box.Text.ShouldBe("Hello");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }

        [Fact]
        public void Should_Insert_At_Cursor_When_Not_At_End()
        {
            // Given
            var box = new TextBoxWidget().Text("Hllo");
            box.MoveToStart();
            box.MoveRight();

            // When
            box.Insert("e");

            // Then
            box.Text.ShouldBe("Hello");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 2));
        }

        [Fact]
        public void Should_Strip_Newlines_When_SingleLine()
        {
            // Given
            var box = new TextBoxWidget();

            // When
            box.Insert("Hello\nWorld");

            // Then
            box.Text.ShouldBe("HelloWorld");
        }

        [Fact]
        public void Should_Split_Lines_On_Newline_When_MultiLine()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine();

            // When
            box.Insert("Hello\nWorld");

            // Then
            box.Text.ShouldBe("Hello\nWorld");
            box.Cursor.ShouldBe(new TextBoxPosition(1, 5));
        }

        [Fact]
        public void Should_Stop_At_MaxLength()
        {
            // Given
            var box = new TextBoxWidget().MaxLength(3);

            // When
            box.Insert("Hello");

            // Then
            box.Text.ShouldBe("Hel");
            box.Length.ShouldBe(3);
        }

        [Fact]
        public void Should_Be_NoOp_When_ReadOnly()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello").ReadOnly();
            box.MoveToEnd();

            // When
            box.Insert(" World");

            // Then
            box.Text.ShouldBe("Hello");
        }

        [Fact]
        public void Should_Combine_Surrogate_Pair_Into_Single_Grapheme()
        {
            // Given
            var box = new TextBoxWidget();

            // When
            box.Insert("\uD83D");
            box.Insert("\uDE00");

            // Then
            box.Text.ShouldBe("😀");
            box.Length.ShouldBe(1);
            box.Cursor.ShouldBe(new TextBoxPosition(0, 1));
        }

        [Fact]
        public void Should_Combine_Combining_Mark_Into_Single_Grapheme()
        {
            // Given
            var box = new TextBoxWidget();

            // When
            box.Insert("e");
            box.Insert("́");

            // Then
            box.Text.ShouldBe("é");
            box.Length.ShouldBe(1);
            box.Cursor.ShouldBe(new TextBoxPosition(0, 1));
        }

        [Fact]
        public void Should_Insert_Before_Combined_Emoji_Without_Splitting_It()
        {
            // Given
            var box = new TextBoxWidget();
            box.Insert("\uD83D");
            box.Insert("\uDE00");
            box.MoveLeft();

            // When
            box.Insert("X");

            // Then
            box.Text.ShouldBe("X😀");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 1));
        }
    }

    public sealed class TheDeleteBackwardMethod
    {
        [Fact]
        public void Should_Remove_Previous_Grapheme()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            box.MoveToEnd();

            // When
            box.DeleteBackward();

            // Then
            box.Text.ShouldBe("Hell");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 4));
        }

        [Fact]
        public void Should_Be_NoOp_At_Start_When_SingleLine()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            box.MoveToStart();

            // When
            box.DeleteBackward();

            // Then
            box.Text.ShouldBe("Hello");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 0));
        }

        [Fact]
        public void Should_Join_Lines_When_At_Start_Of_Line_In_MultiLine()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("Hello\nWorld");
            box.MoveToStart();
            box.MoveDown();

            // When
            box.DeleteBackward();

            // Then
            box.Text.ShouldBe("HelloWorld");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }

        [Fact]
        public void Should_Be_NoOp_When_ReadOnly()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello").ReadOnly();
            box.MoveToEnd();

            // When
            box.DeleteBackward();

            // Then
            box.Text.ShouldBe("Hello");
        }
    }

    public sealed class TheDeleteForwardMethod
    {
        [Fact]
        public void Should_Remove_Next_Grapheme()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            box.MoveToStart();

            // When
            box.DeleteForward();

            // Then
            box.Text.ShouldBe("ello");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 0));
        }

        [Fact]
        public void Should_Join_Lines_When_At_End_Of_Line_In_MultiLine()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("Hello\nWorld");
            box.MoveToStart();
            box.MoveEnd();

            // When
            box.DeleteForward();

            // Then
            box.Text.ShouldBe("HelloWorld");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }
    }

    public sealed class TheCursorMovementMethods
    {
        [Fact]
        public void MoveLeft_From_Start_Of_Line_Should_Wrap_To_Previous_Line()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("Hello\nWorld");
            box.MoveToStart();
            box.MoveDown();

            // When
            box.MoveLeft();

            // Then
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }

        [Fact]
        public void MoveRight_From_End_Of_Line_Should_Wrap_To_Next_Line()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("Hello\nWorld");
            box.MoveToStart();
            box.MoveEnd();

            // When
            box.MoveRight();

            // Then
            box.Cursor.ShouldBe(new TextBoxPosition(1, 0));
        }

        [Fact]
        public void MoveDown_Should_Restore_Desired_Column_After_Passing_Through_Short_Line()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("HelloWorld\nHi\nThereGoes");
            box.MoveToStart();
            box.MoveRight(7);

            // When
            box.MoveDown();
            var afterShort = box.Cursor;
            box.MoveDown();
            var afterLong = box.Cursor;

            // Then
            afterShort.ShouldBe(new TextBoxPosition(1, 2));
            afterLong.ShouldBe(new TextBoxPosition(2, 7));
        }

        [Fact]
        public void MoveUp_Should_Be_NoOp_When_SingleLine()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            box.MoveToEnd();

            // When
            box.MoveUp();

            // Then
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }

        [Fact]
        public void Should_Allow_Cursor_Movement_When_ReadOnly()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello").ReadOnly();
            box.MoveToEnd();

            // When
            box.MoveLeft();

            // Then
            box.Cursor.ShouldBe(new TextBoxPosition(0, 4));
        }
    }

    public sealed class TheInsertNewLineMethod
    {
        [Fact]
        public void Should_Split_Line_When_MultiLine()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("HelloWorld");
            box.MoveToStart();
            box.MoveRight(5);

            // When
            box.InsertNewLine();

            // Then
            box.Text.ShouldBe("Hello\nWorld");
            box.Cursor.ShouldBe(new TextBoxPosition(1, 0));
        }

        [Fact]
        public void Should_Be_NoOp_When_SingleLine()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            box.MoveToEnd();

            // When
            box.InsertNewLine();

            // Then
            box.Text.ShouldBe("Hello");
            box.Cursor.ShouldBe(new TextBoxPosition(0, 5));
        }
    }

    public sealed class TheRenderMethod
    {
        [Fact]
        public void Should_Render_SingleLine_Text()
        {
            // Given
            var box = new TextBoxWidget().Text("Hello");
            var fixture = new TuiFixture(new Size(10, 1));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe("Hello•••••");
        }

        [Fact]
        public void Should_Render_Placeholder_When_Empty()
        {
            // Given
            var box = new TextBoxWidget().Placeholder("typehere");
            var fixture = new TuiFixture(new Size(15, 1));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe("typehere•••••••");
        }

        [Fact]
        public void Should_Render_Password_Mask_Instead_Of_Text()
        {
            // Given
            var box = new TextBoxWidget().Text("secret").PasswordChar('*');
            var fixture = new TuiFixture(new Size(10, 1));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe("******••••");
        }

        [Fact]
        public void Should_Render_Multi_Line_Text()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("Line1\nLine2\nLine3");
            var fixture = new TuiFixture(new Size(8, 4));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe(
                """
                Line1•••
                Line2•••
                Line3•••
                ••••••••
                """);
        }

        [Fact]
        public void Should_Scroll_Horizontally_To_Keep_Cursor_Visible()
        {
            // Given
            var box = new TextBoxWidget().Text("HelloWorldXYZ");
            box.MoveToEnd();
            var fixture = new TuiFixture(new Size(10, 1));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe("oWorldXYZ•");
        }

        [Fact]
        public void Should_Scroll_Vertically_To_Keep_Cursor_Visible_When_MultiLine()
        {
            // Given
            var box = new TextBoxWidget().AsMultiLine().Text("L0\nL1\nL2\nL3\nL4");
            box.MoveToEnd();
            var fixture = new TuiFixture(new Size(4, 3));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe(
                """
                L2••
                L3••
                L4••
                """);
        }
    }
}
