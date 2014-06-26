using System;

namespace Niels.Collections
{
    /// <summary>
    /// 手番
    /// </summary>
    public enum Turn
    {
        /// <summary>
        /// 先手
        /// </summary>
        Black = 0,

        /// <summary>
        /// 後手
        /// </summary>
        White = 1
    }

    /// <summary>
    /// 手番拡張
    /// </summary>
    public static class ExtensionTurn
    {
        /// <summary>
        /// ターン数
        /// </summary>
        public const int TurnCount = 2;

        /// <summary>
        /// 逆の手番の並び順を取得する
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static int GetOppositeIndex(this Turn turn)
        {
            return (turn == Turn.Black) ? 1 : 0;
        }

        /// <summary>
        /// 逆の手番を取得する
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static Turn GetOppositeTurn(this Turn turn)
        {
            return (turn == Turn.Black) ? Turn.White : Turn.Black;
        }
    }
}