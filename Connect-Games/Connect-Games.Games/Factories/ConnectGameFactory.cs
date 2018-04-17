
using System.Collections.Generic;

using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.DomainModels.MoveBehaviors;
using Connect_Games.DomainModels.Rules;
using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;

namespace Connect_Games.Games.Factories
{
    /// <summary>
    /// Creates Tic-Tac-Toe and Connect Four IGame objects for player training computer student and computer student playing another computer student.
    /// </summary>
    public class ConnectGameFactory : IConnectGameFactory
    {
        private IConnectGamesHistoryRepository _connectGamesHistoryRepository;

        public ConnectGameFactory(IConnectGamesHistoryRepository connectGamesHistoryRepository)
        {
            _connectGamesHistoryRepository = connectGamesHistoryRepository;
        }

        /// <summary>
        /// Creates a Connect Four game between a player and a computer student.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="doesPlayerGoFirst"></param>
        /// <returns></returns>
        public IGame CreateConnectFourGame(int[] sequence, Player player, bool doesPlayerGoFirst)
        {
            var moveBehavior = new BottomRowMoveBehavior(7, 6);

            var connectRuleExecutor = new ConnectRuleExecutor(7, 4);

            var players = new List<IConnectPlayer>();
            
            if (doesPlayerGoFirst)
            {
                players.Add(player);
                players.Add(GetComputerPlayer(player,
                                                moveBehavior,
                                                connectRuleExecutor,
                                                _connectGamesHistoryRepository,
                                                1,
                                                GameTypes.ConnectFour,
                                                "Computer Student"));
            }
            else
            {
                players.Add(GetComputerPlayer(player,
                                                moveBehavior,
                                                connectRuleExecutor,
                                                _connectGamesHistoryRepository,
                                                0,
                                                GameTypes.ConnectFour,
                                                "Computer Student"));
                players.Add(player);
            }

            return new GameEngine(players, moveBehavior, connectRuleExecutor, 
                                    doesPlayerGoFirst ? 0 : 1, -1, sequence);
        }

        /// <summary>
        /// Creates a Connect Four game between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public IGame CreateConnectFourGame(int[] sequence, List<Player> players)
        {
            var moveBehavior = new BottomRowMoveBehavior(7, 6);

            var connectRuleExecutor = new ConnectRuleExecutor(7, 4);

            IList<IConnectPlayer> computerPlayers = new List<IConnectPlayer>();

            computerPlayers.Add(GetComputerPlayer(players[0], moveBehavior, connectRuleExecutor, _connectGamesHistoryRepository, 0, GameTypes.ConnectFour, players[0].Name));
            computerPlayers.Add(GetComputerPlayer(players[1], moveBehavior, connectRuleExecutor, _connectGamesHistoryRepository, 1, GameTypes.ConnectFour, players[1].Name));

            return new GameEngine(computerPlayers, moveBehavior, connectRuleExecutor, 0, -1, sequence);
        }

        /// <summary>
        /// Creates a Tic-Tac-Toe game between player and computer student.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="doesPlayerGoFirst"></param>
        /// <returns></returns>
        public IGame CreateTicTacToeGame(int[] sequence, Player player, bool doesPlayerGoFirst)
        {
            var moveBehavior = new AnyOpenElementMoveBehavior(9);

            var connectRuleExecutor = new ConnectRuleExecutor(3, 3);

            var players = new List<IConnectPlayer>();

            if (doesPlayerGoFirst)
            {
                players.Add(player);
                players.Add(GetComputerPlayer(player,
                                                moveBehavior,
                                                connectRuleExecutor,
                                                _connectGamesHistoryRepository,
                                                1,
                                                GameTypes.TicTacToe,
                                                "Computer Student"));
            }
            else
            {
                players.Add(GetComputerPlayer(player,
                                                moveBehavior,
                                                connectRuleExecutor,
                                                _connectGamesHistoryRepository,
                                                0,
                                                GameTypes.TicTacToe,
                                                "Computer Student"));
                players.Add(player);
            }

            return new GameEngine(players, moveBehavior, connectRuleExecutor, 
                                    doesPlayerGoFirst ? 0 : 1, -1, sequence);
        }

        /// <summary>
        /// Creates a Tic-Tac-Toe game between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public IGame CreateTicTacToeGame(int[] sequence, List<Player> players)
        {
            var moveBehavior = new AnyOpenElementMoveBehavior(9);

            var connectRuleExecutor = new ConnectRuleExecutor(3, 3);

            IList<IConnectPlayer> computerPlayers = new List<IConnectPlayer>();

            computerPlayers.Add(GetComputerPlayer(players[0], moveBehavior, connectRuleExecutor, _connectGamesHistoryRepository, 0, GameTypes.TicTacToe, players[0].Name));
            computerPlayers.Add(GetComputerPlayer(players[1], moveBehavior, connectRuleExecutor, _connectGamesHistoryRepository, 1, GameTypes.TicTacToe, players[1].Name));
            
            return new GameEngine(computerPlayers, moveBehavior, connectRuleExecutor,  0, -1, sequence);
        }

        /// <summary>
        /// Creates a computer player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="moveBehavior"></param>
        /// <param name="connectRuleExecutor"></param>
        /// <param name="gameHistoryRepository"></param>
        /// <param name="computerPlayerIndex"></param>
        /// <param name="gameTypes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private IComputerPlayer GetComputerPlayer(Player player, IMoveBehavior moveBehavior,
                                 IConnectRuleExecutor connectRuleExecutor,
                                 IGameHistoryRepository gameHistoryRepository,
                                 int computerPlayerIndex,
                                 GameTypes gameTypes,
                                 string name)
        {
            return new ComputerPlayer(moveBehavior,
                                        connectRuleExecutor,
                                        gameHistoryRepository,
                                        computerPlayerIndex,
                                        player.ComputerPlayerId,
                                        (int)gameTypes,
                                        name);
        }
    }
}
