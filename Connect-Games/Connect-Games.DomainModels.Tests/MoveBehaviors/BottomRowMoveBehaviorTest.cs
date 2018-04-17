using Connect_Games.DomainModels.MoveBehaviors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect_Games.DomainModels.Tests.MoveBehaviors
{
    /// <summary>
    /// Board Elements/Locations in list
    /// [ 0][ 1][ 2][ 3][ 4]
    /// [ 5][ 6][ 7][ 8][ 9]
    /// [10][11][12][13][14]
    /// [15][16][17][18][19]
    /// [20][21][22][23][24]
    /// [25][26][27][28][29]
    /// </summary>
    [TestClass]
    public class BottomRowMoveBehaviorTest
    {
        [TestMethod]
        public void GetAvailableMovesOneMove()
        {            
            var moveBavior = new BottomRowMoveBehavior(5,6);

            // one move made
            // [ ][ ][ ][ ][ ]
            // [ ][ ][ ][ ][ ]
            // [ ][ ][ ][ ][ ]
            // [ ][ ][ ][ ][ ]
            // [ ][ ][ ][ ][ ]
            // [X][ ][ ][ ][ ]
            var currentMoves = new List<int> { 25 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have 5 remaining moves
            Assert.AreEqual(5, availableMoves.Count);

            // and the 25th location should not be an available move
            Assert.IsFalse(availableMoves.Contains(25));

        }

        [TestMethod]
        public void GetAvailableMovesMultiMoves()
        {
            var moveBavior = new BottomRowMoveBehavior(5, 6);

            // moves made
            // [X][ ][ ][ ][ ]
            // [X][X][ ][ ][ ]
            // [X][X][X][ ][ ]
            // [X][X][X][X][ ]
            // [X][X][X][X][X]
            // [X][X][X][X][X]
            var currentMoves = new List<int> { 0, 5, 10, 15, 20, 25,
                                               6, 11, 16, 21, 26,
                                               12, 17, 22, 27,
                                               18, 23, 28,
                                               24, 29};

            var expectedAvailableMoves = new List<int> { 1, 7, 13, 19 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have 4 remaining moves
            Assert.AreEqual(4, availableMoves.Count);

            // test the list to make sure they are equal
            CollectionAssert.AreEquivalent(expectedAvailableMoves, (List<int>)availableMoves);
        }

        [TestMethod]
        public void GetAvailableMovesInvalidMoves()
        {
            var moveBavior = new BottomRowMoveBehavior(5, 6);

            // moves made plus two invalid moves
            // [X][ ][ ][ ][ ]
            // [X][X][ ][ ][ ]
            // [X][X][X][ ][ ]
            // [X][X][X][X][ ]
            // [X][X][X][X][X]
            // [X][X][X][X][X]
            var currentMoves = new List<int> { 0, 5, 10, 15, 20, 25,
                                               6, 11, 16, 21, 26,
                                               12, 17, 22, 27,
                                               18, 23, 28,
                                               24, 29,
                                               -1, 30};

            var expectedAvailableMoves = new List<int> { 1, 7, 13, 19 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have 4 remaining moves
            Assert.AreEqual(4, availableMoves.Count);

            // test the list to make sure they are equal
            CollectionAssert.AreEquivalent(expectedAvailableMoves, (List<int>)availableMoves);
        }

        [TestMethod]
        public void GetAvailableMovesAllMoves()
        {
            var moveBavior = new BottomRowMoveBehavior(5, 6);

            // board it full
            var currentMoves = new List<int>(Enumerable.Range(0, 30));

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have 0 remaining moves
            Assert.AreEqual(0, availableMoves.Count);
        }

        [TestMethod]
        public void GetAvailableMovesNoMoves()
        {
            var moveBavior = new BottomRowMoveBehavior(5, 6);

            var availableMoves = moveBavior.GetAvailableMoves(null);

            // should have 5 remaining moves
            Assert.AreEqual(5, availableMoves.Count);
        }
    }
}
