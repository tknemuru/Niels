using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;

namespace Niels.Tests.TestHelper
{
    /// <summary>
    /// テスト用打ち手クラス
    /// </summary>
    internal static class MoveForTest
    {
        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <returns></returns>
        internal static uint GetMove(Piece piece, int fromIndex, int toIndex, Turn turn, Piece capturePiece, Promote promote)
        {
            uint transrateFromIndex = (uint)BoardForTest.TransrateIndex(fromIndex);
            uint transrateToIndex = (uint)BoardForTest.TransrateIndex(toIndex);
            BoardType fromBoard = BoardForTest.GetBoardType(fromIndex);
            BoardType toBoard = BoardForTest.GetBoardType(toIndex);

            return Move.GetMove(piece, fromBoard, transrateFromIndex, toBoard, transrateToIndex, turn, capturePiece, promote);
        }

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <returns></returns>
        internal static uint GetMove(Piece piece, int fromIndex, int toIndex, Turn turn, Piece capturePiece)
        {
            return MoveForTest.GetMove(piece, fromIndex, toIndex, turn, capturePiece, Promote.No);
        }

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <returns></returns>
        internal static uint GetMove(Piece piece, int fromIndex, int toIndex, Turn turn, Promote promote)
        {
            return MoveForTest.GetMove(piece, fromIndex, toIndex, turn, Piece.Empty, promote);
        }

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <returns></returns>
        internal static uint GetMove(Piece piece, int fromIndex, int toIndex, Turn turn)
        {
            return MoveForTest.GetMove(piece, fromIndex, toIndex, turn, Piece.Empty, Promote.No);
        }

        /// <summary>
        /// 持ち駒からの打ち手を取得します。
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="toBoard"></param>
        /// <param name="toIndex"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static uint GetHandValueMove(Piece piece, int toIndex, Turn turn)
        {
            uint transrateToIndex = (uint)BoardForTest.TransrateIndex(toIndex);
            BoardType toBoard = BoardForTest.GetBoardType(toIndex);
            return Move.GetHandValueMove(piece, toBoard, transrateToIndex, turn);
        }

        /// <summary>
        /// デバッグ用の文字列に変換して表示する
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        internal static string ToDebugString(this uint ui)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("移動元インデックス：" + BoardForTest.TransrateIndex((int)ui.FromIndex(), ui.FromBoard()));
            sb.AppendLine("移動先インデックス：" + BoardForTest.TransrateIndex((int)ui.ToIndex(), ui.ToBoard()));
            sb.AppendLine("移動する駒：" + (Piece)ui.PutPiece());
            sb.AppendLine("取られる駒：" + (Piece)ui.CapturePiece());
            sb.AppendLine("移動する駒のターン：" + ui.PutPieceTurn());
            sb.AppendLine("成る手：" + ui.IsPromote());
            return sb.ToString();
        }
    }
}
