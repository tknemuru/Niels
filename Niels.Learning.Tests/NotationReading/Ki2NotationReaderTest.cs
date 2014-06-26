using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Niels.Learning.NotationReading;

namespace Niels.Learning.NotationReading.Tests
{
    [TestClass]
    public class Ki2NotationReaderTest
    {
        [TestMethod]
        public void TestKi2NotationReaderBugUp()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲７六歩,△３四歩,▲２六歩,△５四歩,▲２五歩,△５五歩,▲２四歩,△同　歩,▲同　飛,△３二金,▲３四飛,△５二飛,▲５八金右,△５六歩,▲同　歩,△８八角成,▲同　銀,△３三金,▲３六飛,△２二飛,▲２六歩,△２七角,▲４八金,△３六角成,▲同　歩,△２六飛,▲２八歩,△３六飛,▲４九玉,△６二金,▲４六歩,△３四飛,▲５五歩,△３五飛打,▲３七歩,△５五飛,▲７七銀,△５二飛,▲５八金上");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugKing()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲２六歩,△８四歩,▲２五歩,△８五歩,▲７八金,△３二金,▲２四歩,△同　歩,▲同　飛,△２三歩,▲２八飛,△８六歩,▲同　歩,△同　飛,▲８七歩,△８二飛,▲４八銀,△６二銀,▲６九玉");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugPromoteProblem()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲２六歩,△８四歩,▲２五歩,△８五歩,▲７八金,△３二金,▲１六歩,△１四歩,▲２四歩,△同　歩,▲同　飛,△２三歩,▲２六飛,△３四歩,▲７六歩,△６二銀,▲４八銀,△５四歩,▲５六歩,△８六歩,▲同　歩,△同　飛,▲８七歩,△８二飛,▲３六歩,△４一玉,▲６九玉,△５三銀,▲５七銀,△６四銀,▲４六銀,△４四歩,▲３五歩,△６五銀,▲５五歩,△７六銀,▲５四歩,△４二銀,▲６六角,△５六歩,▲５五銀,△４五歩,▲６八銀,△８七銀成,▲８三歩,△同　飛,▲８四歩,△７八成銀");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugMissingGold()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲７六歩,△３四歩,▲２六歩,△５四歩,▲５六歩,△８八角成,▲同　銀,△５七角,▲５三角,△５二飛,▲８六角成,△２四角成,▲５八金右,△３五歩,▲７七馬,△３三桂,▲４六歩,△４二銀,▲４八銀,△５三銀,▲４七金,△４四銀,▲５七銀,△３四馬,▲６八玉,△２四歩,▲７八玉,△６二玉,▲７五歩,△７二玉,▲７六馬,△４二金,▲７七銀,△５三金,▲８八玉,△２五歩,▲同　歩,△同　馬,▲３六歩,△同　歩,▲７八金,△３五銀,▲５五歩,△２六歩,▲５四歩,△同　金,▲５六銀,△６二金,▲６六銀,△３四馬,▲５八飛,△５五歩,▲５三歩,△同金引,▲５五銀直,△２二飛,▲５四銀,△５二金引,▲５三歩,△４二金,▲７四歩,△同　歩,▲６五銀上,△５五歩,▲同　飛,△４四銀,▲３五歩,△１二馬,▲５六飛,△５三金左,▲同銀成,△同　銀,▲３四金,△２五飛,▲２三歩,△５五歩,▲同　飛,△５四歩,▲５七飛,△２七歩成,▲３六金,△２三飛,▲７三歩,△同　桂,▲７四銀,△７五歩,▲同　馬,△６四銀,▲７三銀成,△同　金");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugHandValuePawn()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲７六歩,△８四歩,▲５六歩,△８五歩,▲７七角,△５四歩,▲６八銀,△３四歩,▲４八銀,△６二銀,▲２六歩,△５三銀,▲２五歩,△７七角成,▲同　銀,△２二銀,▲５七銀,△３三銀,▲５八金右,△３二金,▲６六歩,△７四歩,▲６七金,△４一玉,▲６五歩,△６二金,▲６六銀右,△７三桂,▲４六角,△８一飛,▲６八玉,△１四歩,▲７八玉,△３一玉,▲８八玉,△４四銀右,▲７八金,△２二玉,▲１六歩,△５一飛,▲９六歩,△９四歩,▲７五歩,△同　歩,▲１五歩,△同　歩,▲同　香,△同　香,▲７四歩,△７一飛,▲７三歩成,△同　金,▲５五歩,△同　歩,▲２四歩,△同　歩,▲３六桂,△３五銀,▲２四桂,△２三金,▲５五角,△５四歩,▲７三角成,△同　飛,▲５八飛,△２七角,▲２五歩");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugMissingLeftMove()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲２六歩,△３四歩,▲７六歩,△３二金,▲２五歩,△３三角,▲同角成,△同　金,▲８八銀,△２二飛,▲７七銀,△６二玉,▲６八玉,△７二玉,▲７八玉,△４二銀,▲４八銀,△２四歩,▲同　歩,△同　金,▲４六歩,△２五歩,▲４七銀,△５四歩,▲３六歩,△６二銀,▲１六歩,△９四歩,▲９六歩,△７四歩,▲６八金,△３三桂,▲１五歩,△６四歩,▲５八金上,△６三銀,▲４五歩,△６二角,▲４六銀,△２六歩,▲３七桂,△２五桂,▲２九飛,△３七桂成,▲同　銀,△２七桂,▲２八歩,△１九桂成,▲同　飛,△２三金,▲４八金,△３三金,▲３八金,△２一飛,▲４六銀,△８四歩,▲５六歩,△５三銀,▲４九飛,△２五飛,▲４七飛,△７三角,▲６六歩,△６二金,▲６七桂,△１五飛,▲２二角,△３二金,▲１一角成,△１九飛成,▲７五歩,△同　歩,▲同　桂,△７四銀,▲７六香,△８五歩,▲２一馬");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugMissingLeftMoveRightToBottom()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲２六歩,△３四歩,▲７六歩,△５四歩,▲２五歩,△６二銀,▲２四歩,△同　歩,▲同　飛,△３二金,▲６六歩,△５三銀,▲３四飛,△２三金,▲３六飛,△２四金,▲２六飛,△３三角,▲２八飛,△２二飛,▲３八金,△２五金,▲４八銀,△２六歩,▲４六歩,△６四歩,▲６八金,△４二銀上,▲４七銀,△６五歩,▲同　歩,△８八角成,▲同　銀,△３五金,▲４八玉,△２七角,▲４九角,△４六金,▲同　銀,△３八角成,▲同　角,△２七金,▲５六角,△２八金,▲２三歩,△３二飛,▲６四歩,△同　銀,▲９六角,△１九金,▲６三角成,△５二金,▲８一馬,△２八飛,▲５九玉,△６七歩,▲５八金,△６五香,▲７七銀,△２九金");
        }

        [TestMethod]
        public void TestKi2NotationReaderBugMissingLeftMoveGoldYose()
        {
            Ki2NotationReaderForTest reader = new Ki2NotationReaderForTest();
            reader.ReadForTest(@"▲２六歩,△８四歩,▲２五歩,△８五歩,▲７八金,△３二金,▲２四歩,△同　歩,▲同　飛,△２三歩,▲２八飛,△３四歩,▲７六歩,△８六歩,▲同　歩,△同　飛,▲８七歩,△８二飛,▲４八銀,△６二銀,▲５六歩,△５四歩,▲６九玉,△５二金,▲５七銀,△５三銀,▲３六歩,△４一玉,▲４六銀,△４四歩,▲３五歩,△４五歩,▲同　銀,△８五飛,▲３六銀,△３五歩,▲８六歩,△同　飛,▲３五銀,△８五飛,▲４六銀,△４四銀,▲８七歩,△４三金右,▲６八銀,△４二銀,▲５七銀引,△４五歩,▲４八金,△５三銀上,▲３七金,△８二飛,▲２六金,△５五歩,▲同　歩,△同　銀,▲５六歩,△６四銀引,▲２二角成,△同　金,▲３五金,△４六歩,▲同　歩,△３八歩,▲１六角,△４二歩,▲３八角,△３四歩,▲３六金,△６五銀,▲７七銀,△７六銀,▲同　銀,△５四角,▲４七角,△７六角,▲７七歩,△８七角成,▲同　金,△同飛成,▲８八歩,△８二龍,▲７八玉,△６四銀,▲３七金,△３一玉,▲３六角,△５四歩,▲４七金,△１四歩,▲６六銀,△３五歩,▲４五角,△３四銀,▲１八角,△１五歩,▲２七角,△５二龍,▲５七金,△３二金,▲８七歩,△７四歩,▲９六歩,△９四歩,▲３六歩,△同　歩,▲同　角,△３五歩,▲６九角,△７二龍,▲１六歩,△７三桂,▲１五歩,△７五歩,▲７四銀,△３三金寄,▲１四歩,△１二歩,▲３七桂,△４三金寄");
        }
    }
}
