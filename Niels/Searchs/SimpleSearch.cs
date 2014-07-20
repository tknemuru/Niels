using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Generates;
using Niels.Filters;
using Niels.Evaluators;
using Niels.Helper;
using Niels.Diagnostics;

namespace Niels.Searchs
{
    /// <summary>
    /// 単純な指し手探索を行います。
    /// </summary>
    public class SimpleSearch : ISearch
    {
        /// <summary>
        /// 指し手を取得する
        /// </summary>
        public uint GetMove(BoardContext context)
        {
            IEnumerable<uint> moves = MoveProvider.GetAllMoves(context, GenerateTarget.All);

            ScoreIndexEvaluator ev = new ScoreIndexEvaluator();
            Dictionary<uint, int> moveEvaluates = new Dictionary<uint, int>();
            foreach (uint move in moves)
            {
                context.PutPiece(move);
                context.ChangeTurn();
                //IEnumerable<uint> innerMoves = MoveProvider.GetAllMoves(context, GenerateTarget.All);
                //List<int> evaluate = new List<int>();
                //foreach (uint innerMove in innerMoves)
                //{
                //    context.PutPiece(innerMove);
                //    evaluate.Add(ev.Evaluate(context, 0));
                //    context.UndoPutPiece();
                //}
                //var innerBestMove = evaluate.OrderBy(value => value).FirstOrDefault();
                moveEvaluates.Add(move, ev.Evaluate(context));
                context.UndoChangeTurn();
                context.UndoPutPiece();
            }

            var bestMove = moveEvaluates.OrderByDescending(moveKeyValue => moveKeyValue.Value).FirstOrDefault();

            // とりあえずベスト5を書き出してみる
            var orderdMoves = moveEvaluates.OrderByDescending(moveKeyValue => moveKeyValue.Value);
            int rank = 1;
            foreach (var moveKeyValue in orderdMoves)
            {
                FileHelper.WriteLine("Rank：" + rank.ToString());
                FileHelper.WriteLine("Score：" + moveKeyValue.Value);
                FileHelper.WriteLine(MoveDebug.ToDebugString(moveKeyValue.Key));
                rank++;
                if (rank > 5) { break; }
            }
            return bestMove.Key;
        }
    }
}
