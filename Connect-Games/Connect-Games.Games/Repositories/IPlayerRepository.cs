using Connect_Games.Games.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.Games.Repositories
{
    /// <summary>
    /// Defines methods for managing player records.
    /// </summary>
    public interface IPlayerRepository
    {
        Player GetPlayer(Guid playerId);
        IList<IPlayer> GetPlayers();
        void UpdatePlayer(Player player);
    }
}
