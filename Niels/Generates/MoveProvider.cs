using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Niels.Boards;
using Niels.Collections;
using Niels.Fools;
using Niels.Filters;
using Niels.Diagnostics;
using System.Diagnostics;

namespace Niels.Generates
{
    /// <summary>
    /// 利き提供クラス
    /// </summary>
    public static class MoveProvider
    {
        /// <summary>
        /// マスクナンバーテーブルファイルパス
        /// </summary>
        private const string MaskTableFilePath = @"C:\work\visualstudio\Niels\Niels\Data\{0}_mask_numbers.txt";

        /// <summary>
        /// マジックナンバーテーブルファイルパス
        /// </summary>
        private const string MagicTableFilePath = @"C:\work\visualstudio\Niels\Niels\Data\{0}_magic_numbers.txt";

        /// <summary>
        /// アタックテーブルファイルパス
        /// </summary>
        private const string AttackTableFilePath = @"C:\work\visualstudio\Niels\Niels\Data\attack\{0}_attack_{{0}}.txt";

        /// <summary>
        /// 上方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator UpGen = new UpMoveGenerator();
        
        /// <summary>
        /// 下方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator DownGen = new DownMoveGenerator();
        
        /// <summary>
        /// 右上方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator UpRightGen = new UpRightMoveGenerator();

        /// <summary>
        /// 左上方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator UpLeftGen = new UpLeftMoveGenerator();

        /// <summary>
        /// 右下方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator DownRightGen = new DownRightMoveGenerator();

        /// <summary>
        /// 左下方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator DownLeftGen = new DownLeftMoveGenerator();

        /// <summary>
        /// 右方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator RightGen = new RightMoveGenerator();

        /// <summary>
        /// 左方向の利きを生成する
        /// </summary>
        private static readonly MoveGenerator LeftGen = new LeftMoveGenerator();

        /// <summary>
        /// 角の利きを生成する
        /// </summary>
        private static readonly MoveGenerator BishopInnerGen = new MagicBitBoardMoveGenerator(
                                                                        string.Format(MagicTableFilePath, "bishop"),
                                                                        string.Format(MaskTableFilePath, "bishop"),
                                                                        string.Format(AttackTableFilePath, "bishop"),
                                                                        14);

        /// <summary>
        /// 飛車の利きを生成する
        /// </summary>
        private static readonly MoveGenerator RookInnerGen = new MagicBitBoardMoveGenerator(
                                                                        string.Format(MagicTableFilePath, "rook"),
                                                                        string.Format(MaskTableFilePath, "rook"),
                                                                        string.Format(AttackTableFilePath, "rook"),
                                                                        14);

        /// <summary>
        /// 香車(先手)の利きを生成する
        /// </summary>
        private static readonly MoveGenerator LaunceBlackInnerGen = new MagicBitBoardMoveGenerator(
                                                                        string.Format(MagicTableFilePath, "launce_black"),
                                                                        string.Format(MaskTableFilePath, "launce_black"),
                                                                        string.Format(AttackTableFilePath, "launce_black"),
                                                                        7);

        /// <summary>
        /// 香車(後手)の利きを生成する
        /// </summary>
        private static readonly MoveGenerator LaunceWhiteInnerGen = new MagicBitBoardMoveGenerator(
                                                                        string.Format(MagicTableFilePath, "launce_white"),
                                                                        string.Format(MaskTableFilePath, "launce_white"),
                                                                        string.Format(AttackTableFilePath, "launce_white"),
                                                                        7);

        /// <summary>
        /// 持ち駒からの打ち手を生成する
        /// </summary>
        private static readonly HandValueMoveGenerator HandValueMoveGenerator = new HandValueMoveGenerator();

        /// <summary>
        /// 歩(先手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> PawnBlackGen =
            new List<MoveGenerator>()
        {
            UpGen
        };

        /// <summary>
        /// 歩(後手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> PawnWhiteGen =
            new List<MoveGenerator>()
        {
            DownGen
        };

