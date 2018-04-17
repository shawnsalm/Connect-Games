using System;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public interface IConnectPlayer
    {
        Guid PlayerId { get; set; }
        string Name { get; set; }
    }
}
