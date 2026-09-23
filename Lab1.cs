
using System;

class Program
{
    static bool IsPositive(double a, double b, double c)
    {
        return a > 0 && b > 0 && c > 0;
    }

    static bool TriangleExists(double a, double b, double c)
    {
        return a + b > c && a + c > b && b + c > a;
    }

    static double GetPerimeter(double a, double b, double c)
    {
        return a + b + c;
    }

    static double GetArea(double a, double b, double c)
    {
        double p = GetPerimeter(a, b, c) / 2;
        return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
    }

    static string GetType(double a, double b, double c)
    {
        if (a == b && b == c)
            return "Рівносторонній";

        if (a == b || a == c || b == c)
            return "Рівнобедрений";

        if (a * a + b * b == c * c ||
            a * a + c * c == b * b ||
            b * b + c * c == a * a)
            return "Прямокутний";

        return "Довільний";
    }

    static void Main()
    {
        Console.Write("Введіть сторону a: ");
        double a = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть сторону b: ");
        double b = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть сторону c: ");
        double c = Convert.ToDouble(Console.ReadLine());

        if (!IsPositive(a, b, c))
        {
            Console.WriteLine("Сторони повинні бути додатними.");
            return;
        }

        if (!TriangleExists(a, b, c))
        {
            Console.WriteLine("Трикутник не існує.");
            return;
        }

        Console.WriteLine($"Периметр: {GetPerimeter(a, b, c)}");
        Console.WriteLine($"Площа: {GetArea(a, b, c):F2}");
        Console.WriteLine($"Тип: {GetType(a, b, c)}");
    }
}