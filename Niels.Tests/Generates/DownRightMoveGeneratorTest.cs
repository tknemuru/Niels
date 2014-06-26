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
    public class DownRightMoveGeneratorTest
    {
        [TestMethod]
        public void TestDownRightMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 9, 1, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 10, 2, Turn.Black, Piece.Dragon));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 16, 8, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 31, 23, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 49, 41, Turn.Black, Piece.Rook));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 52, 44, Turn.Black, Piece.Pawn));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 72, 64, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 79, 71, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Silver, 0);
            context.AddPiece(Piece.Silver, 9);
            context.AddPiece(Piece.Silver, 10);
            context.AddPiece(Piece.Silver, 16);
            context.AddPiece(Piece.Silver, 31);
            context.AddPiece(Piece.Silver, 39);
            context.AddPiece(Piece.Silver, 49);
            context.AddPiece(Piece.Silver, 52);
            context.AddPiece(Piece.Silver, 72);
            context.AddPiece(Piece.Silver, 79);
            context.AddPiece(Piece.Silver, 80);

            context.AddOppositePiece(Piece.Dragon, 2);
            context.AddOppositePiece(Piece.Rook, 41);
            context.AddOppositePiece(Piece.Pawn, 44);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new DownRightMoveGenerator();
            var actualValue = gen.Generate(context, Piece.Silver);
            actualValue = order.MoveOrdering(actualValue).ToList();

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            expectedValue.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/expected_move.txt")
                );

            actualValue.ToList().ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/actual_move.txt")
                );

            CollectionAssert.AreEqual(expectedValue, actualValue.ToList());
        }
    }
}
