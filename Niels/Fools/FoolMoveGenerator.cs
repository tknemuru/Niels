using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Generates;
using Niels.Boards;
using Niels.Collections;

namespace Niels.Fools
{
    /// <summary>
    /// 指定された個数分の打ち手を返す
    /// </summary>
    public class FoolMoveGenerator
    {
        /// <summary>
        /// 一度に生成する個数
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        private uint SeqNo { get; set; }

        /// <summary>
        /// ダミーの打ち手
        /// </summary>
        private static readonly uint DummyMove = Move.GetMove(Piece.Pawn, BoardType.Main, 1, BoardType.Main, 0, Turn.Black);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="values"></param>
        public FoolMoveGenerator(int count)
        {
            this.Count = count;
            this.SeqNo = 0;
        }

        /// <summary>
        /// 打ち手を生成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<uint> Generate(BoardContext context)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var move = Move.GetMove(Piece.Pawn, BoardType.Main, this.SeqNo, BoardType.Main, 0, context.Turn);
                this.SeqNo++;
                yield return move;
            }
        }
    }
}
