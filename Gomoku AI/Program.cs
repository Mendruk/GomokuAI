using System.Drawing;

namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;

    public static Point GetNextTurn(int[,] map)
    {
        int maxTurnsNumber = map.GetLength(0) * map.GetLength(1);

        (int x, int y, int turnToWin) firstPlayerTurn =
            TryGetTurnToWin(1, maxTurnsNumber, out bool hasFirstPlayerWinningTurn);

        (int x, int y, int turnToWin) secondPlayerTurn =
            TryGetTurnToWin(2, firstPlayerTurn.turnToWin, out bool hasSecondPlayerWinningTurn);

        if (!hasFirstPlayerWinningTurn &&
            !hasSecondPlayerWinningTurn)
            return GetDefaultPoint(map);

        if (!hasSecondPlayerWinningTurn)
            return new Point(firstPlayerTurn.x, firstPlayerTurn.y);

        return new Point(secondPlayerTurn.x, secondPlayerTurn.y);

        (int x, int y, int turnTowin) TryGetTurnToWin(int player, int turnLimit, out bool hasWinningTurn)
        {
            Queue<List<Point>> pointQueue = new();

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] != 0 ||
                        !HasAdjacentNonEmptyPoints(x, y, player))
                        continue;

                    if (IsWinningTurn(map, x, y, player))
                    {
                        hasWinningTurn = true;
                        return (x, y, 1);
                    }

                    pointQueue.Enqueue(new List<Point> { new(x, y) });
                }

            while (pointQueue.Count > 0)
            {
                List<Point> currentList = pointQueue.Dequeue();

                if (currentList.Count > turnLimit)
                {
                    hasWinningTurn = false;
                    return (currentList[0].X, currentList[0].Y, currentList.Count);
                }

                foreach (Point point in currentList)
                    map[point.X, point.Y] = player;

                for (int x = 0; x < map.GetLength(0); x++)
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] != 0 ||
                            !HasAdjacentNonEmptyPoints(x, y, player))
                            continue;

                        if (IsWinningTurn(map, x, y, player))
                        {
                            hasWinningTurn = true;
                            return (currentList[0].X, currentList[0].Y, currentList.Count);
                        }

                        List<Point> newList = currentList.ToList();

                        newList.Add(new Point(x, y));
                        pointQueue.Enqueue(newList);
                    }

                foreach (Point point in currentList)
                    map[point.X, point.Y] = 0;
            }

            hasWinningTurn = false;
            return (66, 99, 99);
        }

        bool HasAdjacentNonEmptyPoints(int x, int y, int player)
        {
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                    if (x + dx >= 0 && x + dx < map.GetLength(0) &&
                        y + dy >= 0 && y + dy < map.GetLength(1) &&
                        map[x + dx, y + dy] == player)
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
        int[,] map = new int[1, 1];

        while (true)
        {
            int player = int.Parse(Console.ReadLine());

            int n = int.Parse(Console.ReadLine());

            if (map.GetLength(0) != n ||
                map.GetLength(1) != n)
                map = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    map[i, j] = int.Parse(Console.ReadLine());

            Point nextWinningCell = GetNextTurn(map);

            Console.WriteLine(nextWinningCell.X + 1 + ":" + (nextWinningCell.Y + 1));
        }
    }
}