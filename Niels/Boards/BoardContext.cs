using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Collections;
using System.Diagnostics;
using Niels.Extensions.Number;

namespace Niels.Boards
{
    /// <summary>
    /// 盤の状態を表します。
    /// </summary>
    public class BoardContext
    {
        /// <summary>
        /// 持ち駒のシフト量
        /// </summary>
        protected static readonly int[] HandValueShift =
        {
            // 歩 香 桂 銀 金 角 飛
            0, 5, 8, 11, 14, 17, 19
        };

        /// <summary>
        /// 持ち駒のマスク量
        /// </summary>
        protected static readonly ulong[] HandValueMask =
        {
            // 歩 香 桂 銀 金 角 飛
            0x1f, 0x7, 0x7, 0x7, 0x7, 0x3, 0x3
        };

        /// <summary>
        /// 持ち駒の数をクリアするためのマスク量
        /// </summary>
        protected static readonly ulong[] HandValueClearMask =
        {
            ~((~0u & HandValueMask[Piece.Pawn.GetIndex()]) << HandValueShift[Piece.Pawn.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Launce.GetIndex()]) << HandValueShift[Piece.Launce.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Knight.GetIndex()]) << HandValueShift[Piece.Knight.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Silver.GetIndex()]) << HandValueShift[Piece.Silver.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Gold.GetIndex()]) << HandValueShift[Piece.Gold.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Bishop.GetIndex()]) << HandValueShift[Piece.Bishop.GetIndex()]),
            ~((~0u & HandValueMask[Piece.Rook.GetIndex()]) << HandValueShift[Piece.Rook.GetIndex()])
        };

        /// <summary>
        /// 駒の状態
        /// 1番目の配列：先手/後手
        /// 2番目の配列：駒の種類
        /// 3番目の配列：盤の種類ごとの駒の状態
        /// </summary>
        public ulong[][][] PieceBoards { get; set; }

        /// <summary>
        /// 占領状態を示す盤
        /// 1番目の配列：先手/後手
        /// 3番目の配列：盤の種類ごとの駒の状態
        /// </summary>
        public ulong[][] OccupiedBoards { get; set; }

        /// <summary>
        /// 持ち駒
        /// 1番目：先手
        /// 2番目：後手
        /// </summary>
        public ulong[] HandValues { get; set; }

        /// <summary>
        /// 盤状態の追加情報
        /// </summary>
        public BoardContextAdditionalInfo AdditionalInfo { get; set; }

        /// <summary>
        /// ターン
        /// </summary>
        public Turn Turn { get; set; }

        /// <summary>
        /// ターン経過数
        /// </summary>
        public int TurnCount { get; set; }

        /// <summary>
        /// 打ち手の履歴
        /// </summary>
        public Stack<uint> MovedRecord { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BoardContext()
            : this(Turn.Black)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BoardContext(Turn turn)
        {
            this.Turn = turn;
            this.TurnCount = 1;

            this.PieceBoards = new ulong[ExtensionTurn.TurnCount][][];
            this.PieceBoards[(int)Turn.Black] = new ulong[ExtensionPiece.PieceCount][];
            for (int i = 0; i < ExtensionPiece.PieceCount; i++)
            {
                this.PieceBoards[(int)Turn.Black][i] = new ulong[Board.BoardTypeCount];
            }

            this.PieceBoards[(int)Turn.White] = new ulong[ExtensionPiece.PieceCount][];
            for (int i = 0; i < ExtensionPiece.PieceCount; i++)
            {
                this.PieceBoards[(int)Turn.White][i] = new ulong[Board.BoardTypeCount];
            }

            this.OccupiedBoards = new ulong[ExtensionTurn.TurnCount][];
            this.OccupiedBoards[(int)Turn.Black] = new ulong[Board.BoardTypeCount];
            this.OccupiedBoards[(int)Turn.White] = new ulong[Board.BoardTypeCount];

            this.HandValues = new ulong[ExtensionTurn.TurnCount];

            this.MovedRecord = new Stack<uint>();

            this.AdditionalInfo = new BoardContextAdditionalInfo(this);
        }

        /// <summary>
        /// 平手初期局面の状態にします。
        /// </summary>
        public void SetDefaultStartPosition()
        {
            this.PieceBoards[(int)Turn.Black][Piece.Pawn.GetIndex()][(int)BoardType.Main] = 0x4040404040404040;
            this.PieceBoards[(int)Turn.Black][Piece.Pawn.GetIndex()][(int)BoardType.SubRight] = 0x040;
            this.PieceBoards[(int)Turn.Black][Piece.Pawn.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.White][Piece.Pawn.GetIndex()][(int)BoardType.Main] = 0x0404040404040404;
            this.PieceBoards[(int)Turn.White][Piece.Pawn.GetIndex()][(int)BoardType.SubRight] = 0x004;
            this.PieceBoards[(int)Turn.White][Piece.Pawn.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Launce.GetIndex()][(int)BoardType.Main] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Launce.GetIndex()][(int)BoardType.SubRight] = 0x100;
            this.PieceBoards[(int)Turn.Black][Piece.Launce.GetIndex()][(int)BoardType.SubBottom] = 0x8000000000000000;

            this.PieceBoards[(int)Turn.White][Piece.Launce.GetIndex()][(int)BoardType.Main] = 0x0100000000000000;
            this.PieceBoards[(int)Turn.White][Piece.Launce.GetIndex()][(int)BoardType.SubRight] = 0x001;
            this.PieceBoards[(int)Turn.White][Piece.Launce.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Knight.GetIndex()][(int)BoardType.Main] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Knight.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Knight.GetIndex()][(int)BoardType.SubBottom] = 0x0080000000000080;

            this.PieceBoards[(int)Turn.White][Piece.Knight.GetIndex()][(int)BoardType.Main] = 0x0001000000000001;
            this.PieceBoards[(int)Turn.White][Piece.Knight.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.Knight.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Silver.GetIndex()][(int)BoardType.Main] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Silver.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Silver.GetIndex()][(int)BoardType.SubBottom] = 0x0000800000008000;

            this.PieceBoards[(int)Turn.White][Piece.Silver.GetIndex()][(int)BoardType.Main] = 0x0000010000000100;
            this.PieceBoards[(int)Turn.White][Piece.Silver.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.Silver.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Gold.GetIndex()][(int)BoardType.Main] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Gold.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Gold.GetIndex()][(int)BoardType.SubBottom] = 0x0000008000800000;

            this.PieceBoards[(int)Turn.White][Piece.Gold.GetIndex()][(int)BoardType.Main] = 0x0000000100010000;
            this.PieceBoards[(int)Turn.White][Piece.Gold.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.Gold.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Bishop.GetIndex()][(int)BoardType.Main] = 0x0080000000000000;
            this.PieceBoards[(int)Turn.Black][Piece.Bishop.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Bishop.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.White][Piece.Bishop.GetIndex()][(int)BoardType.Main] = 0x0000000000000002;
            this.PieceBoards[(int)Turn.White][Piece.Bishop.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.Bishop.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.Rook.GetIndex()][(int)BoardType.Main] = 0x0000000000000080;
            this.PieceBoards[(int)Turn.Black][Piece.Rook.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.Rook.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.White][Piece.Rook.GetIndex()][(int)BoardType.Main] = 0x0002000000000000;
            this.PieceBoards[(int)Turn.White][Piece.Rook.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.Rook.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.PieceBoards[(int)Turn.Black][Piece.King.GetIndex()][(int)BoardType.Main] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.King.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.Black][Piece.King.GetIndex()][(int)BoardType.SubBottom] = 0x0000000080000000;

            this.PieceBoards[(int)Turn.White][Piece.King.GetIndex()][(int)BoardType.Main] = 0x0000000001000000;
            this.PieceBoards[(int)Turn.White][Piece.King.GetIndex()][(int)BoardType.SubRight] = 0;
            this.PieceBoards[(int)Turn.White][Piece.King.GetIndex()][(int)BoardType.SubBottom] = 0;

            this.OccupiedBoards[(int)Turn.Black][(int)BoardType.Main] = 0x40c04040404040c0;
            this.OccupiedBoards[(int)Turn.Black][(int)BoardType.SubRight] = 0x140;
            this.OccupiedBoards[(int)Turn.Black][(int)BoardType.SubBottom] = 0x8080808080808080;

            this.OccupiedBoards[(int)Turn.White][(int)BoardType.Main] = 0x0507050505050507;
            this.OccupiedBoards[(int)Turn.White][(int)BoardType.SubRight] = 0x005;
            this.OccupiedBoards[(int)Turn.White][(int)BoardType.SubBottom] = 0;

            this.HandValues[(int)Turn.Black] = 0;
            this.HandValues[(int)Turn.White] = 0;
        }

        /// <summary>
        /// 盤に駒を置きます。
        /// </summary>
        /// <param name="move"></param>
        public void PutPiece(uint move)
        {
            if (move.IsHandValueMove())
            {
                // 持ち駒
                this.PutHandValuePiece(move);
            }
            else
            {
                // 盤上の駒
                this.InnerPutPiece(move);
            }
        }

        /// <summary>
        /// 盤に駒を置く
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="piece"></param>
        /// <param name="setBoard"></param>
        private void InnerPutPiece(uint move)
        {
            // 移動先に駒を打つ
            Debug.Assert(this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, string.Format("移動先に駒が置かれています。(PieceBoards) 盤：{0} インデックス{1} 駒{2}", move.ToBoard(), move.ToIndex(), move.PutPiece()));
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, string.Format("移動先に駒が置かれています。(OccupiedBoards) 盤：{0} インデックス{1} 駒{2}", move.ToBoard(), move.ToIndex(), move.PutPiece()));
            this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());

            // 移動元を空白にする
            Piece fromPiece = (move.IsPromote()) ? move.PutPiece().UndoPromoted() : move.PutPiece();
            Debug.Assert(this.PieceBoards[(int)this.Turn][fromPiece.GetIndex()][(int)move.FromBoard()].IsPositive((int)move.FromIndex()), string.Format("移動元に駒がありません。盤：{0} インデックス{1} 駒{2}", move.FromBoard(), move.FromIndex(), move.PutPiece()));
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.FromBoard()].IsPositive((int)move.FromIndex()), "移動元に駒がありません。");
            this.PieceBoards[(int)this.Turn][fromPiece.GetIndex()][(int)move.FromBoard()] &= ~(1ul << (int)move.FromIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.FromBoard()] &= ~(1ul << (int)move.FromIndex());

            // 取った駒は持ち駒に追加する
            // TODO:King対策のこの実装はまずいかもしれない
            if (move.IsCapture() && move.CapturePiece() != Piece.King)
            {
                var capturePiece = move.CapturePiece();
                var undoPromotedCapturePiece = move.CapturePiece().UndoPromoted();
                var count = (this.HandValues[(int)this.Turn] >> HandValueShift[undoPromotedCapturePiece.GetIndex()]) & HandValueMask[undoPromotedCapturePiece.GetIndex()];
                Debug.Assert((count >= 0), "持ち駒数が負の値です。");
                this.HandValues[(int)this.Turn] &= HandValueClearMask[undoPromotedCapturePiece.GetIndex()];
                this.HandValues[(int)this.Turn] |= (count + 1) << HandValueShift[undoPromotedCapturePiece.GetIndex()];

                // 取られたマスを空白にする
                Debug.Assert(this.PieceBoards[this.Turn.GetOppositeIndex()][capturePiece.GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), string.Format("駒を取った場所に駒がありません。盤：{0} インデックス：{1} 取った駒：{2} 指した駒{3}", move.ToBoard(), move.ToIndex(), move.CapturePiece(), move.PutPiece()));
                Debug.Assert(this.OccupiedBoards[this.Turn.GetOppositeIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), string.Format("駒を取った場所に駒がありません。盤：{0} インデックス：{1} 取った駒：{2} 指した駒{3}", move.ToBoard(), move.ToIndex(), move.CapturePiece(), move.PutPiece()));
                this.PieceBoards[this.Turn.GetOppositeIndex()][capturePiece.GetIndex()][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
                this.OccupiedBoards[this.Turn.GetOppositeIndex()][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
            }

            this.MovedRecord.Push(move);
        }

        /// <summary>
        /// 置いた駒を元に戻します。
        /// </summary>
        /// <param name="move"></param>
        public void UndoPutPiece()
        {
            // 直前の打ち手を取得
            uint move = this.MovedRecord.Pop();

            if (move.IsHandValueMove())
            {
                // 持ち駒
                this.UndoPutHandValuePiece(move);
            }
            else
            {
                // 盤上の駒
                this.InnerUndoPutPiece(move);
            }
        }

        /// <summary>
        /// 置いた駒を元に戻す
        /// </summary>
        private void InnerUndoPutPiece(uint move)
        {
            // 取った駒を元に戻す
            // TODO:King対策のこの実装はまずいかもしれない
            if (move.IsCapture() && move.CapturePiece() != Piece.King)
            {
                var capturePiece = move.CapturePiece();
                var undoPromotedCapturePiece = move.CapturePiece().UndoPromoted();
                var count = (this.HandValues[(int)this.Turn] >> HandValueShift[undoPromotedCapturePiece.GetIndex()]) & HandValueMask[undoPromotedCapturePiece.GetIndex()];
                Debug.Assert((count > 0), "持ち駒数が負の値になります。");
                this.HandValues[(int)this.Turn] &= HandValueClearMask[undoPromotedCapturePiece.GetIndex()];
                this.HandValues[(int)this.Turn] |= (count - 1) << HandValueShift[undoPromotedCapturePiece.GetIndex()];

                // 取られたマスに駒を置く
                Debug.Assert(this.PieceBoards[this.Turn.GetOppositeIndex()][capturePiece.GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, "取ったはずの場所に駒があります。");
                Debug.Assert(this.OccupiedBoards[this.Turn.GetOppositeIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, "取ったはずの場所に駒があります。");
                this.PieceBoards[this.Turn.GetOppositeIndex()][capturePiece.GetIndex()][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());
                this.OccupiedBoards[this.Turn.GetOppositeIndex()][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());
            }

            // 移動元に駒を戻す
            Piece fromPiece = (move.IsPromote()) ? move.PutPiece().UndoPromoted() : move.PutPiece();
            Debug.Assert(this.PieceBoards[(int)this.Turn][fromPiece.GetIndex()][(int)move.FromBoard()].IsPositive((int)move.FromIndex()) == false, "移動元に駒があります。");
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.FromBoard()].IsPositive((int)move.FromIndex()) == false, "移動元に駒があります。");
            this.PieceBoards[(int)this.Turn][fromPiece.GetIndex()][(int)move.FromBoard()] |= (1ul << (int)move.FromIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.FromBoard()] |= (1ul << (int)move.FromIndex());

            // 移動先の駒を元に戻す
            Debug.Assert(this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), "移動先に駒がありません。");
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), "移動先に駒がありません。");
            this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
        }

        /// <summary>
        /// 盤に持ち駒を打つ
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="piece"></param>
        /// <param name="setBoard"></param>
        private void PutHandValuePiece(uint move)
        {
            // 持ち駒を一つ減らす
            Debug.Assert(!move.PutPiece().IsPromoted(), "持ち駒が成っていることはあり得ません。");
            ulong count = this.GetHandValueCount(move.PutPiece(), this.Turn);
            Debug.Assert(count > 0, string.Format("打てる持ち駒がありません。 駒：{0} ターン：{1}", move.PutPiece(), this.Turn));
            this.HandValues[(int)this.Turn] &= HandValueClearMask[move.PutPiece().GetIndex()];
            this.HandValues[(int)this.Turn] |= (ulong)(count - 1) << HandValueShift[move.PutPiece().GetIndex()];

            // 移動先に駒を打つ
            Debug.Assert(this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, string.Format("移動先に駒が置かれています。(PieceBoards) 盤：{0} インデックス{1} 駒{2}", move.ToBoard(), move.ToIndex(), move.PutPiece()));
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, string.Format("移動先に駒が置かれています。(OccupiedBoards) 盤：{0} インデックス{1} 駒{2}", move.ToBoard(), move.ToIndex(), move.PutPiece()));
            Debug.Assert(this.OccupiedBoards[(int)this.Turn.GetOppositeTurn()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()) == false, string.Format("移動先に敵駒が置かれています。(OccupiedBoards) 盤：{0} インデックス{1} 駒{2}", move.ToBoard(), move.ToIndex(), move.PutPiece()));
            this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()] |= (1ul << (int)move.ToIndex());

            this.MovedRecord.Push(move);
        }

        /// <summary>
        /// 置いた持ち駒を元に戻す
        /// </summary>
        private void UndoPutHandValuePiece(uint move)
        {
            Debug.Assert(!move.IsCapture(), "持ち駒からの打ち手が駒を取ることはあり得ません。");
            Debug.Assert(!move.IsPromote(), "持ち駒からの打ち手が成る手であることはあり得ません。");
            Debug.Assert(!move.PutPiece().IsPromoted(), "持ち駒が成っていることはあり得ません。");

            // 駒を手元に戻す
            var count = (this.HandValues[(int)this.Turn] >> HandValueShift[move.PutPiece().GetIndex()]) & HandValueMask[move.PutPiece().GetIndex()];
            Debug.Assert((count >= 0), "持ち駒数が不正です。");
            this.HandValues[(int)this.Turn] &= HandValueClearMask[move.PutPiece().GetIndex()];
            this.HandValues[(int)this.Turn] |= (count + 1) << HandValueShift[move.PutPiece().GetIndex()];

            // 移動先の駒を元に戻す
            Debug.Assert(this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), "移動先に駒がありません。");
            Debug.Assert(this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()].IsPositive((int)move.ToIndex()), "移動先に駒がありません。");
            this.PieceBoards[(int)this.Turn][move.PutPiece().GetIndex()][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
            this.OccupiedBoards[(int)this.Turn][(int)move.ToBoard()] &= ~(1ul << (int)move.ToIndex());
        }

        /// <summary>
        /// ターンを変更します。
        /// </summary>
        public void ChangeTurn()
        {
            this.Turn = this.Turn.GetOppositeTurn();
            this.TurnCount++;
        }

        /// <summary>
        /// ターン変更を元に戻します。
        /// </summary>
        public void UndoChangeTurn()
        {
            this.Turn = this.Turn.GetOppositeTurn();
            this.TurnCount--;
        }

        /// <summary>
        /// 指定したインデックスのターン付き駒を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TurnPiece GetTurnPiece(BoardType boardType, int index)
        {
            foreach (Piece piece in ExtensionPiece.Pieces)
            {
                bool isExistsBlack = this.PieceBoards[(int)Turn.Black][piece.GetIndex()][(int)boardType].IsPositive(index);
                bool isExitsWhite = this.PieceBoards[(int)Turn.White][piece.GetIndex()][(int)boardType].IsPositive(index);
                if (isExistsBlack)
                {
                    return piece.ToTurnPiece(Turn.Black);
                }
                else if (isExitsWhite)
                {
                    return piece.ToTurnPiece(Turn.White);
                }
            }
            return TurnPiece.Empty;
        }

        /// <summary>
        /// 全体の占領状態を取得します。
        /// </summary>
        /// <returns></returns>
        public ulong[] GetAllOccupiedBoards()
        {
            ulong[] occupiedBoards = new ulong[Board.BoardTypeCount];
            for (int i = 0; i < Board.BoardTypeCount; i++)
            {
                occupiedBoards[i] = this.OccupiedBoards[(int)Turn.Black][i] | this.OccupiedBoards[(int)Turn.White][i];
            }

            return occupiedBoards;
        }

        /// <summary>
        /// 指定したターン、駒の持ち駒数を取得します。
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public ulong GetHandValueCount(Piece piece, Turn turn)
        {
            return (this.HandValues[(int)turn] >> HandValueShift[piece.GetIndex()]) & HandValueMask[piece.GetIndex()];
        }

        /// <summary>
        /// 自分自身をコピーして新しいインスタンスを作成します。
        /// </summary>
        /// <returns></returns>
        public BoardContext Clone()
        {
            BoardContext newContext = new BoardContext(this.Turn);

            ExtensionPiece.Pieces.ToList().ForEach
                (
                    piece =>
                    {
                        for (int i = (int)BoardType.Main; i <= (int)BoardType.SubBottom; i++)
                        {
                            newContext.PieceBoards[(int)Turn.Black][piece.GetIndex()][i] = this.PieceBoards[(int)Turn.Black][piece.GetIndex()][i];
                            newContext.PieceBoards[(int)Turn.White][piece.GetIndex()][i] = this.PieceBoards[(int)Turn.White][piece.GetIndex()][i];
                        }
                    }
                );

            for (int i = (int)BoardType.Main; i <= (int)BoardType.SubBottom; i++)
            {
                newContext.OccupiedBoards[(int)Turn.Black][i] = this.OccupiedBoards[(int)Turn.Black][i];
                newContext.OccupiedBoards[(int)Turn.White][i] = this.OccupiedBoards[(int)Turn.White][i];
            }

            newContext.HandValues[(int)Turn.Black] = this.HandValues[(int)Turn.Black];
            newContext.HandValues[(int)Turn.White] = this.HandValues[(int)Turn.White];

            uint[] records = new uint[this.MovedRecord.Count()];
            this.MovedRecord.CopyTo(records, 0);
            newContext.MovedRecord = new Stack<uint>(records);

            newContext.TurnCount = this.TurnCount;
            newContext.AdditionalInfo = new BoardContextAdditionalInfo(newContext);

            return newContext;
        }
    }
}