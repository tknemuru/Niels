using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Usi;
using Niels.Tests.TestHelper;

namespace Niels.Tests.Usi
{
    [TestClass]
    public class UsiCommandReceiverTest
    {
        [TestMethod]
        public void TestReceivePosition()
        {
            UsiCommandReceiverForTest receiver = new UsiCommandReceiverForTest();
            UsiCommand command = UsiCommand.Parse("position startpos");
            receiver.ReceivePositionForTest(command);
            Assert.IsTrue(true);
        }
    }
}
