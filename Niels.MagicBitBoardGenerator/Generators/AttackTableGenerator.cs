using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Helper;
using Niels.MagicBitBoardGenerator.Converters;
using Niels.MagicBitBoardGenerator.Accessor;
using Niels.MagicBitBoardGenerator.Config;
using System.Diagnostics;
using Niels.Collections;

namespace Niels.MagicBitBoardGenerator.Generators
{
    /// <summary>
    /// 角・飛車の利きテーブルを生成します。
    /// </summary>
    internal class AttackTableGenerator
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        private MagicBitBoardGeneratorConfig Config { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config"></param>
        internal AttackTableGenerator(MagicBitBoardGeneratorConfig config)
        {
            this.Config = config;
        }

        /// <summary>
        /// アタックテーブルを生成します。
        /// </summary>
        internal void Generate()
        {
            var boards = BoardProvider.GetAll();

            // マジックナンバーテーブルを取得
            ulong[][] magicTable = GetMagicTable();

            foreach (Board board in boards)
            {
                foreach (int index in board.UsingIndexs)
                {
                    int seqIndex = board.GetSequanceIndex(index);

                    // ファイル名にインデックスを付与
                    string filepath = string.Format(this.Config.AttackTableFilePath, seqIndex);

                    // ファイルを初期化
                    FileHelper.Write(string.Empty, filepath, false);

                    // 全てのパターンを生成していく
                    List<Dictionary<int, bool>> patterns = this.Config.PatternGenerator.Generate(index, board.BoardType, this.Config.TargetPiece);
                    foreach (var pattern in patterns)
                    {
                        // パターンをマスクされた盤の状態に変換する（キーとして使用）
                        ulong[] boardState = BoardStateConverter.ToBoardState(pattern);

                        // それぞれの盤にマジックナンバーをかける
                        boardState[(int)BoardType.Main] *= magicTable[seqIndex][(int)BoardType.Main];
                        boardState[(int)BoardType.SubRight] *= magicTable[seqIndex][(int)BoardType.SubRight];
                        boardState[(int)BoardType.SubBottom] *= magicTable[seqIndex][(int)BoardType.SubBottom];

                        // 盤同士のxorを求める
                        ulong magicCode = boardState[(int)BoardType.Main] ^ boardState[(int)BoardType.SubRight] ^ boardState[(int)BoardType.SubBottom];

                        // 関係のあるマス目だけ残るようにシフト
                        magicCode >>= (64 - this.Config.ShiftMagicCode);

                        // 利きを生成する
                        IEnumerable<uint> moves = this.Config.AttackGenerator.Generate(BoardAccesor.GetSequanceIndex(index, board.BoardType), pattern, this.Config.TargetPiece);

                        // ファイルに書き出す
                        FileHelper.Write(magicCode.ToString(), filepath);
                        foreach (uint move in moves)
                        {
                            Debug.Assert(move.FromIndex() == index, "利きのインデックスが不正です。");
                            Debug.Assert(move.FromBoard() == board.BoardType, "利きの盤種別が不正です。");
                            FileHelper.Write("," + move.ToString(), filepath);
                        }
                        FileHelper.Write("|", filepath);
                    }
                }
            }
        }

        /// <summary>
        /// マジックナンバーテーブルを取得します。
        /// </summary>
        /// <returns></returns>
        private ulong[][] GetMagicTable()
        {
            ulong[][] magicTable = new ulong[81][];
            string csv = FileHelper.ReadToEnd(this.Config.MagicNumberFilePath);
            string[] magicNumbers = csv.Split(',');
            for (int i = 0; i < magicNumbers.Length; i += 4)
            {
                // TODO:あとでデータをちゃんとなおす
                if (string.IsNullOrEmpty(magicNumbers[i])) { continue; }

                int index = int.Parse(magicNumbers[i]);
                magicTable[index] = new ulong[3];
                magicTable[index][0] = ulong.Parse(magicNumbers[i + 1]);
                magicTable[index][1] = ulong.Parse(magicNumbers[i + 2]);
                magicTable[index][2] = ulong.Parse(magicNumbers[i + 3]);
            }

            return magicTable;
        }
    }
}
