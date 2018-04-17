using Connect_Games.DomainModels.MoveBehaviors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Connect_Games.DomainModels.Tests.MoveBehaviors
{
    /// <summary>
    /// Board Elements/Locations in list
    /// [0][1][2]
    /// [3][4][5]
    /// [6][7][8]
    /// </summary>
    [TestClass]
    public class AnyOpenElementMoveBehaviorTest
    {
        [TestMethod]
        public void GetAvailableMovesNoCurrentMoves()
        { 
            var moveBavior = new AnyOpenElementMoveBehavior(9);

            var availableMoves = moveBavior.GetAvailableMoves(null);

            Assert.AreEqual(9, availableMoves.Count);
        }

        [TestMethod]
        public void GetAvailableMovesOneMove()
        {
            var moveBavior = new AnyOpenElementMoveBehavior(9);

            // one move made
            // [X][ ][ ]
            // [ ][ ][ ]
            // [ ][ ][ ]
            var currentMoves = new List<int> { 0 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have eight remaining moves
            Assert.AreEqual(8, availableMoves.Count);

            // and the zero location should not be an available move
            Assert.IsFalse(availableMoves.Contains(0));
        }

        [TestMethod]
        public void GetAvailableMovesInvalidMove()
        {
            var moveBavior = new AnyOpenElementMoveBehavior(9);

            // one valid move made, and one invalid move made (out of range)
            // [X][ ][ ]
            // [ ][ ][ ]
            // [ ][ ][ ]
            var currentMoves = new List<int> { 0, 10, -5 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have eight remaining moves as the invalid move should be ignored
            Assert.AreEqual(8, availableMoves.Count);

            // the zero location should not be an available move
            Assert.IsFalse(availableMoves.Contains(0));
        }

        [TestMethod]
        public void GetAvailableMovesMultiMoves()
        {
            var moveBavior = new AnyOpenElementMoveBehavior(9);

            var currentMoves = new List<int>();

            var totalMovesLength = (9);

            var randomMovePicker = new Random();

            var totalMoves = new List<int>(Enumerable.Range(0, totalMovesLength));

            // Because the behavior does not determine a winner, only available locations,
            // we can randomly make moves and check that the availables moves returned
            // are the correct amount and not already taken
            for (var moveIndex = 1; moveIndex <= totalMovesLength; moveIndex++)
            {
                var numMovesLeft = 9 - moveIndex;

                var currentMove = totalMoves[randomMovePicker.Next(0, numMovesLeft)];

                currentMoves.Add(currentMove);

                totalMoves.Remove(currentMove);

                var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

                // test remianing move count
                Assert.AreEqual(numMovesLeft, availableMoves.Count);

                // should always be zero
                Assert.AreEqual(0, availableMoves.Intersect(currentMoves).Count());
            }
        }

        [TestMethod]
        public void GetAvailableMovesAllMoves()
        {
            var moveBavior = new AnyOpenElementMoveBehavior(9);

            // all moves have been made
            // [X][O][O]
            // [O][O][X]
            // [X][X][X]
            var currentMoves = new List<int> { 0, 3, 8, 4, 5, 1, 7, 2, 6 };

            var availableMoves = moveBavior.GetAvailableMoves(currentMoves);

            // should have no remaining moves
            Assert.AreEqual(0, availableMoves.Count);
        }
    }
}
