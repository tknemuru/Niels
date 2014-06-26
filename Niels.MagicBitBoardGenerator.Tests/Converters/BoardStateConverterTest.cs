using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Generators;
using Niels.MagicBitBoardGenerator.Converters;
using Niels.MagicBitBoardGenerator.Config;

namespace Niels.Support.Tests.Converters
{
    [TestClass]
    public class BoardStateConverterTest
    {
        [TestMethod]
        public void TestToBoardState()
        {
            var gen = MagicBitBoardGeneratorConfigProvider.GetRookConfig().MoveRelationIndexsGenerator;
            var canMovesIndexs = gen.Generate(56, BoardType.Main);

            ulong[] expected = new ulong[Board.BoardTypeCount];
            expected[(int)BoardType.Main] = 0xfe01010101010101;
            expected[(int)BoardType.SubRight] = 0;
            expected[(int)BoardType.SubBottom] = 0;

            ulong[] actual = BoardStateConverter.ToBoardState(canMovesIndexs);

            CollectionAssert.AreEqual(expected, actual);

            canMovesIndexs = gen.Generate(7, BoardType.SubBottom);
            expected = new ulong[Board.BoardTypeCount];
            expected[(int)BoardType.Main] = 0xfe;
            expected[(int)BoardType.SubRight] = 0;
            expected[(int)BoardType.SubBottom] = 0x0080808080808000;
            actual = BoardStateConverter.ToBoardState(canMovesIndexs);
            CollectionAssert.AreEqual(expected, actual);

            canMovesIndexs = gen.Generate(0, BoardType.SubRight);
            expected = new ulong[Board.BoardTypeCount];
            expected[(int)BoardType.Main] = 0x0001010101010101;
            expected[(int)BoardType.SubRight] = 0x0fe;
            expected[(int)BoardType.SubBottom] = 0;
            actual = BoardStateConverter.ToBoardState(canMovesIndexs);
            CollectionAssert.AreEqual(expected, actual);

            canMovesIndexs = gen.Generate(35, BoardType.Main);
            expected = new ulong[Board.BoardTypeCount];
            expected[(int)BoardType.Main] = 0x000808f608080808;
            expected[(int)BoardType.SubRight] = 0;
            expected[(int)BoardType.SubBottom] = 0;
            actual = BoardStateConverter.ToBoardState(canMovesIndexs);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
