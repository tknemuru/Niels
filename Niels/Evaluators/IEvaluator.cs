using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Searchs;

namespace Niels.Evaluators
{
    /// <summary>
    /// 盤面を評価する機能を提供する
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
#if DEBUG
        int Evaluate(BoardContext context, int nodeId);
#else
        int Evaluate(BoardContext context, int nodeId);
#endif
    }
}
