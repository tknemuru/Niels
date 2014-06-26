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
    public class HorseMoveGeneratorTest
    {
        [TestMethod]
        public void TestHorseMoveGenerate()
        {
            var order = new FoolOrder();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Horse, 20);
            context.AddOppositePiece(Piece.Bishop, 10);
            context.AddPiece(Piece.Pawn, 11);
            context.AddPiece(Piece.Pawn, 12);
            context.AddPiece(Piece.Pawn, 21);
            context.AddPiece(Piece.Pawn, 30);
            context.AddPiece(Piece.Pawn, 29);
            context.AddPiece(Piece.Pawn, 28);
            context.AddPiece(Piece.Pawn, 19);
            
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Horse, 20, 10, Turn.Black, Piece.Bishop, Promote.No));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetHorseMoves(context);
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual.ToList());

            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Horse, 76);
            context.AddOppositePiece(Piece.King, 44);

            uint singleExpected = MoveForTest.GetMove(Piece.Horse, 76, 44, Turn.Black, Piece.King, Promote.No);
            
            actual = MoveProvider.GetHorseMoves(context);
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

            Assert.IsTrue(actual.Contains(singleExpected));
        }
    }
}
