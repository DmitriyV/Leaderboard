namespace Leaderboard.Models;

public class User : IUserWithScore
{
    public long UserId { get; }
    public int Score { get; }

    public User(long userId, int score)
    {
        UserId = userId;
        Score = score;
    }
}
