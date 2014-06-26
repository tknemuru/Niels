using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using Niels.Collections;
using Niels.Boards;
using Niels.Evaluators;
using Niels.Helper;
using Niels.Learning.Settings;
using System.Diagnostics;


namespace Niels.Learning.Evaluating
{
    /// <summary>
    /// 評価の更新値の調整機能を提供します。
    /// </summary>
    internal class SteepestDescentEvaluateUpdator : EvaluateVectorUpdator
    {
        /// <summary>
        /// 駒割の初期評価値
        /// </summary>
        private static readonly List<double> DefaultPieceEvaluate = new List<double>()
        {
            87,
            232,
            257,
            369,
            444,
            569,
            642,
            0,
            534,
            489,
            510,
            495,
            827,
            945
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings"></param>
        internal SteepestDescentEvaluateUpdator(LearningSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// 評価値を更新します。
        /// </summary>
        /// <param name="evaluateVector"></param>
        /// <param name="featureVector"></param>
        /// <param name="featureUpdateCountVector"></param>
        /// <param name="evError"></param>
        /// <returns></returns>
        internal override SparseVector<double> Update(SparseVector<double> evaluateVector, SparseVector<double> featureVector, SparseVector<double> featureUpdateCountVector, double evError)
        {
            foreach (KeyValuePair<int, double> keyValue in featureVector.NoSparseKeyValues)
            {
                if (this.FeatureUpdateCountVector[keyValue.Key] == 0D && featureUpdateCountVector[keyValue.Key] != 0D)
                {
                    // 一度でも出現したらMinから0に上げる
                    evaluateVector[keyValue.Key] = 0;

                    // 駒割の価値は個別に初期化
                    //if (keyValue.Key >= (int)SequencePositionFeatureVector.ExtraInfo.PieceCount && keyValue.Key < (int)SequencePositionFeatureVector.ExtraInfo.Parity)
                    //{
                    //    evaluateVector[keyValue.Key] = DefaultPieceEvaluate[keyValue.Key - (int)SequencePositionFeatureVector.ExtraInfo.PieceCount];
                    //}
                }

                double normalizeAlpha = 0D;
                if (featureUpdateCountVector[keyValue.Key] < 1000)
                {
                    normalizeAlpha = MathHelper.Min<double>((this.Settings.Alpha / 100), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                else if (featureUpdateCountVector[keyValue.Key] < 30000)
                {
                    normalizeAlpha = MathHelper.Max<double>((this.Settings.Alpha / 1000), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                else if (featureUpdateCountVector[keyValue.Key] < 100000)
                {
                    normalizeAlpha = MathHelper.Max<double>((this.Settings.Alpha / 5000), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                else if (featureUpdateCountVector[keyValue.Key] < 500000)
                {
                    normalizeAlpha = MathHelper.Max<double>((this.Settings.Alpha / 10000), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                else if (featureUpdateCountVector[keyValue.Key] < 1000000)
                {
                    normalizeAlpha = MathHelper.Max<double>((this.Settings.Alpha / 100000), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                else
                {
                    normalizeAlpha = MathHelper.Min<double>((this.Settings.Alpha / 100000), (this.Settings.Alpha / featureUpdateCountVector[keyValue.Key]));
                }
                evaluateVector[keyValue.Key] += (normalizeAlpha * evError * keyValue.Value);
            }

            // 回数を保存
            this.FeatureUpdateCountVector = featureUpdateCountVector.Clone();

            return evaluateVector;
        }

        /// <summary>
        /// 評価値を更新します。
        /// </summary>
        /// <param name="evaluateVector"></param>
        /// <param name="teacherFeatureVector"></param>
        /// <param name="featureVector"></param>
        /// <param name="modificationVector"></param>
        /// <param name="isMatched"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        internal override SparseVector<double> Update(SparseVector<double> evaluateVector, SparseVector<double> teacherFeatureVector, SparseVector<double> featureVector, SparseVector<double> modificationVector, bool isMatched, Turn turn)
        {
            throw new NotImplementedException("実装されていません。");
        }
    }
}
