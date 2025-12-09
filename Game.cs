using System;

namespace TicTacToe.ConsoleGame
{
    /// <summary>
    /// Orchestrates the console UI and the game loop.
    /// </summary>
    public sealed class Game
    {
        private readonly Board _board = new();
        private readonly ComputerPlayer _computer = new();

        private const Mark HumanMark = Mark.X;
        private const Mark ComputerMark = Mark.O;

        public void Run()
        {
            PrintWelcome();
            _computer.Difficulty = SelectDifficulty();

            while (true)
            {
                _board.Clear();
                PlaySingleRound();

                if (!AskYesNo("Play again? (y/n): "))
                {
                    Console.WriteLine("Thanks for playing. Goodbye!");
                    return;
                }

                Console.Clear();
            }
        }

        private void PlaySingleRound()
        {
            var current = HumanStartsThisRound()
                ? HumanMark
                : ComputerMark;

            Console.WriteLine();
            Console.WriteLine($"You are '{HumanMark}', computer is '{ComputerMark}'.");
            Console.WriteLine($"{(current == HumanMark ? "You" : "Computer")} go first.");
            Console.WriteLine();

            while (true)
            {
                RenderBoard();

                if (current == HumanMark)
                {
                    var move = ReadHumanMove();
                    _board.TryPlaceMark(move.Row, move.Column, HumanMark);
                }
                else
                {
                    var move = _computer.ChooseMove(_board, ComputerMark, HumanMark);
                    _board.TryPlaceMark(move.Row, move.Column, ComputerMark);
                    Console.WriteLine(
                        $"Computer plays at row {move.Row + 1}, column {move.Column + 1}.");
                }

                var winner = _board.GetWinner();

                if (winner != Mark.Empty)
                {
                    RenderBoard();
                    Console.WriteLine(
                        winner == HumanMark
                            ? "You win! Nicely played."
                            : "Computer wins. Better luck next time.");
                    Console.WriteLine();
                    return;
                }

                if (_board.IsFull)
                {
                    RenderBoard();
                    Console.WriteLine("It's a draw.");
                    Console.WriteLine();
                    return;
                }

                current = current == HumanMark ? ComputerMark : HumanMark;
            }
        }

        private void RenderBoard()
        {
            Console.WriteLine();
            Console.WriteLine("   1   2   3");
            Console.WriteLine("  -----------");

            for (var row = 0; row < 3; row++)
            {
                Console.Write($"{row + 1} |");
                for (var col = 0; col < 3; col++)
                {
                    var mark = _board[row, col].ToDisplayChar();
                    Console.Write($" {mark} |");
                }

                Console.WriteLine();
                Console.WriteLine("  -----------");
            }

            Console.WriteLine();
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("=== Tic-Tac-Toe ===");
            Console.WriteLine();
            Console.WriteLine("Single-player against a computer opponent.");
            Console.WriteLine("Board is 3x3. You play as 'X'.");
            Console.WriteLine("Enter moves as 'row column', e.g. '1 3'.");
            Console.WriteLine("Type 'q' to quit at any time.");
            Console.WriteLine();
        }

        private static Difficulty SelectDifficulty()
        {
            while (true)
            {
                Console.WriteLine("Select difficulty:");
                Console.WriteLine("1. Godmode (Unbeatable)");
                Console.WriteLine("2. Random (Plays well but occasionally makes random moves)");
                Console.Write("Enter your choice (1 or 2): ");

                var input = Console.ReadLine();

                if (input is "1")
                {
                    Console.WriteLine();
                    return Difficulty.Godmode;
                }
                else if (input is "2")
                {
                    Console.WriteLine();
                    return Difficulty.Random;
                }
                else
                {
                    Console.WriteLine("Please enter 1 or 2.");
                }
            }
        }

        private static bool HumanStartsThisRound()
        {
            // Small bit of variation between rounds.
            // Could be made configurable; for now we randomize.
            return Random.Shared.Next(2) == 0;
        }

        private static Position ReadHumanMove()
        {
            while (true)
            {
                Console.Write("Your move (row column): ");
                var line = Console.ReadLine();

                if (line is null)
                {
                    // EOF or redirected input. Safest to exit.
                    Environment.Exit(0);
                }

                line = line.Trim();

                if (line.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Quitting game.");
                    Environment.Exit(0);
                }

                var parts = line.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out var rowInput) ||
                    !int.TryParse(parts[1], out var colInput))
                {
                    Console.WriteLine("Please enter row and column as two numbers, e.g. '2 3'.");
                    continue;
                }

                var row = rowInput - 1;
                var col = colInput - 1;

                if (!Board.IsInBounds(row, col))
                {
                    Console.WriteLine("Row and column must be between 1 and 3.");
                    continue;
                }

                if (!GameStateSnapshot.BoardIsCellFree(row, col))
                {
                    // We don't have direct access to Board here, so we ask via snapshot.
                    Console.WriteLine("That cell is already taken. Try another.");
                    continue;
                }

                return new Position(row, col);
            }
        }

        /// <summary>
        /// Lightweight wrapper to query the current board state from static context.
        /// This keeps <see cref="ReadHumanMove"/> focused on input parsing.
        /// </summary>
        private static class GameStateSnapshot
        {
            private static Board? _board;

            public static void Attach(Board board)
            {
                _board = board ?? throw new ArgumentNullException(nameof(board));
            }

            public static bool BoardIsCellFree(int row, int col)
            {
                if (_board is null)
                {
                    throw new InvalidOperationException("Board not attached to snapshot.");
                }

                return _board.IsCellEmpty(row, col);
            }
        }

        public Game()
        {
            // Attach board so the input parser can check if a move is legal.
            GameStateSnapshot.Attach(_board);
        }

        private static bool AskYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (input is null)
                    return false;

                input = input.Trim().ToLowerInvariant();

                if (input is "y" or "yes")
                    return true;

                if (input is "n" or "no")
                    return false;

                Console.WriteLine("Please answer 'y' or 'n'.");
            }
        }
    }
}
