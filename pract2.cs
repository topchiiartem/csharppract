// Завдання 1. Середній бал у групах студентів (зубчастий масив)

// Умова:
// На потоці є кілька груп (3-5). Кожна підгрупа містить від 10 до 30 студентів. 
// Оцінки студентів задані у вигляді зубчастого масиву int[][], де кожен елемент — це масив оцінок однієї підгрупи.

namespace PZ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            int[][] groups = new int[3][];

            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new int[random.Next(10, 31)];
            }

            for (int i = 0; i < groups.Length; i++)
            {
                for (int j = 0; j < groups[i].Length; j++)
                {
                    groups[i][j] = random.Next(60, 101);
                }
            }

            int totalSum = 0;
            int totalCount = 0;

            int PotikMin = groups[0][0];
            int PotikMax = groups[0][0];

            for (int i = 0; i < groups.Length; i++)
            {
                Console.WriteLine($"Група {i + 1} (кількість студентів: {groups[i].Length}):");

                int sum = 0;
                int min = groups[i][0];
                int max = groups[i][0];

                for (int j = 0; j < groups[i].Length; j++)
                {
                    Console.Write(groups[i][j] + " ");

                    sum += groups[i][j];

                    if (groups[i][j] < min)
                        min = groups[i][j];

                    if (groups[i][j] > max)
                        max = groups[i][j];

                    if (groups[i][j] < PotikMin)
                        PotikMin = groups[i][j];

                    if (groups[i][j] > PotikMax)
                        PotikMax = groups[i][j];

                    totalSum += groups[i][j];
                    totalCount++;
                }

                double average = (double)sum / groups[i].Length;

                Console.WriteLine();
                Console.WriteLine($"Середній бал: {average:F2}");
                Console.WriteLine($"Мінімальна оцінка: {min}");
                Console.WriteLine($"Максимальна оцінка: {max}");
                Console.WriteLine();
            }

            double streamAverage = (double)totalSum / totalCount;

            Console.WriteLine("Весь потік:");
            Console.WriteLine($"Середній бал: {streamAverage:F2}");
            Console.WriteLine($"Мінімальна оцінка: {PotikMin}");
            Console.WriteLine($"Максимальна оцінка: {PotikMax}");
        }
    }
}