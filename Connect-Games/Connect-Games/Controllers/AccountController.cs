using System;
using System.Linq;
using System.Threading.Tasks;

using Connect_Games.Games.Models;
using Connect_Games.Games.Repositories;
using Connect_Games.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Games.Controllers
{
    /// <summary>
    /// Controller that manages the user’s login, logout, and registration processes.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        #region Private Members

        // Built in class for managing users
        private readonly UserManager<Player> _userManager;
        // Built in class for handling signing in
        private readonly SignInManager<Player> _signInManager;
        // Repository class for handling acccess to the players in the database
        private readonly IPlayerRepository _playerRepository;

        #endregion

        #region Constructors

        public AccountController(UserManager<Player> userManager,
            SignInManager<Player> signInManager,
            IPlayerRepository playerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _playerRepository = playerRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Manages the view for the beginning Login page (before the user submits their information.)
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Manages the post back of the login screen to log the user into the game.
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("Home", "Home");

                    return Redirect(loginViewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        /// <summary>
        /// Manages the view for the beginning of the Registration page (before the user enters their information.)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Manages the post back of the registration page, registering the user.
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                // test that we don't already have the user name
                if(_playerRepository.GetPlayers().Where(p => p.Name == loginViewModel.UserName).Any())
                {
                    ModelState.AddModelError("", "Username already exists.  Please pick a different name.");
                    View(loginViewModel);
                }

                var user = new Player()
                                {
                                    UserName = loginViewModel.UserName,
                                    ComputerAvailableConnectFour = false,
                                    ComputerAvailableTicTacToe = false,
                                    ComputerPlayerId = Guid.NewGuid(),
                                    Name = loginViewModel.UserName,
                                    PlayerId = Guid.NewGuid()
                };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(loginViewModel);
        }

        /// <summary>
        /// Manages logging out the user.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}