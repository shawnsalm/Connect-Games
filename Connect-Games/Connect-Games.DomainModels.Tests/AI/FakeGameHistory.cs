using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using System;
using System.Collections.Generic;


namespace Connect_Games.DomainModels.Tests.AI
{
    public class FakeGameHistory : IGameHistory
    {
        public GameResult GameResult { get; set; } 

        public IList<int> Moves { get; set; }

        public int Priority { get; set; }
    }
}
