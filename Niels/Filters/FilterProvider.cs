using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Filters
{
    /// <summary>
    /// 指し手のフィルタを提供します。
    /// </summary>
    public static class FilterProvider
    {
        /// <summary>
        /// 行き所のない駒の指し手をフィルタします。（1列）
        /// </summary>
        public static readonly CanNotMoveFilter CanNotMoveFilterOneRank = new CanNotMoveFilter();

        /// <summary>
        /// 行き所のない駒の指し手をフィルタします。（2列）
        /// </summary>
        public static readonly CanNotMoveFilter CanNotMoveFilterTwoRank = new CanNotMoveFilter(2);

        /// <summary>
        /// 自玉を相手駒の利きにさらす指し手をフィルタします。
        /// </summary>
        public static readonly CheckedByMyselefMoveFilter CheckedByMyselefMoveFilter = new CheckedByMyselefMoveFilter();

        /// <summary>
        /// 自駒が既に存在している場所に指す手をフィルタします。
        /// </summary>
        public static readonly PieceDuplicateExistsMoveFilter PieceDuplicateExistsMoveFilter = new PieceDuplicateExistsMoveFilter();

        /// <summary>
        /// 二歩をフィルタします。
        /// </summary>
        public static readonly FileDuplicatePawnExistsMoveFilter FileDuplicatePawnExistsMoveFilter = new FileDuplicatePawnExistsMoveFilter();
    }
}
