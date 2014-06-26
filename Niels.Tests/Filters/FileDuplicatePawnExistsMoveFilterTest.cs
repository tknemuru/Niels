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
using Niels.Helper;
using Niels.Fools;

namespace Niels.Tests.Filters
{
    [TestClass]
    public class FileDuplicatePawnExistsMoveFilterTest
    {
        [TestMethod]
        public void TestFileDuplicatePawnExistsMoveFilter()
        {
            var order = new FoolOrder();
            FileDuplicatePawnExistsMoveFilter filter = new FileDuplicatePawnExistsMoveFilter();

            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 72);
            context.AddPiece(Piece.Pawn, 70);
            context.AddPiece(Piece.Pawn, 54, Turn.White);
            
            List<uint> moves = new List<uint>();
            List<uint> expected = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 79, 78, Turn.Black));
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 63, Turn.Black));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 61, 60, Turn.Black));
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 61, Turn.Black));            

            expected.Add(MoveForTest.GetMove(Piece.Pawn, 61, 60, Turn.Black));
            expected.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 61, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            List<uint> actual = filter.Filter(context, moves).ToList();
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 右盤
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 0);

            moves = new List<uint>();
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 8, Turn.Black));

            expected = new List<uint>();
            expected = order.MoveOrdering(expected).ToList();

            actual = filter.Filter(context, moves).ToList();
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual);

            context = new BoardContextForTest(Turn.Black);

            moves = new List<uint>();
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 8, Turn.Black));

            expected = new List<uint>();
            expected.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 8, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            actual = filter.Filter(context, moves).ToList();
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual);

            // 下
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 72);

            moves = new List<uint>();
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 80, Turn.Black));

            expected = new List<uint>();
            expected = order.MoveOrdering(expected).ToList();

            actual = filter.Filter(context, moves).ToList();
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual);

            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 71);

            moves = new List<uint>();
            moves.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 80, Turn.Black));

            expected = new List<uint>();
            expected.Add(MoveForTest.GetHandValueMove(Piece.Pawn, 80, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            actual = filter.Filter(context, moves).ToList();
            actual = order.MoveOrdering(actual).ToList();

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            expected.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/expected_move.txt")
                );

            actual.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/actual_move.txt")
                );

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
