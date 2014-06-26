using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// サブ盤クラス（底）
    /// </summary>
    public class BoardSubBottom : Board
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
            63,	55,	47,	39,	31,	23,	15,	7
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
        /// 上に移動する場合のインデックス増減
        /// </summary>
        public override int Up
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 下に移動する場合のインデックス増減
        /// </summary>
        public override int Down
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
                return -8;
            }
        }

        /// <summary>
        /// 左上に移動する場合のインデックス増減
        /// </summary>
        public override int UpLeft
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 右下に移動する場合のインデックス増減
        /// </summary>
        public override int DownRight
        {
            get
            {
                return -8;
            }
        }

        /// <summary>
        /// 左下に移動する場合のインデックス増減
        /// </summary>
        public override int DownLeft
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 内部盤構造の種類
        /// </summary>
        public override BoardType BoardType
        {
            get { return _BoardType; }
        }
        private const BoardType _BoardType = Boards.BoardType.SubBottom;

        /// <summary>
        /// 盤の各段をクリアする
        /// </summary>
        public override ulong[] ClearRank
        {
            get { return _ClearRank; }
        }
        private static readonly ulong[] _ClearRank =
        {
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0xffffffffffffffff,
            0x7f7f7f7f7f7f7f7f
        };

        /// <summary>
        /// 盤の各筋をクリアする
        /// </summary>
        public override ulong[] ClearFile
        {
            get { return _ClearFile; }
        }
        public static readonly ulong[] _ClearFile = 
        {
            0xffffffffffffffff,
            0xffffffffffffff7f,
            0xffffffffffff7fff,
            0xffffffffff7fffff,
            0xffffffff7fffffff,
            0xffffff7fffffffff,
            0xffff7fffffffffff,
            0xff7fffffffffffff,
            0x7fffffffffffffff
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
            0,
            0x8080808080808080
        };

        /// <summary>
        /// マス目の連番の基数
        /// </summary>
        private static readonly int SequenceBaseIndex = new BoardMain().Length + new BoardSubRight().Length;

        /// <summary>
        /// 全ての盤の中で一意となるマス目の連番を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetSequanceIndex(int index)
        {
            return (index / 8) + SequenceBaseIndex;
        }
    }
}