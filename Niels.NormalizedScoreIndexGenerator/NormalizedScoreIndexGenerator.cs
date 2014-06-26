using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Helper;
using Niels.Collections;
using Niels.Evaluators;

namespace Niels.NormalizedScoreIndexGenerating
{
    /// <summary>
    /// 正規化スコアインデックスを生成します。
    /// </summary>
    public class NormalizedScoreIndexGenerator
    {
        /// <summary>
        /// インデックスを構成するマス目数
        /// </summary>
        private const int UnitCount = 4;

        /// <summary>
        /// インデックスを構成する駒数
        /// </summary>
        private const int PieceCount = 17;

        /// <summary>
        /// スコアインデックスの最大値
        /// </summary>
        private static readonly int MaxScoreIndex = ((int)Math.Pow(PieceCount, UnitCount) - 1);

        /// <summary>
        /// 駒に割り当てられたインデックス辞書
        /// </summary>
        private static readonly Dictionary<int, TurnPiece> PieceIndexs = InitializePieceIndex();

        /// <summary>
        /// 駒に割り当てられたインデックス辞書を初期化します。
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, TurnPiece> InitializePieceIndex()
        {
            Dictionary<int, TurnPiece> pieceIndexs = new Dictionary<int, TurnPiece>();
            pieceIndexs.Add(0, TurnPiece.Empty);
            pieceIndexs.Add(1, TurnPiece.BlackPawn);
            pieceIndexs.Add(2, TurnPiece.BlackLaunce);
            pieceIndexs.Add(3, TurnPiece.BlackKnight);
            pieceIndexs.Add(4, TurnPiece.BlackSilver);
            pieceIndexs.Add(5, TurnPiece.BlackGold);
            pieceIndexs.Add(6, TurnPiece.BlackBishop);
            pieceIndexs.Add(7, TurnPiece.BlackRook);
            pieceIndexs.Add(8, TurnPiece.BlackKing);
            pieceIndexs.Add(9, TurnPiece.WhitePawn);
            pieceIndexs.Add(10, TurnPiece.WhiteLaunce);
            pieceIndexs.Add(11, TurnPiece.WhiteKnight);
            pieceIndexs.Add(12, TurnPiece.WhiteSilver);
            pieceIndexs.Add(13, TurnPiece.WhiteGold);
            pieceIndexs.Add(14, TurnPiece.WhiteBishop);
            pieceIndexs.Add(15, TurnPiece.WhiteRook);
            pieceIndexs.Add(16, TurnPiece.WhiteKing);
            return pieceIndexs;
        }

        /// <summary>
        /// 正規化スコアインデックスを生成します。
        /// </summary>
        public Dictionary<int, int> Generate()
        {
            Dictionary<int, int> normalizedScoreIndexs = new Dictionary<int, int>();

            for (int index = 0; index <= MaxScoreIndex; index++)
            {
                int normalizedIndex = index;
                if (normalizedScoreIndexs.ContainsKey(normalizedIndex)) { continue; }

                // 自分自身
                normalizedScoreIndexs.Add(normalizedIndex, index);

                // リバース
                normalizedIndex = Reverse(index);
                if (!normalizedScoreIndexs.ContainsKey(normalizedIndex))
                {
                    normalizedScoreIndexs.Add(normalizedIndex, index);
                }

                // ターン逆
                normalizedIndex = ChangeTurn(index);
                if (!normalizedScoreIndexs.ContainsKey(normalizedIndex))
                {
                    normalizedScoreIndexs.Add(normalizedIndex, -index);
                }

                // ターン逆＆リバース
                normalizedIndex = ChangeTurnAndReverse(index);
                if (!normalizedScoreIndexs.ContainsKey(normalizedIndex))
                {
                    normalizedScoreIndexs.Add(normalizedIndex, -index);
                }
            }

            return normalizedScoreIndexs;
        }

        /// <summary>
        /// インデックスの順序を逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static int Reverse(int index)
        {
            string cnvIndexStr = RadixConvertHelper.ToString(index, PieceCount, UnitCount, true);
            string reverseIndexStr = string.Empty;
            foreach (char cr in cnvIndexStr.ToCharArray().Reverse())
            {
                reverseIndexStr += cr;
            }
            int newIndex = RadixConvertHelper.ToInt32(reverseIndexStr, PieceCount);
            return newIndex;
        }

        /// <summary>
        /// 駒のターンを逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static int ChangeTurn(int index)
        {
            string cnvIndexStr = RadixConvertHelper.ToString(index, PieceCount, UnitCount, true);
            string changeIndex = string.Empty;
            foreach (char crPiece in cnvIndexStr.ToCharArray())
            {
                TurnPiece changePiece = PieceIndexs[RadixConvertHelper.ToInt32(crPiece.ToString(), PieceCount)].ChangeTurn();
                changeIndex += RadixConvertHelper.ToString(SequencePositionFeatureVector.GetPieceIndex(changePiece), PieceCount, true);
            }
            int newIndex = RadixConvertHelper.ToInt32(changeIndex, PieceCount);
            return newIndex;
        }

        /// <summary>
        /// 駒のターンを逆にしてインデックスの順序を逆にします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static int ChangeTurnAndReverse(int index)
        {
            return Reverse(ChangeTurn(index));
        }
    }
}
