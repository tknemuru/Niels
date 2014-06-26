using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Extensions.Number;
using Niels.Boards;
using Niels.Collections;
using Niels.Filters;
using Niels.Diagnostics;

namespace Niels.Generates
{
    /// <summary>
    /// 持ち駒からの打ち手を生成します。
    /// </summary>
    public class HandValueMoveGenerator
    {
        /// <summary>
        /// 持ち駒からの打ち手を生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<uint> Generate(BoardContext context)
        {
            ulong[] allOccupiedBoards = context.GetAllOccupiedBoards();
            foreach (Piece piece in ExtensionPiece.PiecesRemovedPromoted)
            {
                // 玉を持ち駒にしていることはあり得ないのでパス
                if (piece == Piece.King) { continue; }

                ulong count = context.GetHandValueCount(piece, context.Turn);
                if (count > 0)
                {
                    foreach (Board board in BoardProvider.GetAll())
                    {
                        foreach (int index in board.UsingIndexs)
                        {
                            if (!allOccupiedBoards[(int)board.BoardType].IsPositive(index))
                            {
                                uint move = Move.GetHandValueMove(piece, board.BoardType, (uint)index, context.Turn);
                                if (this.HandValueValidate(context, move, piece))
                                {
                                    yield return move;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 正当な打ち手かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        private bool HandValueValidate(BoardContext context, uint move, Piece piece)
        {
            // 歩、香車、桂馬は行き所のない駒の指し手をフィルタする
            if (piece == Piece.Pawn)
            {
                // 二歩もフィルタ
                return (FilterProvider.CanNotMoveFilterOneRank.Validate(context, move)
                        && FilterProvider.FileDuplicatePawnExistsMoveFilter.Validate(context, move));
            }
            else if (piece == Piece.Launce)
            {
                return FilterProvider.CanNotMoveFilterOneRank.Validate(context, move);
            }
            else if (piece == Piece.Knight)
            {
                return FilterProvider.CanNotMoveFilterTwoRank.Validate(context, move);
            }
            return true;
        }
    }
}
