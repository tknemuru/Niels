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
    /// 飛車の利き生成の機能を提供します。
    /// </summary>
    internal class RookAttackGenerator : AttackGenerator
    {
        /// <summary>
        /// 利きを生成します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="boardState"></param>
        /// <returns></returns>
        internal override IEnumerable<uint> Generate(int index, Dictionary<int, bool> pattern, Piece targetPiece)
        {
            IEnumerable<uint> moves = new List<uint>();
            moves = this.GetOneDirectionMoves(index, pattern, BoardAccesor.BoardDirection.Up, targetPiece);
            moves = moves.Union(this.GetOneDirectionMoves(index, pattern, BoardAccesor.BoardDirection.Right, targetPiece));
            moves = moves.Union(this.GetOneDirectionMoves(index, pattern, BoardAccesor.BoardDirection.Down, targetPiece));
            moves = moves.Union(this.GetOneDirectionMoves(index, pattern, BoardAccesor.BoardDirection.Left, targetPiece));         

            return moves;
        }
    }
}
