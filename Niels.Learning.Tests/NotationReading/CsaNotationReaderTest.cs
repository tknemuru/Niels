using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Fools;
using Niels.Tests.TestHelper;
using Niels.Helper;
using Niels.Learning.NotationReading;
using System.Collections.Generic;
using System.Linq;

namespace Niels.NotationReader.Tests.NotationReading
{
    [TestClass]
    public class CsaNotationReaderTest
    {
        [TestMethod]
        public void TestConvertToNielsMove()
        {
            CsaNotationReader notation = new CsaNotationReader();
            BoardContextForTest context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Silver, 54);
            uint expected = MoveForTest.GetMove(Piece.Silver, 54, 46, Turn.White);
            uint actual = notation.ConvertToNielsMove("7162GI", context);
            Assert.AreEqual(expected, actual);
            
            context.AddPiece(Piece.Gold, 46, Turn.Black);
            expected = MoveForTest.GetMove(Piece.Silver, 54, 46, Turn.White, Piece.Gold);
            actual = notation.ConvertToNielsMove("7162GI", context);
            Assert.AreEqual(expected, actual);

            context = new BoardContextForTest(Turn.White);
            context.AddPiece(Piece.Launce, 38);
            expected = MoveForTest.GetMove(Piece.LauncePromoted, 38, 36, Turn.White, Piece.Empty, Promote.Yes);
            actual = notation.ConvertToNielsMove("5351NY", context);
            Assert.AreEqual(expected, actual);

            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 10, Turn.Black);
            expected = MoveForTest.GetMove(Piece.Dragon, 10, 19, Turn.Black, Piece.Empty, Promote.Yes);
            actual = notation.ConvertToNielsMove("2232RY", context);

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            FileHelper.WriteLine(MoveForTest.ToDebugString(expected), @"./test/expected_move.txt");
            FileHelper.WriteLine(MoveForTest.ToDebugString(actual), @"./test/actual_move.txt");

            Assert.AreEqual(expected, actual);
        }
    }
}
