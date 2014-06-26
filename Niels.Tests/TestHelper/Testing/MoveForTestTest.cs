using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Boards;
using Niels.Collections;
using Niels.Orders;
using Niels.Fools;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.TestHelper.Testing
{
    [TestClass]
    public class MoveForTestTest
    {
        [TestMethod]
        public void TestGetMove()
        {
            var order = new FoolOrder();

            var expectedValue = new List<uint>();
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 6, BoardType.SubRight, 5, Turn.Black, Piece.SilverPromoted));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubBottom, 31, BoardType.Main, 31, Turn.Black, Piece.Knight));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 46, BoardType.Main, 45, Turn.Black, Piece.Gold));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 36, BoardType.Main, 35, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            var actualValue = new List<uint>();
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 6, 5, Turn.Black, Piece.SilverPromoted));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 44, 43, Turn.Black, Piece.Knight));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 60, 59, Turn.Black, Piece.Gold));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 48, Turn.Black));
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue);

            expectedValue = new List<uint>();
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 0, BoardType.SubRight, 1, Turn.White));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 20, BoardType.Main, 21, Turn.White));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 31, BoardType.SubBottom, 31, Turn.White));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 36, BoardType.Main, 37, Turn.White));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 56, BoardType.Main, 57, Turn.White));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            actualValue = new List<uint>();
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 0, 1, Turn.White));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 31, 32, Turn.White));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 43, 44, Turn.White));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 50, Turn.White));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 72, 73, Turn.White));
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue);

            expectedValue = new List<uint>();
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 8, BoardType.SubRight, 7, Turn.Black));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubBottom, 63, BoardType.Main, 63, Turn.Black));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 19, BoardType.Main, 18, Turn.Black));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.Main, 36, BoardType.Main, 35, Turn.Black));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            actualValue = new List<uint>();
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 8, 7, Turn.Black));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 80, 79, Turn.Black));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 30, 29, Turn.Black));
            actualValue.Add(MoveForTest.GetMove(Piece.Pawn, 49, 48, Turn.Black));
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue);
        }
    }
}
