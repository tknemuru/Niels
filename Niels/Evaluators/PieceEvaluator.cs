﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Niels.Collections;
using Niels.Boards;
using Niels.Diagnostics;
using Niels.Extensions.Number;

namespace Niels.Evaluators
{
    /// <summary>
    /// 駒割による評価機能を提供します。
    /// </summary>
    public class PieceEvaluator : Evaluator
    {
        /// <summary>
        /// 駒の価値
        /// </summary>
        private static readonly int[] PieceScores =
        { 87, 232, 257, 369, 444, 569, 642, 99999, 534, 489, 510, 495, 827, 945 };

        /// <summary>
        /// 評価対象の駒
        /// </summary>
        private IEnumerable<Piece> TargetPieces { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PieceEvaluator()
            : this(ExtensionPiece.Pieces)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetPiece"></param>
        public PieceEvaluator(IEnumerable<Piece> targetPiece)
        {
            this.TargetPieces = targetPiece;
        }

        /// <summary>
        /// 評価値を取得します。
        /// </summary>
        /// <returns></returns>
        public override int Evaluate(BoardContext context)
        {
            int score = 0;
            foreach (Piece piece in this.TargetPieces)
            {
                if (piece == Piece.Empty || piece == Piece.King) { continue; }

                foreach (Board board in BoardProvider.GetAll())
                {
                    foreach (int index in board.UsingIndexs)
                    {
                        if (context.PieceBoards[(int)Turn.Black][(int)piece.GetIndex()][(int)board.BoardType].IsPositive(index))
                        {
                            score += PieceScores[(int)piece.GetIndex()];
                        }
                        else if (context.PieceBoards[(int)Turn.White][(int)piece.GetIndex()][(int)board.BoardType].IsPositive(index))
                        {
                            score -= PieceScores[(int)piece.GetIndex()];
                        }
                    }
                }

                if (!piece.IsPromoted())
                {
                    score += ((PieceScores[(int)piece.GetIndex()] * (int)context.GetHandValueCount(piece, Turn.Black)) - (PieceScores[(int)piece.GetIndex()] * (int)context.GetHandValueCount(piece, Turn.White)));
                }
            }

            return score;
        }
    }
}
