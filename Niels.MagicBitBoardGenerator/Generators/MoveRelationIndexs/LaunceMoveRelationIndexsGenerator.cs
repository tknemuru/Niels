﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.MagicBitBoardGenerator.Accessor;
using System.Diagnostics;

namespace Niels.MagicBitBoardGenerator.Generators.MoveRelationIndexs
{
    /// <summary>
    /// 香車の利きに関係のあるマス目のみを抽出します
    /// </summary>
    internal class LaunceMoveRelationIndexsGenerator : MoveRelationIndexsGenerator
    {
        /// <summary>
        /// 生成対象のターン
        /// </summary>
        private Turn Turn { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="turn"></param>
        internal LaunceMoveRelationIndexsGenerator(Turn turn)
        {
            this.Turn = turn;
        }

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
            BoardAccesor.BoardDirection direction = (this.Turn == Turn.Black) ? BoardAccesor.BoardDirection.Up : BoardAccesor.BoardDirection.Down;

            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, direction, canMove));

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
            BoardAccesor.BoardDirection direction = (this.Turn == Turn.Black) ? BoardAccesor.BoardDirection.Up : BoardAccesor.BoardDirection.Down;

            if (BoardAccesor.IsHorizontalLine(seqIndex))
            {
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, direction, canMove));
            }
            else
            {
                canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, direction, canLineMove));
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
            BoardAccesor.BoardDirection direction = (this.Turn == Turn.Black) ? BoardAccesor.BoardDirection.Up : BoardAccesor.BoardDirection.Down;

            canMoveIndexs = canMoveIndexs.Union(this.GetOneDirectionCanMoveIndexs(seqIndex, direction, canMove));

            return canMoveIndexs;
        }
    }
}
