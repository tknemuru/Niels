using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using Niels.Boards;

namespace Niels.Evaluators
{
    /// <summary>
    /// 特徴ベクトル
    /// </summary>
    internal interface IFeatureVector
    {
        /// <summary>
        /// 特徴ベクトルを生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        SparseVector<double> Generate(BoardContext context);
    }
}
