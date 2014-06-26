using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Orders;
using Niels.Fools;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.Collections
{
    [TestClass]
    public class MoveTest
    {
        [TestMethod]
        public void TestGetMove()
        {
            uint[] expectedValue = { 135270465, 85066230, 30404584 };

            uint[] actualValue = new uint[3];
            actualValue[0] = Move.GetMove(Piece.Pawn, BoardType.SubRight, 1, BoardType.SubRight , 0, Turn.Black);
            actualValue[1] = Move.GetMove(Piece.Horse, BoardType.Main, 7, BoardType.SubRight, 8, Turn.White, Piece.SilverPromoted);
            actualValue[2] = Move.GetMove(Piece.King, BoardType.SubBottom, 63, BoardType.Main, 63, Turn.White, Piece.Rook);

            CollectionAssert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetMoveOverload()
        {
            var order = new FoolOrder();

            List<uint> expectedValue = new List<uint>();
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 1, BoardType.SubRight , 0, Turn.Black));
            expectedValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 0, BoardType.SubRight, 1, Turn.White, Piece.Empty));
            expectedValue = order.MoveOrdering(expectedValue).ToList();

            List<uint> actualValue = new List<uint>();
            actualValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 1, BoardType.SubRight, 0, Turn.Black));
            actualValue.Add(Move.GetMove(Piece.Pawn, BoardType.SubRight, 0, BoardType.SubRight, 1, Turn.White));
            actualValue = order.MoveOrdering(actualValue).ToList();

            CollectionAssert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestSetCapturePiece()
        {
            uint move = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Bishop);
            uint actual = move.SetCapturePiece(Piece.SilverPromoted);
            uint expected = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.SilverPromoted);            
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Dragon);
            actual = move.SetCapturePiece(Piece.Pawn);
            expected = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Pawn);
            Assert.AreEqual(expected, actual);

            move = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Dragon);
            actual = move.SetCapturePiece(Piece.Empty);
            expected = Move.GetMove(Piece.Rook, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Empty);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsPromote()
        {
            uint move = Move.GetMove(Piece.Dragon, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Bishop);
            Assert.IsFalse(move.IsPromote());
            move = Move.GetMove(Piece.Dragon, BoardType.Main, 58, BoardType.Main, 34, Turn.Black, Piece.Bishop, Promote.Yes);
            Assert.IsTrue(move.IsPromote());
        }

        [TestMethod]
        public void TestCanPromoteBlack()
        {
            // -> 先手
            // 玉、金は成れない
            uint move = Move.GetMove(Piece.King, BoardType.Main, 59, BoardType.Main, 58, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Gold, BoardType.Main, 59, BoardType.Main, 58, Turn.Black);
            Assert.IsFalse(move.CanPromote());

            // 既に成っていたらNG
            move = Move.GetMove(Piece.SilverPromoted, BoardType.Main, 59, BoardType.Main, 58, Turn.Black);
            Assert.IsFalse(move.CanPromote());

            // 敵陣内へ入るとき、敵陣内で移動するとき、敵陣内から出るときに成れる
            // 敵陣内へ入るとき
            // メイン→メイン
            move = Move.GetMove(Piece.Silver, BoardType.Main, 59, BoardType.Main, 58, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 下→メイン
            move = Move.GetMove(Piece.Rook, BoardType.SubBottom, 63, BoardType.Main, 56, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→メイン
            move = Move.GetMove(Piece.Knight, BoardType.SubRight, 3, BoardType.Main, 1, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Launce, BoardType.SubRight, 3, BoardType.SubRight, 2, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Knight, BoardType.Main, 3, BoardType.SubRight, 1, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 下→右
            move = Move.GetMove(Piece.Bishop, BoardType.SubBottom, 47, BoardType.SubRight, 2, Turn.Black);
            Assert.IsTrue(move.CanPromote());

            // 敵陣内で移動するとき
            // 右→メイン
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 0, BoardType.Main, 56, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Launce, BoardType.Main, 0, BoardType.SubRight, 0, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.Main, 56, BoardType.SubRight, 0, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // メイン→メイン
            move = Move.GetMove(Piece.Rook, BoardType.Main, 56, BoardType.Main, 57, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Silver, BoardType.SubRight, 0, BoardType.SubRight, 1, Turn.Black);
            Assert.IsTrue(move.CanPromote());

            // 敵陣内から出るとき
            // メイン→メイン
            move = Move.GetMove(Piece.Silver, BoardType.Main, 58, BoardType.Main, 59, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // メイン→下
            move = Move.GetMove(Piece.Rook, BoardType.Main, 56, BoardType.SubBottom, 63, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Bishop, BoardType.Main, 2, BoardType.SubRight, 3, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Launce, BoardType.SubRight, 2, BoardType.SubRight, 3, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→メイン
            move = Move.GetMove(Piece.Bishop, BoardType.SubRight, 1, BoardType.Main, 11, Turn.Black);
            Assert.IsTrue(move.CanPromote());
            // 右→下
            move = Move.GetMove(Piece.Bishop, BoardType.SubRight, 2, BoardType.SubBottom, 47, Turn.Black);
            Assert.IsTrue(move.CanPromote());

            // 成れない場合
            move = Move.GetMove(Piece.Bishop, BoardType.SubBottom, 63, BoardType.Main, 19, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.Main, 63, BoardType.Main, 59, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubBottom, 63, BoardType.SubBottom, 39, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubBottom, 63, BoardType.SubRight, 8, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 8, BoardType.SubRight, 5, Turn.Black);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 5, BoardType.Main, 5, Turn.Black);
            Assert.IsFalse(move.CanPromote());
        }

        [TestMethod]
        public void TestCanPromoteWhite()
        {
            // -> 後手
            // 玉、金は成れない
            uint move = Move.GetMove(Piece.King, BoardType.Main, 31, BoardType.Main, 39, Turn.White);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Gold, BoardType.Main, 31, BoardType.Main, 23, Turn.White);
            Assert.IsFalse(move.CanPromote());

            // 既に成っていたらNG
            move = Move.GetMove(Piece.SilverPromoted, BoardType.Main, 37, BoardType.Main, 38, Turn.White);
            Assert.IsFalse(move.CanPromote());

            // 敵陣内へ入るとき、敵陣内で移動するとき、敵陣内から出るときに成れる
            // 敵陣内へ入るとき
            // メイン→メイン
            move = Move.GetMove(Piece.Silver, BoardType.Main, 61, BoardType.Main, 62, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // メイン→下
            move = Move.GetMove(Piece.Rook, BoardType.Main, 56, BoardType.SubBottom, 63, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→メイン
            move = Move.GetMove(Piece.Knight, BoardType.SubRight, 5, BoardType.Main, 6, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Launce, BoardType.SubRight, 5, BoardType.SubRight, 6, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Knight, BoardType.Main, 5, BoardType.SubRight, 6, Turn.White);
            Assert.IsTrue(move.CanPromote());

            // 敵陣内で移動するとき
            // 右→メイン
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 8, BoardType.Main, 63, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Launce, BoardType.Main, 63, BoardType.SubRight, 8, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // メイン→メイン
            move = Move.GetMove(Piece.Rook, BoardType.Main, 55, BoardType.Main, 47, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Silver, BoardType.SubRight, 8, BoardType.SubRight, 7, Turn.White);
            Assert.IsTrue(move.CanPromote());

            // 敵陣内から出るとき
            // メイン→メイン
            move = Move.GetMove(Piece.Silver, BoardType.Main, 62, BoardType.Main, 61, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 下→メイン
            move = Move.GetMove(Piece.Rook, BoardType.SubBottom, 63, BoardType.Main, 56, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // メイン→右
            move = Move.GetMove(Piece.Bishop, BoardType.Main, 6, BoardType.SubRight, 5, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→右
            move = Move.GetMove(Piece.Launce, BoardType.SubRight, 6, BoardType.SubRight, 5, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→メイン
            move = Move.GetMove(Piece.Bishop, BoardType.SubRight, 7, BoardType.Main, 13, Turn.White);
            Assert.IsTrue(move.CanPromote());
            // 右→下
            move = Move.GetMove(Piece.Bishop, BoardType.SubRight, 5, BoardType.SubBottom, 23, Turn.White);
            Assert.IsTrue(move.CanPromote());

            // 成れない場合
            move = Move.GetMove(Piece.Bishop, BoardType.Main, 19, BoardType.SubRight, 0, Turn.White);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.Main, 60, BoardType.Main, 44, Turn.White);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 2, BoardType.SubRight, 4, Turn.White);
            Assert.IsFalse(move.CanPromote());
            move = Move.GetMove(Piece.Rook, BoardType.SubRight, 5, BoardType.Main, 5, Turn.White);
            Assert.IsFalse(move.CanPromote());
        }
    }
}
