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
    /// 利きに関係のあるマス目のみを抽出する機能を提供します。
    /// </summary>
    internal abstract class MoveRelationIndexsGenerator
    {
        /// <summary>
        /// 利きに関係のあるマス目のみを抽出します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="boardType"></param>
        /// <returns></returns>
        internal IEnumerable<int> Generate(int index, BoardType boardType)
        {
            int seqIndex = BoardAccesor.GetSequanceIndex(index, boardType);
            Debug.Assert(!BoardAccesor.IsWall(seqIndex), "座標が壁です。");

            // 移動可能なインデックスの集合を取得
            IEnumerable<int> canMoveIndexs = this.GetCanMoves(seqIndex);
            return canMoveIndexs;
        }

        /// <summary>
        /// 指定した座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        protected abstract IEnumerable<int> GetCanMoves(int seqIndex);

        /// <summary>
        /// ある方向の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected List<int> GetOneDirectionCanMoveIndexs(int seqIndex, BoardAccesor.BoardDirection direction, Func<int, bool> canMove)
        {
            int moveIndex = seqIndex;
            List<int> canMoveIndexs = new List<int>();

            moveIndex += (int)direction;
            while (!canMove(moveIndex))
            {
                canMoveIndexs.Add(moveIndex);
                moveIndex += (int)direction;
            }

            return canMoveIndexs;
        }
    }
}
