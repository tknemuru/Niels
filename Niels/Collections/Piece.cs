using System;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

namespace Niels.Collections
{
    /// <summary>
    /// 駒
    /// </summary>
    public enum Piece
    {
        /// <summary>
        /// 空（駒無し）
        /// </summary>
        [JapaneseName("・")]
        Empty = 0,

        /// <summary>
        /// 歩
        /// </summary>
        [JapaneseName("歩")]
        Pawn = 1,

        /// <summary>
        /// 香車
        /// </summary>
        [JapaneseName("香")]
        Launce = 2,

        /// <summary>
        /// 桂馬
        /// </summary>
        [JapaneseName("桂")]
        Knight = 3,

        /// <summary>
        /// 銀
        /// </summary>
        [JapaneseName("銀")]
        Silver = 4,

        /// <summary>
        /// 金
        /// </summary>
        [JapaneseName("金")]
        Gold = 5,

        /// <summary>
        /// 角
        /// </summary>
        [JapaneseName("角")]
        Bishop = 6,

        /// <summary>
        /// 飛車
        /// </summary>
        [JapaneseName("飛")]
        Rook = 7,

        /// <summary>
        /// 玉
        /// </summary>
        [JapaneseName("玉")]
        King = 8,

        /// <summary>
        /// と金（ときん）
        /// </summary>
        [JapaneseName("と")]
        PawnPromoted = Pawn | ExtensionPiece.Promoted,

        /// <summary>
        /// 成香（なりきょう）
        /// </summary>
        [JapaneseName("杏")]
        LauncePromoted = Launce | ExtensionPiece.Promoted,

        /// <summary>
        /// 成桂（なりけい）
        /// </summary>
        [JapaneseName("圭")]
        KnightPromoted = Knight | ExtensionPiece.Promoted,

        /// <summary>
        /// 成銀（なりぎん）
        /// </summary>
        [JapaneseName("全")]
        SilverPromoted = Silver | ExtensionPiece.Promoted,

        /// <summary>
        /// 竜馬（りゅうめ、りゅうま）
        /// </summary>
        [JapaneseName("馬")]
        Horse = Bishop | ExtensionPiece.Promoted,

        /// <summary>
        /// 竜王（りゅうおう）
        /// </summary>
        [JapaneseName("竜")]
        Dragon = Rook | ExtensionPiece.Promoted
    }

    /// <summary>
    /// 駒拡張
    /// </summary>
    public static class ExtensionPiece
    {
        /// <summary>
        /// 成り駒を示す
        /// </summary>
        public const int Promoted = 16;

        /// <summary>
        /// 駒の種類数
        /// </summary>
        public const int PieceCount = 14;

        /// <summary>
        /// 駒リスト
        /// Enum.GetValuesを使うよりも若干速いので作ってみた
        /// </summary>
        public static readonly IEnumerable<Piece> Pieces;

        /// <summary>
        /// 成り駒を除いた駒リスト
        /// </summary>
        public static readonly IEnumerable<Piece> PiecesRemovedPromoted;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ExtensionPiece()
        {
            Pieces = ((IEnumerable<Piece>)Enum.GetValues(typeof(Piece))).Where(p => p != Piece.Empty);
            PiecesRemovedPromoted = ((IEnumerable<Piece>)Enum.GetValues(typeof(Piece)))
                                    .Where(p => (p != Piece.Empty && !p.IsPromoted()));
        }

        /// <summary>
        /// インデックスを取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static int GetIndex(this Piece piece)
        {
            if (piece <= Piece.King)
            {
                return (int)piece - 1;
            }
            else
            {
                var noPromotedPiece = ((int)piece & ~Promoted);
                if (noPromotedPiece < (int)Piece.Bishop)
                {
                    return (noPromotedPiece | (int)Piece.King) - 1;
                }
                else
                {
                    return (noPromotedPiece | (int)Piece.King) - 2;
                }
            }
        }

        /// <summary>
        /// 成る
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static Piece Promote(this Piece piece)
        {
            return (Piece)((int)piece | Promoted);
        }

        /// <summary>
        /// 成りを解除する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static Piece UndoPromoted(this Piece piece)
        {
            return (Piece)((int)piece & ~Promoted);
        }

        /// <summary>
        /// 駒の日本語名を取得する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static string GetJapaneseName(this Piece piece)
        {
            var names = (JapaneseNameAttribute[])piece.GetType().GetField(piece.ToString()).GetCustomAttributes(typeof(JapaneseNameAttribute), false);
            return names[0].Name;
        }

        /// <summary>
        /// 成り駒かどうか
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static bool IsPromoted(this Piece piece)
        {
            return (((int)piece & Promoted) == Promoted);
        }
    }

    /// <summary>
    /// 駒の日本語名を表すカスタムアトリビュートクラス
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class JapaneseNameAttribute : Attribute
    {
        /// <summary>
        /// 駒の日本語名
        /// </summary>
        public string Name;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public JapaneseNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}