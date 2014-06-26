using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using Niels.Extensions.Number;
using Niels.MagicBitBoardGenerator.Generators.MoveRelationIndexs;

namespace Niels.MagicBitBoardGenerator.Generators.Pattern
{
    /// <summary>
    /// 利きに関係するマス目のパターンを生成する機能を提供します。
    /// </summary>
    internal class PatternGenerator
    {
        /// <summary>
        /// 利きに関係のあるマス目のみを抽出します。
        /// </summary>
        private MoveRelationIndexsGenerator MoveRelationIndexsGenerator { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gen"></param>
        internal PatternGenerator(MoveRelationIndexsGenerator gen)
        {
            this.MoveRelationIndexsGenerator = gen;
        }

        /// <summary>
        /// 利きに関係するマス目のパターンを生成します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal List<Dictionary<int, bool>> Generate(int index, BoardType boardType, Piece targetPiece)
        {
            List<int> canMoveIndexs = this.MoveRelationIndexsGenerator.Generate(index, boardType).ToList();           
            int count = canMoveIndexs.Count();
            ulong maxValue = (count > 0) ? this.GetMaxValue(count) : 0;
            List<Dictionary<int, bool>> patterns = new List<Dictionary<int, bool>>();
            for (ulong patternSeed = 0; patternSeed <= maxValue; patternSeed++)
            {
                ulong ulongPatternSeed = patternSeed;
                Dictionary<int, bool> pattern = new Dictionary<int, bool>();
                for (int i = 0; i < count; i++)
                {
                    pattern.Add(canMoveIndexs[i], ulongPatternSeed.IsPositive(i));
                }
                patterns.Add(pattern);
            }

            return patterns;
        }

        /// <summary>
        /// 指定した桁数(2進数)の最大値を取得します。
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private ulong GetMaxValue(int count)
        {
            string binaryDigit = string.Empty;
            for (int i = 0; i < count; i++)
            {
                binaryDigit += "1";
            }

            return Convert.ToUInt64(binaryDigit, 2);
        }
    }
}
