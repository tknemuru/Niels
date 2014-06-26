using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.MagicBitBoardGenerator.Accessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Niels.Support.Tests.Accessor
{
    [TestClass]
    public class BoardAccesorTest
    {
        [TestMethod]
        public void TestGetSequanceIndex()
        {
            int expected = 100;
            int actual = BoardAccesor.GetSequanceIndex(56, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 23;
            actual = BoardAccesor.GetSequanceIndex(0, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 30;
            actual = BoardAccesor.GetSequanceIndex(7, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 107;
            actual = BoardAccesor.GetSequanceIndex(63, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 63;
            actual = BoardAccesor.GetSequanceIndex(31, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 78;
            actual = BoardAccesor.GetSequanceIndex(40, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 70;
            actual = BoardAccesor.GetSequanceIndex(35, BoardType.Main);
            Assert.AreEqual(expected, actual);

            expected = 12;
            actual = BoardAccesor.GetSequanceIndex(0, BoardType.SubRight);
            Assert.AreEqual(expected, actual);

            expected = 20;
            actual = BoardAccesor.GetSequanceIndex(8, BoardType.SubRight);
            Assert.AreEqual(expected, actual);

            expected = 16;
            actual = BoardAccesor.GetSequanceIndex(4, BoardType.SubRight);
            Assert.AreEqual(expected, actual);

            expected = 31;
            actual = BoardAccesor.GetSequanceIndex(7, BoardType.SubBottom);
            Assert.AreEqual(expected, actual);

            expected = 108;
            actual = BoardAccesor.GetSequanceIndex(63, BoardType.SubBottom);
            Assert.AreEqual(expected, actual);

            expected = 75;
            actual = BoardAccesor.GetSequanceIndex(39, BoardType.SubBottom);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetOriginalIndex()
        {
            int expected = 56;
            int actual = BoardAccesor.GetOriginalIndex(100);
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = BoardAccesor.GetOriginalIndex(23);
            Assert.AreEqual(expected, actual);

            expected = 7;
            actual = BoardAccesor.GetOriginalIndex(30);
            Assert.AreEqual(expected, actual);

            expected = 63;
            actual = BoardAccesor.GetOriginalIndex(107);
            Assert.AreEqual(expected, actual);

            expected = 40;
            actual = BoardAccesor.GetOriginalIndex(78);
            Assert.AreEqual(expected, actual);

            expected = 31;
            actual = BoardAccesor.GetOriginalIndex(63);
            Assert.AreEqual(expected, actual);

            expected = 20;
            actual = BoardAccesor.GetOriginalIndex(49);
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = BoardAccesor.GetOriginalIndex(12);
            Assert.AreEqual(expected, actual);

            expected = 8;
            actual = BoardAccesor.GetOriginalIndex(20);
            Assert.AreEqual(expected, actual);

            expected = 3;
            actual = BoardAccesor.GetOriginalIndex(15);
            Assert.AreEqual(expected, actual);

            expected = 7;
            actual = BoardAccesor.GetOriginalIndex(31);
            Assert.AreEqual(expected, actual);

            expected = 63;
            actual = BoardAccesor.GetOriginalIndex(108);
            Assert.AreEqual(expected, actual);

            expected = 39;
            actual = BoardAccesor.GetOriginalIndex(75);
            Assert.AreEqual(expected, actual);
        }
    }
}
