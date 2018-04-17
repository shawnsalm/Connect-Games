using System;
using System.Collections.Generic;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Represents a connect game engine.
    /// </summary>
    public interface IGame
    {
        int CurrentPlayerIndex { get; set; }
        IList<int> CurrentMoves { get; }
        IList<IList<int>> CurrentMovesPerPlayer { get; }
        IConnectPlayer CurrentPlayer { get; }
        GameResult CurrentGameResult { get; }
        IList<IConnectPlayer> Players { get; }
        IConnectPlayer Winner { get; }
        IList<IList<int>> WinningSequences { get; }

        bool TryMove(int move);

        bool TryAddPlayers(IList<IConnectPlayer> players);

        event EventHandler<MoveMadeEventArgs> MoveMade;
    }
}
