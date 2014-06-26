using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// 盤提供クラス
    /// </summary>
    public static class BoardProvider
    {
        /// <summary>
        /// 盤
        /// </summary>
        private static readonly Board[] _boards;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static BoardProvider()
        {
            _boards = new Board[] {
                new BoardMain(),
                new BoardSubRight(),
                new BoardSubBottom()
            };

            // 念のためソート
            _boards = _boards.OrderBy(b => b.BoardType).ToArray();
        }

        /// <summary>
        /// メイン盤を取得する
        /// </summary>
        /// <returns></returns>
        public static Board GetMain()
        {
            return _boards[(int)BoardType.Main];
        }

        /// <summary>
        /// サブ盤（右）を取得する
        /// </summary>
        /// <returns></returns>
        public static Board GetSubRight()
        {
            return _boards[(int)BoardType.SubRight];
        }

        /// <summary>
        /// サブ盤（下）を取得する
        /// </summary>
        /// <returns></returns>
        public static Board GetSubBottom()
        {
            return _boards[(int)BoardType.SubBottom];
        }

        /// <summary>
        /// 全ての盤を取得する
        /// </summary>
        /// <returns></returns>
        public static Board[] GetAll()
        {
            return _boards;
        }
    }
}