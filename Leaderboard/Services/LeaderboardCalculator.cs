using System.Collections.Immutable;
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
    public IReadOnlyList<UserWithPlace> CalculatePlaces(IReadOnlyList<IUserWithScore> usersWithScores, LeaderboardMinScores leaderboardMinScores)
    {
        var sortedByScoreUsers = usersWithScores
            .OrderByDescending(user => user.Score)
            .ToArray();

        var result = new List<UserWithPlace>(sortedByScoreUsers.Length);

        var topThreeUsers = sortedByScoreUsers.Take(3).ToList();

        var firstPlaceTaken = false;
        var secondPlaceTaken = false;
        var thirdPlaceTaken = false;

        var finalPlace = 0;

        for (int i = 0; i < topThreeUsers.Count; i++)
        {
            var user = topThreeUsers[i];
            if (!firstPlaceTaken && user.Score >= leaderboardMinScores.FirstPlaceMinScore)
            {
                firstPlaceTaken = true;
                finalPlace = 1;
                result.Add(new UserWithPlace(user.UserId, 1));
            }
            else if (!secondPlaceTaken && user.Score >= leaderboardMinScores.SecondPlaceMinScore)
            {
                secondPlaceTaken = true;
                finalPlace = 2;
                result.Add(new UserWithPlace(user.UserId, 2));
            }
            else if (!thirdPlaceTaken && user.Score >= leaderboardMinScores.ThirdPlaceMinScore)
            {
                thirdPlaceTaken = true;
                finalPlace = 3;
                result.Add(new UserWithPlace(user.UserId, 3));
            }
            else 
            {
                finalPlace = i + 3;
                result.Add(new UserWithPlace(user.UserId, i + 3));
            }
        }

        result.AddRange(sortedByScoreUsers.Skip(3).Select((x, i) => new UserWithPlace(x.UserId, i + finalPlace + 1)));
        return result;
    }

}
