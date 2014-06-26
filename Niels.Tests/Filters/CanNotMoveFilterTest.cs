using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Filters;
using Niels.Tests.TestHelper;

namespace Niels.Tests.Filters
{
    [TestClass]
    public class CanNotMoveFilterTest
    {
        [TestMethod]
        public void TestFilter()
        {
            BoardContext context = new BoardContext(Turn.Black);

            // 黒１列
            CanNotMoveFilter filter = new CanNotMoveFilter();
            List<uint> moves = new List<uint>();
            List<uint> expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 1, 0, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 73, 72, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 65, 64, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 65, 64, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));

            List<uint> actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 白１列
            context.ChangeTurn();
            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 7, 8, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 9, 10, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 69, 70, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 79, 80, Turn.White));

            expected.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 9, 10, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 69, 70, Turn.White));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 黒2列
            context.ChangeTurn();
            filter = new CanNotMoveFilter(2);
            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 1, 0, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 73, 72, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 65, 64, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 白2列
            context.ChangeTurn();
            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 7, 8, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 9, 10, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 69, 70, Turn.White));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 79, 80, Turn.White));

            expected.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Pawn, 9, 10, Turn.White));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
