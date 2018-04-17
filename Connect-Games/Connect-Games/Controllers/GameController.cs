using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Games.Controllers
{
    /// <summary>
    /// Controller for displaying the Tic-Tac-Toe and Connect Four pages.
    /// </summary>
    public class GameController : Controller
    {
        #region Public methods

        /// <summary>
        /// Displays the view for the Tic-Tac-Toe game.
        /// </summary>
        /// <returns></returns>
        public IActionResult TicTacToe()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for the Connect Four game.
        /// </summary>
        /// <returns></returns>
        public IActionResult ConnectFour()
        {
            return View();
        }

        #endregion
    }
}