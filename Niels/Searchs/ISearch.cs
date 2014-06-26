using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Searchs
{
    /// <summary>
    /// 探索インターフェイス
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// 指し手を取得する
        /// </summary>
        uint GetMove(BoardContext context);
    }
}
