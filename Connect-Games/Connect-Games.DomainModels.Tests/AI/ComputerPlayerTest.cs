using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.DomainModels.MoveBehaviors;
using Connect_Games.DomainModels.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.DomainModels.Tests.AI
{
    [TestClass]
    public class ComputerPlayerTest
    {
        private IMoveBehavior _moveBehavior;
        private IConnectRuleExecutor _connectRuleExecutor;
        private IGameHistoryRepository _gameHistoryRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            var gameHistoryList =
               new List<IGameHistory>
                    {
                        new FakeGameHistory
                            {GameResult = GameResult.Win, Moves = new List<int> { 8, 5, 4, 2, 0 }, Priority = 10},
                        new FakeGameHistory
                            {GameResult = GameResult.Win, Moves = new List<int> { 0, 4, 8, 6, 2, 1, 5 }, Priority = 15},
                        new FakeGameHistory
                            {GameResult = GameResult.Tie, Moves = new List<int> { 0, 3, 6, 1, 5, 4, 7, 8, 2 }, Priority = 10},
                        new FakeGameHistory
                            {GameResult = GameResult.Tie, Moves = new List<int> { 0, 4, 6, 3, 5, 1, 7, 8, 2 }, Priority = 10}
                    };

            _gameHistoryRepository = new FakeGameHistoryRepository(gameHistoryList); 
                                    
            _moveBehavior = new AnyOpenElementMoveBehavior(9);

            _connectRuleExecutor = new ConnectRuleExecutor(3, 3);
          
        }

        [TestMethod]
        public void CalculateNextMoveWin()
        {
            var computerPlayer = new ComputerPlayer(_moveBehavior,
                                                    _connectRuleExecutor,
                                                    _gameHistoryRepository,
                                                    1, new Guid(), 1, "Skynet");

            // [X][O][X]
            // [ ][O][ ]
            // [O][ ][X]
            IList<int> currentMoves = new List<int> { 0, 4, 8, 6, 2, 1};

            IList<IList<int>> allPlayersMoves = new List<IList<int>>
                                                    {
                                                        new List<int> {0, 8, 2},
                                                        new List<int> {4, 6, 1}
                                                    };

            var nextMove = computerPlayer.CalculateNextMove(currentMoves, allPlayersMoves);

            Assert.AreEqual(7, nextMove); 
        }

        [TestMethod]
        public void CalculateNextMoveBlock()
        {
            var computerPlayer = new ComputerPlayer(_moveBehavior,
                                                    _connectRuleExecutor,
                                                    _gameHistoryRepository,
                                                    1, new Guid(), 1, "Skynet");

            // [X][ ][ ]
            // [ ][O][ ]
            // [O][ ][X]
            IList<int> currentMoves = new List<int> { 0, 4, 8, 6 };

            IList<IList<int>> allPlayersMoves = new List<IList<int>>
                                                    {
                                                        new List<int> {0, 8 },
                                                        new List<int> {4, 6 }
                                                    };

            var nextMove = computerPlayer.CalculateNextMove(currentMoves, allPlayersMoves);

            Assert.AreEqual(2, nextMove); 
        }

        [TestMethod]
        public void CalculateNextMoveWinHistory()
        {
            var computerPlayer = new ComputerPlayer(_moveBehavior,
                                                     _connectRuleExecutor,
                                                     _gameHistoryRepository,
                                                     0, new Guid(), 1, "Skynet");

            // [ ][ ][ ]
            // [ ][ ][ ]
            // [ ][ ][ ]
            IList<int> currentMoves = new List<int> ();

            IList<IList<int>> allPlayersMoves = new List<IList<int>>
                                                    {
                                                        new List<int> (),
                                                        new List<int> ()
                                                    };

            var nextMove = computerPlayer.CalculateNextMove(currentMoves, allPlayersMoves);

            Assert.AreEqual(0, nextMove); 
        }
              
        [TestMethod]
        public void CalculateNextMoveRandom()
        {
            var computerPlayer = new ComputerPlayer(_moveBehavior,
                                                     _connectRuleExecutor,
                                                     _gameHistoryRepository,
                                                     1, new Guid(), 1, "Skynet");

            // [X][ ][ ]
            // [ ][ ][O]
            // [ ][ ][ ]
            IList<int> currentMoves = new List<int> { 0, 5 };

            IList<IList<int>> allPlayersMoves = new List<IList<int>>
                                                    {
                                                        new List<int> { 0 },
                                                        new List<int> { 5 }
                                                    };

            var nextMove = computerPlayer.CalculateNextMove(currentMoves, allPlayersMoves);

            Assert.IsTrue(nextMove != 0 && nextMove != 5); 
        }
    }
}
