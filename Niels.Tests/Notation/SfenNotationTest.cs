using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Notation;
using Niels.Tests.TestHelper;

namespace Niels.Tests.Notation
{
    [TestClass]
    public class SfenNotationTest
    {
        [TestMethod]
        public void TestConvertToBestMove()
        {
            SfenNotation notation = new SfenNotation();
            uint move = Move.GetMove(Piece.Pawn, BoardType.Main, 56, BoardType.SubRight, 8, Turn.Black);
            string expected = "9a1i";
            string actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.Pawn, BoardType.SubBottom, 63, BoardType.SubRight, 0, Turn.Black);
            expected = "9i1a";
            actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.Pawn, BoardType.SubRight, 8, BoardType.Main, 7, Turn.Black);
            expected = "1i2h";
            actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.Pawn, BoardType.SubBottom, 7, BoardType.Main, 7, Turn.Black);
            expected = "2i2h";
            actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.PawnPromoted, BoardType.SubBottom, 63, BoardType.SubRight, 0, Turn.Black, Piece.Empty, Promote.Yes);
            expected = "9i1a+";
            actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetBoardTypeAndGetIndex()
        {
            SfenNotationForTest notation = new SfenNotationForTest();
            BoardType expectedBoardType = BoardType.Main;
            int expectedIndex = 56;

            string move = "9a";
            BoardType actualBoardType = notation.GetBoardTypeForTest(move);
            int actualIndex = notation.GetIndexForTest(move);

            Assert.AreEqual(expectedBoardType, actualBoardType);
            Assert.AreEqual(expectedIndex, actualIndex);

            expectedBoardType = BoardType.SubRight;
            expectedIndex = 0;

            move = "1a";
            actualBoardType = notation.GetBoardTypeForTest(move);
            actualIndex = notation.GetIndexForTest(move);

            Assert.AreEqual(expectedBoardType, actualBoardType);
            Assert.AreEqual(expectedIndex, actualIndex);

            expectedBoardType = BoardType.SubRight;
            expectedIndex = 8;

            move = "1i";
            actualBoardType = notation.GetBoardTypeForTest(move);
            actualIndex = notation.GetIndexForTest(move);

            Assert.AreEqual(expectedBoardType, actualBoardType);
            Assert.AreEqual(expectedIndex, actualIndex);

            expectedBoardType = BoardType.SubBottom;
            expectedIndex = 63;

            move = "9i";
            actualBoardType = notation.GetBoardTypeForTest(move);
            actualIndex = notation.GetIndexForTest(move);

            Assert.AreEqual(expectedBoardType, actualBoardType);
            Assert.AreEqual(expectedIndex, actualIndex);

            expectedBoardType = BoardType.SubBottom;
            expectedIndex = 7;

            move = "2i";
            actualBoardType = notation.GetBoardTypeForTest(move);
            actualIndex = notation.GetIndexForTest(move);

            Assert.AreEqual(expectedBoardType, actualBoardType);
            Assert.AreEqual(expectedIndex, actualIndex);
        }

        /// <summary>
        /// 持ち駒の打ち手テストを実施します。
        /// </summary>
        [TestMethod]
        public void TestHandValueConvertToSfenMove()
        {
            SfenNotation notation = new SfenNotation();
            uint move = MoveForTest.GetHandValueMove(Piece.Pawn, 30, Turn.Black);
            string expected = "P*4d";
            string actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);

            move = MoveForTest.GetHandValueMove(Piece.Rook, 30, Turn.Black);
            expected = "R*4d";
            actual = notation.ConvertToSfenMove(move);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 持ち駒の打ち手テストを実施します。
        /// </summary>
        public void TestHandValueConvertToNielsMove()
        {
            SfenNotationForTest notation = new SfenNotationForTest();
            uint expected = MoveForTest.GetHandValueMove(Piece.Pawn, 79, Turn.Black);
            BoardContext context = new BoardContext(Turn.Black);
            string sfenMove = "P*9h";
            uint actual = notation.ConvertToNielsMove(sfenMove, context);

            Assert.AreEqual(expected, actual);

            expected = MoveForTest.GetHandValueMove(Piece.Rook, 79, Turn.White);
            context = new BoardContext(Turn.White);
            sfenMove = "R*9h";
            actual = notation.ConvertToNielsMove(sfenMove, context);

            Assert.AreEqual(expected, actual);
        }
    }
}
