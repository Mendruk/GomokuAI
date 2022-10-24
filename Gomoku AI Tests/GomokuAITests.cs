using NUnit.Framework;
using System.Diagnostics;
using System.Drawing;
using static Gomoku_AI.Program;

namespace Gomoku_AI_Tests;

[TestFixture]
public class GomokuAITests
{
    private const int playerNumber = 1;
    private const int gameMapSize = 15;

    //4 of our stones in a row horizontally(vertically, diagonally),
    //but on one side the edge of the field, so the bot must put the
    //stone on the other side and win.
    [TestCase("0 1", "1 1", "2 1", "3 1", ExpectedResult = "4 1")]
    [TestCase("1 0", "1 1", "1 2", "1 3", ExpectedResult = "1 4")]
    [TestCase("0 0", "1 1", "2 2", "3 3", ExpectedResult = "4 4")]
    [TestCase("0 4", "1 3", "2 2", "3 1", ExpectedResult = "4 0")]

    //2 stones, pass, 2 stones. The bot must understand that although
    //this is not a 4, it can also be won in 1 move by placing a stone
    //in the gap in the middle.
    [TestCase("0 1", "1 1", "3 1", "4 1", ExpectedResult = "2 1")]
    [TestCase("1 0", "1 1", "1 3", "1 4", ExpectedResult = "1 2")]
    [TestCase("0 0", "1 1", "3 3", "4 4", ExpectedResult = "2 2")]
    [TestCase("0 4", "1 3", "3 1", "4 0", ExpectedResult = "2 2")]

    // The bot make a move and thereby bring the playing field
    // to a state where you can win in 1 move.
    [TestCase("0 1", "1 1", "2 1", ExpectedResult = "3 1")]
    [TestCase("1 0", "1 1", "1 2", ExpectedResult = "1 3")]
    [TestCase("0 0", "1 1", "2 2", ExpectedResult = "3 3")]
    [TestCase("0 4", "1 3", "2 2", ExpectedResult = "3 1")]

    [TestCase("0 1", "1 1", "3 1", ExpectedResult = "2 1")]
    [TestCase("1 0", "1 1", "1 3", ExpectedResult = "1 2")]
    [TestCase("0 0", "1 1", "3 3", ExpectedResult = "2 2")]
    [TestCase("0 4", "1 3", "3 1", ExpectedResult = "2 2")]

    public string TestGetNextTurn(params string[] points)
    {
        int[,] map = new int[gameMapSize, gameMapSize];

        foreach (string point in points)
        {
            string[] pointCoordinates = point.Split();

            map[int.Parse(pointCoordinates[0]), int.Parse(pointCoordinates[1])] = playerNumber;
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        Point nextWinningTurn = GetNextTurn(map, playerNumber);

        stopwatch.Stop();

        Assert.IsTrue(stopwatch.Elapsed.Milliseconds < 1000);

        return nextWinningTurn.X + " " + nextWinningTurn.Y;
    }
}