using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Orders;
using Niels.Boards;

namespace Niels.Fools
{
    /// <summary>
    /// 評価値を参照しないでソートを行う
    /// </summary>
    public class FoolOrder : IOrder
    {
        /// <summary>
        /// 評価値を参照しないで打ち手の順番をソートする
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IEnumerable<uint> MoveOrdering(IEnumerable<uint> moves, BoardContext context = null)
        {
            return moves.OrderBy(m => m.FromBoard()).ThenBy(m => m.FromIndex()).ThenBy(m => m.ToBoard()).ThenBy(m => m.ToIndex());
        }
    }
}
