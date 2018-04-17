using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.Games.Models
{
    public enum GameTypes {  TicTacToe = 1, ConnectFour = 2 }

    public enum MatchTypes {  PlayerVsComputer = 1, ComputerVsComputer = 2 }

    /// <summary>
    /// Represents an abstract version of a game history record.
    /// </summary>
    public interface IConnectGameHistory
    {
        Guid GameHistoryId { get; set; }
        string MoveSequence { get; set; }
        DateTime DatePlayed { get; set; }
        Guid PlayerId1 { get; set; }
        Guid PlayerId2 { get; set; }
        Guid WinningPlayerId { get; set; }
        GameTypes GameType { get; set; }
        MatchTypes MatchType { get; set; }
    }
}
