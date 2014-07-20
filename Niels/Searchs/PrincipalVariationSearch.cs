using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Generates;
using Niels.Evaluators;
using Niels.Orders;

namespace Niels.Searchs
{
    /// <summary>
    /// PVS探索
    /// </summary>
    public class PrincipalVariationSearch : ISearch
    {
        /// <summary>
        /// 選択した最善手
        /// </summary>
        public uint BestMove { get; private set; }

        /// <summary>
        /// 最善手の評価値
        /// </summary>
        public int BestValue { get; private set; }

        /// <summary>
        /// ノードデバッグ情報リスト
        /// </summary>
        public Dictionary<int, NodeDebugInfo> NodeDebugInfos { get; set; }

        /// <summary>
        /// 探索設定情報
        /// </summary>
        private SearchConfig Config { get; set; }

        /// <summary>
        /// 各ノード番号
        /// </summary>
        private int NodeId { get; set; }

        /// <summary>
        /// 次のノード番号
        /// </summary>
        private int NextNodeId
        {
            get
            {
                this.NodeId++;
                return this.NodeId;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="ev"></param>
        public PrincipalVariationSearch(SearchConfig config)
        {
            this.Config = config;
            this.NodeId = -1;
            this.NodeDebugInfos = new Dictionary<int, NodeDebugInfo>();
        }

        /// <summary>
        /// 指し手を取得する
        /// </summary>
        public uint GetMove(BoardContext context)
        {

#if DEBUG
            this.BestValue = this.SearchMax(context, this.Config.Depth, int.MinValue, int.MaxValue, -1);
#else
            this.BestValue = this.SearchMax(context, this.Config.Depth, int.MinValue, int.MaxValue);
#endif
            return this.BestMove;
        }

        /// <summary>
        /// 探索を実行する
        /// </summary>
        /// <param name="context"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
#if DEBUG
        protected int SearchMax(BoardContext context, int depth, int alpha, int beta, int parentNodeId)
#else
            protected int SearchMax(BoardContext context, int depth, int alpha, int beta)
#endif
        {
            // 評価を実行する地点に達した
            if (this.IsNeedEvaluate(depth))
            {
                return this.Config.Evaluator.Evaluate(context);
            }

            // 打ち手を生成
            uint[] moves = this.Config.MoveGenerate(context).ToArray();

            // 打ち手をソート
            this.Config.Order.MoveOrdering(moves, context);

            int max = int.MinValue;
            int value = int.MinValue;
            for (int i = 0; i < moves.Count(); i++)
            {
#if DEBUG
                this.CreateNodeDebugInfo(parentNodeId, moves[i], depth, "SearchMax");
                var nodeId = this.NodeId;
#endif
                // 前処理
                this.SearchSetUp(context, moves[i]);

                if (i == 0)
                {
                    //this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(nodeId, NodeEventId.FirstNodeSearch, context, value, alpha, beta, max, this.BestMove, BestValue);
                    value = this.SearchMin(context, depth - 1, alpha, beta, nodeId);
#else
                    value = this.SearchMin(context, depth - 1, alpha, beta);
#endif
                }
                else
                {
                    //this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(nodeId, NodeEventId.NullWindowExecute, context, value, alpha, beta, max, this.BestMove, BestValue);
                    value = this.SearchMin(context, depth - 1, alpha, alpha + 1, nodeId);
#else
                    value = this.SearchMin(context, depth - 1, alpha, alpha + 1);
#endif
                    if (alpha < value && value < beta)
                    {
                        this.SetBestMove(moves[i], depth);
#if DEBUG
                        this.AddNodeEvent(nodeId, NodeEventId.NullWindowFailed, context, value, alpha, beta, max, this.BestMove, BestValue);
                        value = this.SearchMin(context, depth - 1, value, beta, nodeId);
#else
                        value = this.SearchMin(context, depth - 1, value, beta);
#endif
                    }
                }

                // 後処理
                this.SearchTearDown(context, moves[i]);

                if (value > max)
                {
                    this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(nodeId, NodeEventId.BestMoveUpdate, context, value, alpha, beta, max, this.BestMove, BestValue);
#endif
                    max = value;
                    if (max > alpha)
                    {
#if DEBUG
                        this.AddNodeEvent(nodeId, NodeEventId.AlphaBetaUpdate, context, value, alpha, beta, max, this.BestMove, BestValue);
#endif
                        alpha = max;
                    }
                    if (alpha >= beta)
                    {
#if DEBUG
                        this.AddNodeEvent(nodeId, NodeEventId.CutStart, context, value, alpha, beta, max, this.BestMove, BestValue);
                        for (int s = i + 1; s < moves.Count(); s++)
                        {
                            this.CreateNodeDebugInfo(parentNodeId, moves[s], depth, "SearchMax");
                            this.AddNodeEvent(this.NodeId, NodeEventId.CutMe, context, value, alpha, beta, max, this.BestMove, BestValue);
                        }
#endif
                        break;
                    }
                }
            }

            return max;
        }

        /// <summary>
        /// 探索を実行する
        /// </summary>
        /// <param name="context"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
#if DEBUG
        protected int SearchMin(BoardContext context, int depth, int alpha, int beta, int parentNodeId)
#else
            protected int SearchMin(BoardContext context, int depth, int alpha, int beta)
#endif
        {
            // 評価を実行する地点に達した
            if (this.IsNeedEvaluate(depth))
            {
                return this.Config.Evaluator.Evaluate(context);
            }

            // 打ち手を生成
            uint[] moves = this.Config.MoveGenerate(context).ToArray();

            // 打ち手をソート
            this.Config.Order.MoveOrdering(moves, context);

            int min = int.MaxValue;
            int value = int.MaxValue;
            for (int i = 0; i < moves.Count(); i++ )
            {
#if DEBUG
                this.CreateNodeDebugInfo(parentNodeId, moves[i], depth, "SearchMin");
#endif

                // 前処理
                this.SearchSetUp(context, moves[i]);

                if (i == 0)
                {
                    //this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(this.NodeId, NodeEventId.FirstNodeSearch, context, value, alpha, beta, min, this.BestMove, BestValue);
                    value = this.SearchMax(context, depth - 1, alpha, beta, this.NodeId);
#else
                    value = this.SearchMax(context, depth - 1, alpha, beta);
#endif
                }
                else
                {
                    //this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(this.NodeId, NodeEventId.NullWindowExecute, context, value, alpha, beta, min, this.BestMove, BestValue);
                    value = this.SearchMax(context, depth - 1, beta - 1, beta, this.NodeId);
#else
                    value = this.SearchMax(context, depth - 1, beta - 1, beta);
#endif
                    if (alpha < value && value < beta)
                    {
                        this.SetBestMove(moves[i], depth);
#if DEBUG
                        this.AddNodeEvent(this.NodeId, NodeEventId.NullWindowFailed, context, value, alpha, beta, min, this.BestMove, BestValue);
                        value = this.SearchMax(context, depth - 1, alpha, value, this.NodeId);
#else
                        value = this.SearchMax(context, depth - 1, alpha, value);
#endif
                    }
                }

                // 後処理
                this.SearchTearDown(context, moves[i]);

                if (value < min)
                {
                    this.SetBestMove(moves[i], depth);
#if DEBUG
                    this.AddNodeEvent(this.NodeId, NodeEventId.BestMoveUpdate, context, value, alpha, beta, min, this.BestMove, BestValue);
#endif
                    min = value;
                    if (min < beta)
                    {
#if DEBUG
                        this.AddNodeEvent(this.NodeId, NodeEventId.AlphaBetaUpdate, context, value, alpha, beta, min, this.BestMove, BestValue);
#endif
                        beta = min;
                    }
                    if (alpha >= beta)
                    {
#if DEBUG
                        this.AddNodeEvent(this.NodeId, NodeEventId.CutStart, context, value, alpha, beta, min, this.BestMove, BestValue);
                        for (int s = i + 1; s < moves.Count(); s++)
                        {
                            this.CreateNodeDebugInfo(parentNodeId, moves[s], depth, "SearchMin");
                            this.AddNodeEvent(this.NodeId, NodeEventId.CutMe, context, value, alpha, beta, min, this.BestMove, BestValue);
                        }
#endif
                        break;
                    }
                }
            }

            return min;
        }

        /// <summary>
        /// 評価の実行が必要かどうかを取得する
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        protected virtual bool IsNeedEvaluate(int depth)
        {
            return (depth == 0);
        }

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected virtual void SearchSetUp(BoardContext context, uint move)
        {
            // 手を指す
            context.PutPiece(move);

            // ターンをまわす
            context.ChangeTurn();
        }

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected virtual void SearchTearDown(BoardContext context, uint move)
        {
            // まわしたターンを戻す
            context.UndoChangeTurn();

            // 指した手を戻す
            context.UndoPutPiece();
        }

        /// <summary>
        /// 最善手をセットする
        /// </summary>
        /// <param name="leaf"></param>
        private void SetBestMove(uint move, int depth)
        {
            if (depth == this.Config.Depth)
            {
                this.BestMove = move;
            }
        }



#if DEBUG
        /// <summary>
        /// デバッグ用ノード情報を作成する
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="move"></param>
        /// <param name="isCut"></param>
        /// <returns></returns>
        private void CreateNodeDebugInfo(int parentNodeId, uint move, int depth, string methodName)
        {
            var nodeInfo = new NodeDebugInfo(this.NextNodeId, parentNodeId, move, depth, methodName);
            this.NodeDebugInfos.Add(nodeInfo.NodeId, nodeInfo);
        }

        /// <summary>
        /// ノードイベントを追加する
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="move"></param>
        /// <param name="isCut"></param>
        /// <returns></returns>
        private void AddNodeEvent(int nodeId
                                  , NodeEventId eventId
                                  , BoardContext context
                                  , int value
                                  , int alpha
                                  , int beta
                                  , int minmax
                                  , uint bestMove
                                  , int bestValue)
        {
            NodeEvent nodeEvent = new NodeEvent(eventId, context, value, alpha, beta, minmax, bestMove, bestValue);
            this.NodeDebugInfos[nodeId].Events.Add(nodeEvent);
        }
#endif
    }
}
