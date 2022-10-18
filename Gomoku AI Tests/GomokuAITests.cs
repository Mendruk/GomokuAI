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


    [TestCase("0 1", "1 1", "2 1", "3 1", ExpectedResult = "4 1")]
    [TestCase("1 0", "1 1", "1 2", "1 3", ExpectedResult = "1 4")]
    [TestCase("0 0", "1 1", "2 2", "3 3", ExpectedResult = "4 4")]
    [TestCase("0 4", "1 3", "2 2", "3 1", ExpectedResult = "4 0")]
    [TestCase("0 1", "1 1", "3 1", "4 1", ExpectedResult = "2 1")]
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