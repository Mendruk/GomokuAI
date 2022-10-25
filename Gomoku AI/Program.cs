namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;

    public static Cell GetNextTurn(int[,] map, int playerNumber)
    {
        Cell[,] cells = new Cell[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                cells[x, y] = new Cell(x, y, map[x, y]);

        IEnumerable<(Cell cell, int turnToWin)> cellsEnumerable = Array.Empty<(Cell cell, int turnToWin)>();

        foreach (Cell cell in cells.OfType<Cell>().Where(cell => cell.PlayerNumber == playerNumber))
        {
            cellsEnumerable = cellsEnumerable.Concat(GetEnumerableOfCellAndTurnToWins(cells, cell, playerNumber));
        }

        if (cellsEnumerable.Any())
            return cellsEnumerable
                .MinBy(cell => cell.turnToWin)
                .cell;

        return GetDefaultPoint(cells);
    }

    private static Cell GetDefaultPoint(Cell[,] cells)
    {
        int offsetX = cells.GetLength(0) / 2;
        int offsetY = cells.GetLength(1) / 2;

        for (int x = 0; x < cells.GetLength(0); x++)
            for (int y = 0; y < cells.GetLength(1); y++)
                if (cells[(x + offsetX) % cells.GetLength(0), (y + offsetY) % cells.GetLength(1)].PlayerNumber == 0)
                    return cells[(x + offsetX) % cells.GetLength(0), (y + offsetY) % cells.GetLength(1)];

        return cells[0, 0];
    }

    private static IEnumerable<(Cell cell, int turnToWin)> GetEnumerableOfCellAndTurnToWins(Cell[,] cells, Cell targetCell, int playerNumber)
    {
        int enemyNumber = playerNumber == 1 ? 2 : 1;
        int x = targetCell.X;
        int y = targetCell.Y;
        int searchOffset = WinningNumber - 1;

        if (GetRightMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetLeftMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetDownMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetUpMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetRightDownMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetLeftUpMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetRightUpMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        if (GetLeftDownMaskCells().All(cell => cell.PlayerNumber != enemyNumber))
        {
            foreach (Cell cell in GetRightMaskCells()
                         .Where(cell => cell.PlayerNumber == 0))
            {
                yield return (cell, GetRightMaskCells()
                    .Where(cell => cell.PlayerNumber == 0)
                    .Count());
            }
        }

        //

        IEnumerable<Cell> GetRightMaskCells()
        {
            if (x + searchOffset >= cells.GetLength(0))
                yield break;

            for (int i = x; i <= x + searchOffset; i++)
            {
                yield return cells[i, y];
            }
        }

        IEnumerable<Cell> GetLeftMaskCells()
        {
            if (x - searchOffset < 0)
                yield break;

            for (int i = x; i >= x - searchOffset; i--)
            {
                yield return cells[i, y];
            }
        }

        IEnumerable<Cell> GetDownMaskCells()
        {
            if (y + searchOffset >= cells.GetLength(1))
                yield break;

            for (int i = y; i <= y + searchOffset; i++)
            {
                yield return cells[x, i];
            }
        }

        IEnumerable<Cell> GetUpMaskCells()
        {
            if (y - searchOffset < 0)
                yield break;

            for (int i = y; i >= y - searchOffset; i--)
            {
                yield return cells[x, i];
            }
        }

        IEnumerable<Cell> GetRightDownMaskCells()
        {
            if (x + searchOffset >= cells.GetLength(0) ||
                y + searchOffset >= cells.GetLength(1))
                yield break;

            for (int i = x; i <= x + searchOffset; i++)
            {
                yield return cells[i, i];
            }
        }

        IEnumerable<Cell> GetLeftUpMaskCells()
        {
            if (x - searchOffset < 0 ||
                y - searchOffset < 0)
                yield break;

            for (int i = x; i >= x - searchOffset; i--)
            {
                yield return cells[i, y];
            }
        }

        IEnumerable<Cell> GetRightUpMaskCells()
        {
            if (x + searchOffset >= cells.GetLength(0) ||
                y + searchOffset >= cells.GetLength(1) ||
                x - searchOffset < 0 ||
                y - searchOffset < 0)
                yield break;

            for (int i = x; i <= x + searchOffset; i++)
            {
                yield return cells[i, searchOffset - i];
            }
        }

        IEnumerable<Cell> GetLeftDownMaskCells()
        {
            if (x + searchOffset >= cells.GetLength(0) ||
                y + searchOffset >= cells.GetLength(1) ||
                x - searchOffset < 0 ||
                y - searchOffset < 0)
                yield break;

            for (int i = x; i >= x - searchOffset; i--)
            {
                yield return cells[i, searchOffset - i];
            }
        }

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

            Cell nextWinningCell = GetNextTurn(map, player);

            Console.WriteLine((nextWinningCell.X + 1) + ":" + (nextWinningCell.Y + 1));
        }
    }
}