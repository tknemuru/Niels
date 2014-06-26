using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Boards;
using Niels.Usi;

namespace Niels.Tests.TestHelper
{
    internal class UsiCommandReceiverForTest : UsiCommandReceiver
    {
        /// <summary>
        /// positionコマンドを受信します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal BoardContext ReceivePositionForTest(UsiCommand command)
        {
            return base.ReceivePosition(command);
        }
    }
}
