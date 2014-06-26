using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Extensions.Number;
using Niels.Boards;
using Niels.Collections;

namespace Niels.Generates
{
    /// <summary>
    /// 左に進む利きを生成します。
    /// </summary>
    public class LeftMoveGenerator : MoveGenerator
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
                                                  & board.ClearFile[8];
            }

            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] << 8) | (clearBoard[(int)BoardType.SubRight] & boards[(int)BoardType.SubRight].ClearRank[8])) & ~ownBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubBottom] = (((clearBoard[(int)BoardType.SubBottom] << 8) | ((clearBoard[(int)BoardType.SubRight] & ~boards[(int)BoardType.SubRight].ClearRank[8]) >> 1)) & ~ownBoard[(int)BoardType.SubBottom]);

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    // メイン→メイン
                    int fromIndex = (toIndex - 8);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.Main, (uint)fromIndex, BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 右→メイン
                    fromIndex = toIndex;
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
                    // 下→下
                    int fromIndex = (toIndex - 8);
                    if (clearBoard[(int)BoardType.SubBottom].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubBottom, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubBottom, (uint)fromIndex, BoardType.SubBottom, (uint)toIndex, context.Turn, caputuredPiece));
                        continue;
                    }

                    // 右→下                    
                    fromIndex = (toIndex + 1);
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