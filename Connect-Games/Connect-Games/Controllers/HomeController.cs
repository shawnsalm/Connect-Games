using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Connect_Games.Models;
using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Connect_Games.Controllers
{
    /// <summary>
    /// Controller for displaying the home pages when the user is logged in and not logged in.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        #region Private members

        // Built in user management object
        private readonly UserManager<Player> _userManager;
        // Provides access to the player data in the database
        private readonly IPlayerRepository _playerRepository;
        // Provides access to the game history data in the database
        private readonly IConnectGamesHistoryRepository _connectGamesHistoryRepository;

        #endregion

        #region Constructors

        public HomeController(UserManager<Player> userManager,
                                 IPlayerRepository playerRepository,
                                IConnectGamesHistoryRepository connectGamesHistoryRepository)
        {
            _userManager = userManager;
            _playerRepository = playerRepository;
            _connectGamesHistoryRepository = connectGamesHistoryRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the default home screen when the user is not logged on. 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the home screen after the user has logged on.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Home()
        {
            ViewData["CurrentUser"] = (await _userManager.GetUserAsync(User)).Name;
            
            MatchesViewModel matchesViewModel = new MatchesViewModel();

            var currentPlayer = (await _userManager.GetUserAsync(User));

            matchesViewModel.TicTacToePlayerStats = CreatePlayerStatsForGame(GameTypes.TicTacToe)
                                                    .OrderByDescending(s => s.Wins).Take(3).ToList();
                                                     
            matchesViewModel.ConnectFourPlayerStats = CreatePlayerStatsForGame(GameTypes.ConnectFour)
                                                    .OrderByDescending(s => s.Wins).Take(3).ToList();
            
            return View(matchesViewModel);
        }

        /// <summary>
        /// Displays a default unhandled error page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the players' statistics for the games played (computer students).
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
        /// Counts the computer students' wins and losses
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

        #endregion
    }
}
