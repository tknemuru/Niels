using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Collections
{
    /// <summary>
    /// 盤の座標情報
    /// </summary>
    public static class BoardPotision
    {
        /// <summary>
        /// 盤種別のシフト量
        /// </summary>
        public const int ShiftBoardType = 7;

        /// <summary>
        /// 盤の座標情報を取得します。
        /// </summary>
        /// <param name="boardType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ushort GetBoardPotision(BoardType boardType, int index)
        {
            return (ushort)(((int)boardType << ShiftBoardType) | index);
        }
    }

    /// <summary>
    /// 盤の座標情報用ushort拡張
    /// </summary>
    public static class ExtensionUshortMove
    {
        /// <summary>
        /// 盤種別
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public static BoardType BoardType(this ushort pointer)
        {
            return (BoardType)(pointer >> BoardPotision.ShiftBoardType);
        }

        /// <summary>
        /// インデックス
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public static int Index(this ushort pointer)
        {
            return (pointer & 0x7f);
        }
    }
}
