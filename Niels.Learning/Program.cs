using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Helper;
using Niels.Learning.NotationReading;
using System.IO;
using Niels.Learning.Settings;

namespace Niels.Learning
{
    /// <summary>
    /// 棋譜データの読み込みを行います。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 学習で使用するデータファイルの書き込みを実行する
            LearningSettings settings = LearnSettingsProvider.GetSteepestDescentLearningSettings();
            //new LearningSeedFileWriter(settings).Write();

            //while (true)
            //{
            //    ;
            //}

            // マスタインデックスデータの書き込みを実行する
            //MasterScoreIndex.Write();

            // 学習を行う
            //LearningSettings settings = new LearningSettings();
            settings.LearningMethod.Learn();
        }
    }
}
