using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Represents a match between two computer players.
    /// </summary>
    public interface IMatch
    {
        void PlayGame(IGame game);
    }
}
