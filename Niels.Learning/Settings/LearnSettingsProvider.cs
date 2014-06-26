using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Learning.Methods;
using Niels.Learning.Evaluating;
using Niels.Learning.Analysis;

namespace Niels.Learning.Settings
{
    /// <summary>
    /// 学習設定情報を提供します。
    /// </summary>
    internal static class LearnSettingsProvider
    {
        /// <summary>
        /// 最急降下法向けの学習設定情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static LearningSettings GetSteepestDescentLearningSettings()
        {
            LearningSettings settings = new LearningSettings();
            settings.Alpha = 0.001;
            settings.ConvergenceCheckUnit = 300000;
            settings.ExamCount = 50;
            settings.ExamRandomRate = 30;
            settings.ExamSuccessMse = 10000D * 10000D;
            settings.OneStageUnitTurn = 10;
            //settings.StageCount = 1;
            settings.IsDivideStage = true;
            settings.IsTurnSeparate = true;
            settings.TargetTurn = Turn.Black;
            settings.MaxEvaluate = 60000D;
            settings.MinEvaluate = -130D;
            settings.TargetStage = 0;
            settings.LearningSeedFilePath = @"./result/scoreindex/{0}.txt";
            settings.LearningVectorFilePath = @"./result/result/{0}.txt";
            //settings.LearnTargetStage = new List<bool>() { true };
            //settings.GetStage = (context =>
            //{
            //    return 0;
            //});
            settings.LearningMethod = new SteepestDescent(settings);
            settings.EvaluateVectorUpdator = new SteepestDescentEvaluateUpdator(settings);
            settings.LearningAnalysis = new SteepestDescentLearningAnalysis(settings);
            return settings;
        }
    }
}
