using NUnit.Framework;
using System.Diagnostics;
using System.Drawing;

[TestFixture]
public class GomokuAITests
{
    const int playerNumber = 1;
    const int gameMapSize = 15;
    int[,] map = new int[gameMapSize, gameMapSize];

    [TestCase("0 1", "1 1", "2 1", "3 1", ExpectedResult = "4 1")]
    [TestCase("1 0", "1 1", "1 2", "1 3", ExpectedResult = "1 4")]
    [TestCase("0 0", "1 1", "2 2", "3 3", ExpectedResult = "4 4")]
    [TestCase("1 1", "2 1", "4 1", "5 1", ExpectedResult = "3 1")]
    public string TestGetNextWinningTurn(params string[] points)
    {
        foreach (string point in points)
        {
            string[] pointCoordinates = point.Split();

            map[int.Parse(pointCoordinates[0]), int.Parse(pointCoordinates[1])] = playerNumber;
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        Point nextTurnPoint = GetNextWinningTurn(gameMapSize, gameMapSize, playerNumber);

        stopwatch.Stop();

        Assert.IsTrue(stopwatch.Elapsed.Milliseconds < 1000);

        return nextTurnPoint.X.ToString() + " " + nextTurnPoint.Y.ToString();
    }
}