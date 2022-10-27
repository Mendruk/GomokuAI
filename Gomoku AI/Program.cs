using System.Drawing;

namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;

    public static Point GetNextTurn(int[,] map, int playerNumber)
    {
        List<(int x, int y, int turnNumber)> turns = new();
        int enemyNumber = playerNumber ==1 ? 2 : 1;

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == playerNumber|| map[x, y] == enemyNumber)
                    continue;

                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)

                        if (x + dx >= 0 && x + dx < map.GetLength(0) &&
                            y + dy >= 0 && y + dy < map.GetLength(1))
                        {
                            if (map[x + dx, y + dy] != 0)
                                continue;

                            if (IsWinningTurn(map, x, y, playerNumber))
                                return new Point(x, y);

                            map[x + dx, y + dy] = playerNumber;
                            GetNextIteration(x + dx, y + dy, x + dx, y + dy, 1, out bool hasWinningTurn);
                            map[x + dx, y + dy] = 0;

                        }
            }

        if (turns.Count > 0)
            return new Point(turns.
                    OrderBy(turn => turn.x).MinBy(turn => turn.turnNumber).x,
                turns.MinBy(turn => turn.turnNumber).y);

        return GetDefaultPoint(map);

        //todo rename
        void GetNextIteration(int x, int y, int firstX, int firstY, int iterationNumber, out bool hasWinningTurn)
        {
            hasWinningTurn = false;

            if (iterationNumber >= WinningNumber - 1)
                return;

            iterationNumber++;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)


                    if (x + dx >= 0 && x + dx < map.GetLength(0) &&
                        y + dy >= 0 && y + dy < map.GetLength(1))
                    {
                        if (map[x + dx, y + dy] != 0)
                            continue;

                        if (IsWinningTurn(map, x + dx, y + dy, playerNumber))
                        {
                            hasWinningTurn = true;
                            turns.Add((firstX, firstY, iterationNumber));
                            return;
                        }

                        map[x + dx, y + dy] = playerNumber;
                        GetNextIteration(x + dx, y + dy, firstX, firstY, iterationNumber, out hasWinningTurn);
                        map[x + dx, y + dy] = 0;
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