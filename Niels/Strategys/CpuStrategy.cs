using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Searchs;
using Niels.Boards;
using Niels.Generates;
using Niels.Fools;
using Niels.Helper;
using Niels.Diagnostics;

namespace Niels.Strategys
{
    /// <summary>
    /// CPUの戦略
    /// </summary>
    public class CpuStrategy :IStrategy
    {
        /// <summary>
        /// 打ち手を取得する
        /// </summary>
        /// <returns></returns>
        public uint GetMove(BoardContext context)
        {
            //PrincipalVariationSearch search = new PrincipalVariationSearch(SearchConfigProvider.DefaultSearchConfig);
            //var move = search.GetMove(context);
            NegaMax search = new NegaMax(SearchConfigProvider.DefaultSearchConfig);
            var move = search.GetMove(context);
            return move;
        }
    }
}
