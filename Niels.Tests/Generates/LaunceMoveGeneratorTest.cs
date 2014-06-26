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
    public class LaunceMoveGeneratorTest
    {
        [TestMethod]
        public void TestLaunceMoveGenerate()
        {
            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Launce, 16);
            context.AddPiece(Piece.Pawn, 12);
            var order = new FoolOrder();
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Launce, 16, 15, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 16, 14, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 16, 13, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetLaunceMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 右
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Launce, 8);

            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 7, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 6, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 5, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 4, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 3, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 2, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 8, 1, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetLaunceMoves(context, GenerateTarget.NoPromote);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 下
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Launce, 80);
            context.AddPiece(Piece.Pawn, 77, Turn.White);

            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Launce, 80, 79, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 80, 78, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Launce, 80, 77, Turn.Black, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetLaunceMoves(context, GenerateTarget.NoPromote);
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
