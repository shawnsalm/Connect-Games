using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.Games.Models
{
    /// <summary>
    /// Class that represents a record in the database.
    /// </summary>
    public class GameHistory : IConnectGameHistory
    {
        public Guid GameHistoryId { get; set; }
        public string MoveSequence { get; set; }
        public DateTime DatePlayed { get; set; }
        public Guid PlayerId1 { get; set; }
        public Guid PlayerId2 { get; set; }
        public Guid WinningPlayerId { get; set; }
        public GameTypes GameType { get; set; }
        public MatchTypes MatchType { get; set; }
    }
}
