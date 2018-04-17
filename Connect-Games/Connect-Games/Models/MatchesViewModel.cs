
using System.Collections.Generic;


namespace Connect_Games.Models
{
    /// <summary>
    /// Container used to hold information about a match between computer students.
    /// </summary>
    public class MatchesViewModel
    {
        public bool EnableTicTacToe { get; set; }
        public List<PlayerStatsViewModel> TicTacToePlayerStats { get; set; }
        public bool EnableConnectFour { get; set; }
        public List<PlayerStatsViewModel> ConnectFourPlayerStats { get; set; }
        public string TicTacToeRecord { get; set; }
        public string ConnectFourRecord { get; set; }
    }
}
