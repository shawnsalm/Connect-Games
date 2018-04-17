using System;

namespace Connect_Games.Models
{
    /// <summary>
    /// Container used to hold information for the player’s statistics.
    /// </summary>
    public class PlayerStatsViewModel
    {
        public Guid PlayerId { get; set; }
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
