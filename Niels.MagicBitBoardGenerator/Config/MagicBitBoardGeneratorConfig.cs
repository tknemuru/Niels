using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Collections;
using Niels.MagicBitBoardGenerator.Generators.Attack;
using Niels.MagicBitBoardGenerator.Generators.MoveRelationIndexs;
using Niels.MagicBitBoardGenerator.Generators.Pattern;

namespace Niels.MagicBitBoardGenerator.Config
{
    /// <summary>
    /// マジックビットボード作成処理の設定情報を保持します。
    /// </summary>
    internal class MagicBitBoardGeneratorConfig
    {
        /// <summary>
        /// マスクナンバーのファイルパス
        /// </summary>
        internal string MaskNumberFilePath { get; set; }

        /// <summary>
        /// マジックナンバーのファイルパス
        /// </summary>
        internal string MagicNumberFilePath { get; set; }

        /// <summary>
        /// アタックテーブルのファイルパス
        /// </summary>
        internal string AttackTableFilePath { get; set; }

        /// <summary>
        /// アタックテーブルのファイル名に一意のインデックスをつけるかどうか
        /// </summary>
        internal bool IsAddSeqIndexToAttackTableFilePath { get; set; }

        /// <summary>
        /// マジックコードのシフト数
        /// </summary>
        internal int ShiftMagicCode { get; set; }

        /// <summary>
        /// 利きに関係のあるマス目のみを抽出する機能
        /// </summary>
        internal MoveRelationIndexsGenerator MoveRelationIndexsGenerator { get; set; }

        /// <summary>
        /// 利き生成の機能
        /// </summary>
        internal AttackGenerator AttackGenerator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal PatternGenerator PatternGenerator { get; set; }

        /// <summary>
        /// 生成対象の駒
        /// </summary>
        internal Piece TargetPiece { get; set; }
    }
}
