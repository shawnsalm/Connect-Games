using System.Collections.Generic;

namespace Connect_Games.DomainModels.MoveBehaviors
{
    /// <summary>
    /// Interface defining the possible moves for a
    /// given behavior.
    /// </summary>
    public interface IMoveBehavior
    {
        /// <summary>
        /// Returns the available moves given the moves that
        /// have already been made.
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        IList<int> GetAvailableMoves(IEnumerable<int> moves);
    }
}
