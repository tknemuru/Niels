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
    public class RightMoveGeneratorTest
    {
        [TestMethod]
        public void TestRightMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 9, 0, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 11, 2, Turn.Black, Piece.Dragon));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 16, 7, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 17, 8, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 36, 27, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 49, 40, Turn.Black, Piece.Pawn));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 62, 53, Turn.Black, Piece.Bishop));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 72, 63, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 79, 70, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 80, 71, Turn.Black));

            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Gold, 3);
            context.AddPiece(Piece.Gold, 9);
            context.AddPiece(Piece.Gold, 11);
            context.AddPiece(Piece.Gold, 13);
            context.AddPiece(Piece.Gold, 16);
            context.AddPiece(Piece.Gold, 17);
            context.AddPiece(Piece.Gold, 35);
            context.AddPiece(Piece.Gold, 36);
            context.AddPiece(Piece.Gold, 43);
            context.AddPiece(Piece.Gold, 49);
            context.AddPiece(Piece.Gold, 62);
            context.AddPiece(Piece.Gold, 72);
            context.AddPiece(Piece.Gold, 79);
            context.AddPiece(Piece.Gold, 80);

            context.AddPiece(Piece.Knight, 4);
            context.AddPiece(Piece.King, 34);
            context.AddPiece(Piece.Launce, 26);

            context.AddOppositePiece(Piece.Dragon, 2);
            context.AddOppositePiece(Piece.Pawn, 40);
            context.AddOppositePiece(Piece.Bishop, 53);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new RightMoveGenerator();
            var actualValue = gen.Generate(context, Piece.Gold);
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
