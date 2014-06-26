using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Accessor;
using System.Diagnostics;

namespace Niels.MagicBitBoardGenerator.Generators.MoveRelationIndexs
{
    /// <summary>
    /// 角の利きに関係のあるマス目のみを抽出します
    /// </summary>
    internal class BishopMoveRelationIndexsGenerator : MoveRelationIndexsGenerator
    {
        /// <summary>
        /// 指定した座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        protected override IEnumerable<int> GetCanMoves(int seqIndex)
        {
            IEnumerable<int> canMoveIndexs = new List<int>();
            Func<int, bool> canMove = (moveIndex => BoardAccesor.IsLine(moveIndex) || BoardAccesor.IsWall(moveIndex));

            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.UpperRight, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.UpperLeft, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.LowerRight, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.LowerLeft, canMove));

            return canMoveIndexs;
        }
    }
}
