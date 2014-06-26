using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;

namespace Niels.Learning.NotationReading
{
    /// <summary>
    /// 棋譜情報
    /// </summary>
    public class NotationInformation
    {
        /// <summary>
        /// 棋譜
        /// </summary>
        public List<uint> Moves { get; set; }

        /// <summary>
        /// 指し数
        /// </summary>
        public int MoveCount { get; set; }

        /// <summary>
        /// 勝ちのターン
        /// </summary>
        public Turn WinTurn { get; set; }

        /// <summary>
        /// 読み込み結果
        /// </summary>
        public ReadResult ReadResult { get; set; }

        /// <summary>
        /// コンストクラタ
        /// </summary>
        public NotationInformation()
        {
            this.Moves = new List<uint>();
            this.ReadResult = ReadResult.Undefined;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", this.MoveCount, this.WinTurn, string.Join(",", this.Moves));
        }
    }
}
