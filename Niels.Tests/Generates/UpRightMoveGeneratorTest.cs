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
    public class UpRightMoveGeneratorTest
    {
        [TestMethod]
        public void TestUpRightMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 16, 6, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 17, 7, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 21, 11, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 44, 34, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 49, 39, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 79, 69, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Gold, 80, 70, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Gold, 0);
            context.AddPiece(Piece.Gold, 2);
            context.AddPiece(Piece.Gold, 8);
            context.AddPiece(Piece.Gold, 9);
            context.AddPiece(Piece.Gold, 16);
            context.AddPiece(Piece.Gold, 17);
            context.AddPiece(Piece.Gold, 21);
            context.AddPiece(Piece.Gold, 31);
            context.AddPiece(Piece.Gold, 44);
            context.AddPiece(Piece.Gold, 49);
            context.AddPiece(Piece.Gold, 72);
            context.AddPiece(Piece.Gold, 79);
            context.AddPiece(Piece.Gold, 80);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new UpRightMoveGenerator();
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

        [TestMethod]
        public void TestUpRightMoveCapturePiece()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Gold, 16);
            context.AddPiece(Piece.Pawn, 6);

            FileHelper.WriteLine("", @"./test/context.txt", false);
            FileHelper.WriteLine(context.ToString(), @"./test/context.txt");

            var gen = new UpRightMoveGenerator();
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
