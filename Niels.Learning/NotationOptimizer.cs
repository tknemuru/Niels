using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections.Math;
using System.IO;
using Niels.Helper;

namespace Niels.Learning
{
    /// <summary>
    /// 棋譜を分析して学習を行います。
    /// </summary>
    public class NotationOptimizer
    {
        #region "定数"
        ///// <summary>
        ///// 学習で使用するスコアインデックスデータ出力パス
        ///// </summary>
        //private const string LearningSeedScoreIndexOutPutPath = @"./result/scoreindex/{0}.txt";

        ///// <summary>
        ///// 学習で使用する結果出力パス
        ///// </summary>
        //private const string LearningSeedResultOutPutPath = @"./result/result/{0}.txt";

        /// <summary>
        /// 学習で使用するスコアインデックスデータ出力パス
        /// </summary>
        private const string LearningSeedScoreIndexOutPutPath = @"C:\work\Niels設計\learnningseedgenerate3\result\scoreindex\{0}.txt";

        /// <summary>
        /// 学習で使用する結果出力パス
        /// </summary>
        private const string LearningSeedResultOutPutPath = @"C:\work\Niels設計\learnningseedgenerate3\result\result\{0}.txt";

        /// <summary>
        /// 学習結果出力パス
        /// </summary>
        private const string LearningAnswerOutPutPath = @"./result/answer/{0}.txt";

        /// <summary>
        /// スコアに履かせる下駄数
        /// </summary>
        private const double ScoreClogs = 100000000D;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotationOptimizer()
        {
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 棋譜の統計を実行する
        /// </summary>
        public void Execute()
        {
            //List<string> StageList = ConfigurationManager.AppSettings["Optimize Stage List"].Split(',').ToList();
            //foreach (string stage in StageList)
            //{
            //    this.Execute(stage);
            //}
            //string stage = this.GetStageFromProgramDirectory();
            //for (int stage = 1; stage <= 1; stage++)
            //{
            //    Console.WriteLine("OptimaizeStage:" + stage);
            //    this.Execute(stage.ToString());
            //}
            int stage = 10;
            Console.WriteLine("OptimaizeStage:" + stage);
            this.Execute(stage.ToString());

        }

        /// <summary>
        /// 棋譜の統計を実行する
        /// </summary>
        public void Execute(string stage)
        {
            // 巨大疎行列の作成
            List<string> files = new List<string>();
            files.Add(string.Format(LearningSeedScoreIndexOutPutPath, stage));
            SparseBigMatrix A = new SparseBigMatrix(files, 83536, 0D);

            // 結果リストの作成
            List<string> resultFiles = new List<string>();
            resultFiles.Add(string.Format(LearningSeedResultOutPutPath, stage));
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
            SparseVector<double> x = new SparseVector<double>(A.Width, 0);

            // 最急降下法を実行
            List<string> masterFiles = new List<string>();
            masterFiles.Add(string.Format(LearningSeedScoreIndexOutPutPath, stage));
            SparseVector<double> trueX = new SteepestDescentUsingSparseBigMatrix(masterFiles).Execute(A, b, x);

            // CSVを出力
            string answerCsv = string.Empty;
            foreach (var answer in trueX)
            {
                if (string.IsNullOrEmpty(answerCsv))
                {
                    answerCsv += string.Format("{0}", (int)(answer * ScoreClogs));
                }
                else
                {
                    answerCsv += string.Format(",{0}", (int)(answer * ScoreClogs));
                }
            }
            FileHelper.Write(answerCsv, string.Format(LearningAnswerOutPutPath, stage), false);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
