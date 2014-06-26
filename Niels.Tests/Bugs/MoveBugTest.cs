using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Fools;
using Niels.Tests.TestHelper;
using Niels.Helper;
using Niels.Usi;
using Niels.Searchs;
using Niels.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.Bugs
{
    /// <summary>
    /// 指し手に関するバグの調査・テストを行ないます。
    /// </summary>
    [TestClass]
    public class MoveBugTest
    {
        [TestMethod]
        public void TestBugGetMove()
        {
            var order = new FoolOrder();
            UsiCommandReceiverForTest receiver = new UsiCommandReceiverForTest();
            UsiCommand command = UsiCommand.Parse(@"position startpos moves 4i3h 8c8d 2h1h 9c9d 7g7f 7c7d 5i6h 7d7e 5g5f 7e7f 6i7h 8d8e 3i4h 9d9e 6h5h 9e9f 8g8f 9f9g 8h9g 8e8f 3h2h 8f8g");
            BoardContext context = receiver.ReceivePositionForTest(command);

            var expected = new List<uint>();
            expected = order.MoveOrdering(expected).ToList();

            // ここで指し先に駒がいるエラーが発生していた。
            // →UpRightの自駒存在チェックが間違っていた。（括弧の位置が間違っていた。）
            var actual = new RandomSearch().GetMove(context);
        }

        [TestMethod]
        public void TestBugMoveChoicePromoteMove()
        {
            SfenNotationForTest notation = new SfenNotationForTest();
            UsiCommandReceiverForTest receiver = new UsiCommandReceiverForTest();
            UsiCommand command = UsiCommand.Parse(@"position startpos moves 3i3h 5c5d 9i9h 6c6d 9g9f 7c7d 5i4h 8c8d 4h5h 9c9d 7i6h 9d9e 9h9g 9e9f 6h5i 8d8e 7g7f 7d7e 8h6f 7e7f 5h6h 8e8f 6f7e 8f8g+");
            BoardContext context = receiver.ReceivePositionForTest(command);

            uint expected = Move.GetMove(Piece.Horse, notation.GetBoardTypeForTest("7e"), (uint)notation.GetIndexForTest("7e"), notation.GetBoardTypeForTest("9c"), (uint)notation.GetIndexForTest("9c"), Turn.Black, Piece.Empty, Promote.Yes);
            // ここで本来なら成る利きが優先的に選ばれるはずだが、選ばれない。
            // →GetBishopMovesの中身がNoPromoteになっていた…
            uint actual = new RandomSearch().GetMove(context);

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            FileHelper.WriteLine(MoveForTest.ToDebugString(expected), @"./test/expected_move.txt");
            FileHelper.WriteLine(MoveForTest.ToDebugString(actual), @"./test/actual_move.txt");

            //Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBugCrushPromoteMove()
        {
            SfenNotationForTest notation = new SfenNotationForTest();
            UsiCommandReceiverForTest receiver = new UsiCommandReceiverForTest();
            UsiCommand command = UsiCommand.Parse(@"position startpos moves 9i9h 6c6d 3i4h 7c7d 4g4f 7d7e 8g8f 5c5d 4i3i 8c8d 2h3h 7e7f 5i4i 7f7g+ 4f4e 6d6e 1g1f 7g7f 8h3c+ 5a6b");
            BoardContext context = receiver.ReceivePositionForTest(command);

            //uint expected = Move.GetMove(Piece.Horse, notation.GetBoardTypeForTest("7e"), (uint)notation.GetIndexForTest("7e"), notation.GetBoardTypeForTest("9c"), (uint)notation.GetIndexForTest("9c"), Turn.Black, Piece.Empty, Promote.Yes);
            // ここでクラッシュする
            // →Move.Promoteが全ての元凶だった。駒を成るだけでなく、毎回成る指し手にしてしまっていた。
            //   Promoteは成る指し手にするだけで、実際の駒の更新はMove.SetPutPieceで行なうように修正した。
            uint move = new RandomSearch().GetMove(context);
        }

        [TestMethod]
        public void TestBugCheckedByMyselfMiss()
        {
            SfenNotationForTest notation = new SfenNotationForTest();
            UsiCommandReceiverForTest receiver = new UsiCommandReceiverForTest();
            UsiCommand command = UsiCommand.Parse(@"position startpos moves 1i1h 8c8d 9i9h 7c7d 8h9i 6c6d 2h8h 5c5d 1g1f 4c4d 1f1e 3c3d 7i7h 2c2d 6i6h 5d5e 6h6i 3d3e 1h1g 4d4e 1e1d 1c1d 1g1e 1d1e 9g9f 8d8e 8g8f 8e8f 9f9e 7d7e 9e9d 9c9d 9h9d 9a9d P*8c 8b8c 4g4f 4e4f 6g6f 7e7f 7g7f 6d6e 6f6e 8c6c 5g5f 6c6e 5f5e 6e5e");
            BoardContext context = receiver.ReceivePositionForTest(command);
            uint move = new SimpleSearch().GetMove(context);
            uint errorMove = MoveForTest.GetHandValueMove(Piece.Pawn, 1, Turn.Black);
            FileHelper.WriteLine(MoveForTest.ToDebugString(move), @"./test/actual_move.txt");
            Assert.AreNotEqual(move, errorMove);
        }
    }
}
