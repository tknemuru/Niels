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
    public class BishopMoveGeneratorTest
    {
        [TestMethod]
        public void TestBishopMoveGenerate()
        {
            // メイン盤
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Bishop, 15);
            context.AddPiece(Piece.KnightPromoted, 5, Turn.White);
            context.AddPiece(Piece.Silver, 31);
            context.AddPiece(Piece.Silver, 35, Turn.White);
            context.AddPiece(Piece.Gold, 7);
            var order = new FoolOrder();
            var expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 15, 5, Turn.Black, Piece.KnightPromoted));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 15, 23, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 15, 25, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 15, 35, Turn.Black, Piece.Silver));
            expected = order.MoveOrdering(expected).ToList();

            var actual = MoveProvider.GetBishopMoves(context, GenerateTarget.NoPromote);
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
