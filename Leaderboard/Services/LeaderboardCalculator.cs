using Leaderboard.Models;

// ReSharper disable once InvalidXmlDocComment
/**
 * ТРЕБОВАНИЕ:
 *
 * Необходимо реализовать функцию CalculatePlaces.
 * Функция распределяет места пользователей, учитывая ограничения для получения первых мест и набранные пользователями очки.
 * Подробное ТЗ смотреть в readme.txt
 */

/**
 * ТЕХНИЧЕСКИЕ ОГРАНИЧЕНИЯ:
 *
 * количество очков это всегда целое положительное число
 * FirstPlaceMinScore > SecondPlaceMinScore > ThirdPlaceMinScore > 0
 * в конкурсе участвует от 1 до 100 пользователей
 * 2 пользователя не могут набрать одинаковое количество баллов (разные баллы у пользователей гарантируются бизнес-логикой, не стоит усложнять алгоритм)
 * нет ограничений на скорость работы функции и потребляемую ей память
 * при реализации функции разрешается использование любых библиотек, любого стиля написания кода
 * в функцию передаются только валидные данные, которые соответствуют предыдущим ограничениям (проверять это в функции не нужно)
 */

/**
 * ВХОДНЫЕ ДАННЫЕ:
 *
 * usersWithScores - это список пользователей и заработанные каждым из них очки,
 * это неотсортированный массив вида [{userId: "id1", score: score1}, ... , {userId: "idn", score: scoreN}], где score1 ... scoreN различные положительные целые числа, id1 ... idN произвольные неповторяющиеся идентификаторы
 *
 * leaderboardMinScores - это значения минимального количества очков для первых 3 мест
 * это объект вида { FirstPlaceMinScore: score1, SecondPlaceMinScore: score2, ThirdPlaceMinScore : score3 }, где score1 > score2 > score3 > 0 целые положительные числа
 */

/**
 * РЕЗУЛЬТАТ:
 *
 * Функция должна вернуть пользователей с занятыми ими местами
 * Массив вида (сортировка массива не важна): [{UserId: "id1", Place: user1Place}, ..., {UserId: "idN", Place: userNPlace}], где user1Place ... userNPlace это целые положительные числа равные занятым пользователями местами, id1 ... idN идентификаторы пользователей из массива users
 */

namespace Leaderboard.Services;

public class LeaderboardCalculator : ILeaderboardCalculator
{
    private const int NumberOfPlaces = 3; //only 1st, 2nd, and 3rd places are eligible for an award
    private const int FirstPlace = 1;
    private const int SecondPlace = 2;
    private const int ThirdPlace = 3;

    public IReadOnlyList<UserWithPlace> CalculatePlaces(IReadOnlyList<IUserWithScore> usersWithScores,
        LeaderboardMinScores leaderboardMinScores)
    {
        var sortedByScoreUsers = SortUsersByScore(usersWithScores);

        var topScoreUsers = GetUsersEligibleForAward(leaderboardMinScores, sortedByScoreUsers);

        var winners = GetAwardUsers(topScoreUsers, leaderboardMinScores);

        var losers = GetNoAwardUsers(sortedByScoreUsers, topScoreUsers);

        return [.. winners, .. losers];

        static IUserWithScore[] SortUsersByScore(IReadOnlyList<IUserWithScore> usersWithScores) => [.. usersWithScores.OrderByDescending(user => user.Score)];
    }

    private static List<IUserWithScore> GetUsersEligibleForAward(LeaderboardMinScores leaderboardMinScores, IUserWithScore[] sortedByScoreUsers) =>
        [.. sortedByScoreUsers
            .Where(user => user.Score >= leaderboardMinScores.ThirdPlaceMinScore) // users with score lower than 3rd place are not eligible
            .Take(NumberOfPlaces)];

    private static List<UserWithPlace> GetAwardUsers(
        List<IUserWithScore> topUsers,
        LeaderboardMinScores leaderboardMinScores)
    {
        var placeAssignments = new[]
        {
            (leaderboardMinScores.FirstPlaceMinScore, FirstPlace),
            (leaderboardMinScores.SecondPlaceMinScore, SecondPlace),
            (leaderboardMinScores.ThirdPlaceMinScore, ThirdPlace)
        };

        List<UserWithPlace> awardPlaces = [];
        foreach (var user in topUsers)
        {
            foreach (var (minScore, place) in placeAssignments)
            {
                if (NotEnoughScore(user, minScore) || PlaceIsTaken(awardPlaces, place))
                    continue;

                awardPlaces.Add(new UserWithPlace(user.UserId, place));
                break;
            }
        }

        return awardPlaces;

        static bool NotEnoughScore(IUserWithScore user, int minScore) => user.Score < minScore;

        static bool PlaceIsTaken(List<UserWithPlace> result, int place) => result.Any(r => r.Place == place);
    }

    private static IEnumerable<UserWithPlace> GetNoAwardUsers(IUserWithScore[] sortedByScoreUsers, List<IUserWithScore> topUsers) =>
        sortedByScoreUsers
            .Skip(topUsers.Count)
            .Select((user, index) => new UserWithPlace(user.UserId, index + NumberOfPlaces + 1));
}