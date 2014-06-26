using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.MagicBitBoardGenerator.Accessor
{
    /// <summary>
    /// 盤へのアクセス、操作機能を提供します。
    /// </summary>
    internal static class BoardAccesor
    {
        /// <summary>
        /// 下部盤に属する座標
        /// </summary>
        private static readonly List<int> BottomIndexs = new List<int>()
        {
            31, 42, 53, 64, 75, 86, 97, 108
        };

        /// <summary>
        /// 壁に属する座標
        /// </summary>
        private static readonly List<int> WallIndexs = new List<int>()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
            11, 22, 33, 44, 55, 66, 77, 88, 99,
            21, 32, 43, 54, 65, 76, 87, 98, 109
        };

        /// <summary>
        /// 右辺に属する座標
        /// </summary>
        private static readonly List<int> RightLineIndexs = new List<int>()
        {
            12, 13, 14, 15, 16, 17, 18, 19, 20
        };

        /// <summary>
        /// 左辺に属する座標
        /// </summary>
        private static readonly List<int> LeftLineIndexs = new List<int>()
        {
            100, 101, 102, 103, 104, 105, 106, 107, 108
        };

        /// <summary>
        /// 上辺に属する座標
        /// </summary>
        private static readonly List<int> UpperLineIndexs = new List<int>()
        {
            12, 23, 34, 45, 56, 67, 78, 89, 100
        };

        /// <summary>
        /// 下辺に属する座標
        /// </summary>
        private static readonly List<int> LowerLineIndexs = new List<int>()
        {
            20, 31, 42, 53, 64, 75, 86, 97, 108
        };

        /// <summary>
        /// 辺に属する座標
        /// </summary>
        private static readonly List<int> LineIndexs;

        /// <summary>
        /// 角に属する座標
        /// </summary>
        private static readonly List<int> EdgeIndexs = new List<int>()
        {
            12, 20, 100, 108
        };

        /// <summary>
        /// 盤の進む方向
        /// </summary>
        internal enum BoardDirection
        {
            Up = -1,
            Down = 1,
            Right = -11,
            Left = 11,
            UpperRight = -12,
            UpperLeft = 10,
            LowerRight = -10,
            LowerLeft = 12
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static BoardAccesor()
        {
            LineIndexs = RightLineIndexs.Union(LeftLineIndexs).Union(LowerLineIndexs).Union(UpperLineIndexs).ToList();
        }

        /// <summary>
        /// インデックスをオリジナルのインデックスに変換する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int GetOriginalIndex(int index)
        {
            if (index >= 12 && index <= 20)
            {
                return index - 12;
            }

            int file = ((index - 23) / 11);
            int subIndex = (BottomIndexs.Contains(index)) ? 24 : 23;
            
            return (index - subIndex - (file * 3));
        }

        /// <summary>
        /// オリジナルのインデックスをテスト用インデックスに変換する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int GetSequanceIndex(int index, BoardType boardType)
        {
            if (boardType == BoardType.SubRight)
            {
                return index + 12;
            }

            int file = (index / 8);
            int addIndex = (boardType == BoardType.Main) ? 23 : 24;
            return (index + addIndex + (file * 3));
        }

        /// <summary>
        /// インデックスが属する盤タイプを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static BoardType GetBoardType(int index)
        {
            if (index >= 12 && index <= 20)
            {
                return BoardType.SubRight;
            }

            if (BottomIndexs.Contains(index))
            {
                return BoardType.SubBottom;
            }

            return BoardType.Main;
        }

        /// <summary>
        /// 指定した座標が壁かどうかを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static bool IsWall(int index)
        {
            return WallIndexs.Contains(index);
        }

        /// <summary>
        /// 指定した座標が辺かどうかを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static bool IsLine(int index)
        {
            return LineIndexs.Contains(index);
        }

        /// <summary>
        /// 指定した座標が垂直方向の辺かどうかを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static bool IsVerticalLine(int index)
        {
            return RightLineIndexs.Contains(index) || LeftLineIndexs.Contains(index);
        }

        /// <summary>
        /// 指定した座標が平行方向の辺かどうかを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static bool IsHorizontalLine(int index)
        {
            return UpperLineIndexs.Contains(index) || LowerLineIndexs.Contains(index);
        }

        /// <summary>
        /// 指定した座標が角かどうかを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static bool IsEdge(int index)
        {
            return EdgeIndexs.Contains(index);
        }
    }
}