        /// <summary>
        /// 歩のフィルタを行なう
        /// </summary>
        private static readonly IEnumerable<MoveFilter> PawnFilter =
            new List<MoveFilter>()
        {
            FilterProvider.CanNotMoveFilterOneRank
        };

        /// <summary>
        /// 香車(先手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> LaunceBlackGen =
            new List<MoveGenerator>()
        {
            LaunceBlackInnerGen
        };

        /// <summary>
        /// 香車(後手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> LaunceWhiteGen =
            new List<MoveGenerator>()
        {
            LaunceWhiteInnerGen
        };

        /// <summary>
        /// 香車のフィルタを行なう
        /// </summary>
        private static readonly IEnumerable<MoveFilter> LaunceFilter =
            new List<MoveFilter>()
        {
            FilterProvider.CanNotMoveFilterOneRank
        };

        /// <summary>
        /// 桂馬(先手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> KnightBlackGen =
            new List<MoveGenerator>()
        {
            new KnightUpRightMoveGenerator(),
            new KnightUpLeftMoveGenerator(),
        };

        /// <summary>
        /// 桂馬(後手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> KnightWhiteGen =
            new List<MoveGenerator>()
        {
            new KnightDownRightMoveGenerator(),
            new KnightDownLeftMoveGenerator(),
        };

        /// <summary>
        /// 桂馬のフィルタを行なう
        /// </summary>
        private static readonly IEnumerable<MoveFilter> KnightFilter =
            new List<MoveFilter>()
        {
            FilterProvider.CanNotMoveFilterTwoRank
        };

        /// <summary>
        /// 金(先手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> GoldBlackGen =
            new List<MoveGenerator>()
        {
            UpGen,
            UpRightGen,
            UpLeftGen,
            RightGen,
            LeftGen,
            DownGen
        };

        /// <summary>
        /// 金(後手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> GoldWhiteGen =
            new List<MoveGenerator>()
        {
            DownGen,
            DownLeftGen,
            DownRightGen,
            LeftGen,
            RightGen,
            UpGen
        };

        /// <summary>
        /// 銀(先手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> SilverBlackGen =
            new List<MoveGenerator>()
        {
            UpGen,
            UpRightGen,
            UpLeftGen,
            DownRightGen,
            DownLeftGen
        };

        /// <summary>
        /// 銀(後手)の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> SilverWhiteGen =
            new List<MoveGenerator>()
        {
            DownGen,
            DownLeftGen,
            DownRightGen,
            UpRightGen,
            UpLeftGen
        };

        /// <summary>
        /// 玉の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> KingGen =
            new List<MoveGenerator>()
        {
            UpGen,
            UpRightGen,
            UpLeftGen,
            RightGen,
            LeftGen,
            DownGen,
            DownRightGen,
            DownLeftGen
        };

        /// <summary>
        /// 角の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> BishopGen =
            new List<MoveGenerator>()
        {
            BishopInnerGen
        };

        /// <summary>
        /// 飛車の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> RookGen =
            new List<MoveGenerator>()
        {
            RookInnerGen
        };

        /// <summary>
        /// 竜馬の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> HorseGen =
            new List<MoveGenerator>()
        {
            BishopInnerGen,
            UpGen,
            DownGen,
            RightGen,
            LeftGen
        };

        /// <summary>
        /// 竜馬の利きを生成する
        /// </summary>
        private static readonly IEnumerable<MoveGenerator> DragonGen =
            new List<MoveGenerator>()
        {
            RookInnerGen,
            UpRightGen,
            UpLeftGen,
            DownRightGen,
            DownLeftGen
        };

        /// <summary>
        /// 歩の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetPawnMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, PawnBlackGen, PawnWhiteGen, PawnFilter, Piece.Pawn, generateTarget);
        }

