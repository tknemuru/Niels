using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Fools;
using Niels.Tests.TestHelper;
using Niels.Helper;
using Niels.Evaluators;
using System.Collections.Generic;
using System.Linq;

namespace Niels.Tests.Evaluators
{
    [TestClass]
    public class SequencePositionFeatureVectorTest
    {
        /// <summary>
        /// インデックスを構成する駒数
        /// </summary>
        private const int PieceCount = 17;

        [TestMethod]
        public void TestGetNormalizedScoreIndex()
        {
            // 基準
            int scoreIndex = 0;
            List<TurnPiece> pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackPawn);
            pieces.Add(TurnPiece.WhiteLaunce);
            pieces.Add(TurnPiece.BlackKnight);
            pieces.Add(TurnPiece.WhiteSilver);
            scoreIndex = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));

            // リバース
            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.WhiteSilver);
            pieces.Add(TurnPiece.BlackKnight);
            pieces.Add(TurnPiece.WhiteLaunce);
            pieces.Add(TurnPiece.BlackPawn);
            int otherScoreIndex1 = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));

            // ターン逆
            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.WhitePawn);
            pieces.Add(TurnPiece.BlackLaunce);
            pieces.Add(TurnPiece.WhiteKnight);
            pieces.Add(TurnPiece.BlackSilver);
            int otherScoreIndex2 = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));

            // ターン逆＆リバース
            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackSilver);
            pieces.Add(TurnPiece.WhiteKnight);
            pieces.Add(TurnPiece.BlackLaunce);
            pieces.Add(TurnPiece.WhitePawn);
            int otherScoreIndex3 = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));

            FileHelper.WriteLine(RadixConvertHelper.ToString(Math.Abs(scoreIndex), PieceCount, 4, true));
            FileHelper.WriteLine(RadixConvertHelper.ToString(Math.Abs(otherScoreIndex1), PieceCount, 4, true));
            FileHelper.WriteLine(RadixConvertHelper.ToString(Math.Abs(otherScoreIndex2), PieceCount, 4, true));
            FileHelper.WriteLine(RadixConvertHelper.ToString(Math.Abs(otherScoreIndex3), PieceCount, 4, true));

            Assert.AreEqual(Math.Abs(scoreIndex), Math.Abs(otherScoreIndex1));
            Assert.AreEqual(Math.Abs(scoreIndex), Math.Abs(otherScoreIndex2));
            Assert.AreEqual(Math.Abs(scoreIndex), Math.Abs(otherScoreIndex3));

            int minusCount = 0;
            if (scoreIndex < 0) { minusCount++; }
            if (otherScoreIndex1 < 0) { minusCount++; }
            if (otherScoreIndex2 < 0) { minusCount++; }
            if (otherScoreIndex3 < 0) { minusCount++; }

            Assert.AreEqual(2, minusCount);
        }

        [TestMethod]
        public void TestScoreIndexGenerate()
        {
            // 斜め（上⇒左）
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.Pawn, 45);
            context.AddOppositePiece(Piece.Launce, 55);
            context.AddPiece(Piece.Knight, 65);
            context.AddOppositePiece(Piece.Silver, 75);

            List<TurnPiece> pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackPawn);
            pieces.Add(TurnPiece.WhiteLaunce);
            pieces.Add(TurnPiece.BlackKnight);
            pieces.Add(TurnPiece.WhiteSilver);
            int expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            int expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            var scoreIndexs = SequencePositionFeatureVector.Generate(context);
            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 斜め（上⇒左）の続き。成り駒は大丈夫？
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 40);
            context.AddOppositePiece(Piece.Horse, 50);
            context.AddPiece(Piece.Dragon, 60);
            //context.AddPiece(Piece.Empty, 70);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.Empty);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 斜め（上⇒左）の続き。端っこのテスト
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 50);
            context.AddOppositePiece(Piece.Horse, 60);
            context.AddPiece(Piece.Dragon, 70);
            context.AddOppositePiece(Piece.Pawn, 80);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 斜め（上⇒右）
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 32);
            context.AddOppositePiece(Piece.Horse, 24);
            context.AddPiece(Piece.Dragon, 16);
            context.AddOppositePiece(Piece.Pawn, 8);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 横
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 35);
            context.AddOppositePiece(Piece.Horse, 26);
            context.AddPiece(Piece.Dragon, 17);
            context.AddOppositePiece(Piece.Pawn, 8);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 横
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 72);
            context.AddOppositePiece(Piece.Horse, 63);
            context.AddPiece(Piece.Dragon, 54);
            context.AddOppositePiece(Piece.Pawn, 45);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 横
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 27);
            context.AddOppositePiece(Piece.Horse, 18);
            context.AddPiece(Piece.Dragon, 9);
            context.AddOppositePiece(Piece.Pawn, 0);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 縦
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 0);
            context.AddOppositePiece(Piece.Horse, 1);
            context.AddPiece(Piece.Dragon, 2);
            context.AddOppositePiece(Piece.Pawn, 3);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 縦
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 5);
            context.AddOppositePiece(Piece.Horse, 6);
            context.AddPiece(Piece.Dragon, 7);
            context.AddOppositePiece(Piece.Pawn, 8);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 縦
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 72);
            context.AddOppositePiece(Piece.Horse, 73);
            context.AddPiece(Piece.Dragon, 74);
            context.AddOppositePiece(Piece.Pawn, 75);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);

            // 縦
            context = new BoardContextForTest(Turn.Black);
            context.AddPiece(Piece.King, 15);
            context.AddOppositePiece(Piece.King, 19);
            context.AddPiece(Piece.SilverPromoted, 77);
            context.AddOppositePiece(Piece.Horse, 78);
            context.AddPiece(Piece.Dragon, 79);
            context.AddOppositePiece(Piece.Pawn, 80);

            pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackGold);
            pieces.Add(TurnPiece.WhiteBishop);
            pieces.Add(TurnPiece.BlackRook);
            pieces.Add(TurnPiece.WhitePawn);
            expectedKey = SequencePositionFeatureVector.GetNormalizedScoreIndex(this.GetScoreIndex(pieces));
            expectedValue = 1;
            if (expectedKey < 0)
            {
                expectedKey = -expectedKey;
                expectedValue = -1;
            }

            scoreIndexs = SequencePositionFeatureVector.Generate(context);
            FileHelper.WriteLine(context.ToString());

            Assert.IsTrue(scoreIndexs.ContainsKey(expectedKey));
            Assert.AreEqual(scoreIndexs[expectedKey], expectedValue);
        }

        /// <summary>
        /// スコアインデックスを取得します。
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        private int GetScoreIndex(List<TurnPiece> pieces)
        {
            int scoreIndex = 0;
            for (int i = 0; i < pieces.Count(); i++)
            {
                scoreIndex += SequencePositionFeatureVector.GetPieceIndex(pieces[i]);
                if (i < pieces.Count - 1)
                {
                    scoreIndex *= PieceCount;
                }
            }
            return scoreIndex;
        }
    }
}
