using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Diagnostics;
using Niels.Extensions.Number;
using System.Diagnostics;

namespace Niels.Filters
{
    /// <summary>
    /// 自玉を相手駒の利きにさらす指し手をフィルタします。
    /// </summary>
    public class CheckedByMyselefMoveFilter : MoveFilter
    {
        /// <summary>
        /// フィルタ対象外の正当な手かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public override bool Validate(BoardContext context, uint move)
        {
            // 王手になっていなければ、大半の指し手はチェック対象から除外できる
            if (!context.AdditionalInfo.IsChecked)
            {
                // 打ち手で自玉をさらすことはありえないので対象外
                if (move.IsHandValueMove()) { return true; }

                // 敵の飛び利きを遮っていない、かつ、移動する駒が王以外ならばフィルタ対象外
                if (!this.IsJumpPreventPiece(context, move) && move.PutPiece() != Piece.King) { return true; }
            }

            // 指す
            context.PutPiece(move);

            // 王手になっているかどうかの情報を更新
            context.AdditionalInfo.UpdateIsChecked();

            bool isExistsCheckMove = context.AdditionalInfo.IsChecked;

            // 指し手を戻す
            context.UndoPutPiece();

            // 王手になっているかどうかの情報を更新(元に戻す)
            context.AdditionalInfo.UpdateIsChecked();

            return !isExistsCheckMove;
        }

        /// <summary>
        /// 指し駒が敵の飛び利きを遮っている駒かどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool IsJumpPreventPiece(BoardContext context, uint move)
        {
            ushort potision = BoardPotision.GetBoardPotision(move.FromBoard(), (int)move.FromIndex());
            foreach (ushort preventPotision in context.AdditionalInfo.JumpPreventPieces)
            {
                if (potision == preventPotision)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
