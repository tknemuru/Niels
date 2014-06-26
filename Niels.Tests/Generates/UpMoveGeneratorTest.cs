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
    public class UpMoveGeneratorTest
    {
        [TestMethod]
        public void TestUpMove()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 30, 29, Turn.Black));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 48, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 0);
            context.AddPiece(Piece.Gold, 5);
            context.AddPiece(Piece.Pawn, 6);
            context.AddPiece(Piece.Pawn, 8);
            context.AddPiece(Piece.Gold, 16);
            context.AddPiece(Piece.Pawn, 17);
            context.AddPiece(Piece.Pawn, 30);
            context.AddPiece(Piece.Pawn, 31);
            context.AddPiece(Piece.Gold, 43);
            context.AddPiece(Piece.Pawn, 44);
            context.AddPiece(Piece.Pawn, 49);
            context.AddPiece(Piece.Gold, 59);
            context.AddPiece(Piece.Pawn, 60);
            context.AddPiece(Piece.Pawn, 72);
            context.AddPiece(Piece.Pawn, 80);

            var gen = new UpMoveGenerator();
            var actualValue = gen.Generate(context, Piece.Pawn);
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

            CollectionAssert.AreEqual(expectedValue.ToList(), actualValue.ToList());
        }

        [TestMethod]
        public void TestUpMoveExistsCapture()
        {
            var order = new FoolOrder();

            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 6, 5, Turn.Black, Piece.SilverPromoted));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 44, 43, Turn.Black, Piece.Knight));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 60, 59, Turn.Black, Piece.Gold));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 48, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();
            
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 6);
            context.AddPiece(Piece.Pawn, 44);
            context.AddPiece(Piece.Pawn, 49);
            context.AddPiece(Piece.Pawn, 60);
            context.AddOppositePiece(Piece.SilverPromoted, 5);
            context.AddOppositePiece(Piece.Gold, 59);
            context.AddOppositePiece(Piece.Knight, 43);

            var gen = new UpMoveGenerator();
            var actualValue = gen.Generate(context, Piece.Pawn);
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue.ToList());
        }
    }
}
