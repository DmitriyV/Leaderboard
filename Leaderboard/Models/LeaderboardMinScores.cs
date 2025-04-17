namespace Leaderboard.Models;

public class LeaderboardMinScores
{
    public int FirstPlaceMinScore { get; }
    public int SecondPlaceMinScore { get; }
    public int ThirdPlaceMinScore { get; }

    public LeaderboardMinScores(int firstPlaceMinScore, int secondPlaceMinScore, int thirdPlaceMinScore)
    {
        FirstPlaceMinScore = firstPlaceMinScore;
        SecondPlaceMinScore = secondPlaceMinScore;
        ThirdPlaceMinScore = thirdPlaceMinScore;
    }
}
