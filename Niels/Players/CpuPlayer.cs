using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Niels.Strategys;
using Niels.Boards;
using Niels.Searchs;

namespace Niels.Players
{
    /// <summary>
    /// CPUプレイヤー
    /// </summary>
    public class CpuPlayer : Player
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CpuPlayer()
        {
            this.Searcher = new NegaMax(SearchConfigProvider.DefaultSearchConfig);
        }

        /// <summary>
        /// 駒を置く
        /// </summary>
        public override uint Put(BoardContext context)
        {            
            uint move = this.Searcher.GetMove(context);
            this.Searcher.SearchInfo.IsSearchEnd = true;
            return move;
        }
    }
}