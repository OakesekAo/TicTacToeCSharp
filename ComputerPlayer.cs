using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.ConsoleGame
{
    /// <summary>
    /// Difficulty levels for the computer opponent.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Plays optimally using minimax; unbeatable.
        /// </summary>
        Godmode,

        /// <summary>
        /// Plays well but occasionally makes random moves.
        /// </summary>
        Random
    }

    /// <summary>
    /// Computer opponent using a minimax search.
    /// Plays optimally; it will not lose if legal moves are available.
    /// </summary>
    public sealed class ComputerPlayer
    {
        public Difficulty Difficulty { get; set; } = Difficulty.Godmode;

        public Position ChooseMove(Board board, Mark computerMark, Mark humanMark)
        {
            if (board is null) throw new ArgumentNullException(nameof(board));
            if (computerMark is Mark.Empty || humanMark is Mark.Empty ||
                computerMark == humanMark)
            {
                throw new ArgumentException("Marks must be distinct and non-empty.");
            }

            if (Difficulty == Difficulty.Random)
            {
                // 30% chance to play randomly, 70% chance to play optimally
                if (Random.Shared.Next(100) < 30)
                {
                    var emptyPositions = board.GetEmptyPositions().ToList();
                    return emptyPositions[Random.Shared.Next(emptyPositions.Count)];
                }
            }

            // Godmode or the 70% chance from Random difficulty
            return ChooseBestMove(board, computerMark, humanMark);
        }

        private Position ChooseBestMove(Board board, Mark computerMark, Mark humanMark)
        {
            var bestScore = int.MinValue;
            var bestMove = new Position(0, 0);
            var first = true;

            foreach (var pos in board.GetEmptyPositions())
            {
                var clone = board.Clone();
                clone.TryPlaceMark(pos.Row, pos.Column, computerMark);

                var score = Minimax(
                    clone,
                    computerMark,
                    humanMark,
                    isMaximizing: false,
                    depth: 1);

                if (first || score > bestScore)
                {
                    bestScore = score;
                    bestMove = pos;
                    first = false;
                }
            }

            return bestMove;
        }

        private static int Minimax(
            Board board,
            Mark aiMark,
            Mark humanMark,
            bool isMaximizing,
            int depth)
        {
            var winner = board.GetWinner();

            if (winner == aiMark)
            {
                // Prefer quicker wins.
                return 10 - depth;
            }

            if (winner == humanMark)
            {
                // Prefer slower losses.
                return depth - 10;
            }

            if (board.IsFull)
            {
                return 0;
            }

            if (isMaximizing)
            {
                var bestScore = int.MinValue;

                foreach (var pos in board.GetEmptyPositions())
                {
                    var clone = board.Clone();
                    clone.TryPlaceMark(pos.Row, pos.Column, aiMark);

                    var score = Minimax(
                        clone,
                        aiMark,
                        humanMark,
                        isMaximizing: false,
                        depth + 1);

                    bestScore = Math.Max(bestScore, score);
                }

                return bestScore;
            }
            else
            {
                var bestScore = int.MaxValue;

                foreach (var pos in board.GetEmptyPositions())
                {
                    var clone = board.Clone();
                    clone.TryPlaceMark(pos.Row, pos.Column, humanMark);

                    var score = Minimax(
                        clone,
                        aiMark,
                        humanMark,
                        isMaximizing: true,
                        depth + 1);

                    bestScore = Math.Min(bestScore, score);
                }

                return bestScore;
            }
        }
    }
}
