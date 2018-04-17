using Connect_Games.DomainModels.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.Games.Models
{
    /// <summary>
    /// Interface that defines an abstract version of a player record
    /// </summary>
    public interface IPlayer : IConnectPlayer
    {
        Guid ComputerPlayerId { get; set; }
        bool ComputerAvailableTicTacToe { get; set; }
        bool ComputerAvailableConnectFour { get; set; }
    }
}
