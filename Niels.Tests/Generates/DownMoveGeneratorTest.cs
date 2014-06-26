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
    public class DownMoveGeneratorTest
    {
        [TestMethod]
        public void TestDownMoveWhite()
        {
            var order = new FoolOrder();
            var expectedValue = new List<uint>();
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 31, 32, Turn.White));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 43, 44, Turn.White));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 50, Turn.White));
            expectedValue.Add(MoveForTest.GetMove(Piece.Pawn, 72, 73, Turn.White));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            BoardContextForTest context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Pawn, 0);
            context.AddPiece(Piece.Pawn, 5);
            context.AddPiece(Piece.Gold, 6);
            context.AddPiece(Piece.Pawn, 8);
            context.AddPiece(Piece.Pawn, 16);
            context.AddPiece(Piece.Gold, 17);
            context.AddPiece(Piece.Pawn, 30);
            context.AddPiece(Piece.Pawn, 31);
            context.AddPiece(Piece.Pawn, 43);
            context.AddPiece(Piece.Pawn, 49);
            context.AddPiece(Piece.Pawn, 59);
            context.AddPiece(Piece.Gold, 60);
            context.AddPiece(Piece.Pawn, 72);
            context.AddPiece(Piece.Pawn, 80);

            var gen = new DownMoveGenerator();
            var actualValue = gen.Generate(context, Piece.Pawn);
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue.ToList());
        }
    }
}
