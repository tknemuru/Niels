using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Evaluators;
using Niels.Boards;
using Niels.Collections;
using Niels.Collections.Math;
using Niels.Diagnostics;

namespace Niels.Learning.Evaluating
{
    /// <summary>
    /// 連続して隣り合っている駒の関係性から特徴ベクトルを生成します。
    /// </summary>
    internal class SequencePositionFeatureVectorGenerator : IFeatureVectorGenerator
    {
        /// <summary>
        /// 特徴ベクトルを生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public SparseVector<double> Generate(BoardContext context)
        {
            var featureDic = SequencePositionFeatureVector.Generate(context);
            var featureDoubleDic = (from dic in featureDic
                                    select new { Key = dic.Key, Value = (double)dic.Value })
                                    .ToDictionary(item => item.Key, item => item.Value);
            var ret = new SparseVector<double>(0D, SequencePositionFeatureVector.Length, featureDoubleDic);
            return ret;
        }
    }
}
