using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Players
{
    /// <summary>
    /// プレイヤーインターフェイス
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// 駒を置く
        /// </summary>
        uint Put(BoardContext context);
    }
}
