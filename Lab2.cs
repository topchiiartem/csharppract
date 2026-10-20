using System;

class Hunter
{
    public string Name;
    public int Health;
    public int Gold;
    public int Arrows;
    public int Loot;
    static Random rnd = new Random();

    public Hunter(string name)
    {
        Name = name;
        Health = 100;
        Gold = 50;
        Arrows = 5;
        Loot = 0;
    }

    public void Hunt()
    {
        if (Health <= 0)
        {
            Console.WriteLine(Name + " не може полювати: немає здоров'я!");
            return;
        }
        if (Arrows <= 0)
        {
            Console.WriteLine(Name + " не може полювати: немає стріл!");
            return;
        }

        Arrows--;
        int damage = rnd.Next(0, 21);
        Health -= damage;
        if (Health < 0) Health = 0;

        if (rnd.Next(2) == 0)
        {
            int prey = rnd.Next(1, 6);
            int reward = prey * 10;
            Loot += prey;
            Gold += reward;
            Console.WriteLine(Name + " влучив! Здобич: " + prey + ", винагорода: " + reward + " золота.");
        }
        else
        {
            Console.WriteLine(Name + " схибив.");
        }
        Console.WriteLine("Втрачено здоров'я: " + damage);
    }

    public void BuyArrows(int count)
    {
        int price = count * 5;
        if (count <= 0)
        {
            Console.WriteLine("Некоректна кількість.");
        }
        else if (Gold < price)
        {
            Console.WriteLine("Недостатньо золота. Потрібно: " + price);
        }
        else
        {
            Gold -= price;
            Arrows += count;
            Console.WriteLine(Name + " купив стріл: " + count + " за " + price + " золота.");
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine("Мисливець: " + Name + " | Здоров'я: " + Health + " | Золото: " + Gold + " | Стріли: " + Arrows + " | Здобич: " + Loot);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("1 - Керувати мисливцем");
        Console.WriteLine("2 - Змагання двох мисливців");
        Console.Write("Ваш вибір: ");
        string mode = Console.ReadLine();

        if (mode == "1")
            Single();
        else
            Competition();
    }

    static void Single()
    {
        Hunter h = new Hunter("Робін");
        h.ShowInfo();

        bool run = true;
        while (run)
        {
            Console.WriteLine("\n1 - Полювати");
            Console.WriteLine("2 - Купити стріли");
            Console.WriteLine("3 - Інформація");
            Console.WriteLine("0 - Вихід");
            Console.Write("Дія: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    h.Hunt();
                    break;
                case "2":
                    Console.Write("Скільки стріл купити? ");
                    int count = int.Parse(Console.ReadLine());
                    h.BuyArrows(count);
                    break;
                case "3":
                    h.ShowInfo();
                    break;
                case "0":
                    run = false;
                    break;
                default:
                    Console.WriteLine("Невідома дія.");
                    break;
            }
        }
    }

    static void Competition()
    {
        Hunter a = new Hunter("Робін");
        Hunter b = new Hunter("Артур");

        Console.WriteLine("Початок змагання!");
        a.ShowInfo();
        b.ShowInfo();
        Console.WriteLine();

        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine("--- Раунд " + i + " ---");
            a.Hunt();
            b.Hunt();
        }

        Console.WriteLine("\nРезультати:");
        a.ShowInfo();
        b.ShowInfo();

        if (a.Loot > b.Loot)
            Console.WriteLine("Переможець: " + a.Name);
        else if (b.Loot > a.Loot)
            Console.WriteLine("Переможець: " + b.Name);
        else
            Console.WriteLine("Нічия!");
    }
}