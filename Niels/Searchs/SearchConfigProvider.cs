using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Generates;
using Niels.Evaluators;
using Niels.Orders;

namespace Niels.Searchs
{
    /// <summary>
    /// 探索設定情報を提供する
    /// </summary>
    public static class SearchConfigProvider
    {
        /// <summary>
        /// デフォルト設定情報
        /// </summary>
        public static SearchConfig DefaultSearchConfig { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static SearchConfigProvider()
        {
            DefaultSearchConfig = new SearchConfig();
            DefaultSearchConfig.Depth = 3;
            DefaultSearchConfig.MoveGenerate = MoveProvider.GetAllMoves;
            DefaultSearchConfig.Evaluator = new ScoreIndexEvaluator();
            DefaultSearchConfig.Order = new DummyOrder();
        }
    }
}
