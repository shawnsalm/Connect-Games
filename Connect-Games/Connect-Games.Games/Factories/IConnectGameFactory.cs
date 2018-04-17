using Connect_Games.DomainModels.Game;
using Connect_Games.Games.Models;

using System.Collections.Generic;


namespace Connect_Games.Games.Factories
{
    /// <summary>
    /// Creates Tic-Tac-Toe and Connect Four IGame objects for player training computer student
    /// and computer student playing another computer student.
    /// </summary>
    public interface IConnectGameFactory
    {
        /// <summary>
        /// Creates a Tic-Tac-Toe game between player and computer student.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="doesPlayerGoFirst"></param>
        /// <returns></returns>
        IGame CreateTicTacToeGame(int[] sequence, Player player, bool doesPlayerGoFirst);

        /// <summary>
        /// Creates a Tic-Tac-Toe game between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        IGame CreateTicTacToeGame(int[] sequence, List<Player> players);

        /// <summary>
        /// Creates a Connect Four game between a player and a computer student.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="player"></param>
        /// <param name="doesPlayerGoFirst"></param>
        /// <returns></returns>
        IGame CreateConnectFourGame(int[] sequence, Player player, bool doesPlayerGoFirst);

        /// <summary>
        /// Creates a Connect Four game between two computer students.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        IGame CreateConnectFourGame(int[] sequence, List<Player> players);
    }
}
