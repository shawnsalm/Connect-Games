using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Connect_Games.DomainModels.Game;
using Connect_Games.Games.Factories;
using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;
using Connect_Games.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Games.Controllers
{
    /// <summary>
    /// Controller for displaying and managing the matches between computer students.
    /// </summary>
    [Authorize]
    public class MatchController : Controller
    {
        #region Private members

        // Built in object for managingusers.
        private readonly UserManager<Player> _userManager;
        // Provides access to the players data.
        private readonly IPlayerRepository _playerRepository;
        // Provides access to the game history sequences.
        private readonly IConnectGamesHistoryRepository _connectGamesHistoryRepository;
        // Abstract factory creating Tic-Tac-Toe and Connect Four games
        private readonly IConnectGameFactory _connectGameFactory;

        #endregion

        #region Constructors

        public MatchController(UserManager<Player> userManager, 
                                IPlayerRepository playerRepository,
                                IConnectGamesHistoryRepository connectGamesHistoryRepository,
                                IConnectGameFactory connectGameFactory)
        {
            _userManager = userManager;
            _playerRepository = playerRepository;
            _connectGamesHistoryRepository = connectGamesHistoryRepository;
            _connectGameFactory = connectGameFactory;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the default matches screen.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MatchesViewModel matchesViewModel = new MatchesViewModel();

            var currentPlayer = (await _userManager.GetUserAsync(User));

            matchesViewModel.EnableTicTacToe = currentPlayer.ComputerAvailableTicTacToe;
            matchesViewModel.EnableConnectFour = currentPlayer.ComputerAvailableConnectFour;

            if (matchesViewModel.EnableTicTacToe)
            {
                matchesViewModel.TicTacToePlayerStats = CreatePlayerStatsForGame(GameTypes.TicTacToe);

                var currentRecord = matchesViewModel.TicTacToePlayerStats.Where(p => p.PlayerId == currentPlayer.PlayerId).First();

                matchesViewModel.TicTacToePlayerStats.Remove(currentRecord);

                matchesViewModel.TicTacToeRecord = "(" + currentRecord.Wins + "," + currentRecord.Losses + ")";
            }

            if (matchesViewModel.EnableConnectFour)
            {
                matchesViewModel.ConnectFourPlayerStats = CreatePlayerStatsForGame(GameTypes.ConnectFour);

                var currentRecord = matchesViewModel.ConnectFourPlayerStats.Where(p => p.PlayerId == currentPlayer.PlayerId).First();

                matchesViewModel.ConnectFourPlayerStats.Remove(currentRecord);

                matchesViewModel.ConnectFourRecord = "(" + currentRecord.Wins + "," + currentRecord.Losses + ")";
            }

            return View(matchesViewModel);
        }

        /// <summary>
        /// Handles toggling the user’s computer students ability to play Tic-Tac-Toe matches.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ToggleTicTacToeEnable()
        {
            var currentPlayer = (await _userManager.GetUserAsync(User));

            var player = _playerRepository.GetPlayer(currentPlayer.PlayerId);

            player.ComputerAvailableTicTacToe = !player.ComputerAvailableTicTacToe;

            _playerRepository.UpdatePlayer(player);

            return RedirectToAction("Index", "Match");
        }

        /// <summary>
        /// Handles toggling the user’s computer students ability to play Connect Four matches.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ToggleConnectFourEnable()
        {
            var currentPlayer = (await _userManager.GetUserAsync(User));

            var player = _playerRepository.GetPlayer(currentPlayer.PlayerId);

            player.ComputerAvailableConnectFour = !player.ComputerAvailableConnectFour;

            _playerRepository.UpdatePlayer(player);

            return RedirectToAction("Index", "Match");
        }

        /// <summary>
        /// Manages the match between two computer students playing Tic-Tac-Toe.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlayTicTacToeMatch(Guid playerId)
        {
            var player = (await _userManager.GetUserAsync(User));

            var opponentPlayer = _playerRepository.GetPlayer(playerId);

            Random random = new Random();

            var playGoesFirst = random.Next(2);

            List<Player> players = new List<Player>();

            if(playGoesFirst == 0)
            {
                players.Add(player);
                players.Add(opponentPlayer);
            }
            else
            {
                players.Add(opponentPlayer);
                players.Add(player);
            }

            IGame game = _connectGameFactory.CreateTicTacToeGame(null, players);

            Match match = new Match();

            match.PlayGame(game);

            GameResponseViewModel ticTacToeResponseViewModel =
                CreateResults(game.CurrentMoves.ToArray(), player, game);

            SaveGame(game, GameTypes.TicTacToe);

            return View("TicTacToeResults", ticTacToeResponseViewModel);
        }

        /// <summary>
        /// Manages the match between two computer students playing Connect Four.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlayConnectFourMatch(Guid playerId)
        {
            var player = (await _userManager.GetUserAsync(User));

            var opponentPlayer = _playerRepository.GetPlayer(playerId);

            Random random = new Random();

            var playGoesFirst = random.Next(2);

            List<Player> players = new List<Player>();

            if (playGoesFirst == 0)
            {
                players.Add(player);
                players.Add(opponentPlayer);
            }
            else
            {
                players.Add(opponentPlayer);
                players.Add(player);
            }

            IGame game = _connectGameFactory.CreateConnectFourGame(null, players);

            Match match = new Match();

            match.PlayGame(game);

            GameResponseViewModel connectFourResponseViewModel =
                CreateResultsConnectFour(game.CurrentMoves.ToArray(), player, game);

            SaveGame(game, GameTypes.ConnectFour);

            return View("ConnectFourResults", connectFourResponseViewModel);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the player's computer student statistics
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        private List<PlayerStatsViewModel> CreatePlayerStatsForGame(GameTypes gameType)
        {
            var matches = _connectGamesHistoryRepository.GetGameHistories().
                            Where(g => g.GameType == gameType &&
                                        g.MatchType == MatchTypes.ComputerVsComputer).ToArray(); ;

            var players = _playerRepository.GetPlayers().
                Where(p => ((gameType == GameTypes.TicTacToe && p.ComputerAvailableTicTacToe) ||
                            (gameType == GameTypes.ConnectFour && p.ComputerAvailableConnectFour))
                            ).ToArray();

            Dictionary<Guid, PlayerStatsViewModel> playerStatsMap = new Dictionary<Guid, PlayerStatsViewModel>();

            Dictionary<Guid, Guid> computerStudentToPlayerMap = new Dictionary<Guid, Guid>();

            foreach (var player in players)
            {
                playerStatsMap.Add(player.PlayerId,
                    new PlayerStatsViewModel()
                    {
                        PlayerId = player.PlayerId,
                        UserName = player.Name
                    });
                computerStudentToPlayerMap.Add(player.ComputerPlayerId, player.PlayerId);
            }

            foreach (var match in matches)
            {
                RecordWinOrLoss(match.PlayerId1, playerStatsMap, computerStudentToPlayerMap, match);
                RecordWinOrLoss(match.PlayerId2, playerStatsMap, computerStudentToPlayerMap, match);
            }

            return playerStatsMap.Values.ToList();
        }

        /// <summary>
        /// Counts the wins and losses of the computer student
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerStatsMap"></param>
        /// <param name="computerStudentToPlayerMap"></param>
        /// <param name="match"></param>
        private static void RecordWinOrLoss(Guid playerId, Dictionary<Guid, PlayerStatsViewModel> playerStatsMap, Dictionary<Guid, Guid> computerStudentToPlayerMap, GameHistory match)
        {
            Guid computerStudentPlayerId;

            if (computerStudentToPlayerMap.TryGetValue(playerId, out computerStudentPlayerId))
            {
                PlayerStatsViewModel playerStatsViewModel = playerStatsMap[computerStudentPlayerId];

                if (playerId == match.WinningPlayerId)
                {
                    playerStatsMap[computerStudentPlayerId].Wins++;
                }
                else
                {
                    playerStatsMap[computerStudentPlayerId].Losses++;
                }
            }
        }

        /// <summary>
        /// Saves the sequence of the game.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="gameType"></param>
        private void SaveGame(IGame game, GameTypes gameType)
        {
            if (game.CurrentGameResult == GameResult.Win)
            {
                GameHistory gameHistory = new GameHistory()
                {
                    DatePlayed = DateTime.Now,
                    GameType = gameType,
                    MatchType = MatchTypes.ComputerVsComputer,
                    MoveSequence = string.Join(",", game.CurrentMoves),
                    PlayerId1 = game.Players[0].PlayerId,
                    PlayerId2 = game.Players[1].PlayerId,
                    WinningPlayerId = game.Winner.PlayerId
                };

                _connectGamesHistoryRepository.SaveGameHistory(gameHistory);
            }
        }

        /// <summary>
        /// Creates the results of the match between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static GameResponseViewModel CreateResults(int[] sequence, Player player, IGame game)
        {
            return new GameResponseViewModel()
            {
                ComputerMove = -1,
                IsGameFinsihed = true,
                Sequence = sequence,
                Winner = game.Winner == null ? "Tie Game" : game.Winner.PlayerId == player.ComputerPlayerId ? "Your Student Won" :
                              "Your Student Lost",
                WinningSequence = game.WinningSequences != null && game.WinningSequences.Count > 0 ? game.WinningSequences[0].ToArray() : new int[0],
                WinningWentFirst = game.Winner != null && game.Winner == game.Players[0],
                Info = game.Players[0].PlayerId == player.ComputerPlayerId ? "Your student is X " : "Your student is O"
            };
        }

        /// <summary>
        /// Creates the results of a match between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static GameResponseViewModel CreateResultsConnectFour(int[] sequence, Player player, IGame game)
        {
            return new GameResponseViewModel()
            {
                ComputerMove = -1,
                IsGameFinsihed = true,
                Sequence = sequence,
                Winner = game.Winner == null ? "Tie Game" : game.Winner.PlayerId == player.ComputerPlayerId ? "Your Student Won" :
                              "Your Student Lost",
                WinningSequence = game.WinningSequences != null && game.WinningSequences.Count > 0 ? game.WinningSequences[0].ToArray() : new int[0],
                WinningWentFirst = game.Winner != null && game.Winner == game.Players[0],
                Info = game.Players[0].PlayerId == player.ComputerPlayerId ? "Your student is black " : "Your student is red"
            };
        }

        #endregion
    }
}