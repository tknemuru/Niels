using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Collections.Math;
using Niels.Extensions.Number;
using Niels.Generates;
using Niels.Helper;
using Niels.Diagnostics;
using System.Diagnostics;

namespace Niels.Evaluators
{
    /// <summary>
    /// 連続して隣り合っている駒の関係性から生成される特徴ベクトルです。
    /// </summary>
    public static class SequencePositionFeatureVector
    {
        /// <summary>
        /// 追加情報
        /// </summary>
        public enum ExtraInfo
        {
            /// <summary>
            /// 駒数（駒別）
            /// </summary>
            PieceCount = 83521,

            /// <summary>
            /// パリティ
            /// </summary>
            Parity = 83535,
        }

        /// <summary>
        /// 長さ
        /// </summary>
        public const int Length = 83536;

        /// <summary>
        /// インデックスを構成するマス目数
        /// </summary>
        private const int UnitCount = 4;

        /// <summary>
        /// インデックスを構成する駒数
        /// </summary>
        private const int PieceCount = 17;

        /// <summary>
        /// スコアインデックスの最大値
        /// </summary>
        private static readonly int MaxScoreIndex = ((int)Math.Pow(PieceCount, UnitCount) - 1);

        /// <summary>
        /// 正規化インデックスのファイルパス
        /// </summary>
        private const string NormalizedScoreIndexFIlePath = @"C:\work\visualstudio\Niels\Niels\Data\normalize\normalized_score_index.txt";

        /// <summary>
        /// 正規化インデックス辞書
        /// </summary>
        private static readonly Dictionary<int, int> NormalizedScoreIndexs = InitializeNormalizeScoreIndexs();

        /// <summary>
        /// 駒に割り当てられたインデックス辞書
        /// </summary>
        private static readonly Dictionary<TurnPiece, int> PieceIndexs = InitializePieceIndex();

        /// <summary>
        /// 駒に割り当てられたインデックス辞書を初期化します。
        /// </summary>
        /// <returns></returns>
        private static Dictionary<TurnPiece, int> InitializePieceIndex()
        {
            Dictionary<TurnPiece, int> pieceIndexs = new Dictionary<TurnPiece, int>();
            pieceIndexs.Add(TurnPiece.Empty, 0);
            pieceIndexs.Add(TurnPiece.BlackPawn, 1);
            pieceIndexs.Add(TurnPiece.BlackLaunce, 2);
            pieceIndexs.Add(TurnPiece.BlackKnight, 3);
            pieceIndexs.Add(TurnPiece.BlackSilver, 4);
            pieceIndexs.Add(TurnPiece.BlackGold, 5);
            pieceIndexs.Add(TurnPiece.BlackBishop, 6);
            pieceIndexs.Add(TurnPiece.BlackRook, 7);
            pieceIndexs.Add(TurnPiece.BlackKing, 8);
            pieceIndexs.Add(TurnPiece.BlackPawnPromoted, 5);
            pieceIndexs.Add(TurnPiece.BlackLauncePromoted, 5);
            pieceIndexs.Add(TurnPiece.BlackKnightPromoted, 5);
            pieceIndexs.Add(TurnPiece.BlackSilverPromoted, 5);
            pieceIndexs.Add(TurnPiece.BlackHorse, 6);
            pieceIndexs.Add(TurnPiece.BlackDragon, 7);
            pieceIndexs.Add(TurnPiece.WhitePawn, 9);
            pieceIndexs.Add(TurnPiece.WhiteLaunce, 10);
            pieceIndexs.Add(TurnPiece.WhiteKnight, 11);
            pieceIndexs.Add(TurnPiece.WhiteSilver, 12);
            pieceIndexs.Add(TurnPiece.WhiteGold, 13);
            pieceIndexs.Add(TurnPiece.WhiteBishop, 14);
            pieceIndexs.Add(TurnPiece.WhiteRook, 15);
            pieceIndexs.Add(TurnPiece.WhiteKing, 16);
            pieceIndexs.Add(TurnPiece.WhitePawnPromoted, 13);
            pieceIndexs.Add(TurnPiece.WhiteLauncePromoted, 13);
            pieceIndexs.Add(TurnPiece.WhiteKnightPromoted, 13);
            pieceIndexs.Add(TurnPiece.WhiteSilverPromoted, 13);
            pieceIndexs.Add(TurnPiece.WhiteHorse, 14);
            pieceIndexs.Add(TurnPiece.WhiteDragon, 15);
            return pieceIndexs;
        }

