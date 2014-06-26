using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Fools;
using Niels.Tests.TestHelper;
using Niels.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.Generates
{
    [TestClass]
    public class RookMoveGeneratorTest
    {
        [TestMethod]
        public void TestRookBlackMoveGenerate()
        {
            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 57);
            context.AddPiece(Piece.Pawn, 39);
            context.AddPiece(Piece.Pawn, 56);
            context.AddPiece(Piece.Pawn, 58);
            context.AddPiece(Piece.Pawn, 66, Turn.White);
            var order = new FoolOrder();
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 48, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 66, Turn.Black, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右盤
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 8);
            context.AddPiece(Piece.Knight, 6);
            context.AddPiece(Piece.Gold, 26, Turn.White);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 7, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 17, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 26, Turn.Black, Piece.Gold));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 8);
            expected = new List<uint>();
            for (int i = 0; i < 8; i++)
            {
                expected.Add(MoveForTest.GetMove(Piece.Rook, 8, i, Turn.Black));
            }
            for (int i = 17; i <= 80; i += 9)
            {
                expected.Add(MoveForTest.GetMove(Piece.Rook, 8, i, Turn.Black));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 下盤
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 80);
            context.AddPiece(Piece.Launce, 62);
            context.AddPiece(Piece.SilverPromoted, 78, Turn.White);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 71, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 79, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 78, Turn.Black, Piece.SilverPromoted));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            expected.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/expected_move.txt")
                );

            actual.ToList().ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/actual_move.txt")
                );

            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        [TestMethod]
        public void TestRookWhiteMoveGenerate()
        {
            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Rook, 57);
            context.AddPiece(Piece.Pawn, 39);
            context.AddPiece(Piece.Pawn, 56);
            context.AddPiece(Piece.Pawn, 58);
            context.AddPiece(Piece.Pawn, 66, Turn.Black);
            var order = new FoolOrder();
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 48, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 66, Turn.White, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右盤
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Rook, 8);
            context.AddPiece(Piece.Knight, 6);
            context.AddPiece(Piece.Gold, 26, Turn.Black);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 7, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 17, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 8, 26, Turn.White, Piece.Gold));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Rook, 8);
            expected = new List<uint>();
            for (int i = 0; i < 8; i++)
            {
                expected.Add(MoveForTest.GetMove(Piece.Rook, 8, i, Turn.White));
            }
            for (int i = 17; i <= 80; i += 9)
            {
                expected.Add(MoveForTest.GetMove(Piece.Rook, 8, i, Turn.White));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 下盤
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Rook, 80);
            context.AddPiece(Piece.Launce, 62);
            context.AddPiece(Piece.SilverPromoted, 78, Turn.Black);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 71, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 79, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 80, 78, Turn.White, Piece.SilverPromoted));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetRookMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            expected.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/expected_move.txt")
                );

            actual.ToList().ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/actual_move.txt")
                );

            CollectionAssert.AreEqual(expected, actual.ToList());
        }
    }
}
