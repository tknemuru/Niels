using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using System.Diagnostics;

namespace Niels.Filters
{
    /// <summary>
    /// 自駒が既に存在している場所に指す手をフィルタします。
    /// </summary>
    public class PieceDuplicateExistsMoveFilter : MoveFilter
    {
        /// <summary>
        /// フィルタ対象外の正当な手かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public override bool Validate(BoardContext context, uint move)
        {
            BoardType boardType = move.ToBoard();
            ulong state = (1ul << (int)move.ToIndex()) & context.OccupiedBoards[(int)context.Turn][(int)boardType];

            return (state == 0);
        }
    }
}
