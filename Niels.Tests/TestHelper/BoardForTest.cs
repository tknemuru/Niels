using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Tests.TestHelper
{
    /// <summary>
    /// テスト用盤
    /// </summary>
    internal class BoardForTest
    {
        /// <summary>
        /// 下部盤に属する座標
        /// </summary>
        private static readonly List<int> BottomIndexs = new List<int>()
        {
            17, 26, 35, 44, 53, 62, 71, 80
        };

        /// <summary>
        /// インデックスをオリジナルのインデックスに変換する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int TransrateIndex(int index)
        {
            if (index >= 0 && index <= 8)
            {
                return index;
            }

            if (BottomIndexs.Contains(index))
            {
                return (index - (10 + ((index - 10) / 9)));
            }

            return (index - (9 + ((index / 9) - 1)));
        }

        /// <summary>
        /// インデックスが属する盤タイプを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static BoardType GetBoardType(int index)
        {
            if (index >= 0 && index <= 8)
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
        /// オリジナルのインデックスをテスト用インデックスに変換する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int TransrateIndex(int index, BoardType board)
        {
            if (board == BoardType.SubRight)
            {
                return index;
            }

            int file = (index / 8);
            int addIndex = (board == BoardType.Main) ? 9 : 10;
            return (index + addIndex + file);
        }
    }
}
