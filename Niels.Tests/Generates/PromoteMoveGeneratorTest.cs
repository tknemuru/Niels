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
    public class PromoteMoveGeneratorTest
    {
        [TestMethod]
        public void TestPromoteMoveGenerate()
        {
            var order = new FoolOrder();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 75);
            context.AddPiece(Piece.Pawn, 74, Turn.White);
            context.AddPiece(Piece.Pawn, 66);
            context.AddPiece(Piece.Pawn, 76);
            var expected = new List<uint>();
            // 成れるのに成らない手はAllでも隠すことにする
            //expected.Add(MoveForTest.GetMove(Piece.Rook, 75, 74, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Dragon, 75, 74, Turn.Black, Piece.Pawn, Promote.Yes));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetRookMoves(context, GenerateTarget.All);
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
