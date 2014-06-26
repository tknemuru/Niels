using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Tests.TestHelper;
using Niels.Helper;
using Niels.Collections;
using Niels.Learning.NotationReading;

namespace Niels.Learning.NotationReading.Tests
{
    /// <summary>
    /// .ki2形式の棋譜の読み込み機能を提供します。
    /// </summary>
    internal class Ki2NotationReaderForTest : Ki2NotationReader
    {
        /// <summary>
        /// .ki2形式の棋譜ファイルの読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal NotationInformation ReadForTest(string ki2Moves)
        {
            // 指し手の変換
            // TODO:Testプロジェクトの参照は無理がある
            BoardContextForTest context = new BoardContextForTest(Turn.Black);
            context.SetDefaultStartPosition();
            List<string> splitKi2Moves = ki2Moves.Split(',').ToList();
            string moveLog = string.Empty;
            foreach (string ki2Move in splitKi2Moves)
            {
                moveLog += ki2Move + ",";
                FileHelper.WriteLine(moveLog);
                FileHelper.WriteLine(context.ToString());
                uint move = 0;
                move = this.ConvertToNielsMove(ki2Move, context);

                context.PutPiece(move);
                context.ChangeTurn();
            }

            return new NotationInformation();
        }
    }
}
