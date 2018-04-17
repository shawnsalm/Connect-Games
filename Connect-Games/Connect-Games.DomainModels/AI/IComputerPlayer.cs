using Connect_Games.DomainModels.Game;
using System.Collections.Generic;


namespace Connect_Games.DomainModels.AI
{
    /// <summary>
    /// Represents a computer player in the game
    /// </summary>
    public interface IComputerPlayer : IConnectPlayer
    {
        int CalculateNextMove(IList<int> currentMoves, IList<IList<int>> allPlayersMoves);
    }
}
