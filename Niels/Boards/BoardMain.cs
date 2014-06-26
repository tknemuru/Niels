using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niels.Boards
{
    /// <summary>
    /// メイン盤クラス
    /// </summary>
    public class BoardMain : Board
    {
        /// <summary>
        /// 使用するインデックス
        /// </summary>
        public override int[] UsingIndexs
        {
            get { return _usingIndexs; }
        }
        private static readonly int[] _usingIndexs =
        {
            56,	48,	40,	32,	24,	16,	8, 0,
            57,	49,	41,	33,	25,	17,	9, 1,
            58,	50,	42,	34,	26,	18,	10,	2,
            59,	51,	43,	35,	27,	19,	11,	3,
            60,	52,	44,	36,	28,	20,	12,	4,
            61,	53,	45,	37,	29,	21,	13,	5,
            62,	54,	46,	38,	30,	22,	14,	6,
            63,	55,	47,	39,	31,	23,	15,	7
        };
        
        /// <summary>
        /// 盤のマス目数
        /// </summary>
        public override int Length
        {
            get { return _Length; }
        }
        private static readonly int _Length = _usingIndexs.Count();

        /// <summary>
        /// 内部盤構造の種類
        /// </summary>
        public override BoardType BoardType
        {
            get { return _BoardType; }
        }
        private const BoardType _BoardType = Boards.BoardType.Main;

        /// <summary>
        /// 盤の各段をクリアする
        /// </summary>
        public override ulong[] ClearRank
        {
            get { return _clearRank; }
        }
        private static readonly ulong[] _clearRank =
        {
            0xfefefefefefefefe,
            0xfdfdfdfdfdfdfdfd,
            0xfbfbfbfbfbfbfbfb,
            0xf7f7f7f7f7f7f7f7,
            0xefefefefefefefef,
            0xdfdfdfdfdfdfdfdf,
            0xbfbfbfbfbfbfbfbf,
            0x7f7f7f7f7f7f7f7f,
            0xffffffffffffffff
        };

        /// <summary>
        /// 盤の各筋をクリアする
        /// </summary>
        public override ulong[] ClearFile
        {
            get { return _clearFile; }
        }
        private static readonly ulong[] _clearFile = 
        {
            0xffffffffffffffff,
            0xffffffffffffff00,
            0xffffffffffff00ff,
            0xffffffffff00ffff,
            0xffffffff00ffffff,
            0xffffff00ffffffff,
            0xffff00ffffffffff,
            0xff00ffffffffffff,
            0x00ffffffffffffff,
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
            0x0707070707070707,
            0xc0c0c0c0c0c0c0c0
        };

        /// <summary>
        /// 全ての盤の中で一意となるマス目の連番を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetSequanceIndex(int index)
        {
            return index;
        }
    }
}