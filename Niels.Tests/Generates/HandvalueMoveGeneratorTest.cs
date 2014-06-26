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
    public class HandvalueMoveGeneratorTest
    {
        [TestMethod]
        public void TestHandvalueMoveGenerateBlack()
        {
            var order = new FoolOrder();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 50);
            context.AddOppositePiece(Piece.Pawn, 30);
            context.AddHandValue(Piece.Bishop);

            var expected = new List<uint>();
            for(int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Bishop, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetHandValueMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 歩、香車、桂馬は行き所のない駒の指し手をフィルタする
            // 歩
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Bishop, 50);
            context.AddOppositePiece(Piece.Pawn, 30);
            context.AddHandValue(Piece.Pawn);

            expected = new List<uint>();
            List<int> filterIndexs = new List<int>();
            for (int i = 0; i <= 72; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Pawn, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 香車
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Bishop, 50);
            context.AddOppositePiece(Piece.Rook, 30);
            context.AddHandValue(Piece.Launce);
            context.AddHandValue(Piece.Launce);

            expected = new List<uint>();
            filterIndexs = new List<int>();
            for (int i = 0; i <= 72; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Launce, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 桂馬
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Bishop, 50);
            context.AddOppositePiece(Piece.Rook, 30);
            context.AddHandValue(Piece.Knight);

            expected = new List<uint>();
            filterIndexs = new List<int>();
            for (int i = 0; i <= 72; i += 9)
            {
                filterIndexs.Add(i);
            }
            for (int i = 1; i <= 73; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Knight, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
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
        public void TestHandvalueMoveGenerateWhite()
        {
            var order = new FoolOrder();
            BoardContextForTest context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Pawn, 50);
            context.AddOppositePiece(Piece.Pawn, 30);
            context.AddHandValue(Piece.Bishop);

            var expected = new List<uint>();
            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Bishop, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetHandValueMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 歩、香車、桂馬は行き所のない駒の指し手をフィルタする
            // 歩
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Knight, 50);
            context.AddOppositePiece(Piece.Pawn, 30);
            context.AddHandValue(Piece.Pawn);

            expected = new List<uint>();
            List<int> filterIndexs = new List<int>();
            for (int i = 8; i <= 80; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Pawn, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            // 香車
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Bishop, 50);
            context.AddOppositePiece(Piece.Rook, 30);
            context.AddHandValue(Piece.Launce);
            context.AddHandValue(Piece.Launce);

            expected = new List<uint>();
            filterIndexs = new List<int>();
            for (int i = 8; i <= 80; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Launce, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
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

            // 桂馬
            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Bishop, 50);
            context.AddOppositePiece(Piece.Rook, 30);
            context.AddHandValue(Piece.Knight);

            expected = new List<uint>();
            filterIndexs = new List<int>();
            for (int i = 8; i <= 80; i += 9)
            {
                filterIndexs.Add(i);
            }
            for (int i = 7; i <= 79; i += 9)
            {
                filterIndexs.Add(i);
            }

            for (int i = 0; i <= 80; i++)
            {
                if (i == 50 || i == 30) { continue; }
                if (filterIndexs.Contains(i)) { continue; }
                expected.Add(MoveForTest.GetHandValueMove(Piece.Knight, i, context.Turn));
            }
            expected = order.MoveOrdering(expected).ToList();

            actual = MoveProvider.GetHandValueMoves(context);
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
