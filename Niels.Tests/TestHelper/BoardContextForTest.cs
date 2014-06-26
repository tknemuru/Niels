using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Niels.Boards;
using Niels.Collections;
using Niels.Extensions.Number;
using Niels.Helper;

namespace Niels.Tests.TestHelper
{
    /// <summary>
    /// テスト用盤状態
    /// </summary>
    internal class BoardContextForTest : BoardContext
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="turn"></param>
        internal BoardContextForTest(Turn turn)
            : base(turn)
        {
        }

        /// <summary>
        /// 駒を追加する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="index"></param>
        internal void AddPiece(Piece piece, int index)
        {
            this.AddPiece(piece, index, this.Turn);
        }

        /// <summary>
        /// 駒を追加する（反対のターン）
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="index"></param>
        internal void AddOppositePiece(Piece piece, int index)
        {
            this.AddPiece(piece, index, this.Turn.GetOppositeTurn());
        }

        /// <summary>
        /// 駒を追加する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="index"></param>
        internal void AddPiece(Piece piece, int index, Turn turn)
        {
            var board = BoardForTest.GetBoardType(index);
            var trasrateIndex = BoardForTest.TransrateIndex(index);

            this.PieceBoards[(int)turn][piece.GetIndex()][(int)board] |= (1ul << trasrateIndex);
            this.OccupiedBoards[(int)turn][(int)board] |= (1ul << trasrateIndex);
        }

        /// <summary>
        /// 持ち駒を追加する
        /// </summary>
        /// <param name="piece"></param>
        internal void AddHandValue(Piece piece)
        {
            this.AddHandValue(piece, this.Turn);
        }

        /// <summary>
        /// 持ち駒を追加する
        /// </summary>
        /// <param name="piece"></param>
        internal void AddOppositeHandValue(Piece piece)
        {
            this.AddHandValue(piece, this.Turn.GetOppositeTurn());
        }

        /// <summary>
        /// 持ち駒を追加する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="turn"></param>
        private void AddHandValue(Piece piece, Turn turn)
        {
            Piece undoPromotedPiece = piece.UndoPromoted();
            var count = (this.HandValues[(int)turn] >> HandValueShift[undoPromotedPiece.GetIndex()]) & HandValueMask[undoPromotedPiece.GetIndex()];
            this.HandValues[(int)this.Turn] &= HandValueClearMask[undoPromotedPiece.GetIndex()];
            this.HandValues[(int)this.Turn] |= (count + 1) << HandValueShift[undoPromotedPiece.GetIndex()];
        }

        /// <summary>
        /// 指定した駒の持ち駒数を取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        internal ulong GetHandValueCount(Piece piece)
        {
            return this.GetHandValueCount(piece, this.Turn);
        }

        /// <summary>
        /// 指定した駒の持ち駒数を取得する(反対のターン)
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        internal ulong GetOppositeHandValue(Piece piece)
        {
            return this.GetHandValueCount(piece, this.Turn.GetOppositeTurn());
        }

        /// <summary>
        /// 指定したインデックスの駒を取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal Piece GetPiece(int index)
        {
            int orgIndex = BoardForTest.TransrateIndex(index);
            BoardType boardType = BoardForTest.GetBoardType(index);
            return this.GetTurnPiece(boardType, orgIndex).ToPiece();
        }

        /// <summary>
        /// 指定したインデックスに駒が存在するかどうかを判定します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool IsOccupied(int index, Turn turn)
        {
            int orgIndex = BoardForTest.TransrateIndex(index);
            BoardType board = BoardForTest.GetBoardType(index);

            return this.OccupiedBoards[(int)turn][(int)board].IsPositive(orgIndex);
        }

        /// <summary>
        /// コンテクストの全要素が同一であるかチェックする
        /// </summary>
        /// <param name="context"></param>
        internal void AssertAllAreEqual(BoardContext context)
        {
            ExtensionPiece.Pieces.ToList().ForEach
                (
                    piece =>
                    {
                        CollectionAssert.AreEqual(this.PieceBoards[(int)Turn.Black][piece.GetIndex()], context.PieceBoards[(int)Turn.Black][piece.GetIndex()]);
                        CollectionAssert.AreEqual(this.PieceBoards[(int)Turn.White][piece.GetIndex()], context.PieceBoards[(int)Turn.White][piece.GetIndex()]);
                    }                    
                );

            CollectionAssert.AreEqual(this.OccupiedBoards[(int)Turn.Black], context.OccupiedBoards[(int)Turn.Black]);
            Assert.AreEqual(this.HandValues[(int)Turn.Black], context.HandValues[(int)Turn.Black]);

            CollectionAssert.AreEqual(this.OccupiedBoards[(int)Turn.White], context.OccupiedBoards[(int)Turn.White]);
            Assert.AreEqual(this.HandValues[(int)Turn.White], context.HandValues[(int)Turn.White]);

            Assert.AreEqual(this.Turn, context.Turn);
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<int> startIndex = new List<int>() { 72, 73, 74, 75, 76, 77, 78, 79, 80 };
            StringBuilder sb = new StringBuilder();
            startIndex.ForEach
                (
                    (start) =>
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            Piece piece = this.GetPiece(start - (9 * i));
                            sb.Append(piece.GetJapaneseName());
                        }
                        sb.AppendLine("");
                    }
                );
            return sb.ToString();
        }

        /// <summary>
        /// 自分自身をコピーして新しいインスタンスを作成します。
        /// </summary>
        /// <returns></returns>
        public new BoardContextForTest Clone()
        {
            BoardContextForTest newContext = new BoardContextForTest(this.Turn);

            ExtensionPiece.Pieces.ToList().ForEach
                (
                    piece =>
                    {
                        for(int i = (int)BoardType.Main; i <= (int)BoardType.SubBottom; i++)
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
