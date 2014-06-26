using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Generators;
using Niels.MagicBitBoardGenerator.Accessor;
using Niels.MagicBitBoardGenerator.Config;

namespace Niels.MagicBitBoardGenerator.Tests.Generators
{
    [TestClass]
    public class BishopMoveRelationIndexsTest
    {
        [TestMethod]
        public void TestBishopMoveRelationIndexsGenerate()
        {
            // メイン
            IEnumerable<int> expected = new List<int>()
            {
                70, 80, 90,
                48, 36, 24,
                50, 40, 30,
                72, 84, 96
            };
            this.TestGenerate(expected, 60);

            // 右
            expected = new List<int>()
            {
                30, 40, 50, 60, 70, 80, 90
            };
            this.TestGenerate(expected, 20);

            // 下
            expected = new List<int>()
            {
                96, 84, 72, 60, 48, 36, 24
            };
            this.TestGenerate(expected, 108);
        }

        private void TestGenerate(IEnumerable<int> expected, int seqIndex)
        {
            int index = BoardAccesor.GetOriginalIndex(seqIndex);
            BoardType boardType = BoardAccesor.GetBoardType(seqIndex);

            MagicBitBoardGeneratorConfig config = MagicBitBoardGeneratorConfigProvider.GetBishopConfig();
            var gen = config.MoveRelationIndexsGenerator;

            expected = expected.OrderBy(item => item);

            IEnumerable<int> actual = gen.Generate(index, boardType);
            actual = actual.OrderBy(item => item);

            CollectionAssert.AreEqual(expected.ToList(), actual.ToList());
        }
    }
}
