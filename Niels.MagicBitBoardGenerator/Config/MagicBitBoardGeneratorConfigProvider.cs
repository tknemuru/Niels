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
    /// マジックビットボード生成で使用する駒種別ごとの設定情報を提供します。
    /// </summary>
    internal static class MagicBitBoardGeneratorConfigProvider
    {
        /// <summary>
        /// 飛車の設定情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static MagicBitBoardGeneratorConfig GetRookConfig()
        {
            MagicBitBoardGeneratorConfig config = new MagicBitBoardGeneratorConfig();
            config.MaskNumberFilePath = string.Format(@"./log/rook_mask_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.MagicNumberFilePath = string.Format(@"./log/rook_magic_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.AttackTableFilePath = @"./attack/rook_attack_{0}.txt";
            config.IsAddSeqIndexToAttackTableFilePath = true;
            config.ShiftMagicCode = 14;
            config.MoveRelationIndexsGenerator = new RookMoveRelationIndexsGenerator();
            config.AttackGenerator = new RookAttackGenerator();
            config.PatternGenerator = new PatternGenerator(config.MoveRelationIndexsGenerator);
            config.TargetPiece = Piece.Rook;

            return config;
        }

        /// <summary>
        /// 角の設定情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static MagicBitBoardGeneratorConfig GetBishopConfig()
        {
            MagicBitBoardGeneratorConfig config = new MagicBitBoardGeneratorConfig();
            config.MaskNumberFilePath = string.Format(@"./log/bishop_mask_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.MagicNumberFilePath = string.Format(@"./log/bishop_magic_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.AttackTableFilePath = @"./attack/bishop_attack_{0}.txt";
            config.IsAddSeqIndexToAttackTableFilePath = true;
            config.ShiftMagicCode = 14;
            config.MoveRelationIndexsGenerator = new BishopMoveRelationIndexsGenerator();
            config.AttackGenerator = new BishopAttackGenerator();
            config.PatternGenerator = new PatternGenerator(config.MoveRelationIndexsGenerator);
            config.TargetPiece = Piece.Bishop;

            return config;
        }

        /// <summary>
        /// 香車(先手)の設定情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static MagicBitBoardGeneratorConfig GetLaunceBlackConfig()
        {
            MagicBitBoardGeneratorConfig config = new MagicBitBoardGeneratorConfig();
            config.MaskNumberFilePath = string.Format(@"./log/launce_black_mask_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.MagicNumberFilePath = string.Format(@"./log/launce_black_magic_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.AttackTableFilePath = @"./attack/launce_black_attack_{0}.txt";
            config.IsAddSeqIndexToAttackTableFilePath = false;
            config.ShiftMagicCode = 7;
            config.MoveRelationIndexsGenerator = new LaunceMoveRelationIndexsGenerator(Turn.Black);
            config.AttackGenerator = new LaunceAttackGenerator(Turn.Black);
            config.PatternGenerator = new PatternGenerator(config.MoveRelationIndexsGenerator);
            config.TargetPiece = Piece.Launce;

            return config;
        }

        /// <summary>
        /// 香車(後手)の設定情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static MagicBitBoardGeneratorConfig GetLaunceWhiteConfig()
        {
            MagicBitBoardGeneratorConfig config = new MagicBitBoardGeneratorConfig();
            config.MaskNumberFilePath = string.Format(@"./log/launce_white_mask_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.MagicNumberFilePath = string.Format(@"./log/launce_white_magic_{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
            config.AttackTableFilePath = @"./attack/launce_white_attack_{0}.txt";
            config.IsAddSeqIndexToAttackTableFilePath = false;
            config.ShiftMagicCode = 7;
            config.MoveRelationIndexsGenerator = new LaunceMoveRelationIndexsGenerator(Turn.White);
            config.AttackGenerator = new LaunceAttackGenerator(Turn.White);
            config.PatternGenerator = new PatternGenerator(config.MoveRelationIndexsGenerator);
            config.TargetPiece = Piece.Launce;

            return config;
        }
    }
}
