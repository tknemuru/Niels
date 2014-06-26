using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Learning.NotationReading
{
    /// <summary>
    /// 棋譜の読み込み機能を提供します。
    /// </summary>
    public interface INotationReader
    {
        /// <summary>
        /// 棋譜を読み込みます。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IEnumerable<NotationInformation> Read(string filePath);
    }
}
