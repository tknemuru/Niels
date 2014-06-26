using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Tests.TestHelper;
using Niels.Collections;

namespace Niels.Tests.TestHelper.Testing
{
    [TestClass]
    public class BoardContextForTestTest
    {
        [TestMethod]
        public void TestAddPiece()
        {
            ulong[] pawnBoard = { 0x0000401000000000, 0x040, 0x0000000080000000 };
            ulong[] myOwnBoard = { 0x0000401000000000, 0x040, 0x0000000080000000 };
            ulong[] enemyOwnBoard = { 0x0000200080000000, 0x020, 0 };
            ulong[] goldBoard = { 0x0000200000000000, 0, 0 };
            ulong[] silverPromotedBoard = { 0, 0x020, 0 };
            ulong[] knightBoard = { 0x0000000080000000, 0, 0 };

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Pawn, 6);
            context.AddPiece(Piece.Pawn, 44);
            context.AddPiece(Piece.Pawn, 49);
            context.AddPiece(Piece.Pawn, 60);

            context.AddOppositePiece(Piece.SilverPromoted, 5);
            context.AddOppositePiece(Piece.Gold, 59);
            context.AddOppositePiece(Piece.Knight, 43);

            CollectionAssert.AreEqual(pawnBoard, context.PieceBoards[(int)context.Turn][Piece.Pawn.GetIndex()]);
            CollectionAssert.AreEqual(myOwnBoard, context.OccupiedBoards[(int)context.Turn]);
            CollectionAssert.AreEqual(enemyOwnBoard, context.OccupiedBoards[context.Turn.GetOppositeIndex()]);

            CollectionAssert.AreEqual(goldBoard, context.PieceBoards[context.Turn.GetOppositeIndex()][Piece.Gold.GetIndex()]);
            CollectionAssert.AreEqual(silverPromotedBoard, context.PieceBoards[context.Turn.GetOppositeIndex()][Piece.SilverPromoted.GetIndex()]);
            CollectionAssert.AreEqual(knightBoard, context.PieceBoards[context.Turn.GetOppositeIndex()][Piece.Knight.GetIndex()]);
        }
    }
}
