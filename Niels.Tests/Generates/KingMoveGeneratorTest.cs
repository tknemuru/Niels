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
    public class KingMoveGeneratorTest
    {
        [TestMethod]
        public void TestKingMoveGenerate()
        {
            var order = new FoolOrder();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddPiece(Piece.Gold, 35);
            context.AddPiece(Piece.Silver, 34);
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.King, 44, 43, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.King, 44, 52, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.King, 44, 53, Turn.Black));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetKingMoves(context);
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
