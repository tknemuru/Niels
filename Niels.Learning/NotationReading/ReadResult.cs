using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Learning.NotationReading
{
    /// <summary>
    /// 読み込み結果
    /// </summary>
    public enum ReadResult
    {
        /// <summary>
        /// 不定
        /// </summary>
        Undefined,

        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// サポート外の手合割
        /// </summary>
        NoSupportedStartPotision,

        /// <summary>
        /// 勝ったターンが未確定
        /// </summary>
        UndefinedWinTurn,

        /// <summary>
        /// 候補の合法手が見つからない
        /// </summary>
        MoveNotFound,

        /// <summary>
        /// 候補の合法手が多すぎる
        /// </summary>
        TooManyMoves,

        /// <summary>
        /// 同じ筋のため特定不可能
        /// </summary>
        SameFile,

        /// <summary>
        /// 合法手が特定できない
        /// </summary>
        UndefinedMove,
    }
}
