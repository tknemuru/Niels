using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Searchs;

namespace Niels.Players
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// 探索エンジン
        /// </summary>
        public Searcher Searcher { get; set; }

        /// <summary>
        /// 駒を置く
        /// </summary>
        public abstract uint Put(BoardContext context);
    }
}
