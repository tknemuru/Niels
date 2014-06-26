using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.MagicBitBoardGenerator.Config;
using Niels.MagicBitBoardGenerator.Generators;

namespace Niels.MagicBitBoardGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //// 飛車
            //Generate(MagicBitBoardGeneratorConfigProvider.GetRookConfig());

            //// 角
            //Generate(MagicBitBoardGeneratorConfigProvider.GetBishopConfig());

            // 香車(先手)
            Generate(MagicBitBoardGeneratorConfigProvider.GetLaunceBlackConfig());

            // 香車(後手)
            Generate(MagicBitBoardGeneratorConfigProvider.GetLaunceWhiteConfig());
        }

        /// <summary>
        /// マジックビットボードを生成します。
        /// </summary>
        private static void Generate(MagicBitBoardGeneratorConfig config)
        {
            // マスクナンバーの生成
            new MaskNumberGenerator(config).Generate();

            // マジックナンバーの生成
            new MagicNumberGenerator(config).Generate();

            // アタックテーブルの生成
            new AttackTableGenerator(config).Generate();
        }
    }
}
