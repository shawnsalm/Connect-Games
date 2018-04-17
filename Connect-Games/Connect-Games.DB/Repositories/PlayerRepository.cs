using System;
using System.Collections.Generic;
using System.Linq;

using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;


namespace Connect_Games.DAL.Repositories
{
    /// <summary>
    /// Implements IPlayerRepository.
    /// </summary>
    public class PlayerRepository : IPlayerRepository
    {
        /// <summary>
        /// Context for getting data from the database
        /// </summary>
        private readonly ConnectGamesDbContext _connectGamesDbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectGamesDbContext"></param>
        public PlayerRepository(ConnectGamesDbContext connectGamesDbContext)
        {
            _connectGamesDbContext = connectGamesDbContext;
        }

        /// <summary>
        /// Gets a player from the system
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Player GetPlayer(Guid playerId)
        {
            return _connectGamesDbContext.Players.Where(p => p.PlayerId == playerId).FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all players from the system.
        /// </summary>
        /// <returns></returns>
        public IList<IPlayer> GetPlayers()
        {
            return _connectGamesDbContext.Players.Cast<IPlayer>().ToList();
        }

        /// <summary>
        /// Updates a player's info in the system.
        /// </summary>
        /// <param name="player"></param>
        public void UpdatePlayer(Player player)
        {
            _connectGamesDbContext.Update(player);
            _connectGamesDbContext.SaveChanges();
        }
    }
}
