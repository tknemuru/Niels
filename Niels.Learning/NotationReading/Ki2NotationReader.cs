using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Collections;
using Niels.Generates;
using Niels.Notation;
using Niels.Helper;
using Niels.Tests.TestHelper;
using System.Diagnostics;

namespace Niels.Learning.NotationReading
{
    /// <summary>
    /// .ki2形式の棋譜の読み込み機能を提供します。
    /// </summary>
    public class Ki2NotationReader : NotationBase
    {
        /// <summary>
        /// 筋を示す文字列
        /// </summary>
        private const string FilesSign = "０１２３４５６７８９";
        
        /// <summary>
        /// 段を示す文字列
        /// </summary>
        private const string RanksSign = "零一二三四五六七八九";

        /// <summary>
        /// 駒を示す文字列
        /// </summary>
        //private const string PiecesSign = "歩香桂銀金角飛玉と杏圭全馬龍";

        /// <summary>
        /// 駒を示す文字列リスト
        /// </summary>
        private static readonly string[] PiecesSign = 
        {
            "歩",
            "香",
            "桂",
            "銀",
            "金",
            "角",
            "飛",
            "玉",
            "と",
            "成香",
            "成桂",
            "成銀",
            "馬",
            "龍"
        };

        /// <summary>
        /// .ki2形式の棋譜ファイルの読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public NotationInformation Read(string filePath)
        {
            NotationInformation info = new NotationInformation();
            List<string> lines = FileHelper.ReadListData(filePath);
            string ki2Moves = string.Empty;
            string result = string.Empty;
            foreach (string line in lines)
            {
                if (line.Length < 2) { continue; }

                if (line.Substring(0, 1) == "▲")
                {
                    // ▲で始まっている行は指し手行とみなす
                    ki2Moves += line;
                }
                else if (line.Substring(0, 2) == "まで")
                {
                    // 「まで」で始まっている行は結果行とみなす
                    result = line;
                }
                else if (line.Substring(0, 3) == "手合割")
                {
                    // 手合割は現在サポート対象外
                    info.ReadResult = ReadResult.NoSupportedStartPotision;
                    return info;
                }
            }

            // 指し手の変換
            // TODO:Testプロジェクトの参照は無理がある
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            List<string> splitKi2Moves = this.SplitKi2Moves(ki2Moves);
            string moveLog = string.Empty;
            foreach (string ki2Move in splitKi2Moves)
            {
                moveLog += ki2Move + ",";
                FileHelper.WriteLine(moveLog);
                FileHelper.WriteLine(context.ToString());
                uint move = 0;

                try
                {
                    move = this.ConvertToNielsMove(ki2Move, context);
                }
                catch (Exception ex)
                {
                    if (ex.Data.Contains("result"))
                    {
                        info.ReadResult = (ReadResult)ex.Data["result"];
                    }
                    else
                    {
                        info.ReadResult = ReadResult.Undefined;
                    }
                    return info;
                }

                context.PutPiece(move);
                context.ChangeTurn();
                info.Moves.Add(move);
            }
            info.MoveCount = info.Moves.Count();

            // どちらが勝ったか
            if (result.Contains("先手"))
            {
                info.WinTurn = Turn.Black;
            }
            else if(result.Contains("後手"))
            {
                info.WinTurn = Turn.White;
            }
            else
            {
                Console.WriteLine(string.Format("勝ちのターンを特定できませんでした。引き分け？ {0}", result));
                info.ReadResult = ReadResult.UndefinedWinTurn;
                return info;
            }

            info.ReadResult = ReadResult.Success;
            return info;
        }

        /// <summary>
        /// .ki2形式の棋譜リストを１手ごとに分割します。
        /// </summary>
        /// <param name="ki2Moves"></param>
        /// <returns></returns>
        protected List<string> SplitKi2Moves(string ki2Moves)
        {
            ki2Moves = ki2Moves.Replace("▲", ",▲").Replace("△", ",△");
            List<string> splitMoves = (from m in ki2Moves.Split(',')
                                       where !string.IsNullOrEmpty(m.Trim())
                                       select m.Trim()).ToList();
            return splitMoves;
        }

