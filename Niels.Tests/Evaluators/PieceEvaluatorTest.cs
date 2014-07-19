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
    public class PieceEvaluatorTest
    {
        [TestMethod]
        public void TestPieceEvaluator()
        {
            PieceEvaluator ev = new PieceEvaluator();
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            ev.Evaluate(context, 0);
        }
    }
}
