namespace Leaderboard.Models;

public class UserWithPlace
{
    public long UserId { get; }
    public int Place { get; }

    public UserWithPlace(long userId, int place)
    {
        UserId = userId;
        Place = place;
    }
}
