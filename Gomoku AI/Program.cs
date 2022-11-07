﻿using System.Drawing;

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

        if (turns.Count > 0)
            return turns.OrderBy(value => value.turnToWin)
                .ThenByDescending(value => value.player == playerNumber)
                .Select(value => new Point(value.x, value.y))
                .First();

        return GetDefaultPoint(map);

        void GetNextIteration(int firstX, int firstY, int iterationNumber, int player)
        {
            if (iterationNumber > WinningNumber - 2)
                return;

            iterationNumber++;

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == player ||
                        !HasAdjacentNonEmptyPoints(x, y, player))
                        continue;

                    if (IsWinningTurn(map, x, y, player))
                    {
                        turns.Add(iterationNumber <= 1
                            ? (x, y, iterationNumber, player)
                            : (firstX, firstY, iterationNumber, player));

                        return;
                    }

                    map[x, y] = player;

                    if (iterationNumber <= 1)
                        GetNextIteration(x, y, iterationNumber, player);
                    else
                        GetNextIteration(firstX, firstY, iterationNumber, player);

                    map[x, y] = 0;
                }
        }

        bool HasAdjacentNonEmptyPoints(int x, int y, int player)
        {
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                    if (x + dx >= 0 && x + dx < map.GetLength(0) &&
                        y + dy >= 0 && y + dy < map.GetLength(1))
                        if (map[x + dx, y + dy] == player)
                            return true;

            return false;
        }
    }

    private static bool IsWinningTurn(int[,] map, int x, int y, int playerNumber)
    {
        int searchOffset = WinningNumber - 1;

        return IsWinningCombination(1, 0) ||
               IsWinningCombination(0, 1) ||
               IsWinningCombination(1, 1) ||
               IsWinningCombination(1, -1);

        bool IsWinningCombination(int xShift, int yShift)
        {
            int numberInRow = 0;

            for (int i = -searchOffset; i <= searchOffset; i++)
            {
                int xDisplaced = x + i * xShift;
                int yDisplaced = y + i * yShift;

                if (xDisplaced < 0 || xDisplaced >= map.GetLength(0) ||
                    yDisplaced < 0 || yDisplaced >= map.GetLength(1))
                    continue;

                if (map[xDisplaced, yDisplaced] == playerNumber ||
                    (xDisplaced == x && yDisplaced == y))
                    numberInRow++;
                else
                    numberInRow = 0;

                if (numberInRow == WinningNumber)
                    return true;
            }

            return false;
        }
    }

    private static Point GetDefaultPoint(int[,] map)
    {
        int offsetX = map.GetLength(0) / 2;
        int offsetY = map.GetLength(1) / 2;

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Point point = new((x + offsetX) % map.GetLength(0), (y + offsetY) % map.GetLength(1));

                if (map[point.X, point.Y] == 0)
                    return point;
            }

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