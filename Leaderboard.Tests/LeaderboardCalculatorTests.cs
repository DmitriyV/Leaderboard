using Leaderboard.Models;
using Leaderboard.Services;

namespace Leaderboard.Tests;

public class LeaderboardCalculatorTests
{
    private readonly ILeaderboardCalculator _leaderboardCalculator = new LeaderboardCalculator();

    #region Default Tests

    // 4 5 6
    [Fact]
    public void NotEnoughScoreForFirstPlaces()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new(userId: 1, score: 3),
            new(userId: 2, score: 2),
            new(userId: 3, score: 1)
        };

        var result = _leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new(userId: 1, place: 4),
            new(userId: 2, place: 5),
            new(userId: 3, place: 6)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    // 1 4 5 6
    [Fact]
    public void OnlyFirstPlaceAndLowScores()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new(userId: 1, score: 100),
            new(userId: 2, score: 3),
            new(userId: 3, score: 2),
            new(userId: 4, score: 1)
        };

        var result = _leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new(userId: 1, place: 1),
            new(userId: 2, place: 4),
            new(userId: 3, place: 5),
            new(userId: 4, place: 6)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    // 1
    [Fact]
    public void OnlyFirstPlace()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new(userId: 1, score: 111)
        };

        var result = _leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new(userId: 1, place: 1)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    // 2
    [Fact]
    public void OnlySecondPlace()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new(userId: 1, score: 55)
        };

        var result = _leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new(userId: 1, place: 2)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    // 3
    [Fact]
    public void OnlyThirdlace()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new(userId: 1, score: 15)
        };

        var result = _leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new(userId: 1, place: 3)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    #endregion

    #region Helper Methods

    private static bool CheckResult(IReadOnlyList<UserWithPlace>? result, IReadOnlyList<UserWithPlace> expectedResult)
    {
        if (result == null)
        {
            return false;
        }

        List<UserWithPlace> resultList;
        try
        {
            resultList = result.ToList();
        }
        catch
        {
            return false;
        }

        var expectedResultList = expectedResult.ToList();
        if (resultList.Count != expectedResultList.Count)
        {
            return false;
        }

        foreach (var expectedResultUser in expectedResultList)
        {
            var resultUser = resultList.FirstOrDefault(e => e?.UserId == expectedResultUser.UserId);
            if (resultUser == null || resultUser.Place != expectedResultUser.Place)
            {
                return false;
            }
        }

        return true;
    }

    #endregion
}
