using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Niels.Usi;
using System.IO;
using Niels.Tests.TestHelper;
using Niels.Collections;

namespace Niels.Tests.Usi
{
    [TestClass]
    public class UsiCommandSenderTest
    {
        [TestMethod]
        public void TestSendUsiOk()
        {
            UsiCommandSender sender = new UsiCommandSender();
            sender.SendNameAuthor();
            sender.SendUsiOk();

            List<string> expected = new List<string>();
            expected.Add("id name Niels");
            expected.Add("id author tknemuru");
            expected.Add("usiok");

            List<string> actual = sender.SendLog;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSendReadyOk()
        {
            UsiCommandSender sender = new UsiCommandSender();
            sender.SendReadyOk();

            List<string> expected = new List<string>();
            expected.Add("readyok");

            List<string> actual = sender.SendLog;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSendBestMove()
        {
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            UsiCommandSender sender = new UsiCommandSender();
            sender.SendBestMove(context);
        }
    }
}
