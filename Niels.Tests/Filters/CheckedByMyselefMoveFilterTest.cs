using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Filters;
using Niels.Tests.TestHelper;

namespace Niels.Tests.Filters
{
    [TestClass]
    public class CheckedByMyselefMoveFilterTest
    {
        [TestMethod]
        public void TestCheckedByMyselefMoveFilter()
        {
            // 自殺手じゃない通常の指し手
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 35);
            bool beforeIsChecked = context.AdditionalInfo.IsChecked;
            BoardContextForTest beforeContext = context.Clone();
            uint move = MoveForTest.GetMove(Piece.Gold, 35, 34, Turn.Black);

            bool expected = true;
            bool actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 王手になっているのに回避しなかったらアウト
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 35);
            context.AddOppositePiece(Piece.PawnPromoted, 43);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 35, 34, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 王手になっているのに持ち駒を打って回避しなかったらアウト
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 35);
            context.AddOppositePiece(Piece.PawnPromoted, 43);
            context.AddHandValue(Piece.Silver);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetHandValueMove(Piece.Silver, 58, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 王手になっているのに持ち駒を打って回避しなかったらアウトその２
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 35);
            context.AddOppositePiece(Piece.Rook, 40);
            context.AddHandValue(Piece.Pawn);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetHandValueMove(Piece.Pawn, 1, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);


            // 王手になっていて回避していればセーフ
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 35);
            context.AddOppositePiece(Piece.PawnPromoted, 43);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 35, 43, Turn.Black, Piece.PawnPromoted, Promote.No);

            expected = true;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 王手になっていないが、敵の飛び利きの遮りを解除したらアウト
            // 角
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 60);
            context.AddOppositePiece(Piece.Bishop, 76);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 60, 59, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 竜馬
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 60);
            context.AddOppositePiece(Piece.Horse, 76);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 60, 59, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 飛車
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 43);
            context.AddOppositePiece(Piece.Rook, 39);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 43, 33, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 竜王
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 43);
            context.AddOppositePiece(Piece.Dragon, 39);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 43, 33, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 香車
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 43);
            context.AddOppositePiece(Piece.Launce, 39);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 43, 33, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 敵の飛び利きを遮っている駒を移動したが、移動後も遮り続けていればセーフ
            // 角
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 60);
            context.AddOppositePiece(Piece.Bishop, 76);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 60, 68, Turn.Black);

            expected = true;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 角
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddPiece(Piece.Gold, 60);
            context.AddPiece(Piece.Silver, 52);
            context.AddOppositePiece(Piece.Bishop, 76);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.Gold, 60, 59, Turn.Black);

            expected = true;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 自玉が移動して自爆したらアウト
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddOppositePiece(Piece.PawnPromoted, 42);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.King, 44, 43, Turn.Black);

            expected = false;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);

            // 自玉が移動しても自爆していいなかったらセーフ
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 44);
            context.AddOppositePiece(Piece.King, 36);

            context.AddOppositePiece(Piece.PawnPromoted, 42);
            beforeIsChecked = context.AdditionalInfo.IsChecked;
            beforeContext = context.Clone();
            move = MoveForTest.GetMove(Piece.King, 44, 35, Turn.Black);

            expected = true;
            actual = FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(beforeIsChecked, context.AdditionalInfo.IsChecked);
            context.AssertAllAreEqual(beforeContext);
        }
    }
}
