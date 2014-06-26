using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Boards;
using Niels.Collections;
using Niels.Extensions.Number;
using Niels.Diagnostics;

namespace Niels.Generates
{
    /// <summary>
    /// 取られる駒を生成する
    /// </summary>
    public static class CaptureGenerator
    {
        /// <summary>
        /// 取られる駒を生成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Piece GenerateAnyCapturedPiece(BoardType toBoard, int toIndex, BoardContext context)
        {
            var isExistsEnemyPiece = context.OccupiedBoards[context.Turn.GetOppositeIndex()][(int)toBoard].IsPositive(toIndex);

            // 敵駒が移動先になければ空（駒無し）を返して処理終了
            if (!isExistsEnemyPiece)
            {
                return Piece.Empty;
            }

            // 取られる駒を特定して返す
            foreach (Piece piece in ExtensionPiece.Pieces)
            {
                Piece capturedPiece = GenerateCapturedPiece(toBoard, toIndex, context, piece);
                if(piece == capturedPiece)
                {
                    return capturedPiece;
                }
            }

            // ここにくることはあり得ない
            throw new ApplicationException("取られる駒を正常に生成することができませんでした。");
        }

        /// <summary>
        /// 取られる駒を生成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Piece GenerateCapturedPiece(BoardType toBoard, int toIndex, BoardContext context, Piece capturedPiece)
        {
            var isCanGenerate = context.PieceBoards[context.Turn.GetOppositeIndex()][capturedPiece.GetIndex()][(int)toBoard].IsPositive(toIndex);

            return isCanGenerate ? capturedPiece : Piece.Empty;
        }
    }
}