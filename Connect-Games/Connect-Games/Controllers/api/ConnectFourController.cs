using System;
using System.Linq;
using System.Threading.Tasks;
using Connect_Games.DomainModels.AI;
using Connect_Games.DomainModels.Game;
using Connect_Games.Games.Factories;
using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;
using Connect_Games.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Games.Controllers.api
{
    /// <summary>
    /// Controller class for managing connect four taining
    /// games with the computer student.
    /// </summary>
    [Produces("application/json")]
    [Route("api/ConnectFour")]
    [Authorize]
    public class ConnectFourController : Controller
    {
        #region Private members

        // built in class for managing user information
        private readonly UserManager<Player> _userManager;
        // Factory class for building Connect Four games
        private readonly IConnectGameFactory _connectGameFactory;
        // Repository class for accessing game history database records
        private readonly IConnectGamesHistoryRepository _connectGamesHistoryRepository;

        #endregion

        #region Constructors

        public ConnectFourController(UserManager<Player> userManager, IConnectGameFactory connectGameFactory,
                                    IConnectGamesHistoryRepository connectGamesHistoryRepository)
        {
            _userManager = userManager;
            _connectGameFactory = connectGameFactory;
            _connectGamesHistoryRepository = connectGamesHistoryRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Controls the management of a Connect Four game between the user and the computer student.
        /// </summary>
        /// <param name="connectFourRequestViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GameResponseViewModel> Post([FromBody] GameRequestViewModel connectFourRequestViewModel)
        {
            var player = (await _userManager.GetUserAsync(User));

            IGame game = _connectGameFactory.CreateConnectFourGame(connectFourRequestViewModel.Sequence,
                                                                   player,
                                                                   connectFourRequestViewModel.DoesPlayerGoFirst);
            
            // did the player just win, lose, or tie
            if (game.CurrentGameResult != GameResult.NotComplete)
            {
                if (game.CurrentGameResult != GameResult.Tie)
                {
                    SaveGame(game);
                }

                return CreateResults(connectFourRequestViewModel, player, game);
            }

            if(!connectFourRequestViewModel.DoesPlayerGoFirst && (game.CurrentMoves.Count % 2) == 0)
            {
                game.CurrentPlayerIndex = 0;
            }

            // otherwise, computer student needs to make move
            var computerMove = ((ComputerPlayer)game.CurrentPlayer).CalculateNextMove(connectFourRequestViewModel.Sequence,
                                                                    game.CurrentMovesPerPlayer);

            if (!game.TryMove(computerMove))
            {
                throw new Exception("Computer made invalid move");
            }

            if (game.CurrentGameResult != GameResult.NotComplete)
            {
                if (game.CurrentGameResult != GameResult.Tie)
                {
                    SaveGame(game);
                }

                return new GameResponseViewModel()
                {
                    ComputerMove = computerMove,
                    IsGameFinsihed = true,
                    Sequence = game.CurrentMoves.ToArray(),
                    Winner = game.Winner == null ? "Tie Game" : game.Winner.PlayerId == player.PlayerId ? "You Win" :
                             game.Winner.PlayerId == player.ComputerPlayerId ? "Computer Student Wins" : "Tie Game",
                    WinningSequence = game.WinningSequences != null && game.WinningSequences.Count > 0 ? game.WinningSequences[0].ToArray() : new int[0]
                };
            }

            return new GameResponseViewModel()
            {
                ComputerMove = computerMove,
                IsGameFinsihed = false,
                Sequence = game.CurrentMoves.ToArray(),
                Winner = "",
                WinningSequence = null
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves a game sequence if it's a winning game
        /// </summary>
        /// <param name="game"></param>
        private void SaveGame(IGame game)
        {
            if (game.CurrentGameResult == GameResult.Win)
            {
                GameHistory gameHistory = new GameHistory()
                {
                    DatePlayed = DateTime.Now,
                    GameType = GameTypes.ConnectFour,
                    MatchType = MatchTypes.PlayerVsComputer,
                    MoveSequence = string.Join(",", game.CurrentMoves),
                    PlayerId1 = game.Players[0].PlayerId,
                    PlayerId2 = game.Players[1].PlayerId,
                    WinningPlayerId = game.Winner.PlayerId
                };

                _connectGamesHistoryRepository.SaveGameHistory(gameHistory);
            }
        }

        /// <summary>
        /// Creates the results to be sent back to the browser
        /// </summary>
        /// <param name="connectFourRequestViewModel"></param>
        /// <param name="player"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static GameResponseViewModel CreateResults(GameRequestViewModel connectFourRequestViewModel, Player player, IGame game)
        {
            return new GameResponseViewModel()
            {
                ComputerMove = -1,
                IsGameFinsihed = true,
                Sequence = connectFourRequestViewModel.Sequence,
                Winner = game.Winner == null ? "Tie Game" : game.Winner.PlayerId == player.PlayerId ? "You Win" :
                             game.Winner.PlayerId == player.ComputerPlayerId ? "Computer Student Wins" : "Tie Game",
                WinningSequence = game.WinningSequences != null && game.WinningSequences.Count > 0 ? game.WinningSequences[0].ToArray() : new int[0]
            };
        }

        #endregion
    }
}