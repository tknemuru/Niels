using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;

namespace Niels.Generates
{
    /// <summary>
    /// 指し手を生成する機能を提供します。
    /// </summary>
    public abstract class MoveGenerator
    {
        /// <summary>
        /// 対象インデックスのデフォルト値
        /// </summary>
        private static readonly int[][] DefaultTargetIndexs = InitializeDefaultTargetIndexs();

        /// <summary>
        /// 空の対象インデックス
        /// </summary>
        private static readonly int[] EmptyTargetIndexs = new int[0];

        /// <summary>
        /// 指し手を生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<uint> Generate(BoardContext context, Piece piece)
        {
            return this.Generate(context, piece, DefaultTargetIndexs);
        }

        /// <summary>
        /// 指定した位置への指し手を生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="piece"></param>
        /// <param name="boardType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<uint> Generate(BoardContext context, Piece piece, BoardType boardType, int index)
        {
            // 対象インデックスの作成
            int[][] targetIndexs = new int[Board.BoardTypeCount][];
            targetIndexs[(int)BoardType.Main] = EmptyTargetIndexs;
            targetIndexs[(int)BoardType.SubRight] = EmptyTargetIndexs;
            targetIndexs[(int)BoardType.SubBottom] = EmptyTargetIndexs;
            targetIndexs[(int)boardType] = new int[] { index };

            // 指し手の生成
            return this.Generate(context, piece, targetIndexs);
        }

        /// <summary>
        /// 打ち手を生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="piece"></param>
        /// <param name="targetIndexs"></param>
        /// <returns></returns>
        protected abstract IEnumerable<uint> Generate(BoardContext context, Piece piece, int[][] targetIndexs);

        /// <summary>
        /// 対象インデックスのデフォルト値を初期化します。
        /// </summary>
        /// <returns></returns>
        private static int[][] InitializeDefaultTargetIndexs()
        {
            int[][] targetIndexs = new int[Board.BoardTypeCount][];
            targetIndexs[(int)BoardType.Main] = BoardProvider.GetMain().UsingIndexs;
            targetIndexs[(int)BoardType.SubRight] = BoardProvider.GetSubRight().UsingIndexs;
            targetIndexs[(int)BoardType.SubBottom] = BoardProvider.GetSubBottom().UsingIndexs;
            return targetIndexs;
        }
    }
}
