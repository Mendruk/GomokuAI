using System.Drawing;

namespace Gomoku_AI;

public class Program
{
    public static Point GetNextTurn(int[,] map, int playerNumber)
    {
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                if (IsWinningTurn(map, playerNumber, x, y))
                {
                    if (map[x, y] != 0)
                        continue;

                    return new Point(x, y);
                }

        return new Point(0, 0);
    }

    private static void Main(string[] args)
    {
        while (true)
        {
            int player = int.Parse(Console.ReadLine());

            int n = int.Parse(Console.ReadLine());

            int[,] map = new int[n, n];

            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    map[i, j] = int.Parse(Console.ReadLine());

            GetNextTurn(map, player);
            Console.WriteLine(GetNextTurn(map, player).X + " " + GetNextTurn(map, player).Y);
        }
    }

    private static bool IsWinningTurn(int[,] map, int playerNumber, int x, int y)
    {
        int numberInHorizontalRow = 0;

        for (int i = x - 4; i <= x + 4; i++)
        {
            if (i < 0 || i >= map.GetLength(0))
                continue;

            if (map[i, y] == playerNumber || i == x)
                numberInHorizontalRow++;
            else
                numberInHorizontalRow = 0;

            if (numberInHorizontalRow == 5)
                return true;
        }

        int numberInVerticalRow = 0;

        for (int i = y - 4; i <= y + 4; i++)
        {
            if (i < 0 || i >= map.GetLength(1))
                continue;

            if (map[x, i] == playerNumber || i == y)
                numberInVerticalRow++;
            else
                numberInVerticalRow = 0;

            if (numberInVerticalRow == 5)
                return true;
        }

        int numberInAcross = 0;

        for (int i = x - 4; i <= x + 4; i++)
        {
            if (i < 0 || i >= map.GetLength(1))
                continue;

            if (map[i, i] == playerNumber || (i == x && i == y))
                numberInAcross++;
            else
                numberInAcross = 0;

            if (numberInAcross == 5)
                return true;
        }

        //int numberInReverseAcross = 0;

        //for (int i = x - 4; i <= x + 4; i++)
        //{
        //    if (i < 0 || i >= map.GetLength(1))
        //        continue;

        //    if (map[i, i] == playerNumber || (i == x && i == y))
        //        numberInReverseAcross++;
        //    else
        //        numberInReverseAcross = 0;

        //    if (numberInReverseAcross == 5)
        //        return true;
        //}

        return false;
    }
}