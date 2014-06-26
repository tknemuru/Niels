using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Generates;
using Niels.Extensions.Number;
using System.Diagnostics;

namespace Niels.Boards
{
    /// <summary>
    /// 盤の状態に関する付加情報を提供します。
    /// </summary>
    public class BoardContextAdditionalInfo
    {
        /// <summary>
        /// 盤状態
        /// </summary>
        private readonly BoardContext Context;

        /// <summary>
        /// 敵の飛び利きを遮っている自駒
        /// </summary>
        public List<ushort> JumpPreventPieces
        {
            get
            {
                if (this._jumpPreventPieces == null)
                {
                    this.GenerateJumpPreventPieces();
                }
                return this._jumpPreventPieces;
            }
        }
        private List<ushort> _jumpPreventPieces;

        /// <summary>
        /// 現在のターンが王手されているかどうか
        /// </summary>
        public bool IsChecked
        {
            get
            {
                if (!this._isChecked.HasValue)
                {
                    this.UpdateIsChecked();
                }
                return this._isChecked.Value;
            }
        }
        private bool? _isChecked;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context"></param>
        public BoardContextAdditionalInfo(BoardContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// 王の位置を取得します。
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public ushort GetKingBoardPotision(Turn turn)
        {
            for (int boardType = (int)BoardType.Main; boardType <= (int)BoardType.SubBottom; boardType++)
            {
                ulong board = this.Context.PieceBoards[(int)turn][Piece.King.GetIndex()][boardType];
                if (board > 0)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        if (board.IsPositive(i))
                        {
                            // 位置を特定
                            return BoardPotision.GetBoardPotision((BoardType)boardType, i);
                        }
                    }
                }
            }

            // ここにくることは有り得ない
            throw new ApplicationException("王の位置が特定できませんでした。");
        }

        /// <summary>
        /// 現在のターンが王手されているかどうかの値を更新します。
        /// TODO:無理やり感がある…
        /// </summary>
        /// <param name="context"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public void UpdateIsChecked()
        {
            // 自王の位置を特定
            ushort kingPotision = this.GetKingBoardPotision(this.Context.Turn);

            // 敵のターンにする
            this.Context.ChangeTurn();

            bool isExistsCheckMove = MoveProvider.IsExistsCheckMove(this.Context, kingPotision.BoardType(), kingPotision.Index());

            // ターンを戻す
            this.Context.UndoChangeTurn();

            // 値を更新
            this._isChecked = isExistsCheckMove;
        }

        /// <summary>
        /// 敵の飛び利きを遮っている自駒リストを生成します。
        /// </summary>
        private void GenerateJumpPreventPieces()
        {
            this._jumpPreventPieces = new List<ushort>();
            this.GenerateJumpPreventPieces(this.Context, MoveProvider.GetBishopMoves(this.Context, GenerateTarget.All));
            this.GenerateJumpPreventPieces(this.Context, MoveProvider.GetRookMoves(this.Context, GenerateTarget.All));
            this.GenerateJumpPreventPieces(this.Context, MoveProvider.GetHorseMoves(this.Context));
            this.GenerateJumpPreventPieces(this.Context, MoveProvider.GetDragonMoves(this.Context));
            this.GenerateJumpPreventPieces(this.Context, MoveProvider.GetLaunceMoves(this.Context, GenerateTarget.All));
        }

        /// <summary>
        /// 敵の飛び利きを遮っている自駒リストを生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="moves"></param>
        private void GenerateJumpPreventPieces(BoardContext context, IEnumerable<uint> moves)
        {
            // 欲しいのは敵の飛び利きを遮っている自駒
            context.ChangeTurn();

            foreach (uint move in moves)
            {
                if (move.IsCapture())
                {
                    this.JumpPreventPieces.Add(BoardPotision.GetBoardPotision(move.ToBoard(), (int)move.ToIndex()));
                }
            }

            context.UndoChangeTurn();
        }
    }
}
