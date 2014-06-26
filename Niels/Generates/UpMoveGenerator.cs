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
    /// 上に進む利きを生成する
    /// </summary>
    public class UpMoveGenerator : MoveGenerator
    {
        /// <summary>
        /// 上に進む利きを生成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs)
        {
            Board[] boards = BoardProvider.GetAll();
            ulong[] clearBoard = new ulong[Board.BoardTypeCount];
            ulong[] movedBoard = new ulong[Board.BoardTypeCount];
            ulong[] occupiedBoard = context.OccupiedBoards[(int)context.Turn];

            // 利き生成対象外の駒を盤上から除く
            foreach (Board board in boards)
            {
                clearBoard[(int)board.BoardType] = (context.PieceBoards[(int)context.Turn][piece.GetIndex()][(int)board.BoardType] & board.ClearRank[0]);
            }

            // 駒を動かす
            movedBoard[(int)BoardType.Main] = (((clearBoard[(int)BoardType.Main] >> 1) | clearBoard[(int)BoardType.SubBottom]) & ~occupiedBoard[(int)BoardType.Main]);
            movedBoard[(int)BoardType.SubBottom] = clearBoard[(int)BoardType.SubBottom] & ~occupiedBoard[(int)BoardType.Main];
            movedBoard[(int)BoardType.SubRight] = (clearBoard[(int)BoardType.SubRight] >> 1) & ~occupiedBoard[(int)BoardType.SubRight];

            foreach (int toIndex in targetIndexs[(int)BoardType.Main])
            {
                if (movedBoard[(int)BoardType.Main].IsPositive(toIndex))
                {
                    // メイン→メイン
                    int fromIndex = (toIndex + 1);
                    if (clearBoard[(int)BoardType.Main].IsPositive(fromIndex))
                    {
                        Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, toIndex, context);
                        yield return (Move.GetMove(piece, (uint)BoardType.Main, (uint)fromIndex, (uint)BoardType.Main, (uint)toIndex, context.Turn, caputuredPiece));
                    }
                }
            }

            foreach (int formToIndex in targetIndexs[(int)BoardType.SubBottom])
            {
                // 下→メイン
                if (movedBoard[(int)BoardType.SubBottom].IsPositive(formToIndex))
                {
                    Piece caputuredPiece = CaptureGenerator.GenerateAnyCapturedPiece(BoardType.Main, formToIndex, context);
                    yield return (Move.GetMove(piece, BoardType.SubBottom, (uint)formToIndex, BoardType.Main, (uint)formToIndex, context.Turn, caputuredPiece));
                }
            }

            foreach (int toIndex in targetIndexs[(int)BoardType.SubRight])
            {
                if (movedBoard[(int)BoardType.SubRight].IsPositive(toIndex))
                {
                    // 右→右
                    int fromIndex = (toIndex + 1);
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
