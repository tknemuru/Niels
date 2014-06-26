using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Extensions.Number;
using Niels.Boards;
using Niels.Collections;
using System.Diagnostics;

namespace Niels.Generates
{
    /// <summary>
    /// 桂馬の左上に進む利きを生成します。
    /// </summary>
    public class KnightUpLeftMoveGenerator : MoveGenerator
    {
        /// 利きを生成します。
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs)
        {
            Debug.Assert(piece == Piece.Knight, "この利きは桂馬以外に使用することはできません。");
            Board[] boards = BoardProvider.GetAll();
            ulong[] clearBoard = new ulong[Board.BoardTypeCount];
            ulong[] movedBoard = new ulong[Board.BoardTypeCount];
            ulong[] ownBoard = context.OccupiedBoards[(int)context.Turn];

            foreach (Board board in boards)
            {
                clearBoard[(int)board.BoardType] = context.PieceBoards[(int)context.Turn][piece.GetIndex()][(int)board.BoardType]
                                                  & board.ClearRank[0]
                                                  & board.ClearRank[1]
                                                  & board.ClearFile[8];
            }

            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] << 6) | (clearBoard[(int)BoardType.SubBottom] << 7) | (clearBoard[(int)BoardType.SubRight] >> 2)) & ~ownBoard[(int)BoardType.Main]);

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    // メイン→メイン
                    int fromIndex = (toIndex - 6);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, (uint)BoardType.Main, (uint)fromIndex, (uint)BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 右→メイン
                    fromIndex = (toIndex + 2);
                    if (clearBoard[(int)BoardType.SubRight].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubRight, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 下→メイン
                    fromIndex = (toIndex - 7);
                    if (clearBoard[(int)BoardType.SubBottom].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubBottom, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }
        }
    }
}