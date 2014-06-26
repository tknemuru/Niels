using System;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

namespace Niels.Collections
{
    /// <summary>
    /// ターン付き駒
    /// </summary>
    public enum TurnPiece
    {
        /// <summary>
        /// 空（駒無し）
        /// </summary>
        Empty = Piece.Empty,

        /// <summary>
        /// 歩
        /// </summary>
        BlackPawn = Piece.Pawn,

        /// <summary>
        /// 香車
        /// </summary>
        BlackLaunce = Piece.Launce,

        /// <summary>
        /// 桂馬
        /// </summary>
        BlackKnight = Piece.Knight,

        /// <summary>
        /// 銀
        /// </summary>
        BlackSilver = Piece.Silver,

        /// <summary>
        /// 金
        /// </summary>
        BlackGold = Piece.Gold,

        /// <summary>
        /// 角
        /// </summary>
        BlackBishop = Piece.Bishop,

        /// <summary>
        /// 飛車
        /// </summary>
        BlackRook = Piece.Rook,

        /// <summary>
        /// 玉
        /// </summary>
        BlackKing = Piece.King,

        /// <summary>
        /// と金（ときん）
        /// </summary>
        BlackPawnPromoted = Piece.PawnPromoted,

        /// <summary>
        /// 成香（なりきょう）
        /// </summary>
        BlackLauncePromoted = Piece.LauncePromoted,

        /// <summary>
        /// 成桂（なりけい）
        /// </summary>
        BlackKnightPromoted = Piece.KnightPromoted,

        /// <summary>
        /// 成銀（なりぎん）
        /// </summary>
        BlackSilverPromoted = Piece.SilverPromoted,

        /// <summary>
        /// 竜馬（りゅうめ、りゅうま）
        /// </summary>
        BlackHorse = Piece.Horse,

        /// <summary>
        /// 竜王（りゅうおう）
        /// </summary>
        BlackDragon = Piece.Dragon,

        /// <summary>
        /// 歩
        /// </summary>
        WhitePawn = Piece.Pawn | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 香車
        /// </summary>
        WhiteLaunce = Piece.Launce | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 桂馬
        /// </summary>
        WhiteKnight = Piece.Knight | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 銀
        /// </summary>
        WhiteSilver = Piece.Silver | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 金
        /// </summary>
        WhiteGold = Piece.Gold | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 角
        /// </summary>
        WhiteBishop = Piece.Bishop | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 飛車
        /// </summary>
        WhiteRook = Piece.Rook | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 玉
        /// </summary>
        WhiteKing = Piece.King | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// と金（ときん）
        /// </summary>
        WhitePawnPromoted = Piece.PawnPromoted | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 成香（なりきょう）
        /// </summary>
        WhiteLauncePromoted = Piece.LauncePromoted | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 成桂（なりけい）
        /// </summary>
        WhiteKnightPromoted = Piece.KnightPromoted | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 成銀（なりぎん）
        /// </summary>
        WhiteSilverPromoted = Piece.SilverPromoted | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 竜馬（りゅうめ、りゅうま）
        /// </summary>
        WhiteHorse = Piece.Horse | ExtensionTurnPiece.WhitePiece,

        /// <summary>
        /// 竜王（りゅうおう）
        /// </summary>
        WhiteDragon = Piece.Dragon | ExtensionTurnPiece.WhitePiece,
    }

    /// <summary>
    /// ターン付き駒拡張
    /// </summary>
    public static class ExtensionTurnPiece
    {
        /// <summary>
        /// 後手駒を示す
        /// </summary>
        public const int WhitePiece = 0x20;

        /// <summary>
        /// 駒に変換します。
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static Piece ToPiece(this TurnPiece piece)
        {
            return (Piece)((int)piece & ~WhitePiece);
        }

        /// <summary>
        /// ターン付き駒に変換します。
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static TurnPiece ToTurnPiece(this Piece piece, Turn turn)
        {
            return (turn == Turn.Black) ? (TurnPiece)piece : (TurnPiece)((int)piece | WhitePiece);
        }

        /// <summary>
        /// ターンを変更します。
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static TurnPiece ChangeTurn(this TurnPiece piece)
        {
            return ((int)piece < WhitePiece && piece != TurnPiece.Empty) ? (TurnPiece)((int)piece | WhitePiece) : (TurnPiece)((int)piece & ~WhitePiece);
        }
    }
}