        /// <summary>
        /// 正規化インデックス辞書を初期化します。
        /// </summary>
        private static Dictionary<int, int> InitializeNormalizeScoreIndexs()
        {
            Dictionary<int, int> normalizedScoreIndexs = new Dictionary<int, int>();
            string csv = FileHelper.ReadToEnd(NormalizedScoreIndexFIlePath);
            if (string.IsNullOrEmpty(csv)) { return new Dictionary<int, int>(); }
            string[] csvList = csv.Split(',');
            Debug.Assert((csvList.Length % 2 == 0), "要素数が奇数です。");
            for (int i = 0; i < csvList.Length; i += 2)
            {
                normalizedScoreIndexs.Add(int.Parse(csvList[i]), int.Parse(csvList[i + 1]));
            }
            return normalizedScoreIndexs;
        }

        /// <summary>
        /// スコアインデックスを生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<int, int> Generate(BoardContext context)
        {
            Dictionary<int, int> scoreIndexs = new Dictionary<int, int>();
            int start = (9 - UnitCount);
            int end = (9 - UnitCount);

            // 横
            for (int rank = 0; rank < 9; rank++)
            {
                for(int file = 0; file <= end; file++)
                {
                    int scoreIndex = 0;
                    int targetRank = rank;
                    int targetFile = file;
                    for (int count = 0; count < UnitCount; count++)
                    {
                        // 駒を取得
                        TurnPiece piece = GetPiece(targetFile, targetRank, context);

                        scoreIndex += PieceIndexs[piece];
                        // 最終回でかけると桁が実際より一桁上がってしまう
                        if (count < UnitCount - 1)
                        {
                            scoreIndex *= PieceCount;
                        }

                        targetFile++;
                    }

                    // スコアインデックスを追加
                    AddScoreIndex(scoreIndexs, scoreIndex);
                }
            }

            // 縦
            for (int file = 0; file < 9; file++)
            {
                for (int rank = 0; rank <= end; rank++)
                {
                    int scoreIndex = 0;
                    int targetRank = rank;
                    int targetFile = file;
                    for (int count = 0; count < UnitCount; count++)
                    {
                        // 駒を取得
                        TurnPiece piece = GetPiece(targetFile, targetRank, context);

                        scoreIndex += PieceIndexs[piece];
                        // 最終回でかけると桁が実際より一桁上がってしまう
                        if (count < UnitCount - 1)
                        {
                            scoreIndex *= PieceCount;
                        }

                        targetRank++;
                    }

                    // スコアインデックスを追加
                    AddScoreIndex(scoreIndexs, scoreIndex);
                }
            }

            // 斜め（上⇒左）
            for (int rank = 0; rank <= end; rank++)
            {
                for (int file = start; file >= 0; file--)
                {
                    int scoreIndex = 0;
                    int targetRank = rank;
                    int targetFile = file;
                    for (int count = 0; count < UnitCount; count++)
                    {
                        // 駒を取得
                        TurnPiece piece = GetPiece(targetFile, targetRank, context);

                        scoreIndex += PieceIndexs[piece];
                        // 最終回でかけると桁が実際より一桁上がってしまう
                        if (count < UnitCount - 1)
                        {
                            scoreIndex *= PieceCount;
                        }

                        targetRank++;
                        targetFile++;
                    }

                    // スコアインデックスを追加
                    AddScoreIndex(scoreIndexs, scoreIndex);
                }
            }

            // 斜め（上⇒右）
            for (int rank = 0; rank <= end; rank++)
            {
                for (int file = (UnitCount - 1); file < 9; file++)
                {
                    int scoreIndex = 0;
                    int targetRank = rank;
                    int targetFile = file;
                    for (int count = 0; count < UnitCount; count++)
                    {
                        // 駒を取得
                        TurnPiece piece = GetPiece(targetFile, targetRank, context);

                        scoreIndex += PieceIndexs[piece];
                        // 最終回でかけると桁が実際より一桁上がってしまう
                        if (count < UnitCount - 1)
                        {
                            scoreIndex *= PieceCount;
                        }

                        targetRank++;
                        targetFile--;
                    }

                    // スコアインデックスを追加
                    AddScoreIndex(scoreIndexs, scoreIndex);
                }
            }

            // 駒数
            foreach (Piece piece in ExtensionPiece.Pieces)
            {
                if (piece == Piece.Empty) { continue; }
                int count = 0;
                foreach (Board board in BoardProvider.GetAll())
                {
                    foreach(int index in board.UsingIndexs)
                    {
                        if (context.PieceBoards[(int)Turn.Black][(int)piece.GetIndex()][(int)board.BoardType].IsPositive(index))
                        {
                            count++;
                        }
                        else if (context.PieceBoards[(int)Turn.White][(int)piece.GetIndex()][(int)board.BoardType].IsPositive(index))
                        {
                            count--;
                        }
                    }
                }

                if (!piece.IsPromoted() && piece != Piece.King)
                {
                    count += ((int)context.GetHandValueCount(piece, Turn.Black) - (int)context.GetHandValueCount(piece, Turn.White));
                }

                Debug.Assert(!scoreIndexs.ContainsKey((int)ExtraInfo.PieceCount + piece.GetIndex()), string.Format("既にキーが存在しています。（持ち駒数：{0}）", piece));
                scoreIndexs.Add((int)ExtraInfo.PieceCount + piece.GetIndex(), count);
            }

            // パリティ
            Debug.Assert(!scoreIndexs.ContainsKey((int)ExtraInfo.Parity), "既にキーが存在しています。（パリティ）");
            int parity = (context.Turn == Turn.Black) ? 1 : 0;
            scoreIndexs.Add((int)ExtraInfo.Parity, parity);

            return scoreIndexs;
        }

