using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.DomainModels.MoveBehaviors;
using Connect_Games.DomainModels.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Connect_Games.DomainModels.Tests.Game
{
    [TestClass]
    public class GameEngineTest
    {
        private IMoveBehavior _moveBehavior;
        private IConnectRuleExecutor _connectRuleExecutor;

        private IMoveBehavior _moveBehavior4;
        private IConnectRuleExecutor _connectRuleExecutor4;

        private IList<IConnectPlayer> _players;

        [TestInitialize]
        public void InitializeTest()
        {            
            _moveBehavior = new AnyOpenElementMoveBehavior(9);

            _connectRuleExecutor = new ConnectRuleExecutor(3, 3);
                        
            _moveBehavior4 = new BottomRowMoveBehavior(7, 6);

            _connectRuleExecutor4 = new ConnectRuleExecutor(7, 4);

            _players = new List<IConnectPlayer>
                                         {
                                             new ConnectPlayer(Guid.NewGuid(),  "John Doe"),
                                             new ConnectPlayer(Guid.NewGuid(), "Joe Smith")
                                         };

        }

        [TestMethod]
        public void PlayTicTacToeGame()
        {
            var totalGames = 0;
            var johnsWins = 0;
            var joesWins = 0;
            var ties = 0;

            while (totalGames < 10)
            {
                var totalMove = 0;

                using (var ticTacToeGame = new GameEngine(_players,
                                                           _moveBehavior,
                                                           _connectRuleExecutor, 0))
                {
                    var userMovePicker = new Random();

                    while (ticTacToeGame.CurrentGameResult == GameResult.NotComplete)
                    {
                        while (!ticTacToeGame.TryMove(userMovePicker.Next(9)))
                        {
                        }

                        totalMove++;

                    }

                    Assert.IsTrue(totalMove < 10);

                    if (ticTacToeGame.CurrentGameResult == GameResult.Win)
                    {
                        if (ticTacToeGame.Winner.Name == "Joe Smith")
                        {
                            joesWins++;
                        }
                        else
                        {
                            johnsWins++;
                        }
                    }
                    else if (ticTacToeGame.CurrentGameResult == GameResult.Tie)
                    {
                        ties++;
                    }

                }

                totalGames++;
            }

            Assert.AreEqual(10, ties + joesWins + johnsWins);
        }

        [TestMethod]
        public void PlayConnectFourGame()
        {
            var totalGames = 0;
            var johnsWins = 0;
            var joesWins = 0;
            var ties = 0;

            while (totalGames < 10)
            {
                var totalMove = 0;

                using (var connectFourGame = new GameEngine(_players,
                                                           _moveBehavior4,
                                                           _connectRuleExecutor4, 0))
                {
                    var userMovePicker = new Random();

                    while (connectFourGame.CurrentGameResult == GameResult.NotComplete)
                    {

                        while (!connectFourGame.TryMove(userMovePicker.Next(64)))
                        {
                        }

                        totalMove++;
                    }

                    Assert.IsTrue(totalMove < 65);

                    if (connectFourGame.CurrentGameResult == GameResult.Win)
                    {
                        if (connectFourGame.Winner.Name == "Joe Smith")
                        {
                            joesWins++;
                        }
                        else
                        {
                            johnsWins++;
                        }
                    }
                    else if (connectFourGame.CurrentGameResult == GameResult.Tie)
                    {
                        ties++;
                    }
                }

                totalGames++;
            }

            Assert.AreEqual(10, ties + joesWins + johnsWins);
        }
    }
}
