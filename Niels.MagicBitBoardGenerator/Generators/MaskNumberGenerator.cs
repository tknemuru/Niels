using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Converters;
using Niels.Helper;
using Niels.Collections;

using Niels.MagicBitBoardGenerator.Config;

namespace Niels.MagicBitBoardGenerator.Generators
{
    /// <summary>
    /// マスクするナンバーを生成します。
    /// </summary>
    internal class MaskNumberGenerator
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        private MagicBitBoardGeneratorConfig Config { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config"></param>
        internal MaskNumberGenerator(MagicBitBoardGeneratorConfig config)
        {
            this.Config = config;
        }

        internal void Generate()
        {
            Board[] boards = BoardProvider.GetAll();
            ulong[][] maskTable = new ulong[81][];

            foreach (Board board in boards)
            {
                foreach (int i in board.UsingIndexs)
                {
                    // 利きに関係のあるマス目の集合を取得
                    var moveRelationIndexs = this.Config.MoveRelationIndexsGenerator.Generate(i, board.BoardType);

                    // インデックスの集合をマスク用の数値に変換する
                    int seqIndex = board.GetSequanceIndex(i);
                    maskTable[seqIndex] = BoardStateConverter.ToBoardState(moveRelationIndexs);
                }
            }

            // ファイルに結果を出力
            FileHelper.Write(string.Empty);
            for (int i = 0; i < maskTable.Length; i++)
            {
                FileHelper.Write(string.Format("{0},{1},{2},{3},", i, maskTable[i][0], maskTable[i][1], maskTable[i][2]), this.Config.MaskNumberFilePath);
            }
        }
    }
}
