using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Collections.Math;
using Niels.Boards;
using Niels.Generates;
using Niels.Learning.Settings;
using Niels.Learning.Analysis;
using Niels.Tests.TestHelper;
using Niels.Helper;
using Niels.Diagnostics;

namespace Niels.Learning.Methods
{
    /// <summary>
    /// Bonanzaメソッドを用いて学習を行います。
    /// </summary>
    internal sealed class BonanzaMethod : ILearningMethod
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        private LearningSettings Settings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings"></param>
        internal BonanzaMethod(LearningSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// 学習を行います。
        /// </summary>
        public void Learn()
        {
            // 学習結果として返す評価値ベクトル
            List<SparseVector<double>> answerEvaluateVectors = this.GetInitializedAnswerEvaluateVectors();

            // 棋譜を読み込んでいく
            foreach (var game in this.Settings.NotationReader.Read(this.Settings.NotationFilePath))
            {
                // コンテクストの初期化
                BoardContextForTest context = new BoardContextForTest(Turn.Black);
                context.SetDefaultStartPosition();

                // 手を指していく
                for (int startTurn = 0; startTurn < game.MoveCount; startTurn++ )
                {
                    // 棋譜の指し手
                    uint teacherMove = game.Moves[startTurn];

                    // 更新対象？
                    if (this.Settings.LearnTargetStage[this.Settings.GetStage(context)])
                    {
                        // 棋譜中で実際に指された手の子局面
                        BoardContextForTest teacherContext = context.Clone();
                        for (int turn = startTurn; turn < startTurn + this.Settings.PonderDepth; turn++)
                        {
                            if (turn >= game.MoveCount) { break; }
                            teacherContext.PutPiece(game.Moves[turn]);
                            teacherContext.ChangeTurn();
                        }

                        // 合法手を生成
                        int stage = this.Settings.GetStage(context);
                        SparseVector<double> modificationVector = answerEvaluateVectors[stage].Clone();
                        BoardContextForTest childContext = context.Clone();
                        uint move = 0;
                        for (int turn = startTurn; turn < startTurn + this.Settings.PonderDepth; turn++)
                        {
                            if (turn >= game.MoveCount) { break; }
                            uint bestMove = this.Settings.LearningAnalysis.GetBestMove(childContext, answerEvaluateVectors[stage]).Key;
                            if (turn == startTurn)
                            {
                                move = bestMove;
                            }
                            childContext.PutPiece(bestMove);
                            childContext.ChangeTurn();
                        }

                        // 評価値ベクトルを更新する
                        var teacherFeatureVector = this.Settings.FeatureVectorGenerator.Generate(teacherContext);
                        var featureVector = this.Settings.FeatureVectorGenerator.Generate(childContext);
                        this.Settings.EvaluateVectorUpdator.Update(answerEvaluateVectors[stage], teacherFeatureVector, featureVector, modificationVector, (teacherMove == move), context.Turn);

                        // 評価値ベクトルの更新
                        answerEvaluateVectors[stage] = modificationVector.Clone();
                    }

                    // 手を指してターンをまわす
                    this.Settings.LearningAnalysis.WriteTurnCount(context.TurnCount);
                    context.PutPiece(teacherMove);
                    context.ChangeTurn();
                }

                // 収束判定を行う
                this.Settings.LearningAnalysis.NextGameCount();
                if (this.Settings.LearningAnalysis.IsNeedConvergence())
                {
                    var examResults = this.Settings.LearningAnalysis.IsConvergence(answerEvaluateVectors);
                    foreach (var examResult in examResults)
                    {
                        if (examResult.State == ExamState.Success)
                        {
                            // 評価値ベクトルの書き込みを行う
                            this.Settings.EvaluateVectorWriter.Write(answerEvaluateVectors[examResult.Stage], examResult.Stage);
                        }
                    }

                    if (examResults.Where(result => result.State != ExamState.Success || result.State != ExamState.NoTarget).Count() <= 0)
                    {
                        Console.WriteLine("学習が完了しました。");
                        System.Threading.Thread.Sleep(5000);
                        return;
                    }
                }
            }

            // 収束しなかった
            throw new ApplicationException("収束しませんでした。");
        }

        /// <summary>
        /// 初期化された結果評価値ベクトルを取得します。
        /// </summary>
        /// <returns></returns>
        private List<SparseVector<double>> GetInitializedAnswerEvaluateVectors()
        {
            List<SparseVector<double>> vectors = new List<SparseVector<double>>();
            foreach (var stage in this.Settings.LearnTargetStage)
            {
                vectors.Add(new SparseVector<double>(0D, this.Settings.FeatureVectorLength));
            }
            return vectors;
        }
    }
}
