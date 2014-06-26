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
using Niels.NormalizedScoreIndexGenerating.Tests.TestHelper;
using System.Collections.Generic;
using System.Linq;

namespace Niels.NormalizedScoreIndexGenerating.Tests
{
    [TestClass]
    public class NormalizedScoreIndexGeneratorTest
    {
        /// <summary>
        /// インデックスを構成する駒数
        /// </summary>
        private const int PieceCount = 17;

        [TestMethod]
        public void TestReverse()
        {
            // リバース
            int scoreIndex = 0;
            List<TurnPiece> pieces = new List<TurnPiece>();
            pieces.Add(TurnPiece.BlackPawn);
            pieces.Add(TurnPiece.WhiteLaunce);
            pieces.Add(TurnPiece.BlackKnight);
            pieces.Add(TurnPiece.WhiteSilver);

            for (int i = 0; i < pieces.Count(); i++)
            {
                scoreIndex += SequencePositionFeatureVector.GetPieceIndex(pieces[i]);
                if (i < pieces.Count - 1)
                {
                    scoreIndex *= PieceCount;
                }
            }
            int actual = NormalizedScoreIndexGeneratorForTest.ReverseForTest(scoreIndex);

            int expected = 0;
            List<TurnPiece> expectedPieces = new List<TurnPiece>();
            expectedPieces.Add(TurnPiece.WhiteSilver);
            expectedPieces.Add(TurnPiece.BlackKnight);
            expectedPieces.Add(TurnPiece.WhiteLaunce);
            expectedPieces.Add(TurnPiece.BlackPawn);

            for (int i = 0; i < expectedPieces.Count(); i++)
            {
                expected += SequencePositionFeatureVector.GetPieceIndex(expectedPieces[i]);
                if (i < expectedPieces.Count - 1)
                {
                    expected *= PieceCount;
                }
            }

            Assert.AreEqual(expected, actual);

            // ターン逆
            actual = NormalizedScoreIndexGeneratorForTest.ChangeTurnForTest(scoreIndex);

            expected = 0;
            expectedPieces = new List<TurnPiece>();
            expectedPieces.Add(TurnPiece.WhitePawn);
            expectedPieces.Add(TurnPiece.BlackLaunce);
            expectedPieces.Add(TurnPiece.WhiteKnight);
            expectedPieces.Add(TurnPiece.BlackSilver);

            for (int i = 0; i < expectedPieces.Count(); i++)
            {
                expected += SequencePositionFeatureVector.GetPieceIndex(expectedPieces[i]);
                if (i < expectedPieces.Count - 1)
                {
                    expected *= PieceCount;
                }
            }

            Assert.AreEqual(expected, actual);

            // ターン逆＆リバース
            actual = NormalizedScoreIndexGeneratorForTest.ChangeTurnAndReverseForTest(scoreIndex);

            expected = 0;
            expectedPieces = new List<TurnPiece>();
            expectedPieces.Add(TurnPiece.BlackSilver);
            expectedPieces.Add(TurnPiece.WhiteKnight);
            expectedPieces.Add(TurnPiece.BlackLaunce);
            expectedPieces.Add(TurnPiece.WhitePawn);

            for (int i = 0; i < expectedPieces.Count(); i++)
            {
                expected += SequencePositionFeatureVector.GetPieceIndex(expectedPieces[i]);
                if (i < expectedPieces.Count - 1)
                {
                    expected *= PieceCount;
                }
            }
        }
    }
}
