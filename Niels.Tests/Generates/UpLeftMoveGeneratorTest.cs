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
    public class UpLeftMoveGeneratorTest
    {
        [TestMethod]
        public void TestUpLeftMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 1, 9, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 2, 10, Turn.Black, Piece.Dragon));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 8, 16, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 17, 25, Turn.Black, Piece.Horse));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 39, 47, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 44, 52, Turn.Black, Piece.Pawn));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 49, 57, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Gold, 0);
            context.AddPiece(Piece.Gold, 1);
            context.AddPiece(Piece.Gold, 2);
            context.AddPiece(Piece.Gold, 8);
            context.AddPiece(Piece.Gold, 17);
            context.AddPiece(Piece.Gold, 31);
            context.AddPiece(Piece.Gold, 39);
            context.AddPiece(Piece.Gold, 44);
            context.AddPiece(Piece.Gold, 49);
            context.AddPiece(Piece.Gold, 72);
            context.AddPiece(Piece.Gold, 79);
            context.AddPiece(Piece.Gold, 80);

            context.AddOppositePiece(Piece.Dragon, 10);
            context.AddOppositePiece(Piece.Horse, 25);
            context.AddOppositePiece(Piece.Pawn, 52);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new UpLeftMoveGenerator();
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
