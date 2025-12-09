namespace TicTacToe.ConsoleGame
{
    public enum Mark
    {
        Empty = 0,
        X = 1,
        O = 2
    }

    public static class MarkExtensions
    {
        public static char ToDisplayChar(this Mark mark) =>
            mark switch
            {
                Mark.X => 'X',
                Mark.O => 'O',
                _ => ' '
            };
    }
}
