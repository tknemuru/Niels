using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// 盤クラス
    /// </summary>
    public abstract class Board
    {
        /// <summary>
        /// 盤のマス目数
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// 内部構造の種類数
        /// </summary>
        public const int BoardTypeCount = 3;

        /// <summary>
        /// 上に移動する場合のインデックス増減
        /// </summary>
        public virtual int Up
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// 下に移動する場合のインデックス増減
        /// </summary>
        public virtual int Down
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 右に移動する場合のインデックス増減
        /// </summary>
        public virtual int Right
        {
            get
            {
                return -8;
            }
        }

        /// <summary>
        /// 左に移動する場合のインデックス増減
        /// </summary>
        public virtual int Left
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 右上に移動する場合のインデックス増減
        /// </summary>
        public virtual int UpRight
        {
            get
            {
                return -9;
            }
        }

        /// <summary>
        /// 左上に移動する場合のインデックス増減
        /// </summary>
        public virtual int UpLeft
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        /// 右下に移動する場合のインデックス増減
        /// </summary>
        public virtual int DownRight
        {
            get
            {
                return -7;
            }
        }

        /// <summary>
        /// 左下に移動する場合のインデックス増減
        /// </summary>
        public virtual int DownLeft
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// 内部盤構造の種類
        /// </summary>
        public abstract BoardType BoardType { get; }

        /// <summary>
        /// 使用するインデックス
        /// </summary>
        public abstract int[] UsingIndexs { get; }

        /// <summary>
        /// 各段をクリアする
        /// </summary>
        public abstract ulong[] ClearRank { get; }

        /// <summary>
        /// 各筋をクリアする
        /// </summary>
        public abstract ulong[] ClearFile { get; }

        /// <summary>
        /// 敵陣にフォーカスする
        /// １番目：先手
        /// ２番目：後手
        /// </summary>
        public abstract ulong[] FocusEnemyArea { get; }

        /// <summary>
        /// 全ての盤の中で一意となるマス目の連番を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract int GetSequanceIndex(int index);
    }
}