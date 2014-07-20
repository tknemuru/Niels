using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;

namespace Niels.Evaluators
{
    /// <summary>
    /// 評価値をイテレータ形式で返します。
    /// </summary>
    public class IteratorEvaluator : Evaluator
    {
        /// <summary>
        /// 評価値のソート順
        /// </summary>
        public enum IteratorOrder
        {
            Asc,
            Desc
        }

        /// <summary>
        /// 降順の開始最大評価値
        /// </summary>
        public const int DescMaxValue = 100000;

        /// <summary>
        /// 評価値
        /// </summary>
        private int ItertorValue { get; set; }

        /// <summary>
        /// ソート順
        /// </summary>
        private IteratorOrder Order { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="order"></param>
        public IteratorEvaluator(IteratorOrder order)
        {
            this.Order = order;
            this.ItertorValue = (order == IteratorOrder.Asc) ? 0 : DescMaxValue;
        }

        /// <summary>
        /// 評価値を取得します。
        /// </summary>
        /// <returns></returns>
        public override int Evaluate(BoardContext context)
        {
            if (this.Order == IteratorOrder.Asc)
            {
                this.ItertorValue++;
            }
            else
            {
                this.ItertorValue--;
            }
            return this.ItertorValue;
        }
    }
}
