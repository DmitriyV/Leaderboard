using Leaderboard.Models;

namespace Leaderboard.Services;

public interface ILeaderboardCalculator
{
    IReadOnlyList<UserWithPlace> CalculatePlaces(IReadOnlyList<IUserWithScore> usersWithScores, LeaderboardMinScores leaderboardMinScores);
}
