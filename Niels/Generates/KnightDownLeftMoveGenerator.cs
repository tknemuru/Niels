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
    /// 桂馬の左下に進む利きを生成します。
    /// </summary>
    public class KnightDownLeftMoveGenerator : MoveGenerator
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
                                                  & board.ClearRank[8]
                                                  & board.ClearRank[7]
                                                  & board.ClearFile[8];
            }

            movedBoard[(int)BoardType.Main] = ((((clearBoard[(int)BoardType.Main] & boards[(int)BoardType.Main].ClearRank[6]) << 10) | ((clearBoard[(int)BoardType.SubRight] & boards[(int)BoardType.SubRight].ClearRank[6]) << 2)) & ~ownBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubBottom] = ((((clearBoard[(int)BoardType.Main] & ~boards[(int)BoardType.Main].ClearRank[6]) << 9) | ((clearBoard[(int)BoardType.SubRight] & 0x40) << 1)) & ~ownBoard[(int)BoardType.SubBottom]);

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex - 10);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, (uint)BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    fromIndex = (toIndex - 2);
                    if (clearBoard[(int)BoardType.SubRight].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubRight, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubBottom])
            {
                if (movedBoard[(int)BoardType.SubBottom].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex - 9);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubBottom, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.SubBottom, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    fromIndex = (toIndex - 1);
                    if (clearBoard[(int)BoardType.SubRight].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubBottom, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubRight, (uint)fromIndex, BoardType.SubBottom, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }
        }
    }
}