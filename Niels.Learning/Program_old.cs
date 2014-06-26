using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Helper;
using Niels.Learning.NotationReading;
using System.IO;

namespace Niels.Learning
{
    /// <summary>
    /// 棋譜データの読み込みを行います。
    /// </summary>
    class Program
    {
        /// <summary>
        /// 棋譜入力パス
        /// </summary>
        private const string NotationInputPath = @"C:\work\Niels設計\kifu";

        /// <summary>
        /// 棋譜のファイルパターン
        /// </summary>
        private const string NotationFilePattern = @"*.ki2";

        /// <summary>
        /// 結果出力パス
        /// </summary>
        private const string InfoOutPutPath = @"./result/info.txt";

        static void Main(string[] args)
        {
            Ki2NotationReader reader = new Ki2NotationReader();
            IEnumerable<string> files = Directory.EnumerateFiles(NotationInputPath, NotationFilePattern, SearchOption.AllDirectories);

            // ファイルの初期化
            FileHelper.Write(string.Empty, InfoOutPutPath, false);

            int successCount = 0;
            int failedCount = 0;

            foreach (string file in files)
            {
                Console.WriteLine(string.Format("Start {0}", file));
                NotationInformation info = reader.Read(file);
                if (info.ReadResult == ReadResult.Success)
                {
                    FileHelper.Write(info.ToString(), InfoOutPutPath);
                    Console.WriteLine(string.Format("Success {0}", file));
                    successCount++;
                }
                else
                {
                    Console.WriteLine(string.Format("Failed to {0} file:{1}", info.ReadResult, file));
                    FileHelper.WriteLine(string.Format("Failed to {0} file:{1}", info.ReadResult, file));
                    failedCount++;
                }

                Console.WriteLine(string.Format("Finish {0}", file));
                Console.WriteLine(string.Format("Success {0}  Failed {1}", successCount, failedCount));
            }
        }
    }
}
