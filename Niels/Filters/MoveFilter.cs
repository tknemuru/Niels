using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Filters
{
    /// <summary>
    /// 指し手のフィルタ機能を提供します。
    /// </summary>
    public abstract class MoveFilter
    {
        /// <summary>
        /// フィルタを実行します。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IEnumerable<uint> Filter(BoardContext context, IEnumerable<uint> moves)
        {
            foreach (uint move in moves)
            {
                if (this.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// フィルタ対象外の正当な手かどうかを判定します。
        /// （正当な手ってちょっと違うんだけどね。フィルタは正当かどうかという判断よりももっと広い概念なので）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public abstract bool Validate(BoardContext context, uint move);
    }
}
