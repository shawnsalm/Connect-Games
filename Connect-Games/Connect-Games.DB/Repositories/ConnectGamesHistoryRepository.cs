using System;
using System.Collections.Generic;
using System.Linq;

using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;


namespace Connect_Games.DAL.Repositories
{
    /// <summary>
    /// Implements IConnectGamesHistoryRepository
    /// </summary>
    public class ConnectGamesHistoryRepository : IConnectGamesHistoryRepository
    {
        /// <summary>
        /// Context for getting data from the database
        /// </summary>
        private readonly ConnectGamesDbContext _connectGamesDbContext;

        public ConnectGamesHistoryRepository(ConnectGamesDbContext connectGamesDbContext)
        {
            _connectGamesDbContext = connectGamesDbContext;
        }

        /// <summary>
        /// Finds the game history sequences that match the current sequence for the given
        /// computer student and type of game.
        /// </summary>
        /// <param name="gameResult"></param>
        /// <param name="PlayerId"></param>
        /// <param name="currentMoves"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public IList<IGameHistory> FindGameHistoryHighestPriority(GameResult gameResult, Guid PlayerId, IList<int> currentMoves, int gameType)
        {
            string sequence = string.Join(",", currentMoves);
                                                
            var connectGameHistories = _connectGamesDbContext.GameHistories.Where(gh => (gh.PlayerId1 == PlayerId ||
                                                                gh.PlayerId2 == PlayerId) &&
                                                                (gh.MoveSequence != null &&
                                                                (sequence.Length == 0 || gh.MoveSequence.StartsWith(sequence)))
                                                                && gh.GameType == (GameTypes)gameType)
                        .GroupBy(gh => gh.MoveSequence).OrderByDescending(gh => gh.Count());

            List<IGameHistory> gameHistoryMins = new List<IGameHistory>();

            int priority = 1;

            foreach (var connectGameHistory in connectGameHistories)
            {
                gameHistoryMins.Add(new GameHistoryMin()
                {
                    GameResult = GameResult.Win,
                    Priority = priority++,
                    Moves = connectGameHistory.Key.Split(new char[] { ',' }).Select(gh1 => int.Parse(gh1)).ToArray()
                });
            }

            return gameHistoryMins;
        }

        /// <summary>
        /// Saves a games sequence.
        /// </summary>
        /// <param name="gameHistory"></param>
        public void SaveGameHistory(GameHistory gameHistory)
        {
            _connectGamesDbContext.GameHistories.Add(gameHistory);
            _connectGamesDbContext.SaveChanges();
        }

        /// <summary>
        /// Returns all the game histories objects using IQueryable (lazy loading)
        /// so that the actual query is not likely to retrieve all of the history records.
        /// </summary>
        /// <returns></returns>
        public IQueryable<GameHistory> GetGameHistories()
        {
            return _connectGamesDbContext.GameHistories;
        }
       
    }
}
