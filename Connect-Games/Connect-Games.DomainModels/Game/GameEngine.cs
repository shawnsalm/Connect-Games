using Connect_Games.DomainModels.MoveBehaviors;
using Connect_Games.DomainModels.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Represents a connect game engine.
    /// </summary>
    [Serializable]
    public sealed class GameEngine : IGame, IDisposable
    {
        #region Private Member Vaiables

        private readonly IList<int> _currentMoves;
        private readonly ReadOnlyCollection<int> _currentMovesReadonly;

        private readonly IList<IConnectPlayer> _players;
        private readonly ReadOnlyCollection<IConnectPlayer> _playersReadonly;

        private ReadOnlyCollection<IList<int>> _winningSequencesReadonly;

        private readonly IMoveBehavior _moveBehavior;
        private readonly IConnectRuleExecutor _connectRuleExecutor;

        private IConnectPlayer _winner;

        private int _currentPlayerIndex;

        private GameResult _gameResult = GameResult.NotComplete;

        [NonSerialized]
        private ReaderWriterLockSlim _movesReaderWriterLock;

        private readonly int _moveTimeout;

        #endregion

        #region Constructors

        public GameEngine(IMoveBehavior moveBehavior,
                    IConnectRuleExecutor connectRuleExecutor,
                    int currentPlayerIndex,
                    int moveTimeout = Timeout.Infinite) : this(new List<IConnectPlayer>(),
                                                                moveBehavior,
                                                                connectRuleExecutor,
                                                                currentPlayerIndex,
                                                                moveTimeout, null)
        {
        }

        public GameEngine(IList<IConnectPlayer> players,
                    IMoveBehavior moveBehavior,
                    IConnectRuleExecutor connectRuleExecutor,
                    int currentPlayerIndex,
                    int moveTimeout = Timeout.Infinite,
                    IList<int> currentMoves = null)
        {
            if (currentMoves == null || currentMoves.Count == 0)
            {
                _currentMoves = new List<int>();
            }
            else
            {
                _currentMoves = new List<int>(currentMoves);
            }
                       
            _currentMovesReadonly = new ReadOnlyCollection<int>(_currentMoves);

            _players = players;
            _playersReadonly = new ReadOnlyCollection<IConnectPlayer>(_players);

            _moveBehavior = moveBehavior;
            _connectRuleExecutor = connectRuleExecutor;

            _movesReaderWriterLock = new ReaderWriterLockSlim();

            _moveTimeout = moveTimeout;

            _currentPlayerIndex = currentPlayerIndex;

            CalculateGameState();
        }

        #endregion

        #region Public Events

        public event EventHandler<MoveMadeEventArgs> MoveMade;

        #endregion

        #region Public Properties

        public int CurrentPlayerIndex
        {
            get
            {
                return _currentPlayerIndex;
            }
            set
            {
                _currentPlayerIndex = value;
            }
        }

        public IList<int> CurrentMoves
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {
                    return _currentMovesReadonly;
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        public IList<IList<int>> CurrentMovesPerPlayer
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {

                    return new ReadOnlyCollection<IList<int>>(
                        CalculateCurrentMovesPerPlayer()
                            .Select(moves => new ReadOnlyCollection<int>(moves))
                            .Cast<IList<int>>().ToList());
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        public IConnectPlayer CurrentPlayer
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {

                    return _playersReadonly[_currentPlayerIndex];
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        public GameResult CurrentGameResult
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {

                    return _gameResult;
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        public IList<IConnectPlayer> Players
        {
            get
            {
                return _playersReadonly;
            }
        }

        public IConnectPlayer Winner
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {

                    return _winner;
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        public IList<IList<int>> WinningSequences
        {
            get
            {
                _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

                try
                {

                    return _winningSequencesReadonly;
                }
                finally
                {
                    _movesReaderWriterLock.ExitReadLock();
                }
            }
        }

        #endregion

        #region Public Methods

        public bool TryMove(int move)
        {
            var moveSuccessful = false;

            _movesReaderWriterLock.TryEnterUpgradeableReadLock(_moveTimeout);

            try
            {
                if (_gameResult == GameResult.NotComplete &&
                    ValidateMove(move))
                {
                    _movesReaderWriterLock.TryEnterWriteLock(_moveTimeout);

                    try
                    {
                        _currentMoves.Add(move);

                        CalculateGameState();

                        moveSuccessful = true;

                        OnMoveMade(new MoveMadeEventArgs(move, _currentPlayerIndex));
                    }
                    finally
                    {
                        _movesReaderWriterLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _movesReaderWriterLock.ExitUpgradeableReadLock();
            }

            return moveSuccessful;
        }

        public bool TryAddPlayers(IList<IConnectPlayer> players)
        {
            var addedPlayers = false;

            _movesReaderWriterLock.TryEnterReadLock(_moveTimeout);

            try
            {
                // We are only allowed to add players before the game starts.
                if (_currentMoves.Count == 0 && players != null &&
                    players.Count > 0)
                {
                    ((List<IConnectPlayer>)_players).AddRange(players);
                    addedPlayers = true;
                }
            }
            finally
            {
                _movesReaderWriterLock.ExitReadLock();
            }

            return addedPlayers;
        }

        public void Dispose()
        {
            if (_movesReaderWriterLock != null)
            {
                _movesReaderWriterLock.Dispose();
            }
        }

        #endregion

        #region Private Methods

        private bool ValidateMove(int move)
        {
            var validateMove = false;

            var availableMoves = _moveBehavior.GetAvailableMoves(_currentMovesReadonly);

            if (availableMoves != null && availableMoves.Count > 0 &&
                availableMoves.Contains(move))
            {
                validateMove = true;
            }

            return validateMove;
        }

        private void CalculateGameState()
        {
            //_currentPlayerIndex
            IList<IList<int>> winningSequences;

            var movesPerPlayers = CalculateCurrentMovesPerPlayer();

            // first determine if winner
            if (_connectRuleExecutor.IsWinner(movesPerPlayers[_currentPlayerIndex],
                                              out winningSequences))
            {
                _gameResult = GameResult.Win;
                _winner = _playersReadonly[_currentPlayerIndex];

                AssignWinningSequences(winningSequences);

            }
            else if (_moveBehavior.GetAvailableMoves(_currentMovesReadonly).Count == 0)
            {
                // game over
                _gameResult = GameResult.Tie;
            }
            else
            {
                _currentPlayerIndex = _currentMoves.Count % 2;
            }
        }

        private IList<IList<int>> CalculateCurrentMovesPerPlayer()
        {
            var numberOfPlayers = _playersReadonly.Count;

            var movesPerPlayers = new List<IList<int>>();

            for (var playerIndex = 0; playerIndex < numberOfPlayers; playerIndex++)
            {
                movesPerPlayers.Add(new List<int>());
            }

            for (var movesIndex = 0; movesIndex < _currentMovesReadonly.Count; movesIndex++)
            {
                movesPerPlayers[movesIndex % numberOfPlayers].Add(_currentMovesReadonly[movesIndex]);
            }

            return movesPerPlayers;
        }

        private void AssignWinningSequences(IEnumerable<IList<int>> winningSequences)
        {
            _winningSequencesReadonly = new ReadOnlyCollection<IList<int>>(
                winningSequences.Select(sequence => new ReadOnlyCollection<int>(sequence))
                    .Cast<IList<int>>().ToList());
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _movesReaderWriterLock = new ReaderWriterLockSlim();
        }

        private void OnMoveMade(MoveMadeEventArgs e)
        {
            MoveMade?.Invoke(this, e);
        }

        #endregion

    }
}
