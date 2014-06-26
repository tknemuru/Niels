using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using Niels.Helper;
using Niels.MagicBitBoardGenerator.Converters;
using Niels.MagicBitBoardGenerator.Config;

namespace Niels.MagicBitBoardGenerator.Generators
{
    /// <summary>
    /// 飛車・角・香車の利き生成に使用するマジックナンバーを生成します。
    /// </summary>
    internal class MagicNumberGenerator
    {
        /// <summary>
        /// マジックナンバーテーブル
        /// インデックス→盤種別→マジックナンバー
        /// </summary>
        private static Dictionary<int, ulong[]> MagicTable { get; set; }

        /// <summary>
        /// 乱数発生機
        /// </summary>
        private static readonly Random Randomiser = new Random();

        /// <summary>
        /// 設定情報
        /// </summary>
        private MagicBitBoardGeneratorConfig Config { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config"></param>
        internal MagicNumberGenerator(MagicBitBoardGeneratorConfig config)
        {
            this.Config = config;
        }

        /// <summary>
        /// マジックナンバーを生成します。
        /// </summary>
        internal void Generate()
        {
            // 出力ファイルを初期化
            FileHelper.Write(string.Empty);

            var boards = BoardProvider.GetAll();
            MagicTable = new Dictionary<int, ulong[]>();

            foreach (Board board in boards)
            {
                foreach (int index in board.UsingIndexs)
                {
                    int seqIndex = board.GetSequanceIndex(index);
                    List<Dictionary<int, bool>> patterns = this.Config.PatternGenerator.Generate(index, board.BoardType, this.Config.TargetPiece);
                    bool isGenerateSuccess = false;
                    int count = 1;
                    Console.WriteLine(string.Format("Generate Start Board:{0} Index{1}", board, seqIndex));
                    while (!isGenerateSuccess)
                    {
                        isGenerateSuccess = TryGenerateMagicNumber(seqIndex, patterns);
                        count++;
                        if ((count % 200) == 0)
                        {
                            Console.WriteLine(string.Format("Generate Still Failing Board:{0} Index{1} Charenged{2}", board, seqIndex, count));
                        }
                    }
                    Console.WriteLine(string.Format("Generate Success !! Board:{0} Index{1}", board, seqIndex));
                    FileHelper.Write(string.Format("{0},{1},{2},{3},", seqIndex, MagicTable[seqIndex][0], MagicTable[seqIndex][1], MagicTable[seqIndex][2]), this.Config.MagicNumberFilePath);
                }
            }
        }

        /// <summary>
        /// マジックナンバーの候補を生成し、衝突がないかどうかを確認します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        private bool TryGenerateMagicNumber(int seqIndex, List<Dictionary<int, bool>> patterns)
        {
            // マジックナンバーの候補を生成
            ulong[] magicNumbers = GenerateMagicNumber();

            HashSet<ulong> magicCodeSet = new HashSet<ulong>();

            // 全てのパターンを試行していく
            foreach (var pattern in patterns)
            {
                // パターンをマスクされた盤の状態に変換する（キーとして使用）
                ulong[] boardState = BoardStateConverter.ToBoardState(pattern);

                // それぞれの盤にマジックナンバーをかける
                boardState[(int)BoardType.Main] *= magicNumbers[(int)BoardType.Main];
                boardState[(int)BoardType.SubRight] *= magicNumbers[(int)BoardType.SubRight];
                boardState[(int)BoardType.SubBottom] *= magicNumbers[(int)BoardType.SubBottom];

                // 盤同士のxorを求める
                ulong magicCode = boardState[(int)BoardType.Main] ^ boardState[(int)BoardType.SubRight] ^ boardState[(int)BoardType.SubBottom];

                // 関係のあるマス目だけ残るようにシフト
                magicCode >>= (64 - this.Config.ShiftMagicCode);

                if (!magicCodeSet.Contains(magicCode))
                {
                    magicCodeSet.Add(magicCode);
                }
                else
                {
                    // 失敗
                    return false;
                }
            }

            // 成功
            MagicTable.Add(seqIndex, magicNumbers);
            return true;
        }

        /// <summary>
        /// マジックナンバーの候補を生成します。
        /// </summary>
        /// <returns></returns>
        private static ulong[] GenerateMagicNumber()
        {
            ulong[] magicNumbers = new ulong[3];
            ulong[] seeds = new ulong[3];
            for (int s = 0; s < magicNumbers.Length; s++)
            {
                for (int i = 0; i < seeds.Length; i++)
                {
                    seeds[i] = Get64BitRandom(1, ulong.MaxValue);
                }

                magicNumbers[s] = (seeds[0] & seeds[1] & seeds[2]);
            }
                
            return magicNumbers;
        }

        /// <summary>
        /// 64bit整数の乱数を取得します。
        /// http://social.msdn.microsoft.com/Forums/en-US/cb9c7f4d-5f1e-4900-87d8-013205f27587/64-bit-strong-random-function
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private static ulong Get64BitRandom(ulong minValue, ulong maxValue)
        {
            // Get a random array of 8 bytes. 
            // As an option, you could also use the cryptography namespace stuff to generate a random byte[8]
            byte[] buffer = new byte[sizeof(ulong)];
            Randomiser.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0) % (maxValue - minValue + 1) + minValue;
        }
    }
}
