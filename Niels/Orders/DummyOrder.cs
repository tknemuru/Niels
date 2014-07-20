using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Orders
{
    /// <summary>
    /// ソート処理が不要なときにこのクラスのインスタンスを使用します。
    /// </summary>
    public class DummyOrder : IOrder
    {
        /// <summary>
        /// 指し手の順番をソートします。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IEnumerable<uint> MoveOrdering(IEnumerable<uint> moves, BoardContext context = null)
        {
            return moves;
        }
    }
}
