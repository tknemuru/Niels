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
    public class PieceDuplicateExistsMoveFilterTest
    {
        [TestMethod]
        public void TestPieceDuplicateExistsMoveFilter()
        {
            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 72);
            context.AddPiece(Piece.Pawn, 0);
            context.AddPiece(Piece.Pawn, 75, Turn.White);

            PieceDuplicateExistsMoveFilter filter = new PieceDuplicateExistsMoveFilter();
            List<uint> moves = new List<uint>();
            List<uint> expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Rook, 72, 0, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 72, 75, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 75, Turn.Black));

            List<uint> actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // メイン盤２
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 72);
            context.AddPiece(Piece.Pawn, 0, Turn.White);
            context.AddPiece(Piece.Pawn, 80);

            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Rook, 72, 0, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 72, 80, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 0, Turn.Black));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 右盤
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 8);
            context.AddPiece(Piece.Pawn, 0);
            context.AddPiece(Piece.Pawn, 3, Turn.White);
            context.AddPiece(Piece.Pawn, 44, Turn.White);
            context.AddPiece(Piece.Pawn, 80);

            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Rook, 8, 0, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 8, 3, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 8, 44, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 8, 80, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 3, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 44, Turn.Black));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 下盤
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 80);
            context.AddPiece(Piece.Pawn, 8);
            context.AddPiece(Piece.Pawn, 72);
            context.AddPiece(Piece.Pawn, 62, Turn.White);
            context.AddPiece(Piece.Pawn, 77, Turn.White);

            moves = new List<uint>();
            expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Rook, 80, 8, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 80, 62, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 80, 72, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 80, 77, Turn.Black));

            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 62, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 77, Turn.Black));

            actual = filter.Filter(context, moves).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
