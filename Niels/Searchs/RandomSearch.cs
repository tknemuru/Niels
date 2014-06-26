using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Generates;
using Niels.Filters;

namespace Niels.Searchs
{
    /// <summary>
    /// 指し手をランダムに選択して返します。
    /// </summary>
    public class RandomSearch : ISearch
    {
        /// <summary>
        /// 指し手を取得する
        /// </summary>
        public uint GetMove(BoardContext context)
        {
            IEnumerable<uint> moves = MoveProvider.GetPawnMoves(context, GenerateTarget.All);
            moves = moves.Concat(MoveProvider.GetLaunceMoves(context, GenerateTarget.All));
            moves = moves.Concat(MoveProvider.GetKnightMoves(context, GenerateTarget.All));
            moves = moves.Concat(MoveProvider.GetSilverMoves(context, GenerateTarget.All));
            moves = moves.Concat(MoveProvider.GetGoldMoves(context));
            moves = moves.Concat(MoveProvider.GetKingMoves(context));
            moves = moves.Concat(MoveProvider.GetBishopMoves(context, GenerateTarget.All));
            moves = moves.Concat(MoveProvider.GetRookMoves(context, GenerateTarget.All));
            moves = moves.Concat(MoveProvider.GetPawnPromotedMoves(context));
            moves = moves.Concat(MoveProvider.GetLauncePromotedMoves(context));
            moves = moves.Concat(MoveProvider.GetKnightPromotedMoves(context));
            moves = moves.Concat(MoveProvider.GetSilverPromotedMoves(context));
            moves = moves.Concat(MoveProvider.GetHorseMoves(context));
            moves = moves.Concat(MoveProvider.GetDragonMoves(context));
            moves = moves.Concat(MoveProvider.GetHandValueMoves(context));

            // TODO:ここじゃだめ
            moves = FilterProvider.CheckedByMyselefMoveFilter.Filter(context, moves);

            // 成る手、成り駒の手、持ち駒からの打ち手があったらそれを優先する
            IEnumerable<uint> priorityMoves = moves.Where(move => (move.IsPromote() || move.PutPiece().IsPromoted() || move.IsHandValueMove()));
            if(priorityMoves.Count() > 0)
            {
                moves = priorityMoves;
            }

            // ランダムに手を選択する
            Random random = new Random();
            return moves.ElementAt(random.Next(moves.Count()));
        }
    }
}
