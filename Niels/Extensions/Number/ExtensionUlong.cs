using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Niels.Extensions.Number
{
    public static class ExtensionUlong
    {
        /// <summary>
        /// 指定したインデックスのビットが1かどうかを返す
        /// http://stackoverflow.com/questions/4854207/get-a-specific-bit-from-byte
        /// </summary>
        /// <param name="ul"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public static bool IsPositive(this ulong ul, int bitIndex)
        {
            return ((ul & ((ulong)1 << bitIndex)) != 0);
        }
    }
}