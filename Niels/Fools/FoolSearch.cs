using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Orders;
using Niels.Searchs;

namespace Niels.Fools
{
    /// <summary>
    /// 最初に見つけた指し手を返す
    /// </summary>
    public class FoolSearch : ISearch
    {
        /// <summary>
        /// 指し手を取得する
        /// </summary>
        public uint GetMove(BoardContext context)
        {
            // 歩
            IEnumerable<uint> moves = MoveProvider.GetPawnMoves(context, GenerateTarget.Promote);

            // テスト用にソート
            var move = new FoolOrder().MoveOrdering(moves);

            return moves.ElementAt(0);
        }
    }
}