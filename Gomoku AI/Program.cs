namespace Gomoku_AI;

public class Program
{
    private const int WinningNumber = 5;

    public static Cell GetNextWinningTurn(Cell[,] cells, int playerNumber, out bool isWinningTurn)
    {
        foreach (Cell cell in cells.OfType<Cell>().Where(cell => cell.PlayerNumber == playerNumber))
        {
            foreach (Cell adjacentCell in EnumerateAdjacentCells(cell, cells))
            {
                if (IsWinningTurn(cells, adjacentCell, playerNumber))
                {
                    isWinningTurn = true;
                    return adjacentCell;
                }
            }
        }

        isWinningTurn = false;
        return GetDefaultPoint(cells);
    }

    public static Cell GetNextTurn(int[,] map, int playerNumber)
    {
        Cell[,] cells = new Cell[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                cells[x, y] = new Cell(x, y, map[x, y]);

        int enemyNumber = playerNumber == 1 ? 2 : 1;

        List<(Cell cell, int turnToWin)> cellList = new();

        foreach (Cell cell in cells.OfType<Cell>().Where(cell => cell.PlayerNumber == playerNumber ))
            GetNextCell(1, cell, out bool hasWinning);

        if (cellList.Count > 0)
            return cellList
                .MinBy(cell => cell.turnToWin)
                .cell;

        return GetDefaultPoint(cells);

        void GetNextCell(int turnNumber, Cell cell, out bool hasWinningTurn)
        {
            hasWinningTurn = false;

            foreach (Cell adjacentCell in EnumerateAdjacentCells(cell, cells).Where(c => c.PlayerNumber == 0))
            {
                Cell cellToReturn = GetNextWinningTurn(cells, playerNumber, out bool isWinningTurn);

                if (isWinningTurn)
                {
                    if (turnNumber == 1)
                        cellList.Add((cellToReturn, turnNumber));

                    hasWinningTurn = true;
                    return;
                }

                if (turnNumber > WinningNumber)
                {
                    hasWinningTurn = false;
                    return;
                }

                turnNumber++;

                adjacentCell.PlayerNumber = playerNumber;

                GetNextCell(turnNumber, adjacentCell, out hasWinningTurn);

                adjacentCell.PlayerNumber = 0;
                turnNumber--;
                if (hasWinningTurn)
                    cellList.Add((adjacentCell, turnNumber));
            }
        }
    }

    private static IEnumerable<Cell> EnumerateAdjacentCells(Cell cell, Cell[,] cells)
    {
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                if (cell.X + dx >= 0 && cell.X + dx < cells.GetLength(0) &&
                    cell.Y + dy >= 0 && cell.Y + dy < cells.GetLength(1))
                    yield return cells[cell.X + dx, cell.Y + dy];
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

    private static bool IsWinningTurn(Cell[,] cells, Cell cell, int playerNumber)
    {
        int x = cell.X;
        int y = cell.Y;
        int numberInHorizontalRow = 0;
        int searchOffset = WinningNumber - 1;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= cells.GetLength(0))
                continue;

            if (cells[i, y].PlayerNumber == playerNumber || i == x)
                numberInHorizontalRow++;
            else
                numberInHorizontalRow = 0;

            if (numberInHorizontalRow == WinningNumber)
                return true;
        }

        int numberInVerticalRow = 0;

        for (int i = y - searchOffset; i <= y + searchOffset; i++)
        {
            if (i < 0 || i >= cells.GetLength(1))
                continue;

            if (cells[x, i].PlayerNumber == playerNumber || i == y)
                numberInVerticalRow++;
            else
                numberInVerticalRow = 0;

            if (numberInVerticalRow == WinningNumber)
                return true;
        }

        int numberInAcross = 0;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= cells.GetLength(1))
                continue;

            if (cells[i, i].PlayerNumber == playerNumber || (i == x && i == y))
                numberInAcross++;
            else
                numberInAcross = 0;

            if (numberInAcross == WinningNumber)
                return true;
        }

        int numberInReverseAcross = 0;

        for (int i = x - searchOffset; i <= x + searchOffset; i++)
        {
            if (i < 0 || i >= cells.GetLength(1) ||
                searchOffset - i < 0 || searchOffset - i >= cells.GetLength(1))
                continue;

            if (cells[i, searchOffset - i].PlayerNumber == playerNumber || (i == x && searchOffset - i == y))
                numberInReverseAcross++;
            else
                numberInReverseAcross = 0;

            if (numberInReverseAcross == WinningNumber)
                return true;
        }

        return false;
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