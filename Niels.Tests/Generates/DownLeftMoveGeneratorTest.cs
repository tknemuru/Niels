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
    public class DownLeftMoveGeneratorTest
    {
        [TestMethod]
        public void TestDownLeftMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 0, 10, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 4, 14, Turn.Black, Piece.KnightPromoted));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 7, 17, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 9, 19, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 11, 21, Turn.Black, Piece.Dragon));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 16, 26, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 36, 46, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 41, 51, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 49, 59, Turn.Black, Piece.Pawn));
            expectedValue.Add(MoveForTest.GetMove(Piece.Silver, 52, 62, Turn.Black, Piece.Knight));


            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Silver, 0);
            context.AddPiece(Piece.Silver, 3);
            context.AddPiece(Piece.Silver, 4);
            context.AddPiece(Piece.Silver, 7);
            context.AddPiece(Piece.Silver, 9);
            context.AddPiece(Piece.Silver, 11);
            context.AddPiece(Piece.Silver, 16);
            context.AddPiece(Piece.Silver, 25);
            context.AddPiece(Piece.Silver, 31);
            context.AddPiece(Piece.Silver, 36);
            context.AddPiece(Piece.Silver, 41);
            context.AddPiece(Piece.Silver, 49);
            context.AddPiece(Piece.Silver, 52);
            context.AddPiece(Piece.Silver, 72);
            context.AddPiece(Piece.Silver, 79);
            context.AddPiece(Piece.Silver, 80);

            context.AddPiece(Piece.Launce, 13);
            context.AddPiece(Piece.Pawn, 35);

            context.AddOppositePiece(Piece.KnightPromoted, 14);
            context.AddOppositePiece(Piece.Dragon, 21);
            context.AddOppositePiece(Piece.Pawn, 59);
            context.AddOppositePiece(Piece.Knight, 62);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new DownLeftMoveGenerator();
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
