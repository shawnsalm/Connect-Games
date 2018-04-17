using Connect_Games.DomainModels.AI;
using Connect_Games.Games.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect_Games.Games.Repositories
{
    /// <summary>
    /// Defines the database methods for the games history records.
    /// </summary>
    public interface IConnectGamesHistoryRepository : IGameHistoryRepository
    {
        void SaveGameHistory(GameHistory gameHistory);

        IQueryable<GameHistory> GetGameHistories();
    }
}
