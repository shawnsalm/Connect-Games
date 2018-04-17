using Connect_Games.DomainModels.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Connect_Games.DomainModels.Tests.Rules
{
    /// <summary>
    /// Board Elements/Locations in list
    /// [0][1][2]
    /// [3][4][5]
    /// [6][7][8]
    /// </summary>
    [TestClass]
    public class ConnectRuleExecutorTest
    {
        [TestMethod]
        public void ConnectRuleExecutorIsWinnerHorizontal()
        {
            IConnectRuleExecutor connectRuleExecutor =
                new ConnectRuleExecutor(3, 3);

            // [X][X][X]
            // [ ][ ][ ]
            // [ ][ ][ ]
            var playersMoves = new List<int> { 0, 1, 2 };

            IList<IList<int>> winningSequences;

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);

            // [ ][ ][ ]
            // [X][X][X]
            // [ ][ ][ ]
            playersMoves = new List<int> { 3, 4, 5 };

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);

            // [ ][ ][ ]
            // [ ][ ][ ]
            // [X][X][X]
            playersMoves = new List<int> { 6, 7, 8 };

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);
        }

        [TestMethod]
        public void ConnectRuleExecutorIsWinnerVertical()
        {
            IConnectRuleExecutor connectRuleExecutor =
                new ConnectRuleExecutor(3, 3);

            // [X][ ][ ]
            // [X][ ][ ]
            // [X][ ][ ]
            var playersMoves = new List<int> { 0, 3, 6 };

            IList<IList<int>> winningSequences;

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);

            // [ ][X][ ]
            // [ ][X][ ]
            // [ ][X][ ]
            playersMoves = new List<int> { 1, 4, 7 };

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);

            // [ ][ ][X]
            // [ ][ ][X]
            // [ ][ ][X]
            playersMoves = new List<int> { 2, 5, 8 };

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);
        }

        [TestMethod]
        public void ConnectRuleExecutorIsWinnerDiagonalLeftToRight()
        {
            IConnectRuleExecutor connectRuleExecutor =
                new ConnectRuleExecutor(3, 3);

            // [X][ ][ ]
            // [ ][X][ ]
            // [ ][ ][X]
            var playersMoves = new List<int> { 0, 4, 8 };

            IList<IList<int>> winningSequences;

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);
        }

        [TestMethod]
        public void ConnectRuleExecutorIsWinnerDiagonalRightToLeft()
        {
            IConnectRuleExecutor connectRuleExecutor =
                new ConnectRuleExecutor(3, 3);

            // [ ][ ][X]
            // [ ][X][ ]
            // [X][ ][ ]
            
            var playersMoves = new List<int> { 2, 4, 6 };

            IList<IList<int>> winningSequences;

            Assert.IsTrue(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            CollectionAssert.AreEquivalent(playersMoves, (List<int>)winningSequences[0]);
        }

        [TestMethod]
        public void ConnectRuleExecutorIsWinnerNeg()
        {
            IConnectRuleExecutor connectRuleExecutor =
              new ConnectRuleExecutor(3, 3);

            // [X][ ][X]
            // [ ][ ][ ]
            // [X][ ][X]
            // No winner
            var playersMoves = new List<int> { 0, 6, 2, 8 };

            IList<IList<int>> winningSequences;

            Assert.IsFalse(connectRuleExecutor.IsWinner(playersMoves, out winningSequences));

            // There should be 0
            Assert.AreEqual(0, winningSequences.Count);
        }
    }
}
