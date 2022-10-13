using System.Drawing;

namespace GomokuAI
{
    class Program
    {
        public static Point GetNextWinningTurn(int[,] map, int playerNumber)
        {
            return new Point(0, 0);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                int player, n;
                int[,] map = new int[100, 100];
                player = int.Parse(Console.ReadLine());
                n = int.Parse(Console.ReadLine());
                for (int i = 1; i <= n; ++i)
                    for (int j = 1; j <= n; ++j)
                        map[i, j] = int.Parse(Console.ReadLine());

                GetNextWinningTurn(map, player);
                Console.WriteLine(GetNextWinningTurn(map, player).X.ToString() + " " + GetNextWinningTurn(map, player).Y.ToString());
            }
        }
    }
}