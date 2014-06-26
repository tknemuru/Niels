using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Helper;

namespace Niels.NormalizedScoreIndexGenerating
{
    class Program
    {
        /// <summary>
        /// 結果出力パス
        /// </summary>
        private const string NormalizedScoreIndexsOutPutPath = @"./result/normalized_score_index.txt";

        static void Main(string[] args)
        {
            var normalizedScoreIndexs = new NormalizedScoreIndexGenerator().Generate();
            string csv = string.Empty;
            foreach (var normalizedScoreIndex in normalizedScoreIndexs)
            {
                if (string.IsNullOrEmpty(csv))
                {
                    csv += string.Format("{0},{1}", normalizedScoreIndex.Key, normalizedScoreIndex.Value);
                }
                else
                {
                    csv += string.Format(",{0},{1}", normalizedScoreIndex.Key, normalizedScoreIndex.Value);
                }
            }
            FileHelper.Write(csv, NormalizedScoreIndexsOutPutPath, false);
        }
    }
}
