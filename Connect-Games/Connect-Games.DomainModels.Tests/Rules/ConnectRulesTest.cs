using Connect_Games.DomainModels.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect_Games.DomainModels.Tests.Rules
{
    /// <summary>
    /// Board Elements/Locations in list
    /// [0][1][2]
    /// [3][4][5]
    /// [6][7][8]
    /// </summary>
    [TestClass]
    public class ConnectRulesTest
    {
        [TestMethod]
        public void DiagonalAscendingConnectRule()
        {
            IConnectRule connectRule = new DiagonalAscendingConnectRule();

            // [ ][ ][X]
            // [ ][X][ ]
            // [ ][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(2, 4, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);

            // [ ][ ][ ]
            // [ ][X][ ]
            // [X][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(4, 6, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);
        }

        [TestMethod]
        public void DiagonalAscendingConnectRuleNegTest()
        {
            IConnectRule connectRule = new DiagonalAscendingConnectRule();

            // [ ][ ][X]
            // [ ][ ][ ]
            // [X][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(2, 6, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);

            // [ ][ ][X]
            // [X][ ][ ]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(2, 3, 3);

            // should be null
            Assert.IsNull(connectRuleResult);

            // [ ][ ][ ]
            // [X][ ][X]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(3, 5, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);
        }

        [TestMethod]
        public void DiagonalDescendingConnectRule()
        {
            IConnectRule connectRule = new DiagonalDescendingConnectRule();

            // [X][ ][ ]
            // [ ][X][ ]
            // [ ][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(0, 4, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);

            // [ ][ ][ ]
            // [ ][X][ ]
            // [ ][ ][X]
            connectRuleResult = connectRule.ExecuteRule(4, 8, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);
        }

        [TestMethod]
        public void DiagonalDescendingConnectRuleNegTest()
        {
            IConnectRule connectRule = new DiagonalDescendingConnectRule();

            // [X][ ][ ]
            // [ ][ ][ ]
            // [ ][ ][X]
            var connectRuleResult = connectRule.ExecuteRule(0, 8, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);

            // [X][ ][ ]
            // [X][ ][ ]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(0, 3, 3);

            // should be null
            Assert.IsNull(connectRuleResult);
        }

        [TestMethod]
        public void HorizontalConnectRule()
        {
            IConnectRule connectRule = new HorizontalConnectRule();

            // [X][X][ ]
            // [ ][ ][ ]
            // [ ][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(0, 1, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);

            // [ ][X][X]
            // [ ][ ][ ]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(1, 2, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);
        }

        [TestMethod]
        public void HorizontalConnectRuleNegTest()
        {
            IConnectRule connectRule = new HorizontalConnectRule();

            // [X][ ][X]
            // [ ][ ][ ]
            // [ ][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(0, 8, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);

            // [X][ ][ ]
            // [X][ ][ ]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(0, 3, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);
        }

        [TestMethod]
        public void VerticalConnectRule()
        {
            IConnectRule connectRule = new VerticalConnectRule();

            // [X][ ][ ]
            // [X][ ][ ]
            // [ ][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(0, 3, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);

            // [ ][ ][ ]
            // [X][ ][ ]
            // [X][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(3, 6, 3);

            // should be true
            Assert.IsTrue(connectRuleResult ?? false);
        }

        [TestMethod]
        public void VerticalConnectRuleNegTest()
        {
            IConnectRule connectRule = new VerticalConnectRule();

            // [X][ ][ ]
            // [ ][ ][ ]
            // [X][ ][ ]
            var connectRuleResult = connectRule.ExecuteRule(0, 6, 3);

            // should be false
            Assert.IsFalse(connectRuleResult ?? false);

            // [X][X][ ]
            // [ ][ ][ ]
            // [ ][ ][ ]
            connectRuleResult = connectRule.ExecuteRule(0, 1, 3);

            // should be null
            Assert.IsNull(connectRuleResult);
        }
    }
}
