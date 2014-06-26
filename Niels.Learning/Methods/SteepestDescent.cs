using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Collections.Math;
using Niels.Helper;
using Niels.Learning.Settings;
using Niels.Learning.Analysis;
using Niels.Evaluators;

namespace Niels.Learning.Methods
{
    internal class SteepestDescent : ILearningMethod
    {
        /// <summary>
        /// 学習設定情報
        /// </summary>
        private LearningSettings Settings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SteepestDescent(LearningSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// 機械学習を行います。
        /// </summary>
        public void Learn()
        {
            foreach (int stage in this.Settings.GetStages())
            {
                // ステージの設定
                this.Settings.TargetStage = stage;
                Console.WriteLine("TargetStage:" + stage);
                System.Threading.Thread.Sleep(3000);

                // 巨大疎行列の作成
                List<string> files = new List<string>();
                files.Add(string.Format(this.Settings.LearningSeedFilePath, stage));
                SparseBigMatrix A = new SparseBigMatrix(files, this.Settings.FeatureVectorLength, 0D);

                // 結果リストの作成
                List<string> resultFiles = new List<string>();
                resultFiles.Add(string.Format(this.Settings.LearningVectorFilePath, stage));
                List<double> b = new List<double>();
                foreach (string resultFile in resultFiles)
                {
                    string csv = FileHelper.ReadToEnd(resultFile);
                    var rangeB = from a in csv.Replace(Environment.NewLine, ",").Split(',')
                                    where a != ""
                                    select double.Parse(a);
                    b.AddRange(rangeB);
                }

                // xベクトルを作成
                SparseVector<double> x = new SparseVector<double>(this.Settings.MinEvaluate, A.Width);

                // 最急降下法を実行
                this.Execute(A, b, x, stage);
            }
        }

        /// <summary>
        /// 最急降下法を実行する
        /// </summary>
        /// <returns></returns>
        private void Execute(SparseBigMatrix A, List<double> b, SparseVector<double> x, int stage)
        {
            try
            {
                x = new SparseVector<double>(this.Settings.MinEvaluate, A.Width);
                this.Execute(A, b, x);
                return;
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Message + " " + "1回目 α：" + this.Settings.Alpha.ToString());
                System.Threading.Thread.Sleep(4000);
                List<string> aFiles = new List<string>() { string.Format(this.Settings.LearningSeedFilePath, stage) };
                A = new SparseBigMatrix(aFiles, A.Width, 0);
                this.Settings.Alpha = this.Settings.Alpha * 0.8;
            }
        }
        /// <summary>
        /// 最急降下法を実行する
        /// </summary>
        /// <returns></returns>
        private void Execute(SparseBigMatrix A, List<double> b, SparseVector<double> x)
        {
            for (int i = 1; i < 2; i++)
            {
                SparseVector<double> N = new SparseVector<double>(0D, x.Count());
                double mse = 0;
                int count = 0;

                foreach (SparseVector<double> vector in A)
                {
                    double score = 0;
                    this.Settings.LearningAnalysis.NextGameCount();
                    // 推定スコアを計算
                    foreach (KeyValuePair<int, double> keyValue in vector.NoSparseKeyValues)
                    {
                        score += x[keyValue.Key] * keyValue.Value;
                        N[keyValue.Key] += Math.Abs(keyValue.Value);
                    }

                    // ある局面の誤差
                    //double err = b[A.Height - 1] - score;
                    double err = this.Settings.MaxEvaluate - score;
                    mse += err * err;
                    count++;
                    //Console.WriteLine("MSE:" + mse);
                    if (count >= 300)
                    {
                        Console.WriteLine("MSE Avg:" + ((mse / (double)count) / 1000000D));
                        //SequencePositionFeatureVector.DisplayPieceEvaluate(x);
                        count = 0;
                        mse = 0D;
                    }

                    // 評価関数の修正
                    x = this.Settings.EvaluateVectorUpdator.Update(x, vector, N, err);

                    // 収束判定を行う
                    if (this.Settings.LearningAnalysis.IsNeedConvergence())
                    {
                        this.Settings.LearningAnalysis.WriteGameCount();
                        List<SparseVector<double>> evaluateVectors = new List<SparseVector<double>>() { x };
                        var examResults = this.Settings.LearningAnalysis.IsConvergence(evaluateVectors);
                        foreach (var examResult in examResults)
                        {
                            if (examResult.State == ExamState.Success)
                            {
                                // 評価値ベクトルの書き込みを行う
                                this.Settings.EvaluateVectorWriter.Write(x, examResult.Stage);
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

                Console.WriteLine(A.Height);
                Console.WriteLine("全て終了しました。");
                List<SparseVector<double>> lastEvaluateVectors = new List<SparseVector<double>>() { x };
                var lastExamResults = this.Settings.LearningAnalysis.IsConvergence(lastEvaluateVectors);
                foreach (var stage in this.Settings.GetStages())
                {
                    if (lastExamResults[stage].State == ExamState.Success || lastExamResults[stage].State == ExamState.Failed)
                    {
                        // 評価値ベクトルの書き込みを行う
                        this.Settings.EvaluateVectorWriter.Write(x, stage);
                    }
                }
                //throw new ApplicationException("収束しませんでした。");
            }
        }
    }
}
