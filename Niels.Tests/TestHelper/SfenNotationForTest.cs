using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Notation;
using Niels.Boards;

namespace Niels.Tests.TestHelper
{
    /// <summary>
    /// SFEN(Shogi Forsyth-Edwards Notation)表記法による文字列処理を行ないます。（テスト用）
    /// </summary>
    internal class SfenNotationForTest : SfenNotation
    {
        /// <summary>
        /// SFEN表記の指し手から盤種別を取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        internal BoardType GetBoardTypeForTest(string move)
        {
            return base.GetBoardType(move);
        }

        /// <summary>
        /// SFEN表記の指し手からインデックスを取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        internal int GetIndexForTest(string move)
        {
            return base.GetIndex(move);
        }
    }
}
