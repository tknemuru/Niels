using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Orders
{
    /// <summary>
    /// 打ち手の順番をソートするメソッドを提供する
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// 打ち手の順番をソートする
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        IEnumerable<uint> MoveOrdering(IEnumerable<uint> moves);
    }
}
