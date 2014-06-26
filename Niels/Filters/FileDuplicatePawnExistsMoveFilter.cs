using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using System.Diagnostics;

namespace Niels.Filters
{
    /// <summary>
    /// 二歩をフィルタします。
    /// </summary>
    public class FileDuplicatePawnExistsMoveFilter : MoveFilter
    {
        /// <summary>
        /// フィルタ対象外の正当な手かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public override bool Validate(BoardContext context, uint move)
        {
            Debug.Assert(move.PutPiece() == Piece.Pawn, "歩以外の指し手にこのフィルタは使用できません。");
            int file = this.GetFile(move.ToBoard(), (int)move.ToIndex());
            ulong maskedBoard = 0;
            foreach (Board board in BoardProvider.GetAll())
            {
                maskedBoard |= (context.PieceBoards[(int)context.Turn][(int)Piece.Pawn.GetIndex()][(int)board.BoardType] & ~board.ClearFile[file]);
            }

            return (maskedBoard == 0);
        }

        /// <summary>
        /// 筋を取得します
        /// </summary>
        /// <returns></returns>
        private int GetFile(BoardType boardType, int index)
        {
            int baseFile = (index / 8);
            if ((boardType == BoardType.Main) || (boardType == BoardType.SubBottom))
            {
                return (baseFile + 1);
            }
            else if (boardType == BoardType.SubRight && index == 8)
            {
                return baseFile - 1;
            }
            else
            {
                return baseFile;
            }
        }
    }
}
