using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Collections.Math;
using Niels.Generates;
using Niels.Helper;
using Niels.Learning.Settings;

namespace Niels.Learning.Analysis
{
    /// <summary>
    /// 最急降下法向けの学習状況の分析機能を提供します。
    /// </summary>
    internal class SteepestDescentLearningAnalysis : LearningAnalysis
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings"></param>
        internal SteepestDescentLearningAnalysis(LearningSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// 収束判定を行います。
        /// </summary>
        /// <returns></returns>
        internal override List<ExamResult> IsConvergence(List<SparseVector<double>> evaluateVectors, Func<ExamResult, Turn, bool> getIsNoNeedExam)
        {
            Console.WriteLine("Exam Start");

            // 初期化した試験結果リストを取得
            var examResults = this.GetInitializedExamResults();

            // 乱数
            Random random = new Random();

            // 棋譜を読み込んでいく
            foreach (var game in this.Settings.NotationReader.Read(this.Settings.NotationFilePath))
            {
                // 抽選で当たった棋譜で試験を行う
                if (random.Next(this.Settings.ExamRandomRate) > 0) { continue; }

                // コンテクストの初期化
                BoardContext context = new BoardContext();
                context.SetDefaultStartPosition();

                // 手を指していく
                foreach (var teacherMove in game.Moves)
                {
                    // 試験対象外条件の検査
                    int stage = this.Settings.GetStage(context);
                    if (getIsNoNeedExam(examResults[stage], context.Turn))
                    {
                        // 手を指してターンをまわす
                        context.PutPiece(teacherMove);
                        context.ChangeTurn();
                        continue;
                    }

                    // 最善手を取得
                    // evaluateVectorsは常に1ステージのみなので0指定
                    var bestMove = this.GetBestMove(context, evaluateVectors[0]);

                    // Mse集計
                    double err = this.Settings.MaxEvaluate - bestMove.Value;
                    double mse = err * err;
                    examResults[stage].MatchedCount++;
                    examResults[stage].MseSum += mse;

                    // 手を指してターンをまわす
                    context.PutPiece(teacherMove);
                    context.ChangeTurn();
                }

                // 収束判定を行う               
                foreach (var examResult in examResults)
                {
                    if (examResult.ExamCount < this.Settings.ExamCount) { continue; }

                    if (examResult.MseAvg <= this.Settings.ExamSuccessMse)
                    {
                        examResult.State = ExamState.Success;

                        // 成功したステージは学習対象から外す
                        this.Settings.LearnTargetStage[examResult.Stage] = false;
                    }
                    else
                    {
                        examResult.State = ExamState.Failed;
                    }

                    Console.WriteLine(string.Format("Exam Finished {0}", examResult.ToString()));

                    // 判定の対象は1ステージのみ
                    this.WriteExamResult(examResults);
                    System.Threading.Thread.Sleep(5000);
                    return examResults;
                }
            }

            this.WriteExamResult(examResults);
            return examResults;
        }

        /// <summary>
        /// 初期化した試験結果リストを取得します。
        /// </summary>
        /// <returns></returns>
        protected override List<ExamResult> GetInitializedExamResults()
        {
            List<ExamResult> examResults = new List<ExamResult>();
            for (int i = 0; i < this.Settings.LearnTargetStage.Count(); i++)
            {
                ExamResult result = new ExamResult(i);
                if (this.Settings.LearnTargetStage[i] && this.Settings.TargetStage == i)
                {
                    result.State = ExamState.Undefined;
                }
                else
                {
                    result.State = ExamState.NoTarget;
                }
                examResults.Add(result);
            }
            return examResults;
        }
    }
}
