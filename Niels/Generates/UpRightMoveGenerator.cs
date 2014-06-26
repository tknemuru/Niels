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
    /// 右上に進む利きを生成します。
    /// </summary>
    public class UpRightMoveGenerator : MoveGenerator
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
                                                  & board.ClearRank[0]
                                                  & board.ClearFile[0];
            }

            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] >> 9) | ((clearBoard[(int)BoardType.SubBottom] & boards[(int)BoardType.Main].ClearFile[1]) >> 8)) & ~ownBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubRight] = ((((clearBoard[(int)BoardType.Main] & ~boards[(int)BoardType.Main].ClearFile[1]) >> 1) | ((clearBoard[(int)BoardType.SubBottom] & 0x80))) & ~ownBoard[(int)BoardType.SubRight]);

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    // メイン→メイン
                    int fromIndex = (toIndex + 9);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, (uint)BoardType.Main, (uint)fromIndex, (uint)BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 下→メイン
                    fromIndex = (toIndex + 8);
                    if (clearBoard[(int)BoardType.SubBottom].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubBottom, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubRight])
            {
                if (movedBoard[(int)BoardType.SubRight].IsPositive(toIndex))
                {
                    // メイン→右
                    int fromIndex = (toIndex + 1);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubRight, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.SubRight, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 下→右
                    fromIndex = toIndex;
                    if (clearBoard[(int)BoardType.SubBottom].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubRight, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubBottom, (uint)fromIndex, BoardType.SubRight, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }
        }
    }
}