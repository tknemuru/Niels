using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Collections;
using Niels.Boards;
using Niels.Searchs;
using Niels.Fools;
using Niels.Helper;
using Niels.Tests.TestHelper;
using Niels.Evaluators;
using Niels.Orders;
using Niels.Diagnostics;

namespace Niels.Tests.Searchs
{
    /// <summary>
    /// PVS探索テストクラス
    /// </summary>
    [TestClass]
    public class PrincipalVariationSearchTest
    {
        [TestMethod]
        public void TestPrincipalVariationSearchGetMove()
        {
            Dictionary<int, int> values = new Dictionary<int, int>();
            values.Add(1, 3);
            values.Add(2, -1);
            values.Add(3, 8);
            values.Add(5, 2);
            values.Add(6, -8);
            values.Add(7, 11);
            values.Add(9, -6);
            values.Add(10, -7);
            values.Add(11, 13);
            uint expectedMove = Move.GetMove(Piece.Pawn, BoardType.Main, 2, BoardType.Main, 0, Turn.Black);
            int expectedValue = -1;

            FoolMoveGenerator gen = new FoolMoveGenerator(3);
            FoolEvaluator ev = new FoolEvaluator(values);
            FoolOrder order = new FoolOrder();
            SearchConfig config = new SearchConfig();
            config.MoveGenerate = gen.Generate;
            config.Evaluator = ev;
            config.Order = order;
            config.Depth = 2;
            PrincipalVariationSearchForTest search = new PrincipalVariationSearchForTest(config);
            BoardContext context = new BoardContext(Turn.Black);
            search.GetMove(context);

            foreach (NodeDebugInfo info in search.NodeDebugInfos.Values)
            {
                FileHelper.WriteLine(info.ToString(), @"./result/search_debug.txt");
            }

            //Assert.AreEqual(expectedMove, search.BestMove);
            Assert.AreEqual(expectedValue, search.BestValue);
        }

        [TestMethod]
        public void TestPrincipalVariationSearchGetMoveUsingIterator()
        {
            // 深さ１
            uint expectedMove = Move.GetMove(Piece.Pawn, BoardType.Main, 0, BoardType.Main, 0, Turn.Black);
            int expectedValue = IteratorEvaluator.DescMaxValue - 1;

            FoolMoveGenerator gen = new FoolMoveGenerator(3);
            IteratorEvaluator ev = new IteratorEvaluator(IteratorEvaluator.IteratorOrder.Desc);
            DummyOrder order = new DummyOrder();
            SearchConfig config = new SearchConfig();
            config.MoveGenerate = gen.Generate;
            config.Evaluator = ev;
            config.Order = order;
            config.Depth = 1;
            PrincipalVariationSearchForTest search = new PrincipalVariationSearchForTest(config);
            BoardContext context = new BoardContext(Turn.Black);
            search.GetMove(context);

            Assert.AreEqual(expectedMove, search.BestMove);
            Assert.AreEqual(expectedValue, search.BestValue);

            // 深さ２
            //expectedMove = Move.GetMove(Piece.Pawn, BoardType.Main, 3, BoardType.Main, 0, Turn.Black);
            //expectedValue = IteratorEvaluator.DescMaxValue - 1;

            //gen = new FoolMoveGenerator(3);
            //ev = new IteratorEvaluator(IteratorEvaluator.IteratorOrder.Desc);
            //order = new DummyOrder();
            //config = new SearchConfig();
            //config.MoveGenerate = gen.Generate;
            //config.Evaluator = ev;
            //config.Order = order;
            //config.Depth = 3;
            //search = new PrincipalVariationSearchForTest(config);
            //context = new BoardContext(Turn.Black);
            //search.GetMove(context);

            //foreach (NodeDebugInfo info in search.NodeDebugInfos.Values)
            //{
            //    FileHelper.WriteLine(info.ToString(), @"./result/search_debug.txt");
            //}

            //FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            //FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            //FileHelper.WriteLine(MoveDebug.ToDebugString(expectedMove), @"./test/expected_move.txt");
            //FileHelper.WriteLine(expectedValue.ToString(), @"./test/expected_move.txt");
            //FileHelper.WriteLine(MoveDebug.ToDebugString(search.BestMove), @"./test/actual_move.txt");
            //FileHelper.WriteLine(search.BestValue.ToString(), @"./test/actual_move.txt");

            //Assert.AreEqual(expectedMove, search.BestMove);
            //Assert.AreEqual(expectedValue, search.BestValue);
        }

        [TestMethod]
        public void TestPrincipalVariationSearchGetMoveStardardExecute()
        {
            PrincipalVariationSearchForTest search = new PrincipalVariationSearchForTest(SearchConfigProvider.DefaultSearchConfig);
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            uint move = search.GetMove(context);
        }

        /// <summary>
        /// テスト用探索クラス
        /// </summary>
        private class PrincipalVariationSearchForTest : PrincipalVariationSearch
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="gen"></param>
            /// <param name="ev"></param>
            public PrincipalVariationSearchForTest(SearchConfig config)
                : base(config)
            {
            }

            /// <summary>
            /// 探索の前処理を行う
            /// </summary>
            protected override void SearchSetUp(BoardContext context, uint move)
            {
            }

            /// <summary>
            /// 探索の後処理を行う
            /// </summary>
            protected override void SearchTearDown(BoardContext context, uint move)
            {
            }
        }
    }
}
