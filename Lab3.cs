using System;

class Hunter
{
    private static Random rnd = new Random();

    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Gold { get; private set; }
    public int Arrows { get; private set; }
    public int Loot { get; private set; }

    protected virtual int HitChance { get { return 50; } }
    protected virtual int MaxDamage { get { return 20; } }

    public Hunter(string name)
    {
        Name = name;
        Health = 100;
        Gold = 50;
        Arrows = 5;
        Loot = 0;
    }

    public bool CanHunt()
    {
        return Health > 0 && Arrows > 0;
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
        LoseHealth(rnd.Next(0, MaxDamage + 1));

        if (rnd.Next(100) < HitChance)
        {
            int prey = rnd.Next(1, 6);
            AddReward(prey, prey * 10);
        }
        else
        {
            Console.WriteLine(Name + " схибив.");
        }
    }

    private void LoseHealth(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
        Console.WriteLine(Name + " втратив здоров'я: " + damage);
    }

    private void AddReward(int prey, int reward)
    {
        Loot += prey;
        Gold += reward;
        Console.WriteLine(Name + " влучив! Здобич: " + prey + ", винагорода: " + reward + " золота.");
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

    public virtual void ShowInfo()
    {
        Console.WriteLine("Мисливець: " + Name + " | Здоров'я: " + Health + " | Золото: " + Gold + " | Стріли: " + Arrows + " | Здобич: " + Loot);
    }
}

class Archer : Hunter
{
    public Archer(string name) : base(name) { }

    protected override int HitChance { get { return 70; } }
    protected override int MaxDamage { get { return 30; } }

    public override void ShowInfo()
    {
        Console.Write("[Лучник] ");
        base.ShowInfo();
    }
}

class Trapper : Hunter
{
    public Trapper(string name) : base(name) { }

    protected override int HitChance { get { return 40; } }
    protected override int MaxDamage { get { return 5; } }

    public override void ShowInfo()
    {
        Console.Write("[Звіролов] ");
        base.ShowInfo();
    }
}

class Competition
{
    private Hunter first;
    private Hunter second;

    public Competition(Hunter first, Hunter second)
    {
        this.first = first;
        this.second = second;
    }

    public void Start(int rounds)
    {
        Console.WriteLine("Початок змагання!");
        first.ShowInfo();
        second.ShowInfo();

        for (int i = 1; i <= rounds; i++)
        {
            Console.WriteLine("\n--- Раунд " + i + " ---");
            first.Hunt();
            second.Hunt();
        }

        ShowResult();
    }

    private void ShowResult()
    {
        Console.WriteLine("\nПідсумки:");
        first.ShowInfo();
        second.ShowInfo();

        if (first.Loot > second.Loot)
            Console.WriteLine("Переможець: " + first.Name);
        else if (second.Loot > first.Loot)
            Console.WriteLine("Переможець: " + second.Name);
        else
            Console.WriteLine("Нічия!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("1 - Керувати мисливцем");
        Console.WriteLine("2 - Змагання мисливців");
        Console.Write("Ваш вибір: ");
        string mode = Console.ReadLine();

        if (mode == "1")
            ControlHunter();
        else
            new Competition(new Archer("Робін"), new Trapper("Артур")).Start(5);
    }

    static void ControlHunter()
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
                    int count;
                    if (int.TryParse(Console.ReadLine(), out count))
                        h.BuyArrows(count);
                    else
                        Console.WriteLine("Введіть число.");
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
}