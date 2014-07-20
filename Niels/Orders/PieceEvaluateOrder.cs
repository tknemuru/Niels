using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Evaluators;
using Niels.Helper;

namespace Niels.Orders
{
    /// <summary>
    /// 駒割の評価値によるソート機能を提供します。
    /// </summary>
    public class PieceEvaluateOrder : IOrder
    {
        /// <summary>
        /// 評価対象の駒
        /// </summary>
        private static readonly Piece[] EvaluateTargetPieces =
        {
            //Piece.Silver,
            //Piece.Gold,
            //Piece.PawnPromoted,
            //Piece.LauncePromoted,
            //Piece.KnightPromoted,

            Piece.Bishop,
            Piece.Rook,
            Piece.King,
            Piece.Horse,
            Piece.Dragon
        };

        /// <summary>
        /// 指し手をソートします。
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IEnumerable<uint> MoveOrdering(IEnumerable<uint> moves, BoardContext context)
        {
            var ev = new PieceEvaluator(EvaluateTargetPieces);
            List<Tuple<uint, int>> evaluatedMoves = new List<Tuple<uint, int>>(moves.Count());
            int parity = ev.GetParity(context);

            foreach(var move in moves)
            {
                // 手を指す
                context.PutPiece(move);

                // 評価値を取得
                evaluatedMoves.Add(new Tuple<uint, int>(move, ev.Evaluate(context) * parity));

                // 手を戻す
                context.UndoPutPiece();
            }

            // ソートして返却
            var orderdMoves = from evMove in evaluatedMoves
                              orderby evMove.Item2 descending
                              select evMove.Item1;
            // 念のため確認
            //var orderdValues = from evMove in evaluatedMoves
            //                  orderby evMove.Item2 descending
            //                  select evMove.Item2;
            //FileHelper.WriteLine(String.Join(", ", orderdValues));

            return orderdMoves;
        }
    }
}
