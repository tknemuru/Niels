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
    public class KnightMoveGeneratorTest
    {
        [TestMethod]
        public void TestKnightBlackMoveGenerate()
        {
            var order = new FoolOrder();

            // メイン＞メイン
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Knight, 79);
            context.AddPiece(Piece.Knight, 74);
            context.AddPiece(Piece.Knight, 38);
            context.AddPiece(Piece.Bishop, 27);
            context.AddOppositePiece(Piece.Bishop, 45);
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 79, 68, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 74, 63, Turn.Black, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 38, 45, Turn.Black, Piece.Bishop, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // メイン＞右
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Knight, 16);
            context.AddPiece(Piece.Knight, 11);
            context.AddPiece(Piece.Knight, 14);
            context.AddPiece(Piece.Launce, 21);
            context.AddOppositePiece(Piece.Gold, 3);
            context.AddPiece(Piece.Knight, 15);
            context.AddPiece(Piece.Pawn, 22);
            context.AddPiece(Piece.Pawn, 4);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 16, 5, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 16, 23, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 11, 0, Turn.Black, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 11, 18, Turn.Black, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 14, 3, Turn.Black, Piece.Gold, Promote.No));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右＞メイン
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Knight, 8);
            context.AddPiece(Piece.Knight, 2);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 8, 15, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 2, 9, Turn.Black, Piece.Empty, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 下＞メイン
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Knight, 80);
            context.AddPiece(Piece.Knight, 26);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 80, 69, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 26, 15, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 26, 33, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 下＞右
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Knight, 17);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 17, 6, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 17, 24, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
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
        public void TestKnightWhiteMoveGenerate()
        {
            var order = new FoolOrder();

            // メイン＞メイン
            BoardContextForTest context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 23);
            context.AddPiece(Piece.Knight, 77);
            context.AddOppositePiece(Piece.Launce, 70);
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 23, 16, Turn.White, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 23, 34, Turn.White, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 77, 70, Turn.White, Piece.Launce, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // メイン＞右
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 9);
            context.AddPiece(Piece.Knight, 15);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 9, 2, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.Knight, 9, 20, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 15, 8, Turn.White, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 15, 26, Turn.White, Piece.Empty, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右＞メイン
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 0);
            context.AddPiece(Piece.Knight, 5);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Knight, 0, 11, Turn.White));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 5, 16, Turn.White, Piece.Empty, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右＞下
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 6);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 6, 17, Turn.White, Piece.Empty, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // メイン＞下
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 33);
            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 33, 26, Turn.White, Piece.Empty, Promote.Yes));
            expected.Add(MoveForTest.GetMove(Piece.KnightPromoted, 33, 44, Turn.White, Piece.Empty, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetKnightMoves(context, GenerateTarget.All);
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
