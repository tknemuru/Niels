using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using Niels.Boards;

namespace Niels.Learning.Evaluating
{
    /// <summary>
    /// 特徴ベクトル生成機能を提供します。
    /// </summary>
    internal interface IFeatureVectorGenerator
    {
        /// <summary>
        /// 特徴ベクトルを生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        SparseVector<double> Generate(BoardContext context);
    }
}
