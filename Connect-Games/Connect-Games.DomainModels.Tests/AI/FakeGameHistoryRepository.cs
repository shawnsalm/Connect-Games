using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.DomainModels.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect_Games.DomainModels.Tests.AI
{
    public class FakeGameHistoryRepository : IGameHistoryRepository
    {
        private List<IGameHistory> _gamehistories;

        public FakeGameHistoryRepository(List<IGameHistory> gamehistories)
        {
            _gamehistories = gamehistories;
        }

        public IList<IGameHistory> FindGameHistoryHighestPriority(GameResult gameResult, Guid PlayerId, IList<int> currentMoves, int gameType)
        {
            return _gamehistories.OrderByDescending(g => g.Priority).ToList();
        }
    }
}