        /// <summary>
        /// 香車の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetLaunceMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, LaunceBlackGen, LaunceWhiteGen, LaunceFilter, Piece.Launce, generateTarget);
        }

        /// <summary>
        /// 桂馬の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKnightMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, KnightBlackGen, KnightWhiteGen, KnightFilter, Piece.Knight, generateTarget);
        }

        /// <summary>
        /// 銀の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetSilverMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, SilverBlackGen, SilverWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.Silver, generateTarget);
        }

        /// <summary>
        /// 金の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetGoldMoves(BoardContext context)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.Gold, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 角の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetBishopMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, BishopGen, BishopGen, Enumerable.Empty<MoveFilter>(), Piece.Bishop, generateTarget);
        }

        /// <summary>
        /// 飛車の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetRookMoves(BoardContext context, GenerateTarget generateTarget)
        {
            return GetMoves(context, RookGen, RookGen, Enumerable.Empty<MoveFilter>(), Piece.Rook, generateTarget);
        }

        /// <summary>
        /// 玉の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKingMoves(BoardContext context)
        {
            return GetMoves(context, KingGen, KingGen, Enumerable.Empty<MoveFilter>(), Piece.King, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// と金の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetPawnPromotedMoves(BoardContext context)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.PawnPromoted, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 成香の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetLauncePromotedMoves(BoardContext context)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.LauncePromoted, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 成桂の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKnightPromotedMoves(BoardContext context)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.KnightPromoted, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 成銀の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetSilverPromotedMoves(BoardContext context)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.SilverPromoted, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 竜馬の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetHorseMoves(BoardContext context)
        {
            return GetMoves(context, HorseGen, HorseGen, Enumerable.Empty<MoveFilter>(), Piece.Horse, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 竜王の利きを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetDragonMoves(BoardContext context)
        {
            return GetMoves(context, DragonGen, DragonGen, Enumerable.Empty<MoveFilter>(), Piece.Dragon, GenerateTarget.NoPromote);
        }

        /// <summary>
        /// 持ち駒からの打ち手を取得する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IEnumerable<uint> GetHandValueMoves(BoardContext context)
        {
            return HandValueMoveGenerator.Generate(context);
        }

        /// <summary>
        /// 歩の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetPawnMoves(BoardContext context, GenerateTarget generateTarget, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, PawnBlackGen, PawnWhiteGen, PawnFilter, Piece.Pawn, generateTarget, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 香車の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetLaunceMoves(BoardContext context, GenerateTarget generateTarget, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, LaunceBlackGen, LaunceWhiteGen, LaunceFilter, Piece.Launce, generateTarget, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 桂馬の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKnightMoves(BoardContext context, GenerateTarget generateTarget, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, KnightBlackGen, KnightWhiteGen, KnightFilter, Piece.Knight, generateTarget, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 銀の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetSilverMoves(BoardContext context, GenerateTarget generateTarget, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, SilverBlackGen, SilverWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.Silver, generateTarget, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 金の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetGoldMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.Gold, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 玉の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKingMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, KingGen, KingGen, Enumerable.Empty<MoveFilter>(), Piece.King, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// と金の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetPawnPromotedMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.PawnPromoted, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 成香の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetLauncePromotedMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.LauncePromoted, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 成桂の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetKnightPromotedMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.KnightPromoted, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 成銀の利きを取得する(特定の位置への利きのみ)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GetSilverPromotedMoves(BoardContext context, BoardType targetBoardType, int targetIndex)
        {
            return GetMoves(context, GoldBlackGen, GoldWhiteGen, Enumerable.Empty<MoveFilter>(), Piece.SilverPromoted, GenerateTarget.NoPromote, targetBoardType, targetIndex);
        }

        /// <summary>
        /// 全ての打ち手を取得します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IEnumerable<uint> GetAllMoves(BoardContext context)
        {
            return GetAllMoves(context, GenerateTarget.All);
        }

        /// <summary>
        /// 全ての打ち手を取得します。
        /// TODO:generateTargetは不要かもしれない
        /// </summary>
        /// <param name="context"></param>
        /// <param name="generateTarget"></param>
        /// <returns></returns>
        public static IEnumerable<uint> GetAllMoves(BoardContext context, GenerateTarget generateTarget)
        {
            foreach (uint move in MoveProvider.GetPawnMoves(context, generateTarget))
            {
                // 自玉を相手駒の利きにさらす指し手をフィルタして返却
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetLaunceMoves(context, generateTarget))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetKnightMoves(context, generateTarget))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetSilverMoves(context, generateTarget))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetGoldMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetKingMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetBishopMoves(context, generateTarget))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetRookMoves(context, generateTarget))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetPawnPromotedMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetLauncePromotedMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetKnightPromotedMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetSilverPromotedMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetHorseMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetDragonMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
            foreach (uint move in MoveProvider.GetHandValueMoves(context))
            {
                if (FilterProvider.CheckedByMyselefMoveFilter.Validate(context, move))
                {
                    yield return move;
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// TODO:パフォーマンス悪そう
        /// 王手が存在するかどうかを判定します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsExistsCheckMove(BoardContext context, BoardType boardType, int index)
        {
            foreach (uint move in GetRookMoves(context, GenerateTarget.All))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetBishopMoves(context, GenerateTarget.All))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetGoldMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetSilverMoves(context, GenerateTarget.All, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetLaunceMoves(context, GenerateTarget.All, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetKnightMoves(context, GenerateTarget.All, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetDragonMoves(context))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetHorseMoves(context))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetSilverPromotedMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetKnightPromotedMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetLauncePromotedMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetPawnPromotedMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetKingMoves(context, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }
            foreach (uint move in GetPawnMoves(context, GenerateTarget.All, boardType, index))
            {
                if (move.CapturePiece() == Piece.King)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 利きを生成する
        /// </summary>
        /// <param name="context"></param>
        /// <param name="blackGen"></param>
        /// <param name="whiteGen"></param>
        /// <returns></returns>
        private static IEnumerable<uint> GetMoves(BoardContext context,
                                           IEnumerable<MoveGenerator> blackGen,
                                           IEnumerable<MoveGenerator> whiteGen,
                                           IEnumerable<MoveFilter> filters,
                                           Piece piece,
                                           GenerateTarget generateTarget)
        {
            // 指し手を生成
            IEnumerable<MoveGenerator> gens = (context.Turn == Turn.Black) ? blackGen : whiteGen;
            bool isGeneratePromote = ((generateTarget & GenerateTarget.Promote) == GenerateTarget.Promote);
            foreach (var gen in gens)
            {
                foreach (uint move in gen.Generate(context, piece))
                {
                    uint retMove = move;
                    if (isGeneratePromote && retMove.CanPromote())
                    {
                        retMove = retMove.Promote();
                    }
                    if (MoveValidate(context, retMove, filters))
                    {
                        yield return retMove;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 指定した位置への利きを取得します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="blackGen"></param>
        /// <param name="whiteGen"></param>
        /// <param name="filters"></param>
        /// <param name="piece"></param>
        /// <param name="generateTarget"></param>
        /// <param name="targetBoardType"></param>
        /// <param name="targetIndex"></param>
        /// <returns></returns>
        private static IEnumerable<uint> GetMoves(BoardContext context,
                                           IEnumerable<MoveGenerator> blackGen,
                                           IEnumerable<MoveGenerator> whiteGen,
                                           IEnumerable<MoveFilter> filters,
                                           Piece piece,
                                           GenerateTarget generateTarget,
                                           BoardType targetBoardType,
                                           int targetIndex)
        {
            // 指し手を生成
            IEnumerable<MoveGenerator> gens = (context.Turn == Turn.Black) ? blackGen : whiteGen;
            bool isGeneratePromote = ((generateTarget & GenerateTarget.Promote) == GenerateTarget.Promote);
            foreach (var gen in gens)
            {
                foreach (uint move in gen.Generate(context, piece, targetBoardType, targetIndex))
                {
                    uint retMove = move;
                    if (isGeneratePromote && retMove.CanPromote())
                    {
                        retMove = retMove.Promote();
                    }
                    if (MoveValidate(context, retMove, filters))
                    {
                        yield return retMove;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 指し手が正当かどうかを判定します。
        /// </summary>
        /// <returns></returns>
        private static bool MoveValidate(BoardContext context, uint move, IEnumerable<MoveFilter> filters)
        {
            // 不要な指し手をフィルタ
            foreach (var filter in filters)
            {
                if (!filter.Validate(context, move))
                {
                    return false;
                }
            }

            return true;
        }
    }
}