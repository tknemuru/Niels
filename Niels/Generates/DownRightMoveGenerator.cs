using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Extensions.Number;
using Niels.Boards;
using Niels.Collections;
using Niels.Diagnostics;

namespace Niels.Generates
{
    /// <summary>
    /// 右下に進む利きを生成します。
    /// </summary>
    public class DownRightMoveGenerator : MoveGenerator
    {
        /// 利きを生成します。
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs)
        {
            Board[] boards = BoardProvider.GetAll();
            ulong[] clearBoard = new ulong[Board.BoardTypeCount];
            ulong[] movedBoard = new ulong[Board.BoardTypeCount];
            ulong[] ownBoard = context.OccupiedBoards[(int)context.Turn];

            foreach (Board board in boards)
            {
                clearBoard[(int)board.BoardType] = context.PieceBoards[(int)context.Turn][piece.GetIndex()][(int)board.BoardType]
                                                  & board.ClearRank[8]
                                                  & board.ClearFile[0];
            }

            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] & boards[(int)BoardType.Main].ClearRank[7] & boards[(int)BoardType.Main].ClearFile[1]) >> 7) & ~ownBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubRight] = (((clearBoard[(int)BoardType.Main] & ~boards[(int)BoardType.Main].ClearFile[1]) << 1) & ~ownBoard[(int)BoardType.SubRight]);
            movedBoard[(int)BoardType.SubBottom] = (((clearBoard[(int)BoardType.Main] & boards[(int)BoardType.Main].ClearFile[1]) >> 8) & ~ownBoard[(int)BoardType.SubBottom]);

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex + 7);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubRight])
            {
                if (movedBoard[(int)BoardType.SubRight].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex - 1);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubRight, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.SubRight, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubBottom])
            {
                if (movedBoard[(int)BoardType.SubBottom].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex + 8);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubBottom, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.SubBottom, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }
        }
    }
}