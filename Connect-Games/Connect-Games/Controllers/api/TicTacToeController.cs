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
    /// Controller that providers a web API interface for training the computer student in Tic-Tac-Toe.
    /// </summary>
    [Produces("application/json")]
    [Route("api/TicTacToe")]
    [Authorize]
    public class TicTacToeController : Controller
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

        public TicTacToeController(UserManager<Player> userManager, IConnectGameFactory connectGameFactory,
                                    IConnectGamesHistoryRepository connectGamesHistoryRepository)
        {
            _userManager = userManager;
            _connectGameFactory = connectGameFactory;
            _connectGamesHistoryRepository = connectGamesHistoryRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Controls the management of a Tic-Tac-Toe game between the user and the computer student.
        /// </summary>
        /// <param name="ticTacToeRequestViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GameResponseViewModel> Post([FromBody] GameRequestViewModel ticTacToeRequestViewModel)
        {
            var player = (await _userManager.GetUserAsync(User));

            IGame game = _connectGameFactory.CreateTicTacToeGame(ticTacToeRequestViewModel.Sequence,
                                                                   player,
                                                                   ticTacToeRequestViewModel.DoesPlayerGoFirst);

            // did the player just win, lose, or tie
            if(game.CurrentGameResult != GameResult.NotComplete)
            {
                if (game.CurrentGameResult != GameResult.Tie)
                {
                    SaveGame(game);
                }

                return CreateResults(ticTacToeRequestViewModel, player, game);
            }

            if (!ticTacToeRequestViewModel.DoesPlayerGoFirst && (game.CurrentMoves.Count % 2) == 0)
            {
                game.CurrentPlayerIndex = 0;
            }

            // otherwise, computer student needs to make move
            var computerMove = ((ComputerPlayer)game.CurrentPlayer).CalculateNextMove(ticTacToeRequestViewModel.Sequence,
                                                                    game.CurrentMovesPerPlayer);

            if(!game.TryMove(computerMove))
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

        /// <summary>
        /// Saves a winning game sequence 
        /// </summary>
        /// <param name="game"></param>
        private void SaveGame(IGame game)
        {
            if (game.CurrentGameResult == GameResult.Win)
            {
                GameHistory gameHistory = new GameHistory()
                {
                    DatePlayed = DateTime.Now,
                    GameType = GameTypes.TicTacToe,
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
        /// Returns the results to the browser.
        /// </summary>
        /// <param name="ticTacToeRequestViewModel"></param>
        /// <param name="player"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static GameResponseViewModel CreateResults(GameRequestViewModel ticTacToeRequestViewModel, Player player, IGame game)
        {
            return new GameResponseViewModel()
            {
                ComputerMove = -1,
                IsGameFinsihed = true,
                Sequence = ticTacToeRequestViewModel.Sequence,
                Winner = game.Winner == null ? "Tie Game" : game.Winner.PlayerId == player.PlayerId ? "You Win" :
                             game.Winner.PlayerId == player.ComputerPlayerId ? "Computer Student Wins" : "Tie Game",
                WinningSequence = game.WinningSequences != null && game.WinningSequences.Count > 0 ? game.WinningSequences[0].ToArray() : new int[0]
            };
        }
    }
}