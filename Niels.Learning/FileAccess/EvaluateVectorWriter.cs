using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using Niels.Helper;
using Niels.Learning.Settings;

namespace Niels.Learning.FileAccess
{
    /// <summary>
    /// 評価値ベクターの書き込み機能を提供します。
    /// </summary>
    internal class EvaluateVectorWriter
    {
        /// <summary>
        /// 評価値に履かせる下駄数
        /// </summary>
        private const double EvaluateInflate = 1;

        /// <summary>
        /// 設定情報
        /// </summary>
        private LearningSettings Settings;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings"></param>
        internal EvaluateVectorWriter(LearningSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// 評価値ベクターの書き込みを行います。
        /// </summary>
        /// <param name="evaluateVector"></param>
        internal void Write(SparseVector<double> evaluateVector, int stage)
        {
            string csv = string.Empty;
            foreach (var evaluate in evaluateVector)
            {
                if (string.IsNullOrEmpty(csv))
                {
                    csv += string.Format("{0}", (int)(evaluate * EvaluateInflate));
                }
                else
                {
                    csv += string.Format(",{0}", (int)(evaluate * EvaluateInflate));
                }
            }
            FileHelper.Write(csv, string.Format(this.Settings.EvaluateVectorOutPutPath, stage), false);
        }
    }
}
