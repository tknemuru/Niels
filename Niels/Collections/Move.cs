using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Niels.Boards;
using System.Diagnostics;

namespace Niels.Collections
{
    /// <summary>
    /// 打ち手
    /// </summary>
    public class Move
    {
        /// <summary>
        /// 打ち駒のシフト量
        /// </summary>
        public const int ShiftPutPiece = 0;

        /// <summary>
        /// 打ち駒成りフラグのシフト量
        /// </summary>
        public const int ShiftPutPiecePromote = 4;

        /// <summary>
        /// 打ち駒先手フラグのシフト量
        /// </summary>
        public const int ShiftPutPieceTurn = 5;

        /// <summary>
        /// Fromインデックスのシフト量
        /// </summary>
        public const int ShiftFromIndex = 6;

        /// <summary>
        /// From盤のシフト量
        /// </summary>
        public const int ShiftFromBoard = 12;
        
        /// <summary>
        /// Toインデックスのシフト量
        /// </summary>
        public const int ShiftToIndex = 14;

        /// <summary>
        /// To盤のシフト量
        /// </summary>
        public const int ShiftToBoard = 20;

        /// <summary>
        /// 取る駒のシフト量
        /// </summary>
        public const int ShiftCapturePiece = 22;

        /// <summary>
        /// 取る駒成りフラグのシフト量
        /// </summary>
        public const int ShiftCapturePiecePromote = 26;

        /// <summary>
        /// 取る駒先手フラグのシフト量
        /// </summary>
        public const int ShiftCapturePieceTurn = 27;

        /// <summary>
        /// 成りフラグのシフト量
        /// </summary>
        public const int ShiftIsPromote = 28;

        /// <summary>
        /// 持ち駒のダミー盤タイプ
        /// </summary>
        public static readonly BoardType HandValueBoardType = BoardType.SubRight;

        /// <summary>
        /// 持ち駒のダミーインデックス
        /// </summary>
        public static readonly uint HandValueIndex = 63;

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static uint GetMove(Piece piece, BoardType fromBoard, uint fromIndex, BoardType toBoard, uint toIndex, Turn turn, Piece capturePiece, Promote promote)
        {
            uint move = ((uint)piece << ShiftPutPiece) | ((uint)turn << ShiftPutPieceTurn)
                         | (fromIndex << ShiftFromIndex) | ((uint)fromBoard << ShiftFromBoard)
                         | (toIndex << ShiftToIndex) | ((uint)toBoard << ShiftToBoard)
                         | ((uint)capturePiece << ShiftCapturePiece) | ((uint)turn.GetOppositeIndex() << ShiftCapturePieceTurn)
                         | ((uint)promote << ShiftIsPromote);
            return move;
        }

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="fromBoard"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toBoard"></param>
        /// <param name="toIndex"></param>
        /// <param name="turn"></param>
        /// <param name="capturePiece"></param>
        /// <param name="promote"></param>
        /// <returns></returns>
        public static uint GetMove(Piece piece, BoardType fromBoard, uint fromIndex, BoardType toBoard, uint toIndex, Turn turn, Piece capturePiece)
        {
            return GetMove(piece, fromBoard, fromIndex, toBoard, toIndex, turn, capturePiece, Promote.No);
        }

        /// <summary>
        /// 手を取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static uint GetMove(Piece piece, BoardType fromBoard, uint fromIndex, BoardType toBoard, uint toIndex, Turn turn)
        {
            return GetMove(piece, fromBoard, fromIndex, toBoard, toIndex, turn, Piece.Empty, Promote.No);
        }

        /// <summary>
        /// 持ち駒からの打ち手を取得します。
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="toBoard"></param>
        /// <param name="toIndex"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static uint GetHandValueMove(Piece piece, BoardType toBoard, uint toIndex, Turn turn)
        {
            return GetMove(piece, HandValueBoardType, HandValueIndex, toBoard, toIndex, turn, Piece.Empty, Promote.No);
        }
    }

