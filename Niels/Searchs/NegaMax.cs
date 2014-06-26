﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Helper;
using Niels.Evaluators;
using Niels.Diagnostics;
using Niels.Collections;
using Niels.Boards;
using Niels.Generates;

namespace Niels.Searchs
{
    /// <summary>
    /// NegaMax法を使用して探索を行います。
    /// </summary>
    public class NegaMax : NegaMaxBase
    {
        #region "定数"
        /// <summary>
        /// キーの初期値
        /// </summary>
        private const uint DEFAULT_KEY = 99;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 探索設定情報
        /// </summary>
        private SearchConfig Config { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaMax(SearchConfig config)
            : base(config.Depth)
        {
            this.Config = config;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected override bool IsLimit(int limit)
        {
            return (limit >= this.m_LimitDepth);
        }

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected override double GetEvaluate(BoardContext context, int nodeId)
        {
            StopWatchLogger.StartEventWatch("SearchBestPointer-GetEvaluate");
            double score = this.Config.Evaluator.Evaluate(context, nodeId);
            StopWatchLogger.StopEventWatch("SearchBestPointer-GetEvaluate");

            return (score * this.GetParity(context));
        }

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected override List<uint> GetAllLeaf(BoardContext context)
        {
            return MoveProvider.GetAllMoves(context).ToList();
        }

        /// <summary>
        /// ソートをする場合はTrueを返す
        /// </summary>
        /// <returns></returns>
        protected override bool IsOrdering(int depth)
        {
            return (depth <= 3);
        }

        /// <summary>
        /// ソートする
        /// </summary>
        /// <param name="allLeaf"></param>
        /// <returns></returns>
        protected override List<uint> MoveOrdering(List<uint> allLeaf)
        {          
            return this.Config.Order.MoveOrdering(allLeaf).ToList();
        }

        /// <summary>
        /// キーの初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected override uint GetDefaultKey()
        {
            return DEFAULT_KEY;
        }

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected override void SearchSetUp(BoardContext context, uint move)
        {
            // 手を指す
            context.PutPiece(move);

            // ターンをまわす
            context.ChangeTurn();
        }

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected override void SearchTearDown(BoardContext context)
        {
            // まわしたターンを戻す
            context.UndoChangeTurn();

            // 指した手を戻す
            context.UndoPutPiece();
        }

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected override void PassSetUp(BoardContext context)
        {
            // ターンをまわす
            context.ChangeTurn();
        }

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected override void PassTearDown(BoardContext context)
        {
            // まわしたターンを戻す
            context.UndoChangeTurn();
        }

        /// <summary>
        /// パリティ
        /// </summary>
        private double GetParity(BoardContext context)
        {
            return (context.Turn == Turn.Black) ? 1D : -1D;
        }
        #endregion
        #endregion
    }
}
