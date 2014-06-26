using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Notation;
using Niels.Evaluators;
using Niels.Helper;
using Niels.Learning.NotationReading;
using Niels.Learning.Settings;
using Niels.Tests.TestHelper;
using Niels.Diagnostics;
using System.IO;

namespace Niels.Learning
{
    /// <summary>
    /// 学習で使用するデータファイルの書き込みを実行します。
    /// </summary>
    internal class LearningSeedFileWriter
    {
        /// <summary>
        /// 棋譜入力パス
        /// </summary>
        private const string NotationInputPath = @"C:\work\Niels設計\2chkifu_csa";

        /// <summary>
        /// 棋譜のファイルパターン
        /// </summary>
        private const string NotationFilePattern = @"*.csa";

        /// <summary>
        /// 学習で使用するスコアインデックスデータ出力パス
        /// </summary>
        private const string LearningSeedScoreIndexOutPutPath = @"./result/scoreindex/{0}.txt";

        /// <summary>
        /// 学習で使用する結果出力パス
        /// </summary>
        private const string LearningSeedResultOutPutPath = @"./result/result/{0}.txt";

        /// <summary>
        /// 学習の設定情報
        /// </summary>
        private LearningSettings Settings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings"></param>
        internal LearningSeedFileWriter(LearningSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// 学習で使用するデータファイルの書き込みを実行します。
        /// </summary>
        internal void Write()
        {
            CsaNotationReader reader = new CsaNotationReader();
            IEnumerable<string> files = Directory.EnumerateFiles(NotationInputPath, NotationFilePattern, SearchOption.AllDirectories);

            // ファイルの初期化
            this.InitializeLearningSeedFile();

            int count = 0;

            foreach (string file in files)
            {
                //Dictionary<int, List<int>> winScores = new Dictionary<int, List<int>>();
                IEnumerable<NotationInformation> infos = reader.Read(file);
                foreach(var info in infos)
                {
                    // ターンごとに評価する場合は対象ターンが勝っている棋譜のみを読み込む
                    if (this.Settings.IsTurnSeparate && this.Settings.TargetTurn != info.WinTurn)
                    {
                        Console.WriteLine(string.Format("Game Skipped Caused By {0} Win", info.WinTurn));
                        continue;
                    }

                    BoardContextForTest context = new BoardContextForTest(Turn.Black);
                    context.SetDefaultStartPosition();
                    //int winScore = (int)this.GetWinScore(info.MoveCount, info.WinTurn);

                    // 手を指していく
                    foreach(uint move in info.Moves)
                    {
                        //FileHelper.WriteLine(MoveForTest.ToDebugString(move));
                        //FileHelper.WriteLine(context.ToString());
                        context.PutPiece(move);
                        context.ChangeTurn();

                        // インデックスを取得
                        var scoreIndexs = SequencePositionFeatureVector.Generate(context);

                        // ファイルに出力
                        int stage = this.Settings.GetStage(context);
                        string csv = string.Empty;
                        foreach (var scoreIndex in scoreIndexs)
                        {
                            if (string.IsNullOrEmpty(csv))
                            {
                                csv += string.Format("{0},{1}", scoreIndex.Key, scoreIndex.Value);
                            }
                            else
                            {
                                csv += string.Format(",{0},{1}", scoreIndex.Key, scoreIndex.Value);
                            }

                            //if (!winScores.ContainsKey(stage))
                            //{
                            //    winScores[stage] = new List<int>();
                            //}
                            //winScores[stage].Add(winScore);
                        }
                        FileHelper.WriteLine(csv, string.Format(LearningSeedScoreIndexOutPutPath, stage));
                    }

                    count++;
                    if ((count % 3) == 0)
                    {
                        Console.WriteLine(string.Format("{0} Finished", count));
                        FileHelper.WriteLine(string.Format("{0} Finished", count));

                        // 結果リストをファイルに出力
                        //foreach (var oneStageWinScores in winScores)
                        //{
                        //    string csv = string.Empty;
                        //    foreach (var stageWinScore in oneStageWinScores.Value)
                        //    {
                        //        csv += string.Format(",{0}", stageWinScore);
                        //    }
                        //    FileHelper.Write(csv, string.Format(LearningSeedResultOutPutPath, oneStageWinScores.Key));
                        //}

                        //// 結果リストをクリア
                        //winScores = new Dictionary<int, List<int>>();

                        if ((count % 300) == 0)
                        {
                            Console.WriteLine("Now Shutdown Timing");
                            for (int i = 5; i > 0; i--)
                            {
                                Console.WriteLine(i + "...");
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// スコアインデックスファイルの初期化を行います。
        /// </summary>
        private void InitializeLearningSeedFile()
        {
            for (int stage = 0; stage < this.Settings.StageCount; stage++ )
            {
                FileHelper.Write(string.Empty, string.Format(LearningSeedScoreIndexOutPutPath, stage), false);
                FileHelper.Write(string.Empty, string.Format(LearningSeedResultOutPutPath, stage), false);
            }
        }

        /// <summary>
        /// 勝ち点を取得します。
        /// </summary>
        /// <param name="moveCount"></param>
        /// <returns></returns>
        private double GetWinScore(int moveCount, Turn winTurn)
        {
            return this.Settings.MaxEvaluate;
        }
    }
}
