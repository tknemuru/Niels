using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Fools;
using Niels.Tests.TestHelper;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.Boards
{
    [TestClass]
    public class BoardContextTest
    {
        [TestMethod]
        public void TestPutPiece()
        {
            BoardContextForTest expectedContext = new BoardContextForTest(Turn.Black);
            expectedContext.AddPiece(Piece.Rook, 0);
            expectedContext.AddPiece(Piece.Silver, 3);
            expectedContext.AddPiece(Piece.Gold, 14);
            expectedContext.AddPiece(Piece.Pawn, 16);
            expectedContext.AddPiece(Piece.Pawn, 49);
            expectedContext.AddPiece(Piece.Gold, 80);

            expectedContext.AddHandValue(Piece.SilverPromoted);
            expectedContext.AddHandValue(Piece.Knight);
            expectedContext.AddHandValue(Piece.Knight);
            expectedContext.AddHandValue(Piece.LauncePromoted);
            expectedContext.AddHandValue(Piece.Bishop);
            expectedContext.AddHandValue(Piece.Dragon);

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Gold, 6);
            context.AddPiece(Piece.Rook, 8);
            context.AddPiece(Piece.Silver, 11);
            context.AddPiece(Piece.Pawn, 17);
            context.AddPiece(Piece.Pawn, 50);
            context.AddPiece(Piece.Gold, 6);
            context.AddPiece(Piece.Gold, 79);

            context.AddOppositePiece(Piece.SilverPromoted, 0);
            context.AddOppositePiece(Piece.Knight, 3);
            context.AddOppositePiece(Piece.Knight, 14);
            context.AddOppositePiece(Piece.LauncePromoted, 16);
            context.AddOppositePiece(Piece.Bishop, 49);
            context.AddOppositePiece(Piece.Dragon, 80);

            BoardContextForTest beforePutContext = context.Clone();

            List<uint> moves = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Silver, 11, 3, Turn.Black, Piece.Knight));
            moves.Add(MoveForTest.GetMove(Piece.Gold, 6, 14, Turn.Black, Piece.Knight));
            moves.Add(MoveForTest.GetMove(Piece.Rook, 8, 0, Turn.Black, Piece.SilverPromoted));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 17, 16, Turn.Black, Piece.LauncePromoted));
            moves.Add(MoveForTest.GetMove(Piece.Pawn, 50, 49, Turn.Black, Piece.Bishop));
            moves.Add(MoveForTest.GetMove(Piece.Gold, 79, 80, Turn.Black, Piece.Dragon));

            moves.ForEach(move => context.PutPiece(move));

            context.AssertAllAreEqual(expectedContext);

            Assert.AreEqual(1u, context.GetHandValueCount(Piece.Silver));
            Assert.AreEqual(2u, context.GetHandValueCount(Piece.Knight));
            Assert.AreEqual(1u, context.GetHandValueCount(Piece.Bishop));
            Assert.AreEqual(1u, context.GetHandValueCount(Piece.Launce));
            Assert.AreEqual(1u, context.GetHandValueCount(Piece.Rook));

            // Undo処理
            context.UndoPutPiece();
            context.UndoPutPiece();
            context.UndoPutPiece();
            context.UndoPutPiece();
            context.UndoPutPiece();
            context.UndoPutPiece();

            context.AssertAllAreEqual(beforePutContext);

            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Silver));
            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Knight));
            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Bishop));
            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Launce));
            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Rook));
        }

        [TestMethod]
        public void TestSetDefaultStartPosition()
        {
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();

            // 歩
            for (int i = 2; i <= 74; i += 9)
            {
                Assert.AreEqual(Piece.Pawn, context.GetPiece(i));
                Assert.IsTrue(context.IsOccupied(i, Turn.White));
            }

            for (int i = 6; i <= 78; i += 9)
            {
                Assert.AreEqual(Piece.Pawn, context.GetPiece(i));
                Assert.IsTrue(context.IsOccupied(i, Turn.Black));
            }

            // 香
            Assert.AreEqual(Piece.Launce, context.GetPiece(0));
            Assert.AreEqual(Piece.Launce, context.GetPiece(72));
            Assert.IsTrue(context.IsOccupied(0, Turn.White));
            Assert.IsTrue(context.IsOccupied(72, Turn.White));

            Assert.AreEqual(Piece.Launce, context.GetPiece(8));
            Assert.AreEqual(Piece.Launce, context.GetPiece(80));
            Assert.IsTrue(context.IsOccupied(8, Turn.Black));
            Assert.IsTrue(context.IsOccupied(80, Turn.Black));

            // 桂
            Assert.AreEqual(Piece.Knight, context.GetPiece(9));
            Assert.AreEqual(Piece.Knight, context.GetPiece(63));
            Assert.IsTrue(context.IsOccupied(9, Turn.White));
            Assert.IsTrue(context.IsOccupied(63, Turn.White));

            Assert.AreEqual(Piece.Knight, context.GetPiece(17));
            Assert.AreEqual(Piece.Knight, context.GetPiece(71));
            Assert.IsTrue(context.IsOccupied(17, Turn.Black));
            Assert.IsTrue(context.IsOccupied(71, Turn.Black));

            // 銀
            Assert.AreEqual(Piece.Silver, context.GetPiece(18));
            Assert.AreEqual(Piece.Silver, context.GetPiece(54));
            Assert.IsTrue(context.IsOccupied(18, Turn.White));
            Assert.IsTrue(context.IsOccupied(54, Turn.White));

            Assert.AreEqual(Piece.Silver, context.GetPiece(26));
            Assert.AreEqual(Piece.Silver, context.GetPiece(62));
            Assert.IsTrue(context.IsOccupied(26, Turn.Black));
            Assert.IsTrue(context.IsOccupied(62, Turn.Black));

            // 金
            Assert.AreEqual(Piece.Gold, context.GetPiece(27));
            Assert.AreEqual(Piece.Gold, context.GetPiece(45));
            Assert.IsTrue(context.IsOccupied(27, Turn.White));
            Assert.IsTrue(context.IsOccupied(45, Turn.White));

            Assert.AreEqual(Piece.Gold, context.GetPiece(35));
            Assert.AreEqual(Piece.Gold, context.GetPiece(53));
            Assert.IsTrue(context.IsOccupied(35, Turn.Black));
            Assert.IsTrue(context.IsOccupied(53, Turn.Black));

            // 角
            Assert.AreEqual(Piece.Bishop, context.GetPiece(10));
            Assert.IsTrue(context.IsOccupied(10, Turn.White));

            Assert.AreEqual(Piece.Bishop, context.GetPiece(70));
            Assert.IsTrue(context.IsOccupied(70, Turn.Black));

            // 飛車
            Assert.AreEqual(Piece.Rook, context.GetPiece(64));
            Assert.IsTrue(context.IsOccupied(64, Turn.White));

            Assert.AreEqual(Piece.Rook, context.GetPiece(16));
            Assert.IsTrue(context.IsOccupied(16, Turn.Black));

            // 玉
            Assert.AreEqual(Piece.King, context.GetPiece(36));
            Assert.IsTrue(context.IsOccupied(36, Turn.White));

            Assert.AreEqual(Piece.King, context.GetPiece(44));
            Assert.IsTrue(context.IsOccupied(44, Turn.Black));
        }

        /// <summary>
        /// 成る手を指すテストを実施します。
        /// </summary>
        [TestMethod]
        public void TestPutPromotePiece()
        {
            BoardContextForTest expectedContext = new BoardContextForTest(Turn.Black);
            expectedContext.AddPiece(Piece.Dragon, 74);
            expectedContext.AddPiece(Piece.Pawn, 66);
            expectedContext.AddPiece(Piece.Pawn, 76);

            expectedContext.AddHandValue(Piece.Pawn);

            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.Rook, 75);
            context.AddOppositePiece(Piece.Pawn, 74);
            context.AddPiece(Piece.Pawn, 66);
            context.AddPiece(Piece.Pawn, 76);

            BoardContextForTest beforePutContext = context.Clone();

            List<uint> moves = new List<uint>();
            moves.Add(MoveForTest.GetMove(Piece.Dragon, 75, 74, Turn.Black, Piece.Pawn, Promote.Yes));

            moves.ForEach(move => context.PutPiece(move));

            context.AssertAllAreEqual(expectedContext);

            Assert.AreEqual(1u, context.GetHandValueCount(Piece.Pawn));

            // Undo処理
            context.UndoPutPiece();

            context.AssertAllAreEqual(beforePutContext);

            Assert.AreEqual(0u, context.GetHandValueCount(Piece.Pawn));
        }

        /// <summary>
        /// 持ち駒の打ち手テストを実施します。
        /// </summary>
        [TestMethod]
        public void TestPutHandvaluePiece()
        {
            foreach (Turn turn in Enum.GetValues(typeof(Turn)))
            {
                foreach (Piece targetPiece in ExtensionPiece.PiecesRemovedPromoted)
                {
                    if (targetPiece == Piece.King) { continue; }

                    BoardContextForTest expectedContext = new BoardContextForTest(turn);
                    expectedContext.AddPiece(targetPiece, 30);

                    BoardContextForTest context = new BoardContextForTest(turn);
                    context.AddHandValue(targetPiece);

                    BoardContextForTest beforePutContext = context.Clone();

                    List<uint> moves = new List<uint>();
                    moves.Add(MoveForTest.GetHandValueMove(targetPiece, 30, context.Turn));

                    moves.ForEach(move => context.PutPiece(move));

                    context.AssertAllAreEqual(expectedContext);

                    Assert.AreEqual(0u, context.GetHandValueCount(targetPiece));

                    // Undo処理
                    context.UndoPutPiece();

                    context.AssertAllAreEqual(beforePutContext);

                    Assert.AreEqual(1u, context.GetHandValueCount(targetPiece));
                }
            }            
        }
    }
}
