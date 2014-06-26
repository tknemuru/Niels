using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Generates;
using Niels.Boards;
using Niels.Helper;
using System.Diagnostics;
using System.ComponentModel;

namespace Niels.Notation
{
    /// <summary>
    /// SFEN(Shogi Forsyth-Edwards Notation)表記法による文字列処理を行ないます。
    /// </summary>
    public class SfenNotation : NotationBase
    {
        /// <summary>
        /// 段を表す文字列
        /// TODO:stringの配列の方がよさそう。
        /// </summary>
        private static readonly string RanksSign = "abcdefghi";

        /// <summary>
        /// 駒を表す文字列
        /// TODO:stringの配列の方がよさそう。
        /// </summary>
        private static readonly string PiecesSign = "PLNSGBRK";

        /// <summary>
        /// 持ち駒として使用可能な駒のリスト
        /// </summary>
        private static readonly Piece[] HandValuePieces = {
                                                     Piece.Pawn,
                                                     Piece.Launce,
                                                     Piece.Knight,
                                                     Piece.Silver,
                                                     Piece.Gold,
                                                     Piece.Bishop,
                                                     Piece.Rook
                                                 };

        /// <summary>
        /// 成る手を示す記号
        /// </summary>
        private const string PromoteSign = "+";

        /// <summary>
        /// 持ち駒を示す記号
        /// </summary>
        private const string HandValueSign = "*";

        /// <summary>
        /// 指し手をSFEN表記に変換します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public string ConvertToSfenMove(uint move)
        {
            if (move.IsHandValueMove())
            {
                return this.ConvertToSfenHandValueMove(move);
            }
            else
            {
                return this.InnerConvertToSfenMove(move);
            }
        }

        /// <summary>
        /// 指し手をSFEN表記に変換します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private string InnerConvertToSfenMove(uint move)
        {
            int fromFile = this.GetFile(move.FromBoard(), (int)move.FromIndex());
            char fromRank = this.GetRank(move.FromBoard(), (int)move.FromIndex());
            int toFile = this.GetFile(move.ToBoard(), (int)move.ToIndex());
            char toRank = this.GetRank(move.ToBoard(), (int)move.ToIndex());
            string promote = move.IsPromote() ? PromoteSign : string.Empty;
            return string.Format("{0}{1}{2}{3}{4}", fromFile, fromRank, toFile, toRank, promote);
        }

        /// <summary>
        /// 持ち駒の打ち手をSFEN表記に変換します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private string ConvertToSfenHandValueMove(uint move)
        {
            Debug.Assert(!move.IsCapture(), "持ち駒からの打ち手が駒を取ることはあり得ません。");
            Debug.Assert(!move.IsPromote(), "持ち駒からの打ち手が成る手であることはあり得ません。");
            Debug.Assert(!move.PutPiece().IsPromoted(), "持ち駒が成っていることはあり得ません。");

            // 持ち駒をSFEN表記に変換（ターンは常に先手でいい）
            string piece = this.ConvertToSfenPiece(move.PutPiece(), Turn.Black);
            int toFile = this.GetFile(move.ToBoard(), (int)move.ToIndex());
            char toRank = this.GetRank(move.ToBoard(), (int)move.ToIndex());
            return string.Format("{0}{1}{2}{3}", piece, HandValueSign, toFile, toRank);
        }

        /// <summary>
        /// 駒をSFEN表記に変換します。
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public string ConvertToSfenPiece(Piece piece, Turn turn)
        {
            string promoteSign = piece.IsPromoted() ? PromoteSign : string.Empty;
            char sfenPiece = PiecesSign[piece.UndoPromoted().GetIndex()];
            sfenPiece = (turn == Turn.Black) ? sfenPiece : sfenPiece.ToString().ToLower().ToCharArray()[0];
            return string.Format("{0}{1}", promoteSign, sfenPiece);
        }

        /// <summary>
        /// SFEN表記の指し手をNielsの指し手に変換します。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public uint ConvertToNielsMove(string sfenMove, BoardContext context)
        {
            if (sfenMove.Contains(HandValueSign))
            {
                return this.ConvertToNielsHandValueMove(sfenMove, context);
            }
            else
            {
                return this.InnerConvertToNielsMove(sfenMove, context);
            }
        }

        /// <summary>
        /// SFEN表記の指し手をNielsの指し手に変換します。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        protected uint InnerConvertToNielsMove(string sfenMove, BoardContext context)
        {
            string from = sfenMove.Substring(0, 2);
            BoardType fromBoardType = this.GetBoardType(from);
            int fromIndex = this.GetIndex(from);

            string to = sfenMove.Substring(2, 2);
            BoardType toBoardType = this.GetBoardType(to);
            int toIndex = this.GetIndex(to);

            Piece piece = context.GetTurnPiece(fromBoardType, fromIndex).ToPiece();
            Promote promote = Promote.No;
            if (this.IsPromote(sfenMove))
            {
                promote = Promote.Yes;
                piece = piece.Promote();
            }
            Piece capturedPiece = CaptureGenerator.GenerateAnyCapturedPiece(toBoardType, toIndex, context);

            return Move.GetMove(piece, fromBoardType, (uint)fromIndex, toBoardType, (uint)toIndex, context.Turn, capturedPiece, promote);
        }

        /// <summary>
        /// SFEN表記の指し手をNielsの持ち駒の打ち手に変換します。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        protected uint ConvertToNielsHandValueMove(string sfenMove, BoardContext context)
        {
            Piece piece = HandValuePieces[PiecesSign.IndexOf(sfenMove.Substring(0, 1))];
            string to = sfenMove.Substring(2, 2);
            BoardType toBoardType = this.GetBoardType(to);
            int toIndex = this.GetIndex(to);
            return Move.GetHandValueMove(piece, toBoardType, (uint)toIndex, context.Turn);
        }

        /// <summary>
        /// 段を取得します（縦：abc...）
        /// </summary>
        /// <param name="boardType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected new char GetRank(BoardType boardType, int index)
        {
            return RanksSign[base.GetRank(boardType, index) - 1];
        }

        /// <summary>
        /// SFEN表記の指し手から盤種別を取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected BoardType GetBoardType(string move)
        {
            Debug.Assert((move.Length == 2), "指し手の文字数が不正です。");
            int file = int.Parse(move.Substring(0, 1));
            int rank = RanksSign.IndexOf(move.Substring(1, 1)) + 1;

            return this.GetBoardType(file, rank);
        }

        /// <summary>
        /// SFEN表記の指し手からインデックスを取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected int GetIndex(string move)
        {
            Debug.Assert((move.Length == 2), "指し手の文字数が不正です。");
            int file = int.Parse(move.Substring(0, 1));
            int rank = RanksSign.IndexOf(move.Substring(1, 1)) + 1;

            return this.GetIndex(file, rank);
        }

        /// <summary>
        /// 指し手が成る手かどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool IsPromote(string move)
        {
            if (move.Length > 4)
            {
                return (move.Substring(4, 1) == "+");
            }
            else
            {
                return false;
            }
        }
    }
}
