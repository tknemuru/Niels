using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.NormalizedScoreIndexGenerating;

namespace Niels.NormalizedScoreIndexGenerating.Tests.TestHelper
{
    /// <summary>
    /// 正規化スコアインデックスを生成します。(テスト用)
    /// </summary>
    internal class NormalizedScoreIndexGeneratorForTest : NormalizedScoreIndexGenerator
    {
        /// <summary>
        /// インデックスの順序を逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int ReverseForTest(int index)
        {
            return NormalizedScoreIndexGenerator.Reverse(index);
        }

        /// <summary>
        /// 駒のターンを逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int ChangeTurnForTest(int index)
        {
            return NormalizedScoreIndexGenerator.ChangeTurn(index);
        }

        /// <summary>
        /// 駒のターンを逆にしてインデックスの順序を逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static int ChangeTurnAndReverseForTest(int index)
        {
            return NormalizedScoreIndexGenerator.ChangeTurnAndReverse(index);
        }
    }
}
