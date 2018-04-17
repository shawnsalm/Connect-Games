using Connect_Games.DomainModels.Game;
using System.Collections.Generic;


namespace Connect_Games.DomainModels.AI
{
    /// <summary>
    /// Represents a game played.
    /// </summary>
    public interface IGameHistory
    {
        GameResult GameResult { get; }
        IList<int> Moves { get; }
        int Priority { get; }
    }
}
