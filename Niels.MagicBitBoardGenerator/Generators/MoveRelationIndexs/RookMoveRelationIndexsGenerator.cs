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
    /// 飛車の利きに関係のあるマス目のみを抽出します
    /// </summary>
    internal class RookMoveRelationIndexsGenerator : MoveRelationIndexsGenerator
    {
        /// <summary>
        /// 指定した座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        protected override IEnumerable<int> GetCanMoves(int seqIndex)
        {
            if (BoardAccesor.IsEdge(seqIndex))
            {
                return this.GetEdgeCanMoves(seqIndex);
            }
            else if (BoardAccesor.IsLine(seqIndex))
            {
                return this.GetLineCanMoves(seqIndex);
            }
            else
            {
                return this.GetNormalCanMoves(seqIndex);
            }
        }

        /// <summary>
        /// 辺以外の座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        private IEnumerable<int> GetNormalCanMoves(int seqIndex)
        {
            Debug.Assert(!BoardAccesor.IsWall(seqIndex) && !BoardAccesor.IsLine(seqIndex), "座標が辺もしくは壁です。");

            IEnumerable<int> canMoveIndexs = new List<int>();
            Func<int, bool> canMove = (moveIndex => BoardAccesor.IsLine(moveIndex));

            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Up, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Down, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Right, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Left, canMove));

            return canMoveIndexs;
        }

        /// <summary>
        /// 辺の座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        private IEnumerable<int> GetLineCanMoves(int seqIndex)
        {
            Debug.Assert(BoardAccesor.IsLine(seqIndex) && !BoardAccesor.IsEdge(seqIndex), "座標が辺じゃありません。");

            IEnumerable<int> canMoveIndexs = new List<int>();
            Func<int, bool> canMove = (moveIndex => (BoardAccesor.IsLine(moveIndex) || BoardAccesor.IsWall(moveIndex)));
            Func<int, bool> canLineMove = (moveIndex => (BoardAccesor.IsEdge(moveIndex) || BoardAccesor.IsWall(moveIndex)));

            if (BoardAccesor.IsHorizontalLine(seqIndex))
            {
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Up, canMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Down, canMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Right, canLineMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Left, canLineMove));
            }
            else
            {
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Up, canLineMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Down, canLineMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Right, canMove));
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Left, canMove));
            }

            return canMoveIndexs;
        }

        /// <summary>
        /// 角の座標の動けるマス目のリストを取得します。
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        private IEnumerable<int> GetEdgeCanMoves(int seqIndex)
        {
            Debug.Assert(BoardAccesor.IsEdge(seqIndex), "座標が角じゃありません。");

            IEnumerable<int> canMoveIndexs = new List<int>();
            Func<int, bool> canMove = (moveIndex => (BoardAccesor.IsEdge(moveIndex) || BoardAccesor.IsWall(moveIndex)));

            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Up, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Down, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Right, canMove));
            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, BoardAccesor.BoardDirection.Left, canMove));

            return canMoveIndexs;
        }
    }
}
