using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Evaluators
{
    /// <summary>
    /// ランダムに評価値を返します。
    /// </summary>
    public class RandomEvaluator : Evaluator
    {
        /// <summary>
        /// 評価値を取得します。
        /// </summary>
        /// <returns></returns>
        public override int Evaluate(BoardContext context)
        {
            Random random = new Random();
            return random.Next(1000);
        }
    }
}
