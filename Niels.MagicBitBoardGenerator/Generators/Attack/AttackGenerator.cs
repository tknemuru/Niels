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
    /// 利き生成の機能を提供します。
    /// </summary>
    internal abstract class AttackGenerator
    {
        /// <summary>
        /// 利きを生成します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="boardState"></param>
        /// <returns></returns>
        internal abstract IEnumerable<uint> Generate(int index, Dictionary<int, bool> pattern, Piece targetPiece);

        /// <summary>
        /// 指定した方向に進む利きを取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pattern"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected List<uint> GetOneDirectionMoves(int index, Dictionary<int, bool> pattern, BoardAccesor.BoardDirection direction, Piece targetPiece)
        {
            uint orgIndex = (uint)BoardAccesor.GetOriginalIndex(index);
            BoardType boardType = BoardAccesor.GetBoardType(index);
            int movedIndex = index + (int)direction;
            List<uint> moves = new List<uint>();

            // 最初から盤外にいっていたら処理終了
            if(BoardAccesor.IsWall(movedIndex))
            {
                return moves;
            }

            while (true)
            {
                uint orgMovedIndex = (uint)BoardAccesor.GetOriginalIndex(movedIndex);
                BoardType movedBoardType = BoardAccesor.GetBoardType(movedIndex);

                if ((pattern.ContainsKey(movedIndex) && pattern[movedIndex]) || !pattern.ContainsKey(movedIndex))
                {
                    // とりあえず歩を取るということにしておく
                    moves.Add(Move.GetMove(targetPiece, boardType, orgIndex, movedBoardType, orgMovedIndex, Turn.Black, Piece.Pawn));

                    // 移動先に駒がある場合、端まで到達した場合は処理終了
                    break;
                }
                else
                {
                    moves.Add(Move.GetMove(targetPiece, boardType, orgIndex, movedBoardType, orgMovedIndex, Turn.Black));
                }

                movedIndex += (int)direction;
            }

            return moves;
        }
    }
}
