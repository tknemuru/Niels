using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Accessor;
using System.Diagnostics;

namespace Niels.MagicBitBoardGenerator.Converters
{
    /// <summary>
    /// インデックスの集合を盤の状態に変換する機能を提供します。
    /// </summary>
    internal static class BoardStateConverter
    {
        /// <summary>
        /// インデックスの集合を盤の状態に変換します。
        /// </summary>
        /// <param name="stateIndexs"></param>
        /// <returns></returns>
        internal static ulong[] ToBoardState(Dictionary<int, bool> stateIndexs)
        {
            Dictionary<int, bool> mainPositives = new Dictionary<int, bool>();
            Dictionary<int, bool> subRightPositives = new Dictionary<int, bool>();
            Dictionary<int, bool> subBottomPositives = new Dictionary<int, bool>();

            foreach (var state in stateIndexs)
            {
                Debug.Assert(!BoardAccesor.IsWall(state.Key), "座標が壁です。");
                int orgIndex = BoardAccesor.GetOriginalIndex(state.Key);
                BoardType boardType = BoardAccesor.GetBoardType(state.Key);

                switch (boardType)
                {
                    case BoardType.Main:
                        mainPositives.Add(orgIndex, state.Value);
                        break;
                    case BoardType.SubRight:
                        subRightPositives.Add(orgIndex, state.Value);
                        break;
                    case BoardType.SubBottom:
                        subBottomPositives.Add(orgIndex, state.Value);
                        break;
                }
            }

            ulong[] boardState = new ulong[Board.BoardTypeCount];
            string mainBinaryDigits = BuildBinaryDidits(mainPositives);
            boardState[(int)BoardType.Main] = Convert.ToUInt64(mainBinaryDigits, 2);
            string subRightBinaryDigits = BuildBinaryDidits(subRightPositives);
            boardState[(int)BoardType.SubRight] = Convert.ToUInt64(subRightBinaryDigits, 2);
            string subBottomBinaryDigits = BuildBinaryDidits(subBottomPositives);
            boardState[(int)BoardType.SubBottom] = Convert.ToUInt64(subBottomBinaryDigits, 2);

            return boardState;
        }

        /// <summary>
        /// インデックスの集合を盤の状態に変換します。
        /// </summary>
        /// <param name="indexs"></param>
        /// <returns></returns>
        internal static ulong[] ToBoardState(IEnumerable<int> indexs)
        {
            Dictionary<int, bool> stateIndexs = indexs.ToDictionary(key => key, value => true);
            return ToBoardState(stateIndexs);
        }

        /// <summary>
        /// 1であるビットのインデックス情報を元に2進数の文字を組み立てます。
        /// </summary>
        /// <param name="positives"></param>
        /// <returns></returns>
        private static string BuildBinaryDidits(Dictionary<int, bool> positives)
        {
            string binaryDigits = string.Empty;
            for (int i = 0; i < 64; i++)
            {
                string addBit = (positives.ContainsKey(i) && positives[i]) ? "1" : "0";
                binaryDigits = addBit + binaryDigits;
            }

            return binaryDigits;
        }
    }
}
