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
    /// 学習状況の分析機能を提供します。
    /// </summary>
    internal class LearningAnalysis
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        protected LearningSettings Settings;

        /// <summary>
        /// 全体の対戦回数
        /// </summary>
        internal int GameCount { get; private set; }

        /// <summary>
        /// 収束判定をする単位内での対戦回数
        /// </summary>
        protected int GameCountInnerOneUnit { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal LearningAnalysis(LearningSettings settings)
        {
            this.GameCount = 0;
            this.GameCountInnerOneUnit = 0;
            this.Settings = settings;
        }

        /// <summary>
        /// 収束判定が必要かどうかを判定します。
        /// </summary>
        internal bool IsNeedConvergence()
        {
            this.GameCountInnerOneUnit++;
            if (this.GameCountInnerOneUnit >= this.Settings.ConvergenceCheckUnit)
            {
                this.GameCountInnerOneUnit = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 対戦回数を増加します。
        /// </summary>
        internal void NextGameCount()
        {
            this.GameCount++;
        }

        /// <summary>
        /// 対戦回数を増加します。
        /// </summary>
        internal void WriteGameCount()
        {
            Console.WriteLine(string.Format("- {0} Game Finished", this.GameCount));
        }

        /// <summary>
        /// ターン数を出力します。
        /// </summary>
        internal void WriteTurnCount(int turn)
        {
            Console.WriteLine(string.Format(" - {0} Turn Finished", turn));
        }

        /// <summary>
        /// 収束判定を行います。
        /// </summary>
        /// <param name="evaluateVectors"></param>
        /// <returns></returns>
        internal List<ExamResult> IsConvergence(List<SparseVector<double>> evaluateVectors)
        {
            Func<ExamResult, Turn, bool> getIsNoNeedExam = ((examResult, turn) =>
                { return examResult.State == ExamState.NoTarget || examResult.ExamCount >= this.Settings.ExamCount; });

            return this.IsConvergence(evaluateVectors, getIsNoNeedExam);
        }

        /// <summary>
        /// ターン指定で収束判定を行います。
        /// </summary>
        /// <param name="evaluateVectors"></param>
        /// <param name="targetTurn"></param>
        /// <returns></returns>
        internal List<ExamResult> IsConvergence(List<SparseVector<double>> evaluateVectors, Turn targetTurn)
        {
            Func<ExamResult, Turn, bool> getIsNoNeedExam = ((examResult, turn) =>
            { return examResult.State == ExamState.NoTarget || examResult.ExamCount >= this.Settings.ExamCount || turn != targetTurn; });

            return this.IsConvergence(evaluateVectors, getIsNoNeedExam);
        }

        /// <summary>
        /// 収束判定を行います。
        /// </summary>
        /// <returns></returns>
        internal virtual List<ExamResult> IsConvergence(List<SparseVector<double>> evaluateVectors, Func<ExamResult, Turn, bool> getIsNoNeedExam)
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
                    uint bestMove = this.GetBestMove(context, evaluateVectors[stage]).Key;

                    // 一致判定
                    if (teacherMove == bestMove)
                    {
                        // 一致
                        examResults[stage].MatchedCount++;
                    }
                    else
                    {
                        // 不一致
                        examResults[stage].UnmatchedCount++;
                    }

                    // 手を指してターンをまわす
                    context.PutPiece(teacherMove);
                    context.ChangeTurn();
                }

                // 収束判定を行う               
                foreach (var examResult in examResults)
                {
                    if (examResult.ExamCount < this.Settings.ExamCount) { continue; }

                    if (examResult.MatchedRate >= this.Settings.ExamSuccessMatchedRate)
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
                }

                // 全ての判定が終わったか？
                if (examResults.Where(result => result.State == ExamState.Undefined).Count() <= 0)
                {
                    Console.WriteLine("All Exam Finished");
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
        protected virtual List<ExamResult> GetInitializedExamResults()
        {
            List<ExamResult> examResults = new List<ExamResult>();
            for(int i = 0; i < this.Settings.LearnTargetStage.Count(); i++)
            {
                ExamResult result = new ExamResult(i);
                result.State = (this.Settings.LearnTargetStage[i]) ? ExamState.Undefined : ExamState.NoTarget;
                examResults.Add(result);
            }
            return examResults;
        }

        /// <summary>
        /// 最善手を取得します。
        /// </summary>
        public KeyValuePair<uint, double> GetBestMove(BoardContext context, SparseVector<double> evaluateVector)
        {
            IEnumerable<uint> moves = MoveProvider.GetAllMoves(context, GenerateTarget.All);
            Dictionary<uint, double> moveEvaluates = new Dictionary<uint, double>();
            foreach (uint move in moves)
            {
                context.PutPiece(move);
                context.ChangeTurn();
                SparseVector<double> featureVector = this.Settings.FeatureVectorGenerator.Generate(context);
                double evaluate = 0D;
                foreach (var keyValue in featureVector.NoSparseKeyValues)
                {
                    evaluate += evaluateVector[keyValue.Key] * keyValue.Value;
                }
                moveEvaluates.Add(move, evaluate);
                context.UndoChangeTurn();
                context.UndoPutPiece();
            }

            KeyValuePair<uint, double> bestMove;
            // TODO:ターンによって逆転させるのが正しいのかどうかがわからない…
            //if (context.Turn == Turn.Black)
            //{
            //    bestMove = moveEvaluates.OrderByDescending(moveKeyValue => moveKeyValue.Value).FirstOrDefault();
            //}
            //else
            //{
            //    bestMove = moveEvaluates.OrderBy(moveKeyValue => moveKeyValue.Value).FirstOrDefault();
            //}
            bestMove = moveEvaluates.OrderByDescending(moveKeyValue => moveKeyValue.Value).FirstOrDefault();

            return bestMove;
        }

        /// <summary>
        /// 試験結果を出力します。
        /// </summary>
        /// <param name="results"></param>
        protected void WriteExamResult(List<ExamResult> results)
        {
            Console.WriteLine(string.Empty);
            FileHelper.WriteLine(string.Empty);

            foreach(var result in results)
            {
                Console.WriteLine(result.ToString());
                FileHelper.WriteLine(result.ToString());
            }
        }
    }

    /// <summary>
    /// 試験結果を記録します。
    /// </summary>
    internal class ExamResult
    {
        /// <summary>
        /// ステージ
        /// </summary>
        internal int Stage { get; set; }

        /// <summary>
        /// 一致した回数
        /// </summary>
        internal int MatchedCount { get; set; }

        /// <summary>
        /// 不一致だった回数
        /// </summary>
        internal int UnmatchedCount { get; set; }

        /// <summary>
        /// 試験実施回数
        /// </summary>
        internal int ExamCount
        {
            get
            {
                return this.MatchedCount + this.UnmatchedCount;
            }
        }

        /// <summary>
        /// 一致率
        /// </summary>
        internal double MatchedRate
        {
            get
            {
                return ((double)this.MatchedCount / (double)this.ExamCount);
            }
        }

        /// <summary>
        /// MSEの合計
        /// </summary>
        internal double MseSum { get; set; }

        /// <summary>
        /// MSEの平均値
        /// </summary>
        internal double MseAvg
        {
            get
            {
                return this.MseSum / (double)this.ExamCount;
            }
        }

        /// <summary>
        /// 試験ステータス
        /// </summary>
        internal ExamState State { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ExamResult(int stage)
        {
            this.Stage = stage;
            this.MatchedCount = 0;
            this.UnmatchedCount = 0;
            this.MseSum = 0D;
            this.State = ExamState.Undefined;
        }

        /// <summary>
        /// 現在のオブジェクトの状態を文字列化して返します。
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        internal string ToString()
        {
            return string.Format("Stage:{0} State:{1} ExamCount:{2} MatchedRate:{3} MseAvg:{4}", this.Stage, this.State, this.ExamCount, this.MatchedRate, (this.MseAvg / 1000000D));
        }
    }

    internal enum ExamState
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,

        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 失敗
        /// </summary>
        Failed,

        /// <summary>
        /// 学習対象外
        /// </summary>
        NoTarget
    }
}