    /// <summary>
    /// 指し手用uint拡張
    /// </summary>
    public static class ExtensionUintMove
    {
        /// <summary>
        /// 移動元インデックス
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint FromIndex(this uint move)
        {
            return (move >> Move.ShiftFromIndex) & 0x0000003f;
        }

        /// <summary>
        /// 移動元の盤
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static BoardType FromBoard(this uint move)
        {
            return (BoardType)((move >> Move.ShiftFromBoard) & 0x00000003);
        }

        /// <summary>
        /// 移動元のターン
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static Turn PutPieceTurn(this uint move)
        {
            return (Turn)((move >> Move.ShiftPutPieceTurn) & 1);
        }

        /// <summary>
        /// 移動先インデックス
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint ToIndex(this uint move)
        {
            return (move >> Move.ShiftToIndex) & 0x0000003f;
        }

        /// <summary>
        /// 移動先の盤
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static BoardType ToBoard(this uint move)
        {
            return (BoardType)((move >> Move.ShiftToBoard) & 0x00000003);
        }

        /// <summary>
        /// 指す駒
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static Piece PutPiece(this uint move)
        {
            return (Piece)(move & 0x0000001f);
        }

        /// <summary>
        /// 指す駒をセットします。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint SetPutPiece(this uint move, Piece piece)
        {
            move &= ~((uint)0x1f << Move.ShiftPutPiece);
            return move | ((uint)piece << Move.ShiftPutPiece);
        }

        /// <summary>
        /// 取られる駒
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static Piece CapturePiece(this uint move)
        {
            return (Piece)((move >> Move.ShiftCapturePiece) & 0x0000001f);
        }

        /// <summary>
        /// 駒を取る手かどうか
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static bool IsCapture(this uint move)
        {
            return (move.CapturePiece() != (uint)Piece.Empty);
        }

        /// <summary>
        /// 取られる駒をセットします。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint SetCapturePiece(this uint move, Piece capturePiece)
        {
            move &= ~((uint)0x1f << Move.ShiftCapturePiece); 
            return move | ((uint)capturePiece << Move.ShiftCapturePiece);
        }

        /// <summary>
        /// 成る手かどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static bool IsPromote(this uint move)
        {
            return (((move >> Move.ShiftIsPromote) & 1u) > 0);
        }

        /// <summary>
        /// 成れる指手かどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <param name="capturePiece"></param>
        /// <returns></returns>
        public static bool CanPromote(this uint move)
        {
            // 玉、金は成れない
            Piece piece = move.PutPiece();
            if (piece == Piece.King || piece == Piece.Gold) { return false; }

            // 既に成っていたらNG
            if (piece.IsPromoted()) { return false; }

            // 敵陣内へ入るとき、敵陣内で移動するとき、敵陣内から出るときに成れる
            return ((IsAtEnemyArea(move.ToBoard(), (int)move.ToIndex(), move.PutPieceTurn()))
                    || (IsAtEnemyArea(move.FromBoard(), (int)move.FromIndex(), move.PutPieceTurn())));
        }

        /// <summary>
        /// 成る指し手にします。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint Promote(this uint move)
        {
            Debug.Assert(!move.PutPiece().IsPromoted(), "既に成り駒です。");
            return (move | (1u << Move.ShiftPutPiecePromote) | (1u << Move.ShiftIsPromote));
        }

        /// <summary>
        /// 持ち駒からの打ち手かどうかを判定します。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static bool IsHandValueMove(this uint move)
        {
            return (move.FromBoard() == Move.HandValueBoardType && move.FromIndex() == Move.HandValueIndex);
        }

        /// <summary>
        /// 敵陣にいるかどうかを判定します。
        /// </summary>
        /// <param name="boardType"></param>
        /// <param name="index"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static bool IsAtEnemyArea(BoardType boardType, int index, Turn turn)
        {
            Board board = BoardProvider.GetAll()[(int)boardType];
            return (((1ul << index) & board.FocusEnemyArea[(int)turn]) > 0);
        }
    }
}