using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Strategys
{
    /// <summary>
    /// 戦略
    /// </summary>
    interface IStrategy
    {
        /// <summary>
        /// 打ち手を取得する
        /// </summary>
        /// <returns></returns>
        uint GetMove(BoardContext context);
    }
}