        /// <summary>
        /// 駒割の評価値を表示します。
        /// </summary>
        /// <param name="evaluateVector"></param>
        public static void DisplayPieceEvaluate(SparseVector<double> evaluateVector)
        {
            foreach (Piece piece in ExtensionPiece.Pieces)
            {
                if (piece == Piece.King) { continue; }
                Console.WriteLine(string.Format("{0}({1}):{2}", piece, piece.GetIndex(), evaluateVector[(int)ExtraInfo.PieceCount + piece.GetIndex()]));
            }
        }

        /// <summary>
        /// 駒に割り当てられたインデックスを取得します。
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static int GetPieceIndex(TurnPiece piece)
        {
            return PieceIndexs[piece];
        }

        /// <summary>
        /// 正規化したインデックスを取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetNormalizedScoreIndex(int index)
        {
            return NormalizedScoreIndexs[index];
        }

        /// <summary>
        /// 正規化したインデックスを取得します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetNormalizedScoreIndex()
        {
            return NormalizedScoreIndexs;
        }

        /// <summary>
        /// 駒を取得します。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static TurnPiece GetPiece(int file, int rank, BoardContext context)
        {
            TurnPiece piece = TurnPiece.Empty;
            if (file == 0)
            {
                piece = context.GetTurnPiece(BoardType.SubRight, rank);
            }
            else if (rank < 8)
            {
                piece = context.GetTurnPiece(BoardType.Main, (((file - 1) * 8) + rank));
            }
            else
            {
                piece = context.GetTurnPiece(BoardType.SubBottom, (((file - 1) * 8) + 7));
            }
            return piece;
        }

        /// <summary>
        /// スコアインデックスを追加します。
        /// </summary>
        /// <param name="scoreIndexs"></param>
        /// <param name="scoreIndex"></param>
        private static void AddScoreIndex(Dictionary<int, int> scoreIndexs, int scoreIndex)
        {
            Debug.Assert(Math.Abs(scoreIndex) < 83521, string.Format("インデックスが最大値(83521)を超えています。{0}", scoreIndex));
            int normalizedIndex = NormalizedScoreIndexs[scoreIndex];            
            int addCount = 1;
            if (NormalizedScoreIndexs[scoreIndex] < 0)
            {
                normalizedIndex = -normalizedIndex;
                addCount = -1;
            }

            if (!scoreIndexs.ContainsKey(normalizedIndex))
            {
                scoreIndexs.Add(normalizedIndex, addCount);
            }
            else
            {
                scoreIndexs[normalizedIndex] += addCount;
            }
        }
    }
}