        /// <summary>
        /// .ki2形式の指し手をNielsの指し手に変換します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected uint ConvertToNielsMove(string move, BoardContext context)
        {
            Turn turn = (move.Substring(0, 1) == "▲") ? Turn.Black : Turn.White;
            int file = 0;
            int rank = 0;
            if (move.Contains("同"))
            {
                // 前回と同じ位置
                uint lastMove = context.MovedRecord.Peek();
                file = this.GetFile(lastMove.ToBoard(), (int)lastMove.ToIndex());
                rank = this.GetRank(lastMove.ToBoard(), (int)lastMove.ToIndex());
            }
            else
            {
                file = FilesSign.IndexOf(move.Substring(1, 1));
                rank = RanksSign.IndexOf(move.Substring(2, 1));
            }
            Piece piece = Piece.Empty;
            for (int i = (PiecesSign.Length - 1); i >= 0; i--)
            {
                if (move.Contains(PiecesSign[i]))
                {
                    piece = ExtensionPiece.Pieces.ElementAt(i);
                    break;
                }
            }
            Debug.Assert(piece != Piece.Empty, string.Format("認識できない駒です。{0}", move));
            BoardType toBoard = this.GetBoardType(file, rank);
            int toIndex = this.GetIndex(file, rank);
            // どこで切れるかわからないからもう初めから全部見よう
            string extraInfo = move.Substring(0);

            if (extraInfo.Contains("打"))
            {
                // 「打」が含まれていたら持ち駒からの打ち手で決定
                // ※「打」が含まれていなくても打ち手の場合があるので注意。打ち手でも必ずここにくるわけではない。
                return Move.GetHandValueMove(piece, toBoard, (uint)toIndex, turn);
            }

            // 成る手かどうか
            bool isPromoteMove = extraInfo.Contains("成") && !extraInfo.Contains("不成") && !piece.IsPromoted();
            if (isPromoteMove)
            {
                piece = piece.Promote();
            }

            // 全ての合法手から候補の手を取得する
            var moves = MoveProvider.GetAllMoves(context, GenerateTarget.All)
                        .Where(m => m.PutPiece() == piece
                               && m.ToBoard() == toBoard
                               && m.ToIndex() == toIndex
                               && m.IsPromote() == isPromoteMove);
            if (moves.Count() < 1)
            {
                var ex = new ArgumentException("候補の合法手が見つかりませんでした。");
                ex.Data.Add("result", ReadResult.MoveNotFound);
                throw ex;
            }
            if (moves.Count() == 1)
            {
                // 候補が１件の場合はそれで決定
                return moves.ElementAt(0);
            }

            // この時点で2件以上存在する場合、持ち駒の打ち手が選択されることは有り得ないので、フィルタする。
            // （通常の指し手と競合している状態で、打ち手が選ばれたら「打」が付く）
            if (moves.Count() >= 2)
            {
                moves = moves.Where(m => !m.IsHandValueMove());
            }

            if (extraInfo.Contains("上"))
            {
                if (context.Turn == Turn.Black)
                {
                    moves = moves.Where(m => this.GetRank(m.FromBoard(), (int)m.FromIndex()) > this.GetRank(m.ToBoard(), (int)m.ToIndex()));
                }
                else
                {
                    moves = moves.Where(m => this.GetRank(m.FromBoard(), (int)m.FromIndex()) < this.GetRank(m.ToBoard(), (int)m.ToIndex()));
                }
            }
            if (extraInfo.Contains("引"))
            {
                if (context.Turn == Turn.Black)
                {
                    moves = moves.Where(m => this.GetRank(m.FromBoard(), (int)m.FromIndex()) < this.GetRank(m.ToBoard(), (int)m.ToIndex()));
                }
                else
                {
                    moves = moves.Where(m => this.GetRank(m.FromBoard(), (int)m.FromIndex()) > this.GetRank(m.ToBoard(), (int)m.ToIndex()));
                }
            }
            if (extraInfo.Contains("寄"))
            {
                moves = moves.Where(m => (this.GetFile(m.FromBoard(), (int)m.FromIndex()) != this.GetFile(m.ToBoard(), (int)m.ToIndex())) && (this.GetRank(m.FromBoard(), (int)m.FromIndex()) == this.GetRank(m.ToBoard(), (int)m.ToIndex())));
            }
            if (extraInfo.Contains("直"))
            {
                moves = moves.Where(m => this.GetFile(m.FromBoard(), (int)m.FromIndex()) == this.GetFile(m.ToBoard(), (int)m.ToIndex()));
            }

            if (moves.Count() == 1)
            {
                return moves.ElementAt(0);
            }
            if (moves.Count() < 1)
            {
                var ex = new ArgumentException("候補の合法手がなくなってしまいました。");
                ex.Data.Add("result", ReadResult.MoveNotFound);
                throw ex;
            }
            // ここで3件以上存在したらお手上げです
            if (moves.Count() > 2)
            {
                foreach (uint errorMove in moves)
                {
                    FileHelper.WriteLine(MoveForTest.ToDebugString(errorMove));
                }
                var ex = new ArgumentException("候補の合法手が多すぎて特定できません。");
                ex.Data.Add("result", ReadResult.TooManyMoves);
                throw ex;
            }
            int file0 = this.GetFile(moves.ElementAt(0).FromBoard(), (int)moves.ElementAt(0).FromIndex());
            int file1 = this.GetFile(moves.ElementAt(1).FromBoard(), (int)moves.ElementAt(1).FromIndex());
            if (file0 == file1)
            {
                var ex = new ArgumentException("同じ筋のため特定できません。");
                ex.Data.Add("result", ReadResult.SameFile);
                throw ex;
            }
            if (extraInfo.Contains("右"))
            {
                if (file0 < file1)
                {
                    return (context.Turn == Turn.Black) ? moves.ElementAt(0) : moves.ElementAt(1);
                }
                else
                {
                    return (context.Turn == Turn.Black) ? moves.ElementAt(1) : moves.ElementAt(0);
                }
            }
            if (extraInfo.Contains("左"))
            {
                if (file0 < file1)
                {
                    return (context.Turn == Turn.Black) ? moves.ElementAt(1) : moves.ElementAt(0);
                }
                else
                {
                    return (context.Turn == Turn.Black) ? moves.ElementAt(0) : moves.ElementAt(1);
                }
            }

            // ここに来たらもう降参です
            var lastEx = new ArgumentException("指し手を特定できません。");
            lastEx.Data.Add("result", ReadResult.UndefinedMove);
            throw lastEx;
        }
    }
}
