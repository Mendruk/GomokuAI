using System.Drawing;

namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;
    private static readonly Random Random = new();

    public static Point GetNextWinningTurn(int[,] map, int playerNumber, out bool isWinningTurn)
    {
        isWinningTurn = false;
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != 0)
                    continue;

                if (IsWinningTurn(map, playerNumber, x, y))
                {
                    isWinningTurn = true;
                    return new Point(x, y);
                }
            }

        return GetRandomPoint(map);
    }

    public static Point GetNextTurn(int[,] map, int playerNumber)
    {
        int enemyNumber = playerNumber == 1 ? 2 : 1;

        Point pointToReturn = GetNextWinningTurn(map, playerNumber, out bool isWinningTurn);

        if (isWinningTurn)
            return pointToReturn;

        pointToReturn = GetNextWinningTurn(map, enemyNumber, out bool isEnemyWinningTurn);

        if (isEnemyWinningTurn)
            return pointToReturn;


        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != 0)
                    continue;
                map[x, y] = playerNumber;

                GetNextWinningTurn(map, playerNumber, out isWinningTurn);

                if (isWinningTurn) return new Point(x, y);
                map[x, y] = 0;
            }

        return GetRandomPoint(map);
    }

    private static Point GetRandomPoint(int[,] map)
    {
        int offsetX = Random.Next(1, map.GetLength(0) - 2);
        int offsetY = Random.Next(1, map.GetLength(1) - 2);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                if (map[(x + offsetX) % map.GetLength(0), (y + offsetY) % map.GetLength(1)] == 0)
                    return new Point((x + offsetX) % map.GetLength(0), (y + offsetY) % map.GetLength(1));

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

            Point nextWinningPoint = GetNextTurn(map, player);

            Console.WriteLine(nextWinningPoint.X + " " + nextWinningPoint.Y);
        }
    }

    private static bool IsWinningTurn(int[,] map, int playerNumber, int x, int y)
    {
        int numberInHorizontalRow = 0;
        int searchOffset = WinningNumber - 1;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= map.GetLength(0))
                continue;

            if (map[i, y] == playerNumber || i == x)
                numberInHorizontalRow++;
            else
                numberInHorizontalRow = 0;

            if (numberInHorizontalRow == WinningNumber)
                return true;
        }

        int numberInVerticalRow = 0;

        for (int i = y - searchOffset; i <= y + searchOffset; i++)
        {
            if (i < 0 || i >= map.GetLength(1))
                continue;

            if (map[x, i] == playerNumber || i == y)
                numberInVerticalRow++;
            else
                numberInVerticalRow = 0;

            if (numberInVerticalRow == WinningNumber)
                return true;
        }

        int numberInAcross = 0;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= map.GetLength(1))
                continue;

            if (map[i, i] == playerNumber || (i == x && i == y))
                numberInAcross++;
            else
                numberInAcross = 0;

            if (numberInAcross == WinningNumber)
                return true;
        }

        int numberInReverseAcross = 0;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= map.GetLength(1) ||
                searchOffset - i < 0 || searchOffset - i >= map.GetLength(1))
                continue;

            if (map[i, searchOffset - i] == playerNumber || (i == x && searchOffset - i == y))
                numberInReverseAcross++;
            else
                numberInReverseAcross = 0;

            if (numberInReverseAcross == WinningNumber)
                return true;
        }

        return false;
    }
}