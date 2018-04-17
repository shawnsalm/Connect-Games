using Connect_Games.DomainModels.Game;
using System;
using System.Collections.Generic;


namespace Connect_Games.DomainModels.AI
{
    /// <summary>
    /// Represents the needed storage methods for the computer player.
    /// </summary>
    public interface IGameHistoryRepository
    {
        IList<IGameHistory> FindGameHistoryHighestPriority(GameResult gameResult, Guid PlayerId,
                                                    IList<int> currentMoves, int gameType);
    }
}
