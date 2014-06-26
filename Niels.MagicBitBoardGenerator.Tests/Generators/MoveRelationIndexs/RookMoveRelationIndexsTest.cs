using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Generators;
using Niels.MagicBitBoardGenerator.Config;

namespace Niels.MagicBitBoardGenerator.Tests.Generators
{
    [TestClass]
    public class RookCanMoveIndexsGeneratorTest
    {
        [TestMethod]
        public void TestGenerate()
        {
            IEnumerable<int> expected = new List<int>()
            {
                89, 78, 67, 56, 45, 34, 23,
                101, 102, 103, 104, 105, 106, 107
            };
            this.TestGenerate(expected, 56, BoardType.Main);

            expected = new List<int>()
            {
                24, 25, 26, 27, 28, 29, 30,
                34, 45, 56, 67, 78, 89
            };
            this.TestGenerate(expected, 0, BoardType.Main);

            expected = new List<int>()
            {
                29, 28, 27, 26, 25, 24,
                41, 52, 63, 74, 85, 96
            };
            this.TestGenerate(expected, 7, BoardType.Main);

            expected = new List<int>()
            {
                106, 105, 104, 103, 102, 101,
                96, 85, 74, 63, 52, 41, 30
            };
            this.TestGenerate(expected, 63, BoardType.Main);

            expected = new List<int>()
            {
                13, 14, 15, 16, 17, 18, 19,
                23, 34, 45, 56, 67, 78, 89
            };
            this.TestGenerate(expected, 0, BoardType.SubRight);

            expected = new List<int>()
            {
                13, 14, 15, 16, 17, 18, 19,
                31, 42, 53, 64, 75, 86, 97
            };
            this.TestGenerate(expected, 8, BoardType.SubRight);

            expected = new List<int>()
            {
                24, 25, 26, 27, 28, 29 , 30,
                42, 53, 64, 75, 86, 97
            };
            this.TestGenerate(expected, 7, BoardType.SubBottom);

            expected = new List<int>()
            {
                101, 102, 103, 104, 105, 106, 107,
                31, 42, 53, 64, 75, 86, 97
            };
            this.TestGenerate(expected, 63, BoardType.SubBottom);

            expected = new List<int>()
            {
                70, 69, 68,
                93, 82,
                60, 49, 38, 27,
                72, 73, 74
            };
            this.TestGenerate(expected, 36, BoardType.Main);

            expected = new List<int>()
            {
                13, 14, 15, 16,
                18, 19,
                28, 39, 50, 61, 72, 83, 94
            };
            this.TestGenerate(expected, 5, BoardType.SubRight);

            expected = new List<int>()
            {
                79, 80, 81, 82, 83, 84, 85,
                97,
                75, 64, 53, 42, 31
            };
            this.TestGenerate(expected, 47, BoardType.SubBottom);
        }

        private void TestGenerate(IEnumerable<int> expected, int index, BoardType boardType)
        {
            var gen = MagicBitBoardGeneratorConfigProvider.GetRookConfig().MoveRelationIndexsGenerator;

            expected = expected.OrderBy(item => item);

            IEnumerable<int> actual = gen.Generate(index, boardType);
            actual = actual.OrderBy(item => item);

            CollectionAssert.AreEqual(expected.ToList(), actual.ToList());
        }
    }
}
