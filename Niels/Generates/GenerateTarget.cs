using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Generates
{
    /// <summary>
    /// 指し手の生成対象
    /// </summary>
    [Flags]
    public enum GenerateTarget
    {
        /// <summary>
        /// 手を生成しません。
        /// </summary>
        None = 0,

        /// <summary>
        /// 成る指し手を除いて生成します。
        /// </summary>
        NoPromote = 0x1,

        /// <summary>
        /// 成る指し手のみを生成します。
        /// </summary>
        Promote = 0x2,

        /// <summary>
        /// 全ての指し手を生成します。
        /// </summary>
        All = NoPromote | Promote
    }
}
