using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;

namespace Niels.Diagnostics
{
    /// <summary>
    /// 指し手に関するデバッグ機能を提供します。
    /// </summary>
    public static class MoveDebug
    {
        /// <summary>
        /// 指し手をデバッグ用の文字列に変換します。
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static string ToDebugString(uint move)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("移動元盤種別：" + move.FromBoard());
            sb.AppendLine("移動元インデックス：" + move.FromIndex());
            sb.AppendLine("移動先盤種別：" + move.ToBoard());
            sb.AppendLine("移動先インデックス：" + move.ToIndex());
            sb.AppendLine("移動する駒：" + (Piece)move.PutPiece());
            sb.AppendLine("取られる駒：" + (Piece)move.CapturePiece());
            sb.AppendLine("移動する駒のターン：" + move.PutPieceTurn());
            sb.AppendLine("成る手：" + move.IsPromote());
            return sb.ToString();
        }
    }
}
