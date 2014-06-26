using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Evaluators;
using Niels.Generates;
using Niels.Learning.NotationReading;
using Niels.Learning.Evaluating;
using Niels.Learning.Analysis;
using Niels.Learning.FileAccess;
using Niels.Learning.Methods;

namespace Niels.Learning.Settings
{
    /// <summary>
    /// 機械学習の設定情報を保持します。
    /// </summary>
    internal sealed class LearningSettings
    {      
        /// <summary>
        /// 機械学習ロジックを提供します。
        /// </summary>
        internal ILearningMethod LearningMethod { get; set; }

        /// <summary>
        /// 棋譜読み込み機能を提供します。
        /// </summary>
        internal INotationReader NotationReader { get; set; }

        /// <summary>
        /// 評価値ベクトルの更新機能を提供します。
        /// </summary>
        internal EvaluateVectorUpdator EvaluateVectorUpdator { get; set; }

        /// <summary>
        /// 特徴ベクトル生成機能を提供します。
        /// </summary>
        internal IFeatureVectorGenerator FeatureVectorGenerator { get; set; }

        /// <summary>
        /// 評価値ベクトルの書き込み機能を提供します。
        /// </summary>
        internal EvaluateVectorWriter EvaluateVectorWriter { get; set; }

        /// <summary>
        /// 学習状況の分析機能を提供します。
        /// </summary>
        internal LearningAnalysis LearningAnalysis { get; set; }

        /// <summary>
        /// 合法手を取得します。
        /// </summary>
        internal Func<BoardContext, IEnumerable<uint>> GetMoves { get; set; }

        /// <summary>
        /// ステージを取得します。
        /// </summary>
        internal Func<BoardContext, int> GetStage { get; set; }

        /// <summary>
        /// 棋譜ファイルパス
        /// </summary>
        internal string NotationFilePath { get; set; }

        /// <summary>
        /// 学習結果出力パス
        /// </summary>
        internal string EvaluateVectorOutPutPath { get; set; }

        /// <summary>
        /// 更新値α
        /// </summary>
        internal double Alpha { get; set; }

        /// <summary>
        /// 特徴ベクトルの長さ
        /// </summary>
        internal int FeatureVectorLength { get; set; }

        /// <summary>
        /// 学習対象のステージ
        /// </summary>
        internal List<bool> LearnTargetStage { get; set; }

        /// <summary>
        /// 学習結果の試験を行う回数
        /// </summary>
        internal int ExamCount { get; set; }

        /// <summary>
        /// 学習結果の試験としてランダムに棋譜を選択する確率
        /// </summary>
        internal int ExamRandomRate { get; set; }

        /// <summary>
        /// 学習結果の試験で収束したと判断する一致確率
        /// </summary>
        internal double ExamSuccessMatchedRate { get; set; }

        /// <summary>
        /// 収束判定を行う単位
        /// </summary>
        internal int ConvergenceCheckUnit { get; set; }

        /// <summary>
        /// 深読みの深さ
        /// </summary>
        internal int PonderDepth { get; set; }

        /// <summary>
        /// 学習の元にするファイルのパス
        /// </summary>
        internal string LearningSeedFilePath { get; set; }

        /// <summary>
        /// 1ステージあたりのターン数
        /// </summary>
        internal int OneStageUnitTurn { get; set; }

        /// <summary>
        /// ステージ数
        /// </summary>
        internal int StageCount { get; set; }

        /// <summary>
        /// ステージを分割するかどうか
        /// </summary>
        internal bool IsDivideStage { get; set; }

        /// <summary>
        /// ステージの一覧を取得します。
        /// </summary>
        internal Func<IEnumerable<int>> GetStages { get; set; }

        /// <summary>
        /// 教師ベクトルのファイルパス
        /// </summary>
        internal string LearningVectorFilePath { get; set; }

        /// <summary>
        /// ターンごとに評価値を分けるかどうか
        /// </summary>
        internal bool IsTurnSeparate { get; set; }

        /// <summary>
        /// 評価対象のターン
        /// </summary>
        internal Turn TargetTurn { get; set; }

        /// <summary>
        /// 評価値の最大値
        /// </summary>
        internal double MaxEvaluate { get; set; }

        /// <summary>
        /// 評価値の最小値
        /// </summary>
        internal double MinEvaluate { get; set; }

        /// <summary>
        /// 学習結果の試験で収束したと判断するMSE
        /// </summary>
        internal double ExamSuccessMse { get; set; }

        /// <summary>
        /// 評価対象のステージ
        /// </summary>
        internal int TargetStage { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal LearningSettings()
        {
            this.Alpha = 0.1;
            this.ConvergenceCheckUnit = 10;
            this.ExamCount = 100;
            this.ExamRandomRate = 30;
            this.ExamSuccessMatchedRate = 0.15;
            this.ExamSuccessMse = 0D;
            this.PonderDepth = 3;
            this.OneStageUnitTurn = 10;
            this.StageCount = 10;
            this.MaxEvaluate = 0;
            this.IsDivideStage = true;
            this.IsTurnSeparate = false;
            this.TargetTurn = Turn.Black;
            this.FeatureVectorLength = SequencePositionFeatureVector.Length;
            this.EvaluateVectorOutPutPath = @"./result/answer/{0}.txt";
            this.NotationFilePath = @"C:\work\Niels設計\2chkifu_csa\2chkifu.csa";
            this.LearningSeedFilePath = string.Empty;
            this.LearningVectorFilePath = string.Empty;
            this.EvaluateVectorUpdator = new EvaluateVectorUpdator(this);
            this.EvaluateVectorWriter = new EvaluateVectorWriter(this);
            this.FeatureVectorGenerator = new SequencePositionFeatureVectorGenerator();
            this.GetMoves = MoveProvider.GetAllMoves;
            this.GetStage = (context =>
            {
                int stage = (context.TurnCount / 10);
                return (stage < 9) ? stage : 9;
            });
            this.GetStages = this.GetStagesDefault;
            this.LearningAnalysis = new LearningAnalysis(this);
            this.LearningMethod = new BonanzaMethod(this);
            this.LearnTargetStage = new List<bool>() { true, true, true, true, true, true, true, true, true, true};
            this.NotationReader = new CsaNotationReader();
        }

        /// <summary>
        /// ステージの一覧を取得します。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetStagesDefault()
        {
            yield return 0;
            if (this.IsDivideStage)
            {
                for (int stage = 1; stage < this.StageCount; stage++)
                {
                    yield return stage;
                }
            }
        }
    }
}
