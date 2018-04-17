using System;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    [Serializable]
    public class ConnectPlayer : IConnectPlayer
    {       
        public ConnectPlayer(Guid playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }

        public string Name { get; set; }

        public Guid PlayerId { get; set; }
    }
}
