using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using Niels.Collections;
using Niels.Boards;
using Niels.Helper;
using Niels.Learning.Settings;
using System.Diagnostics;

namespace Niels.Learning.Evaluating
{
    /// <summary>
    /// 評価値ベクトルの更新機能を提供します。
    /// </summary>
    internal class EvaluateVectorUpdator
    {
        /// <summary>
        /// 特徴の更新回数を記録するベクトル
        /// </summary>
        protected SparseVector<double> FeatureUpdateCountVector { get; set; }

        /// <summary>
        /// 設定情報
        /// </summary>
        protected LearningSettings Settings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="featureVectorLength">特徴ベクトルの長さ</param>
        internal EvaluateVectorUpdator(LearningSettings settings)
        {
            this.Settings = settings;
            this.FeatureUpdateCountVector = new SparseVector<double>(0D, settings.FeatureVectorLength);
        }

        /// <summary>
        /// 評価値ベクトルを更新します。
        /// </summary>
        /// <param name="evaluateVector"></param>
        /// <param name="featureVector"></param>
        /// <param name="featureUpdateCountVector"></param>
        /// <param name="evError"></param>
        /// <returns></returns>
        internal virtual SparseVector<double> Update(SparseVector<double> evaluateVector, SparseVector<double> featureVector, SparseVector<double> featureUpdateCountVector, double evError)
        {
            throw new NotImplementedException("未実装です。");
        }

        /// <summary>
        /// 評価値ベクトルを更新します。
        /// </summary>
        /// <param name="evaluateVector"></param>
        /// <param name="teacherFeatureVector"></param>
        /// <param name="featureVector"></param>
        /// <param name="modificationVector"></param>
        /// <param name="isMatched"></param>
        /// <returns></returns>
        internal virtual SparseVector<double> Update(SparseVector<double> evaluateVector, SparseVector<double> teacherFeatureVector, SparseVector<double> featureVector, SparseVector<double> modificationVector, bool isMatched, Turn turn)
        {
            Debug.Assert(evaluateVector.Count() == featureVector.Count(), "ベクトルの要素数が異なります。");
            Debug.Assert(teacherFeatureVector.Count() == featureVector.Count(), "ベクトルの要素数が異なります。");
            Debug.Assert(this.FeatureUpdateCountVector.Count() == featureVector.Count(), "ベクトルの要素数が異なります。");

            SparseVector<double> tempModificationVector = new SparseVector<double>(0D, featureVector.Count());

            double teacherEvaluate = 0D;
            foreach (var keyValue in teacherFeatureVector.NoSparseKeyValues)
            {
                teacherEvaluate += evaluateVector[keyValue.Key] * keyValue.Value;
                tempModificationVector[keyValue.Key] -= keyValue.Value;
            }

            double evaluate = 0D;
            foreach (var keyValue in featureVector.NoSparseKeyValues)
            {
                evaluate += evaluateVector[keyValue.Key] * keyValue.Value;
                tempModificationVector[keyValue.Key] += keyValue.Value;
            }

            // 評価値の差
            double evaluateDiff = evaluate - teacherEvaluate;
            Console.WriteLine("EvaluateDiff:" + evaluateDiff);
            
            // 学習序盤は一致していなくても評価値の差が0になる場合がある。
            // その場合に更新しないと、一生0のままになってしまうので、固定で評価値の差をセットする。
            if (!isMatched && evaluate == 0 && teacherEvaluate == 0)
            {
                evaluateDiff = (turn == Turn.Black) ? 1.0D : -1.0D;
                //evaluateDiff = 1.0D;
            }
            //Debug.Assert((isMatched && evaluateDiff == 0D) || !isMatched, "一致しているのに評価値の差が0じゃありません。");

            foreach (var keyValue in tempModificationVector.NoSparseKeyValues)
            {
                //if (keyValue.Value > 0D)
                //{
                //    // 合法手mでのみ現れた特徴は評価値を下げる
                //    modificationVector[keyValue.Key] -= this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                //}
                //else if (keyValue.Value < 0D)
                //{
                //    // 棋譜の指し手でのみ現れた特徴は評価値を上げる
                //    modificationVector[keyValue.Key] += this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                //}
                if (turn == Turn.Black)
                {
                    if (keyValue.Value > 0D)
                    {
                        // 合法手mでのみ現れた特徴は評価値を下げる
                        modificationVector[keyValue.Key] -= this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                    }
                    else if (keyValue.Value < 0D)
                    {
                        // 棋譜の指し手でのみ現れた特徴は評価値を上げる
                        modificationVector[keyValue.Key] += this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                    }
                }
                else
                {
                    if (keyValue.Value > 0D)
                    {
                        // 合法手mでのみ現れた特徴は評価値を上げる
                        modificationVector[keyValue.Key] += this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                    }
                    else if (keyValue.Value < 0D)
                    {
                        // 棋譜の指し手でのみ現れた特徴は評価値を下げる
                        modificationVector[keyValue.Key] -= this.CalcModificationEvaluate(keyValue.Key, evaluateDiff);
                    }
                }
            }

            return modificationVector;
        }

        /// <summary>
        /// 評価の更新値を計算します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="evaluateDiff"></param>
        /// <returns></returns>
        private double CalcModificationEvaluate(int key, double evaluateDiff)
        {
            // 差が0なら更新値も0
            if (evaluateDiff == 0) { return 0D; }

            // 罰則項
            // 評価値の差が0未満ならば既に棋譜の指し手の方が高評価になっている。
            // それを際限なく上方修正し続けると上がりすぎてしまうので、更新値を抑えるようにしてみる。
            //double penal = (evaluateDiff >= 0) ? 1D : Math.Abs(evaluateDiff) * 0.1;
            double penal = 1D;

            // 正則化した更新値α
            double normalizeAlpha = MathHelper.Min<double>((this.Settings.Alpha / 100), (this.Settings.Alpha / this.FeatureUpdateCountVector[key]));

            // 更新値
            double modificationEvaluate = Math.Abs(evaluateDiff) * normalizeAlpha * penal;

            return modificationEvaluate;
        }
    }
}
