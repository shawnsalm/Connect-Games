using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;

namespace Connect_Games.Games.Models
{
    /// <summary>
    /// Class the represents a record in the database
    /// </summary>
    public class Player : IdentityUser, IPlayer
    {
        public Guid ComputerPlayerId { get; set; }
        public bool ComputerAvailableTicTacToe { get; set; }
        public bool ComputerAvailableConnectFour { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
    }
}
