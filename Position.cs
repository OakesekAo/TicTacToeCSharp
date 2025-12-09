namespace TicTacToe.ConsoleGame
{
    /// <summary>
    /// Lightweight immutable coordinate on the board.
    /// </summary>
    public readonly record struct Position(int Row, int Column);
}
