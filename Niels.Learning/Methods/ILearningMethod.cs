using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Learning.Methods
{
    /// <summary>
    /// 機械学習ロジックを提供します。
    /// </summary>
    internal interface ILearningMethod
    {
        /// <summary>
        /// 機械学習を行います。
        /// </summary>
        void Learn();
    }
}
