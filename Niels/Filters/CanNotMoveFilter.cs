using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;

namespace Niels.Filters
{
    /// <summary>
    /// 行き所のない駒の指し手をフィルタします。
    /// </summary>
    public class CanNotMoveFilter : MoveFilter
    {
        /// <summary>
        /// フィルタする段数
        /// </summary>
        private int FilterRankCount { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CanNotMoveFilter()
            : this(1) 
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filterRankCount"></param>
        public CanNotMoveFilter(int filterRankCount)
        {
            this.FilterRankCount = filterRankCount;
        }

        /// <summary>
        /// フィルタ対象外の正当な手かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public override bool Validate(BoardContext context, uint move)
        {
            // 成る手ならこのフィルタに引っかかることはありえないので処理終了
            // TODO：この判断をここで行うことが妥当かは微妙なところ
            if (move.IsPromote()) { return true; }
            return (context.Turn == Turn.Black) ? this.IsBlackValidate(move) : this.IsWhiteValidate(move);
        }

        /// <summary>
        /// 先手にとって合法手であるかどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool IsBlackValidate(uint move)
        {
            BoardType boardType = move.ToBoard();
            if (boardType == BoardType.SubBottom)
            {
                return true;
            }
            else
            {
                Board board = BoardProvider.GetAll()[(int)boardType];
                ulong state = (1ul << (int)move.ToIndex());
                for (int i = 0; i < FilterRankCount; i++)
                {
                    state &= board.ClearRank[i];
                }
                return (state > 0);
            }
        }

        /// <summary>
        /// 後手にとって合法手であるかどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool IsWhiteValidate(uint move)
        {
            BoardType boardType = move.ToBoard();
            if (boardType == BoardType.SubBottom)
            {
                return false;
            }
            else
            {
                Board board = BoardProvider.GetAll()[(int)boardType];
                ulong state = (1ul << (int)move.ToIndex());
                for (int i = 0; i < FilterRankCount; i++)
                {
                    state &= board.ClearRank[8 - i];
                }
                return (state > 0);
            }
        }
    }
}
