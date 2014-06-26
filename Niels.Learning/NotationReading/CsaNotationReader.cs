using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.Boards;
using Niels.Generates;
using Niels.Notation;
using Niels.Helper;

namespace Niels.Learning.NotationReading
{
    /// <summary>
    /// CSA形式の棋譜読み込み機能を提供します。
    /// </summary>
    public class CsaNotationReader : NotationBase, INotationReader
    {
        /// <summary>
        /// サマリー行の内容
        /// </summary>
        private enum Summary
        {
            Id,
            DateTime,
            BlackPlayer,
            WhitePlayer,
            WinTurn,
            MoveCount,
            Strategy
        }

        /// <summary>
        /// 指し手セットの内容
        /// </summary>
        private enum MoveUnit
        {
            FromIndex,
            ToIndex,
            PutPiece
        }

        /// <summary>
        /// 駒変換辞書
        /// </summary>
        private static readonly Dictionary<string, Piece> Pieces = InitializePieces();

        /// <summary>
        /// 駒変換辞書を取得します。
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Piece> InitializePieces()
        {
            Dictionary<string, Piece> pieces = new Dictionary<string, Piece>();
            pieces.Add("FU", Piece.Pawn);
            pieces.Add("KY", Piece.Launce);
            pieces.Add("KE", Piece.Knight);
            pieces.Add("GI", Piece.Silver);
            pieces.Add("KI", Piece.Gold);
            pieces.Add("KA", Piece.Bishop);
            pieces.Add("HI", Piece.Rook);
            pieces.Add("OU", Piece.King);
            pieces.Add("TO", Piece.PawnPromoted);
            pieces.Add("NY", Piece.LauncePromoted);
            pieces.Add("NK", Piece.KnightPromoted);
            pieces.Add("NG", Piece.SilverPromoted);
            pieces.Add("UM", Piece.Horse);
            pieces.Add("RY", Piece.Dragon);
            return pieces;
        }

        /// <summary>
        /// 棋譜を読み込みます。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IEnumerable<NotationInformation> Read(string filePath)
        {
            List<string> lines = FileHelper.ReadListData(filePath);
            NotationInformation info = null;

            for (int i = 0; i < lines.Count(); i++)
            {
                
                if ((i % 2) == 0)
                {
                    // サマリー行
                    info = new NotationInformation();
                    var summary = lines[i].Split(' ');
                    info.WinTurn = (summary[(int)Summary.WinTurn] == "1") ? Turn.Black : Turn.White;
                    info.MoveCount = int.Parse(summary[(int)Summary.MoveCount]);
                }
                else
                {
                    // 棋譜行
                    BoardContext context = new BoardContext();
                    context.SetDefaultStartPosition();
                    var csaMoves = StringHelper.ToListSplitCount(lines[i], 6).ToList();
                    foreach(var csaMove in csaMoves)
                    {
                        uint move = this.ConvertToNielsMove(csaMove, context);
                        info.Moves.Add(move);
                        context.PutPiece(move);
                        context.ChangeTurn();
                    }
                    info.ReadResult = ReadResult.Success;
                    yield return info;
                }
            }
        }

        /// <summary>
        /// Nielsの指し手に変換します。
        /// </summary>
        /// <param name="csaMoveUnit"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public uint ConvertToNielsMove(string csaMoveUnit, BoardContext context)
        {
            var csaMoves = StringHelper.ToListSplitCount(csaMoveUnit, 2).ToList();
            uint fromIndex = 0;
            BoardType fromBoardType = BoardType.Main;
            if (csaMoves[(int)MoveUnit.FromIndex] == "00")
            {
                // 持ち駒からの打ち手
                fromIndex = Move.HandValueIndex;
                fromBoardType = Move.HandValueBoardType;
            }
            else
            {
                int formFile = int.Parse(csaMoves[(int)MoveUnit.FromIndex].Substring(0, 1));
                int fromRank = int.Parse(csaMoves[(int)MoveUnit.FromIndex].Substring(1, 1));
                fromIndex = (uint)this.GetIndex(formFile, fromRank);
                fromBoardType = this.GetBoardType(formFile, fromRank);
            }
            int toFile = int.Parse(csaMoves[(int)MoveUnit.ToIndex].Substring(0, 1));
            int toRank = int.Parse(csaMoves[(int)MoveUnit.ToIndex].Substring(1, 1));
            uint toIndex = (uint)this.GetIndex(toFile, toRank);
            BoardType toBoardType = this.GetBoardType(toFile, toRank);

            Piece putPiece = Pieces[csaMoves[(int)MoveUnit.PutPiece]];
            Piece capturePiece = CaptureGenerator.GenerateAnyCapturedPiece(toBoardType, (int)toIndex, context);

            // 成る手かの判断
            Promote promote = (putPiece.IsPromoted() && context.GetTurnPiece(fromBoardType, (int)fromIndex).ToPiece() == putPiece.UndoPromoted()) ? Promote.Yes : Promote.No;
            uint move = Move.GetMove(putPiece, fromBoardType, fromIndex, toBoardType, toIndex, context.Turn, capturePiece, promote);
            return move;
        }
    }
}
