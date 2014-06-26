using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Niels.Collections;
using Niels.Searchs;
using Niels.Boards;

namespace Niels.Tests.Searchs
{
    [TestClass]
    public class FoolSearchTest
    {
        [TestMethod]
        public void GetMove()
        {
            BoardContext context = new BoardContext();


            FoolSearch search = new FoolSearch();

            var expectedValue = 99;
            var actualValue = search.GetMove();

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
