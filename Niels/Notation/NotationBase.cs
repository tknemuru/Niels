using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using System.Diagnostics;
using System.ComponentModel;

namespace Niels.Notation
{
    /// <summary>
    /// 棋譜に関する処理を行います。
    /// </summary>
    public abstract class NotationBase
    {
        /// <summary>
        /// 指し手から盤種別を取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected static BoardType GetBoardType(int file, int rank)
        {
            Debug.Assert((file >= 1 && file <= 9 && rank >= 1 && rank <= 9), string.Format("筋(file)、段(rank)が不正です。　file:{0} rank{1}", file, rank));

            if (file == 1)
            {
                return BoardType.SubRight;
            }
            else if (rank == 9)
            {
                return BoardType.SubBottom;
            }
            else
            {
                return BoardType.Main;
            }
        }

        /// <summary>
        /// 指し手からインデックスを取得します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected static int GetIndex(int file, int rank)
        {
            Debug.Assert((file >= 1 && file <= 9 && rank >= 1 && rank <= 9), string.Format("筋(file)、段(rank)が不正です。　file:{0} rank{1}", file, rank));

            BoardType boardType = GetBoardType(file, rank);

            switch (boardType)
            {
                case BoardType.Main :
                    return (((file - 2) * 8) + (rank - 1));
                case BoardType.SubBottom :
                    return (((file - 2) * 8) + (rank - 2));
                case BoardType.SubRight :
                    return (rank - 1);
                default :
                    throw new InvalidEnumArgumentException("不正な盤種別です。");
            }
        }

        /// <summary>
        /// 筋を取得します(1～9)
        /// </summary>
        /// <returns></returns>
        protected static int GetFile(BoardType boardType, int index)
        {
            int baseFile = (index / 8);
            if ((boardType == BoardType.Main) || (boardType == BoardType.SubBottom))
            {
                return (baseFile + 2);
            }
            else if (boardType == BoardType.SubRight && index == 8)
            {
                return baseFile;
            }
            else
            {
                return (baseFile + 1);
            }
        }

        /// <summary>
        /// 段を取得します(1～9)
        /// </summary>
        /// <param name="boardType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static int GetRank(BoardType boardType, int index)
        {
            if ((boardType == BoardType.Main) || (boardType == BoardType.SubRight && index != 8))
            {
                return ((index % 8) + 1);
            }
            else if (boardType == BoardType.SubRight && index == 8)
            {
                return (index + 1);
            }
            else
            {
                return ((index % 8) + 2);
            }
        }
    }
}
