using System;
using System.Collections.Generic;

namespace TicTacToe.ConsoleGame
{
    /// <summary>
    /// Represents the Tic-Tac-Toe board and encapsulates game rules.
    /// </summary>
    public sealed class Board
    {
        private readonly Mark[,] _cells = new Mark[3, 3];

        public Mark this[int row, int column] => _cells[row, column];

        public bool IsFull
        {
            get
            {
                for (var row = 0; row < 3; row++)
                for (var col = 0; col < 3; col++)
                {
                    if (_cells[row, col] == Mark.Empty)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void Clear()
        {
            for (var row = 0; row < 3; row++)
            for (var col = 0; col < 3; col++)
            {
                _cells[row, col] = Mark.Empty;
            }
        }

        public bool IsCellEmpty(int row, int column)
        {
            EnsureInBounds(row, column);
            return _cells[row, column] == Mark.Empty;
        }

        public bool TryPlaceMark(int row, int column, Mark mark)
        {
            if (mark == Mark.Empty)
            {
                throw new ArgumentException("Cannot place an empty mark.", nameof(mark));
            }

            EnsureInBounds(row, column);

            if (!IsCellEmpty(row, column))
            {
                return false;
            }

            _cells[row, column] = mark;
            return true;
        }

        public Board Clone()
        {
            var clone = new Board();
            for (var row = 0; row < 3; row++)
            for (var col = 0; col < 3; col++)
            {
                clone._cells[row, col] = _cells[row, col];
            }

            return clone;
        }

        /// <summary>
        /// Returns the winning mark, or <see cref="Mark.Empty"/> if there is no winner yet.
        /// </summary>
        public Mark GetWinner()
        {
            // Rows
            for (var row = 0; row < 3; row++)
            {
                if (_cells[row, 0] != Mark.Empty &&
                    _cells[row, 0] == _cells[row, 1] &&
                    _cells[row, 1] == _cells[row, 2])
                {
                    return _cells[row, 0];
                }
            }

            // Columns
            for (var col = 0; col < 3; col++)
            {
                if (_cells[0, col] != Mark.Empty &&
                    _cells[0, col] == _cells[1, col] &&
                    _cells[1, col] == _cells[2, col])
                {
                    return _cells[0, col];
                }
            }

            // Diagonals
            if (_cells[0, 0] != Mark.Empty &&
                _cells[0, 0] == _cells[1, 1] &&
                _cells[1, 1] == _cells[2, 2])
            {
                return _cells[0, 0];
            }

            if (_cells[0, 2] != Mark.Empty &&
                _cells[0, 2] == _cells[1, 1] &&
                _cells[1, 1] == _cells[2, 0])
            {
                return _cells[0, 2];
            }

            return Mark.Empty;
        }

        public IEnumerable<Position> GetEmptyPositions()
        {
            for (var row = 0; row < 3; row++)
            for (var col = 0; col < 3; col++)
            {
                if (_cells[row, col] == Mark.Empty)
                {
                    yield return new Position(row, col);
                }
            }
        }

        public static bool IsInBounds(int row, int column) =>
            row is >= 0 and < 3 && column is >= 0 and < 3;

        private static void EnsureInBounds(int row, int column)
        {
            if (!IsInBounds(row, column))
            {
                throw new ArgumentOutOfRangeException(
                    $"Cell ({row}, {column}) is outside the board.");
            }
        }
    }
}
