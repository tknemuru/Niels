using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Orders
{
    /// <summary>
    /// 指し手のソート機能を提供します。
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// 指し手をソートします。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        IEnumerable<uint> MoveOrdering(IEnumerable<uint> moves, BoardContext context);
    }
}
