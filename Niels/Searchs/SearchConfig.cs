using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Evaluators;

namespace Niels.Searchs
{
    /// <summary>
    /// 探索設定情報
    /// </summary>
    public class SearchConfig
    {
        /// <summary>
        /// 探索する深さ
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 利き生成機能を提供します。
        /// TODO:ここもクラスにしたい
        /// </summary>
        public Func<BoardContext, IEnumerable<uint>> MoveGenerate { get; set; }

        /// <summary>
        /// 指し手をソートする機能を提供します。
        /// </summary>
        public IOrder Order { get; set; }

        /// <summary>
        /// 指し手の評価機能を提供します。
        /// </summary>
        public Evaluator Evaluator { get; set; }

        /// <summary>
        /// ソートを行う最大の深さ
        /// </summary>
        public int MoveOrderingDepth { get; set; }
    }
}
