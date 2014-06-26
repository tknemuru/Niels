using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;

namespace Niels.Searchs
{
    /// <summary>
    /// ゲーム木ノードデバッグ用情報
    /// </summary>
    public class NodeDebugInfo
    {
        /// <summary>
        /// ノードID
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 親ノードID
        /// </summary>
        public int ParentNodeId { get; set; }

        /// <summary>
        /// 打ち手
        /// </summary>
        public uint Move { get; set; }

        /// <summary>
        /// 深さ
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 実行されているメソッド名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// ノードの処理中に発生したイベント
        /// </summary>
        public List<NodeEvent> Events { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="parentNodeId"></param>
        /// <param name="move"></param>
        public NodeDebugInfo(int nodeId, int parentNodeId, uint move, int depth, string methodName)
        {
            this.NodeId = nodeId;
            this.ParentNodeId = parentNodeId;
            this.Move = move;
            this.Depth = depth;
            this.MethodName = methodName;
            this.Events = new List<NodeEvent>();
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("▼---------------- NodeId:" + NodeId + " ----------------▼");
            sb.AppendLine(" ParentNodeId:" + this.ParentNodeId);
            sb.AppendLine(" Move:" + this.Move);
            sb.AppendLine(" Depth:" + this.Depth);
            sb.AppendLine(" MethodName:" + this.MethodName);
            sb.AppendLine(" Events:");
            foreach (NodeEvent nodeEvent in this.Events)
            {
                sb.AppendLine("  +--- EventId:" + nodeEvent.EventId + " ---+");
                sb.AppendLine("   Value:" + nodeEvent.Value);
                sb.AppendLine("   Alpha:" + nodeEvent.Alpha);
                sb.AppendLine("   Beta:" + nodeEvent.Beta);
                sb.AppendLine("   MinMax:" + nodeEvent.MinMax);
                sb.AppendLine("   BestMove:" + nodeEvent.BestMove);
                sb.AppendLine("   BestMove.FromIndex:" + nodeEvent.BestMove.FromIndex());
                sb.AppendLine("   BestValue:" + nodeEvent.BestValue);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// ノードイベントID
    /// </summary>
    public enum NodeEventId
    {
        /// <summary>
        /// 最善手の探索
        /// </summary>
        FirstNodeSearch,

        /// <summary>
        /// 幅１のNull Window 探索
        /// </summary>
        NullWindowExecute,

        /// <summary>
        /// Null Window 探索失敗
        /// </summary>
        NullWindowFailed,

        /// <summary>
        /// 最善手の更新
        /// </summary>
        BestMoveUpdate,

        /// <summary>
        /// α/β値更新
        /// </summary>
        AlphaBetaUpdate,

        /// <summary>
        /// 枝刈りスタート
        /// </summary>
        CutStart,

        /// <summary>
        /// 枝刈りされた子
        /// </summary>
        CutMe
    }

    /// <summary>
    /// ゲーム木探索で発生するイベント
    /// </summary>
    public class NodeEvent
    {
        /// <summary>
        /// イベントID
        /// </summary>
        public NodeEventId EventId { get; set; }

        /// <summary>
        /// 盤状態
        /// </summary>
        public BoardContext Context { get; set; }

        /// <summary>
        /// 評価値
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// α値
        /// </summary>
        public int Alpha { get; set; }

        /// <summary>
        /// β値
        /// </summary>
        public int Beta { get; set; }

        /// <summary>
        /// Min/Max
        /// </summary>
        public int MinMax { get; set; }

        /// <summary>
        /// 最善手
        /// </summary>
        public uint BestMove { get; set; }

        /// <summary>
        /// 最善手の評価値
        /// </summary>
        public int BestValue { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="bestMove"></param>
        /// <param name="bestValue"></param>
        public NodeEvent(NodeEventId id, BoardContext context, int value, int alpha, int beta, int minmax, uint bestMove, int bestValue)
        {
            this.EventId = id;
            this.Context = context;
            this.Value = value;
            this.Alpha = alpha;
            this.Beta = beta;
            this.MinMax = minmax;
            this.BestMove = bestMove;
            this.BestValue = bestValue;
        }
    }
}
