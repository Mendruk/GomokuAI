namespace Gomoku_AI
{
    public class Cell
    {
        public int PlayerNumber;
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(int x, int y, int playerNumber)
        {
            X=x;
            Y=y;
            PlayerNumber=playerNumber;
        }
    }
}
