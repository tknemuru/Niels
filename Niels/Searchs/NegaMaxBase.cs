using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niels.Diagnostics;
using Niels.Helper;
using Niels.Boards;
using Niels.Collections;
using System.Diagnostics;

namespace Niels.Searchs
{
    /// <summary>
    /// NegaMax法を使用して探索を行います。
    /// </summary>
    public abstract class NegaMaxBase : Searcher
    {
        /// <summary>
        /// <para>初期アルファ値</para>
        /// </summary>
        //protected const int DEFAULT_ALPHA = int.MinValue;
        protected const int DEFAULT_ALPHA = -1000000;

        /// <summary>
        /// <para>初期ベータ値</para>
        /// </summary>
        //protected const int DEFAULT_BETA = int.MaxValue;
        protected const int DEFAULT_BETA = 1000000;

        /// <summary>
        /// 返却するキー
        /// </summary>
        protected uint m_Key;

        /// <summary>
        /// 評価値
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaMaxBase(int limit)
            : base()
        {
            this.SearchInfo.Depth = limit;
            this.m_Key = this.GetDefaultKey();
            this.Value = DEFAULT_ALPHA;
        }

        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        protected override uint InnerGetMove(BoardContext context)
        {
            StopWatchLogger.StartEventWatch("NegaMaxBase.SearchBestValue");
            this.Value = this.SearchBestValue(context ,1 , DEFAULT_ALPHA, DEFAULT_BETA);
            FileHelper.WriteLine("BestScore:" + this.Value);
            StopWatchLogger.StopEventWatch("NegaMaxBase.SearchBestValue");
            StopWatchLogger.WriteAllEventTimes("./log/eventtime.txt");
            return this.m_Key;
        }

        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        protected int SearchBestValue(BoardContext context, int depth, int alpha, int beta)
        {
            // 現在の深さを記録
            this.SearchInfo.SelctingDepth = depth;

            // 検索したノード数を記録
            this.SearchInfo.Nodes++;

            // 深さ制限に達した
            if (this.IsLimit(depth)) { return this.GetEvaluate(context); }

            // 可能な手をすべて生成
            var leafList = this.GetAllLeaf(context);

            int maxKeyValue = DEFAULT_ALPHA;
            if (leafList.Count() > 0)
            {
                // ソート
                StopWatchLogger.StartEventWatch("MoveOrdering");
                if (this.IsOrdering(depth)) { leafList = this.MoveOrdering(leafList, context); }
                StopWatchLogger.StopEventWatch("MoveOrdering");

                foreach (uint leaf in leafList)
                {
                    // 読み筋を記録
                    this.SearchInfo.AddPvLog(leaf);

                    // 現在思考中の手を記録
                    if (depth == 1)
                    {
                        this.SearchInfo.CurrentMove = leaf;
                    }

                    // 前処理
                    StopWatchLogger.StartEventWatch("SearchSetUp");
                    this.SearchSetUp(context, leaf);
                    StopWatchLogger.StopEventWatch("SearchSetUp");

                    int value = this.SearchBestValue(context, depth + 1, -beta, -alpha) * -1;

                    // 後処理
                    StopWatchLogger.StartEventWatch("SearchTearDown");
                    this.SearchTearDown(context);
                    StopWatchLogger.StopEventWatch("SearchTearDown");

                    // 記録した読み筋を削除
                    this.SearchInfo.RemoveLastPvLog();

                    // ベータ刈り
                    if (value >= beta)
                    {
                        this.SetKey(leaf, depth);
                        return value;
                    }

                    if (value > maxKeyValue)
                    {
                        // より良い手が見つかった
                        this.SetKey(leaf, depth);
                        maxKeyValue = value;
                        // α値の更新
                        alpha = MathHelper.Max<int>(alpha, maxKeyValue);

                        // 最善の評価値を記録
                        this.SearchInfo.ScoreCentiPawn = maxKeyValue;
                    }
                }
            }
            else
            {
                // ▼パスの場合▼
                // 前処理
                this.PassSetUp(context);

                maxKeyValue = this.SearchBestValue(context ,depth + 1, -beta, -alpha) * -1;

                // 後処理
                this.PassTearDown(context);
            }

            Debug.Assert(((maxKeyValue != DEFAULT_ALPHA) && (maxKeyValue != DEFAULT_BETA)), "デフォルト値のまま返そうとしています。");
            return maxKeyValue;
        }

        /// <summary>
        /// 返却するキーをセットする
        /// </summary>
        /// <param name="leaf"></param>
        private void SetKey(uint leaf, int depth)
        {
            if (depth == 1)
            {
                this.m_Key = leaf;
            }
        }

        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected abstract bool IsLimit(int limit);

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract int GetEvaluate(BoardContext context);

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<uint> GetAllLeaf(BoardContext context);

        /// <summary>
        /// ソートをする場合はTrueを返す
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsOrdering(int depth);

        /// <summary>
        /// ソートする
        /// </summary>
        /// <param name="allLeaf"></param>
        /// <returns></returns>
        protected abstract IEnumerable<uint> MoveOrdering(IEnumerable<uint> allLeaf, BoardContext context);

        /// <summary>
        /// キーの初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract uint GetDefaultKey();

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected abstract void SearchSetUp(BoardContext context, uint leaf);

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected abstract void SearchTearDown(BoardContext context);

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected abstract void PassSetUp(BoardContext context);

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected abstract void PassTearDown(BoardContext context);
    }
}
