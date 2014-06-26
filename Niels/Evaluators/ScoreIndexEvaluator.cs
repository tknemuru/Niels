using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Collections.Math;
using Niels.Helper;
using System.Diagnostics;

namespace Niels.Evaluators
{
    /// <summary>
    /// スコアインデックスによる評価を行います。
    /// </summary>
    public class ScoreIndexEvaluator : IEvaluator
    {
        /// <summary>
        /// 最大ステージ数
        /// </summary>
        public const int MaxStage = 9;
        
        /// <summary>
        /// １ステージのターン数
        /// </summary>
        private const int OneStageRange = 10;

        /// <summary>
        /// スコアインデックスファイルパス
        /// </summary>
        private const string ScoreIndexPath = @"C:\work\visualstudio\Niels\Niels\Data\scoreindex\{0}.txt";

        /// <summary>
        /// スコアインデックステーブル
        /// </summary>
        private static Dictionary<int, List<int>> ScoreIndexTable = new Dictionary<int, List<int>>();

        /// <summary>
        /// 評価値を取得します。
        /// </summary>
        /// <returns></returns>
        public int Evaluate(BoardContext context, int nodeId)
        {
            int stage = GetStage(context.TurnCount);
            if (!ScoreIndexTable.ContainsKey(stage))
            {
                // 未読み込みなら新たに読み込む
                LoadScoreIndexs(stage);
            }

            // 現在のインデックス値を取得
            var scoreIndexs = SequencePositionFeatureVector.Generate(context);

            // 評価値を取得
            var scoreIndexTable = ScoreIndexTable[stage];
            int score = 0;
            foreach (var scoreIndex in scoreIndexs)
            {
                score += scoreIndexTable[scoreIndex.Key] * scoreIndex.Value;
            }

            return score;
        }

        /// <summary>
        /// ターン数に応じたステージを取得します。
        /// </summary>
        /// <param name="turnCount"></param>
        /// <returns></returns>
        public static int GetStage(int turnCount)
        {
            int stage = (turnCount / OneStageRange);
            return (stage < MaxStage) ? stage : MaxStage;
            //return 0;
        }

        /// <summary>
        /// 指定したステージのスコアインデックスファイルを読み込みます。
        /// </summary>
        private static void LoadScoreIndexs(int stage)
        {
            string csv = FileHelper.ReadToEnd(string.Format(ScoreIndexPath, stage));
            IEnumerable<int> scoreIndexs = csv.Split(',').Select(index => int.Parse(index));
            ScoreIndexTable.Add(stage, scoreIndexs.ToList());
        }       
    }
}
