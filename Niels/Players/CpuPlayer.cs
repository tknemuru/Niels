using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Strategys;
using Niels.Boards;

namespace Niels.Players
{
    /// <summary>
    /// CPUプレイヤー
    /// </summary>
    public class CpuPlayer : IPlayer
    {
        /// <summary>
        /// 戦略
        /// </summary>
        private IStrategy _strategy = new CpuStrategy();

        /// <summary>
        /// 駒を置く
        /// </summary>
        public uint Put(BoardContext context)
        {
            uint move = this._strategy.GetMove(context);
            return move;
        }
    }
}