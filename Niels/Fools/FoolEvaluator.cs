using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Evaluators;
using Niels.Boards;
using Niels.Searchs;

namespace Niels.Fools
{
    /// <summary>
    /// 与えられた評価値をそのまま返す
    /// </summary>
    public class FoolEvaluator : IEvaluator
    {
        /// <summary>
        /// 各末端の評価値
        /// </summary>
        private Dictionary<int, int> Values { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="values"></param>
        public FoolEvaluator(Dictionary<int, int> values)
        {
            this.Values = values;
        }

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        public int Evaluate(BoardContext context, int nodeId)
        {
            return this.Values[nodeId];
        }
    }
}
