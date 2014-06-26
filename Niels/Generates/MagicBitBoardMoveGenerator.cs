using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Extensions.Number;
using Niels.Helper;
using Niels.Filters;

namespace Niels.Generates
{
    /// <summary>
    /// マジックビットボードを使用して利きを生成します。
    /// </summary>
    public class MagicBitBoardMoveGenerator : MoveGenerator
    {
        // インデックス→盤種別→マスクで使用する数値
        private readonly ulong[][] MaskTable;

        // インデックス→盤種別→マジックナンバー
        private readonly ulong[][] MagicTable;

        // インデックス→マジックナンバー→利きリスト
        private readonly Dictionary<int, Dictionary<ulong, List<uint>>> AttackTable = new Dictionary<int, Dictionary<ulong,List<uint>>>();

        /// <summary>
        /// アタックテーブルのファイルパス
        /// </summary>
        private string AttackTableFilePath { get; set; }

        /// <summary>
        /// マジックコードのシフト数
        /// </summary>
        private int ShiftMagicCode { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="magicTableFilePath"></param>
        /// <param name="maskTableFilePath"></param>
        /// <param name="attackTableFilePath"></param>
        public MagicBitBoardMoveGenerator(string magicTableFilePath, string maskTableFilePath, string attackTableFilePath, int shiftMagicCode)
        {
            this.MagicTable = new ulong[81][];
            string csv = FileHelper.ReadToEnd(magicTableFilePath);
            string[] magicNumbers = csv.Split(',');
            for (int i = 0; i < magicNumbers.Length; i += 4)
            {
                int index = int.Parse(magicNumbers[i]);
                this.MagicTable[index] = new ulong[3];
                this.MagicTable[index][0] = ulong.Parse(magicNumbers[i + 1]);
                this.MagicTable[index][1] = ulong.Parse(magicNumbers[i + 2]);
                this.MagicTable[index][2] = ulong.Parse(magicNumbers[i + 3]);
            }

            this.MaskTable = new ulong[81][];
            csv = FileHelper.ReadToEnd(maskTableFilePath);
            string[] maskNumbers = csv.Split(',');
            for (int i = 0; i < maskNumbers.Length; i += 4)
            {
                int index = int.Parse(maskNumbers[i]);
                this.MaskTable[index] = new ulong[3];
                this.MaskTable[index][0] = ulong.Parse(maskNumbers[i + 1]);
                this.MaskTable[index][1] = ulong.Parse(maskNumbers[i + 2]);
                this.MaskTable[index][2] = ulong.Parse(maskNumbers[i + 3]);
            }

            this.AttackTableFilePath = attackTableFilePath;
            this.ShiftMagicCode = shiftMagicCode;
        }

        /// <summary>
        /// 利きを生成します。
        /// TODO：このメソッドをoverrideするのはよくない。targetIndexs未使用なので。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="piece"></param>
        /// <param name="targetIndexs"></param>
        /// <returns></returns>
        protected override IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs)
        {
            Board[] boards = BoardProvider.GetAll();
            ulong[] bishopBoard = context.PieceBoards[(int)context.Turn][piece.GetIndex()];
            ulong[] occupiedBoard = context.GetAllOccupiedBoards();

            foreach (Board board in boards)
            {
                foreach (int i in board.UsingIndexs)
                {
                    if (bishopBoard[(int)board.BoardType].IsPositive(i))
                    {
                        // 利きに関係しないマス目をマスクする
                        int seqIndex = board.GetSequanceIndex(i);
                        ulong[] boardState = this.GetBoardState(occupiedBoard, seqIndex);

                        // それぞれの盤にマジックナンバーをかける
                        boardState[(int)BoardType.Main] *= MagicTable[seqIndex][(int)BoardType.Main];
                        boardState[(int)BoardType.SubRight] *= MagicTable[seqIndex][(int)BoardType.SubRight];
                        boardState[(int)BoardType.SubBottom] *= MagicTable[seqIndex][(int)BoardType.SubBottom];

                        // 盤同士のxorを求める
                        ulong magicCode = boardState[(int)BoardType.Main] ^ boardState[(int)BoardType.SubRight] ^ boardState[(int)BoardType.SubBottom];

                        // 関係のあるマス目だけ残るようにシフト
                        magicCode >>= (64 - this.ShiftMagicCode);

                        // 利きを取得する
                        foreach (uint move in this.GetMoves(seqIndex, magicCode))
                        {
                            // 自駒が既に存在している場所に指す手をフィルタする
                            if (FilterProvider.PieceDuplicateExistsMoveFilter.Validate(context, move))
                            {
                                // 利き情報を正しい情報に修正する
                                yield return this.UpdateMoves(context, move, piece);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// マスクした盤を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ulong[] GetBoardState(ulong[] occupiedBoard, int index)
        {
            ulong[] maskedOccupiedBoards = new ulong[Board.BoardTypeCount];
            maskedOccupiedBoards[(int)BoardType.Main] = occupiedBoard[(int)BoardType.Main] & MaskTable[index][(int)BoardType.Main];
            maskedOccupiedBoards[(int)BoardType.SubRight] = occupiedBoard[(int)BoardType.SubRight] & MaskTable[index][(int)BoardType.SubRight];
            maskedOccupiedBoards[(int)BoardType.SubBottom] = occupiedBoard[(int)BoardType.SubBottom] & MaskTable[index][(int)BoardType.SubBottom];

            return maskedOccupiedBoards;
        }

        /// <summary>
        /// 利きを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <param name="magicCode"></param>
        /// <returns></returns>
        private IEnumerable<uint> GetMoves(int seqIndex, ulong magicCode)
        {
            if (!AttackTable.ContainsKey(seqIndex))
            {
                Dictionary<ulong, List<uint>> movesDic = new Dictionary<ulong, List<uint>>();
                string csv = FileHelper.ReadToEnd(string.Format(this.AttackTableFilePath, seqIndex));
                string[] attacks = csv.Split('|');
                foreach (string attack in attacks)
                {
                    // TODO:あとでデータの方をちゃんと直す
                    if (attack == string.Empty) { continue; }

                    string[] moves = attack.Split(',');
                    List<uint> movesList = new List<uint>();
                    for (int i = 1; i < moves.Length; i++)
                    {
                        movesList.Add(uint.Parse(moves[i]));
                    }
                    movesDic.Add(ulong.Parse(moves[0]), movesList);
                }
                AttackTable.Add(seqIndex, movesDic);
            }

            foreach (uint move in AttackTable[seqIndex][magicCode])
            {
                yield return move;
            }
        }

        /// <summary>
        /// 利き情報を正しい情報に修正します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="moves"></param>
        /// <returns></returns>
        private uint UpdateMoves(BoardContext context, uint move, Piece piece)
        {
            // 指す駒
            if (piece.IsPromoted())
            {
                move = move.SetPutPiece(piece.Promote());
            }

            // 先手/後手
            move |= ((uint)context.Turn << Move.ShiftPutPieceTurn);
            move ^= ((uint)context.Turn << Move.ShiftCapturePieceTurn);

            if (move.IsCapture())
            {
                // 取得駒
                Piece capturePiece = CaptureGenerator.GenerateAnyCapturedPiece(move.ToBoard(), (int)move.ToIndex(), context);
                move = move.SetCapturePiece(capturePiece);
            }

            return move;
        }
    }
}
