using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// サブ盤クラス（右）
    /// </summary>
    public class BoardSubRight : Board
    {
        /// <summary>
        /// 使用するインデックス
        /// </summary>
        public override int[] UsingIndexs
        {
            get { return _UsingIndexs; }
        }
        private static readonly int[] _UsingIndexs =
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8
        };

        /// <summary>
        /// 盤のマス目数
        /// </summary>
        public override int Length
        {
            get { return _Length; }
        }
        private static readonly int _Length = _UsingIndexs.Count();

        /// <summary>
        /// 右に移動する場合のインデックス増減
        /// </summary>
        public override int Right
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 左に移動する場合のインデックス増減
        /// </summary>
        public override int Left
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 右上に移動する場合のインデックス増減
        /// </summary>
        public override int UpRight
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// 左上に移動する場合のインデックス増減
        /// </summary>
        public override int UpLeft
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// 右下に移動する場合のインデックス増減
        /// </summary>
        public override int DownRight
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 左下に移動する場合のインデックス増減
        /// </summary>
        public override int DownLeft
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 内部盤構造の種類
        /// </summary>
        public override BoardType BoardType
        {
            get { return _BoardType; }
        }
        private const BoardType _BoardType = Boards.BoardType.SubRight;

        /// <summary>
        /// メイン盤の各段をクリアする
        /// </summary>
        public override ulong[] ClearRank
        {
            get { return _ClearRank; }
        }
        private static readonly ulong[] _ClearRank =
        {
            0xfffffffffffffffe,
            0xfffffffffffffffd,
            0xfffffffffffffffb,
            0xfffffffffffffff7,
            0xffffffffffffffef,
            0xffffffffffffffdf,
            0xffffffffffffffbf,
            0xffffffffffffff7f,
            0xfffffffffffffeff
        };

        /// <summary>
        /// メイン盤の各筋をクリアする
        /// </summary>
        public override ulong[] ClearFile
        {
            get { return _ClearFile; }
        }
        public static readonly ulong[] _ClearFile = 
        {
            0xffffffffffffff00,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff
        };

        /// <summary>
        /// 敵陣にフォーカスする
        /// １番目：先手
        /// ２番目：後手
        /// </summary>
        public override ulong[] FocusEnemyArea
        {
            get { return _focusEnemyArea; }
        }
        private static readonly ulong[] _focusEnemyArea =
        {
            0x007,
            0x1c0
        };

        /// <summary>
        /// マス目の連番の基数
        /// </summary>
        private static readonly int SequenceBaseIndex = new BoardMain().Length;

        /// <summary>
        /// 全ての盤の中で一意となるマス目の連番を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetSequanceIndex(int index)
        {
            return index + SequenceBaseIndex;
        }
    }
}