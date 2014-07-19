using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Usi;

namespace Niels.Searchs
{
    /// <summary>
    /// 探索機能を提供します。
    /// </summary>
    public abstract class Searcher
    {
        /// <summary>
        /// 探索情報
        /// </summary>
        public SearchInfo SearchInfo { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Searcher()
        {
            this.SearchInfo = new SearchInfo();
        }

        /// <summary>
        /// 指し手を取得します。
        /// </summary>
        public uint GetMove(BoardContext context)
        {
            this.SearchInfo.Stopwatch.Start();
            uint bestMove = this.InnerGetMove(context);
            this.SearchInfo.Stopwatch.Stop();

            return bestMove;
        }

        protected abstract uint InnerGetMove(BoardContext context);
    }
}
