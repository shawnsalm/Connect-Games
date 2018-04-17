using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.Games.Models
{
    /// <summary>
    /// Class that represents a minimum version of IGameHistory
    /// </summary>
    public class GameHistoryMin : IGameHistory
    {
        public GameResult GameResult { get; set; }

        public IList<int> Moves { get; set; }

        public int Priority { get; set; }
    }
}
