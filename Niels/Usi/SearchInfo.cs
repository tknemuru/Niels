using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Niels.Notation;
using Niels.Helper;

namespace Niels.Usi
{
    /// <summary>
    /// エンジンの探索に関する情報を保持します。
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// 探索が完了したかどうかを示します。
        /// </summary>
        public bool IsSearchEnd { get; set; }

        /// <summary>
        /// 現在思考中の手の（基本の）探索深さを返します。
        /// </summary>
        public int? Depth { get; set; }

        /// <summary>
        /// <para>現在、選択的に読んでいる手の探索深さを返します。</para>
        /// <para>seldepthを使うときは、必ずその前でdepthを使って基本深さを示す必要があります。</para>
        /// </summary>
        public int? SelctingDepth { get; set; }

        /// <summary>
        /// <para>思考を開始してから経過した時間を返します（単位はミリ秒）。</para>
        /// <para>これはpvと一緒に返す必要があります。</para>
        /// </summary>
        public long Time
        {
            get
            {
                if(!this.Stopwatch.IsRunning)
                {
                    return 0;
                }
                return this.Stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// 思考開始から探索したノード数を返します。これは定期的に返す必要があります。
        /// </summary>
        public int Nodes { get; set; }

        /// <summary>
        /// <para>現在の読み筋を返します。</para>
        /// <para>pvを使う場合、infoのあとに書くサブコマンドの中で最後に書くようにして下さい。</para>
        /// <para>stringとの同時使用はできません。</para>
        /// </summary>
        public string PrincipalVariation
        {
            get
            {
                return String.Join(" ", this.PrincipalVariationLog);
            }
        }

        /// <summary>
        /// エンジンによる現在の評価値を返します。
        /// </summary>
        public int? ScoreCentiPawn { get; set; }

        /// <summary>
        /// 詰み手数を返します。
        /// </summary>
        public int? ScoreMate { get; set; }

        /// <summary>
        /// 現在思考中の手を返します。（思考開始局面から最初に指す手です。）
        /// </summary>
        public uint? CurrentMove { get; set; }

        /// <summary>
        /// <para>エンジンが現在使用しているハッシュの使用率を返します。単位はパーミル（全体を１０００とした値）になります。</para>
        /// <para>このコマンドは定期的に返す必要があります。</para>
        /// </summary>
        public int? Hashfull { get; set; }

        /// <summary>
        /// １秒あたりの探索局面数を返します。これは定期的に返す必要があります。
        /// </summary>
        public int Nps
        {
            get
            {
                if (this.Time <= 0 || this.Nodes <= 0) { return 0; }
                return (int)(((double)this.Nodes / (double)this.Time) * 1000D);
            }
        }

        /// <summary>
        /// GUIに表示させたい任意の文字列を返します。GUIはstringサブコマンド以降の文字列を全てそのまま表示します。
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// 経過時間を計測するストップウォッチ
        /// </summary>
        public Stopwatch Stopwatch { get; private set; }

        /// <summary>
        /// 読み筋のログ
        /// </summary>
        private Stack<string> PrincipalVariationLog { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchInfo()
        {
            this.Stopwatch = new Stopwatch();
            this.PrincipalVariationLog = new Stack<string>();
            this.Nodes = 0;
            this.IsSearchEnd = false;
        }

        /// <summary>
        /// 読み筋のログを追加します。
        /// </summary>
        /// <param name="move"></param>
        public void AddPvLog(uint move)
        {
            this.PrincipalVariationLog.Push(SfenNotation.ConvertToSfenMove(move));
        }

        /// <summary>
        /// 読み筋の最後のログを削除します。
        /// </summary>
        public void RemoveLastPvLog()
        {
            this.PrincipalVariationLog.Pop();
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();

            try
            {

                info.Append("info");
                info.Append(this.Depth.HasValue ? string.Format(" {0} {1}", "depth", this.Depth) : string.Empty);
                info.Append(this.Depth.HasValue && this.SelctingDepth.HasValue ? string.Format(" {0} {1}", "seldepth", this.SelctingDepth) : string.Empty);
                info.Append(string.Format(" {0} {1}", "time", this.Time));
                info.Append(this.Nodes > 0 ? string.Format(" {0} {1}", "nodes", this.Nodes) : string.Empty);
                info.Append(this.ScoreCentiPawn.HasValue ? string.Format(" {0} {1}", "score cp", this.ScoreCentiPawn) : string.Empty);
                info.Append(this.ScoreMate.HasValue ? string.Format(" {0} {1}", "score mate", this.ScoreMate) : string.Empty);
                info.Append(this.CurrentMove.HasValue ? string.Format(" {0} {1}", "currmove", SfenNotation.ConvertToSfenMove(this.CurrentMove.Value)) : string.Empty);
                info.Append(this.Hashfull.HasValue ? string.Format(" {0} {1}", "hashfull", this.Hashfull) : string.Empty);
                info.Append(string.Format(" {0} {1}", "nps", this.Nps));
                info.Append(!string.IsNullOrEmpty(this.AdditionalInfo) && string.IsNullOrEmpty(this.PrincipalVariation) ? string.Format(" {0} {1}", "string", this.AdditionalInfo) : string.Empty);

                // pvは必ず最後
                info.Append(!string.IsNullOrEmpty(this.PrincipalVariation) && string.IsNullOrEmpty(this.AdditionalInfo) ? string.Format(" {0} {1}", "pv", this.PrincipalVariation) : string.Empty);
            }
            catch (Exception ex)
            {
                FileHelper.WriteLine(ex.Message);
                FileHelper.WriteLine(ex.StackTrace);
            }

            return info.ToString();
        }
    }
}
