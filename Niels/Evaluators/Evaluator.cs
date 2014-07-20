using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Searchs;
using Niels.Collections;

namespace Niels.Evaluators
{
    /// <summary>
    /// 盤面を評価する機能を提供する
    /// </summary>
    public abstract class Evaluator
    {
        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        public abstract int Evaluate(BoardContext context);

        /// <summary>
        /// パリティ
        /// </summary>
        public int GetParity(BoardContext context)
        {
            return (context.Turn == Turn.Black) ? 1 : -1;
        }
    }
}
