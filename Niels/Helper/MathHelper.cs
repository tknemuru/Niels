using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Helper
{
    /// <summary>
    /// 計算処理のサポート機能を提供します。
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// <para>大きい方の数値を返す</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static T Max<T>(T x, T y)
            where T : IComparable
        {
            return x.CompareTo(y) > 0 ? x : y;
        }

        /// <summary>
        /// <para>小さい方の数値を返す</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static T Min<T>(T x, T y)
            where T : IComparable
        {
            return x.CompareTo(y) < 0 ? x : y;
        }
    }
}
