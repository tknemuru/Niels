using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// 盤タイプ
    /// </summary>
    public enum BoardType
    {
        /// <summary>
        /// メイン
        /// </summary>
        Main = 0,

        /// <summary>
        /// サブ（右）
        /// </summary>
        SubRight = 1,

        /// <summary>
        /// サブ（下）
        /// </summary>
        SubBottom = 2
    }
}