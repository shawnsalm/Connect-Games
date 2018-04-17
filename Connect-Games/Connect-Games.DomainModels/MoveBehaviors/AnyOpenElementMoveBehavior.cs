using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect_Games.DomainModels.MoveBehaviors
{
    /// <summary>
    /// Models the available moves when the player
    /// can move anywhere on the board.  Example would be
    /// tic tac toe.
    /// </summary>
    [Serializable]
    public sealed class AnyOpenElementMoveBehavior : IMoveBehavior
    {
        private readonly int _totalMovesLength;

        public AnyOpenElementMoveBehavior(int totalMovesLength)
        {
            _totalMovesLength = totalMovesLength;
        }

        /// <summary>
        /// Returns the available moves left given a list of 
        /// already made moves.
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IList<int> GetAvailableMoves(IEnumerable<int> moves)
        {
            // Seed the total moves available.
            var totalMoves = new List<int>(Enumerable.Range(0, _totalMovesLength));

            // If we were passed a null list, create an empty one.
            var currentMoves = (moves == null) ? new List<int>() : new List<int>(moves);

            // Query the list of total available moves and remove any the have already been
            // made.  Make sure that, if the user passes a move(s) that are not in a valid
            // range, the we don't include them in the available list.
            var query = from move in totalMoves.Except(currentMoves)
                        select move;

            return query.ToList();
        }
    }
}
