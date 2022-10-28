using System.Drawing;

namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;

    public static Point GetNextTurn(int[,] map, int playerNumber)
    {
        List<(int x, int y, int turnToWin, int player)> turns = new();
        int enemyNumber = playerNumber == 1 ? 2 : 1;

        GetNextIteration(0, 0, 0, playerNumber);
        GetNextIteration(0, 0, 0, enemyNumber);

        if (playerNumber == 1)
            turns = turns.OrderBy(value => value.turnToWin)
                .ThenBy(value => value.player)
                .ToList();
        else
            turns = turns.OrderBy(value => value.turnToWin)
                .ThenByDescending(value => value.player)
                .ToList();

        if (turns.Count > 0)
            return new Point(turns[0].x, turns[0].y);

        return GetDefaultPoint(map);


        void GetNextIteration(int firstX, int firstY, int iterationNumber, int player)
        {
            if (iterationNumber > WinningNumber - 2)
                return;

            iterationNumber++;

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] != player)
                        continue;

                    for (int dx = -1; dx <= 1; dx++)
                        for (int dy = -1; dy <= 1; dy++)
                            if (x + dx >= 0 && x + dx < map.GetLength(0) &&
                                y + dy >= 0 && y + dy < map.GetLength(1))
                            {
                                if (map[x + dx, y + dy] != 0)
                                    continue;

                                if (IsWinningTurn(map, x + dx, y + dy, player))
                                {
                                    if (iterationNumber <= 1)
                                    {
                                        turns.Add((x + dx, y + dy, iterationNumber, player));
                                        return;
                                    }

                                    turns.Add((firstX, firstY, iterationNumber, player));

                                    return;
                                }

                                map[x + dx, y + dy] = player;

                                if (iterationNumber <= 1)
                                    GetNextIteration(x + dx, y + dy, iterationNumber, player);
                                else
                                    GetNextIteration(firstX, firstY, iterationNumber, player);

                                map[x + dx, y + dy] = 0;
                            }
                }
        }
    }

    private static bool IsWinningTurn(int[,] map, int x, int y, int playerNumber)
    {
        int searchOffset = WinningNumber - 1;

        int numberInHorizontalRow = 0;

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
            if ((i < 0 || i >= map.GetLength(1)) ||
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
    private static Point GetDefaultPoint(int[,] map)
    {
        int offsetX = map.GetLength(0) / 2;
        int offsetY = map.GetLength(1) / 2;

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                if (map[(x + offsetX) % map.GetLength(0), (y + offsetY) % map.GetLength(1)] == 0)
                    return new Point((x + offsetX) % map.GetLength(0), (y + offsetY) % map.GetLength(1));

        return new Point(66, 99);
    }

    private static void Main(string[] args)
    {
        while (true)
        {
            int player = int.Parse(Console.ReadLine());

            int n = int.Parse(Console.ReadLine());

            int[,] map = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    map[i, j] = int.Parse(Console.ReadLine());

            Point nextWinningCell = GetNextTurn(map, player);

            Console.WriteLine((nextWinningCell.X + 1) + ":" + (nextWinningCell.Y + 1));
        }
    }
}