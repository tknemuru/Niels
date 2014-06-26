using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.MagicBitBoardGenerator.Accessor;

namespace Niels.MagicBitBoardGenerator.Generators.Attack
{
    /// <summary>
    /// 香車の利き生成の機能を提供します。
    /// </summary>
    internal class LaunceAttackGenerator : AttackGenerator
    {
        /// <summary>
        /// 生成対象のターン
        /// </summary>
        private Turn Turn { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="turn"></param>
        internal LaunceAttackGenerator(Turn turn)
        {
            this.Turn = turn;
        }

        /// <summary>
        /// 利きを生成します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="boardState"></param>
        /// <returns></returns>
        internal override IEnumerable<uint> Generate(int index, Dictionary<int, bool> pattern, Piece targetPiece)
        {
            BoardAccesor.BoardDirection direction = (this.Turn == Turn.Black) ? BoardAccesor.BoardDirection.Up : BoardAccesor.BoardDirection.Down;
            IEnumerable<uint> moves = new List<uint>();
            moves = this.GetOneDirectionMoves(index, pattern, direction, targetPiece);

            return moves;
        }
    }
}
