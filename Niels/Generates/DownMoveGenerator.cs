using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Extensions.Number;
using Niels.Boards;
using Niels.Collections;

namespace Niels.Generates
{
    /// <summary>
    /// 下に進む利きを生成する
    /// </summary>
    public class DownMoveGenerator : MoveGenerator
    {
        /// <summary>
        /// 下に進む利きを生成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs)
        {
            Board[] boards = BoardProvider.GetAll();
            ulong[] clearBoard = new ulong[Board.BoardTypeCount];
            ulong[] movedBoard = new ulong[Board.BoardTypeCount];
            ulong[] occupiedBoard = context.OccupiedBoards[(int)context.Turn];

            foreach (Board board in boards)
            {
                clearBoard[(int)board.BoardType] = (context.PieceBoards[(int)context.Turn][piece.GetIndex()][(int)board.BoardType] & board.ClearRank[8]);
            }
            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] & boards[(int)BoardType.Main].ClearRank[7]) << 1) & ~occupiedBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubBottom] = clearBoard[(int)BoardType.Main] & ~occupiedBoard[(int)BoardType.SubBottom];
            movedBoard[(int)BoardType.SubRight] = (clearBoard[(int)BoardType.SubRight] << 1) & ~occupiedBoard[(int)BoardType.SubRight];

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex - 1);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, (uint)BoardType.Main, (uint)fromIndex, (uint)BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int formToIndex in targetIndexs[(int)BoardType.SubBottom])
            {
                if (movedBoard[(int)BoardType.SubBottom].IsPositive(formToIndex))
                {
                    Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubBottom, formToIndex, context);
                    yield return (Move.GetMove(piece, BoardType.Main, (uint)formToIndex, BoardType.SubBottom, (uint)formToIndex, context.Turn, caputuredPiece));
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubRight])
            {
                if (movedBoard[(int)BoardType.SubRight].IsPositive(toIndex))
                {
                    int fromIndex = (toIndex - 1);
                    if (clearBoard[(int)BoardType.SubRight].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.SubRight, toIndex, context);
                        yield return (Move.GetMove(piece, BoardType.SubRight, (uint)fromIndex, BoardType.SubRight, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }                
            }
        }
    }
}
