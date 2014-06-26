using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Boards;
using Niels.Collections;
using Niels.MagicBitBoardGenerator.Generators;
using Niels.MagicBitBoardGenerator.Generators.Attack;
using Niels.MagicBitBoardGenerator.Converters;
using Niels.MagicBitBoardGenerator.Accessor;
using Niels.MagicBitBoardGenerator.Config;
using Niels.Tests.TestHelper;
using Niels.Fools;
using Niels.Helper;
using Niels.Filters;

namespace Niels.MagicBitBoardGenerator.Attack.Tests
{
    [TestClass]
    public class AttackGeneratorTest
    {
        [TestMethod]
        public void TestGenerate()
        {
            // 飛車----------------------------------------------------------------
            MagicBitBoardGeneratorConfig config = MagicBitBoardGeneratorConfigProvider.GetRookConfig();
            FoolOrder order = new FoolOrder();
            AttackGenerator gen = config.AttackGenerator;
            int index = BoardAccesor.GetSequanceIndex(42, BoardType.Main);
            Dictionary<int, bool> pattern = new Dictionary<int,bool>();
            pattern.Add(BoardAccesor.GetSequanceIndex(34, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(26, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(18, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(10, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(41, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(50, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(43, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(44, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(45, BoardType.Main), true);

            List<uint> expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 47, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 38, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 55, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 65, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 74, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 57, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 58, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 56, 59, Turn.Black, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            List<uint> actual = gen.Generate(index, pattern, Piece.Rook).ToList();
            actual = order.MoveOrdering(actual).ToList();

            CollectionAssert.AreEqual(expected, actual);
            // --------------------------------------------------------------------

            // --------------------------------------------------------------------
            index = BoardAccesor.GetSequanceIndex(56, BoardType.Main);
            pattern = new Dictionary<int, bool>();
            pattern.Add(BoardAccesor.GetSequanceIndex(48, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(40, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(32, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(24, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(16, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(8, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(0, BoardType.Main), false);

            pattern.Add(BoardAccesor.GetSequanceIndex(57, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(58, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(59, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(60, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(61, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(62, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(63, BoardType.Main), false);

            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 63, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 54, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 45, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 73, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 74, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 75, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 76, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 77, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 78, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 79, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 72, 80, Turn.Black, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            actual = gen.Generate(index, pattern, Piece.Rook).ToList();
            actual = order.MoveOrdering(actual).ToList();
            CollectionAssert.AreEqual(expected, actual);
            // --------------------------------------------------------------------

            // --------------------------------------------------------------------
            index = BoardAccesor.GetSequanceIndex(43, BoardType.Main);
            pattern = new Dictionary<int, bool>();
            pattern.Add(BoardAccesor.GetSequanceIndex(42, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(41, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(35, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(27, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(19, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(11, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(3, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(44, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(45, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(46, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(47, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(51, BoardType.Main), true);

            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 48, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 66, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 39, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 56, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Rook, 57, 58, Turn.Black, Piece.Pawn));
            expected = order.MoveOrdering(expected).ToList();

            actual = gen.Generate(index, pattern, Piece.Rook).ToList();
            actual = order.MoveOrdering(actual).ToList();
            CollectionAssert.AreEqual(expected, actual);
            // --------------------------------------------------------------------

            // 角------------------------------------------------------------------
            config = MagicBitBoardGeneratorConfigProvider.GetBishopConfig();
            gen = config.AttackGenerator;
            index = BoardAccesor.GetSequanceIndex(42, BoardType.Main);
            pattern = new Dictionary<int, bool>();
            pattern.Add(BoardAccesor.GetSequanceIndex(33, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(49, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(51, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(35, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(28, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(21, BoardType.Main), true);
            pattern.Add(BoardAccesor.GetSequanceIndex(14, BoardType.Main), false);
            pattern.Add(BoardAccesor.GetSequanceIndex(7, BoardType.Main), true);

            expected = new List<uint>();
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 46, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 64, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 72, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 48, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 40, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 32, Turn.Black, Piece.Pawn));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 66, Turn.Black));
            expected.Add(MoveForTest.GetMove(Piece.Bishop, 56, 76, Turn.Black, Piece.Pawn));

            expected = order.MoveOrdering(expected).ToList();

            actual = gen.Generate(index, pattern, Piece.Bishop).ToList();
            actual = order.MoveOrdering(actual).ToList();
            // --------------------------------------------------------------------

            FileHelper.WriteLine("", @"./test/expected_move.txt", false);
            FileHelper.WriteLine("", @"./test/actual_move.txt", false);
            expected.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/expected_move.txt")
                );

            actual.ForEach
                (
                    value => FileHelper.WriteLine(MoveForTest.ToDebugString(value), @"./test/actual_move.txt")
                );

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
