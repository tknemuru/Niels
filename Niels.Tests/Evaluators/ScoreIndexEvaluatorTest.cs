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
using Niels.Players;

namespace Niels.Tests.Evaluators
{
    [TestClass]
    public class ScoreIndexEvaluatorTest
    {
        [TestMethod]
        public void TestEvaluate()
        {
            ScoreIndexEvaluator ev = new ScoreIndexEvaluator();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            uint move = MoveForTest.GetMove(Piece.Pawn, 60, 59, Turn.Black);
            context.PutPiece(move);
            ev.Evaluate(context);
        }

        [TestMethod]
        public void TestEvaluateAndSearch()
        {
            ScoreIndexEvaluator ev = new ScoreIndexEvaluator();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            IEnumerable<uint> move = MoveProvider.GetAllMoves(context, GenerateTarget.All);
            uint bestMove = new CpuPlayer().Put(context);
        }
    }
}
