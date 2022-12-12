using System;
using System.Collections.Generic;

internal class Program
{
    static void Main(string[] args)
    {
        Fight game = new Fight();
        game.StartBattle();
    }
}

class Fight
{
    public void StartBattle()
    {
        List<Warrior> fighters = new List<Warrior>();
        byte playerCount = 1;
        string namePlayer1 = EnterName(playerCount);
        playerCount++;
        string namePlayer2 = EnterName(playerCount);
        fighters.Add(ChooseWar(namePlayer1));
        fighters.Add(ChooseWar(namePlayer2));
        Warrior firstFighter = fighters[0];
        Warrior secondFighter = fighters[1];
        Console.WriteLine("Да начнется битва!");

        while (firstFighter.Health > 0 && secondFighter.Health > 0)
        {
            firstFighter.TakeDamage(secondFighter.MakeDamage());
            secondFighter.TakeDamage(firstFighter.MakeDamage());
            ShowStats(firstFighter);
            ShowStats(secondFighter);
            Console.ReadKey();
        }
    }

    private Warrior ChooseWar(string namePlayer)
    {
        const string CommandWarrior = "Воин";
        const string CommandTank = "Танк";
        const string CommandPriest = "Жрец";
        const string CommandRogue = "Разбойник";
        const string CommandShaman = "Шаман";

        Console.WriteLine("\n" + namePlayer + " выберите бойца, написав его имя: ");
        ShowApplicants();

        string userChoice = Console.ReadLine();

        switch (userChoice)
        {
            case CommandWarrior:
                return new Warrior();

            case CommandTank:
                return new Tank();

            case CommandPriest:
                return new Priest();

            case CommandRogue:
                return new Rogue();

            case CommandShaman:
                return new Shaman();
        }

        return null;
    }

    private List<Warrior> GetApplicants()
    {
        List<Warrior> applicants = new List<Warrior>();
        applicants.Add(new Warrior());
        applicants.Add(new Tank());
        applicants.Add(new Priest());
        applicants.Add(new Rogue());
        applicants.Add(new Shaman());

        return applicants;
    }

    private string EnterName(byte playerCount)
    {
        Console.WriteLine("Для старта битвы введите своё имя, игрок номер " + playerCount);
        return Console.ReadLine();
    }

    private void ShowApplicants()
    {
        List<Warrior> applicants = GetApplicants();

        foreach (Warrior warrior in applicants)
        {
            warrior.ShowInfo();
        }
    }

    private void ShowStats(Warrior fighter)
    {
        Console.WriteLine(fighter.Name + " HP: " + fighter.Health);
    }
}

class Warrior
{
    public string Name { get; protected set; }
    public int Health { get; protected set; }
    public int Armor { get; protected set; }
    public int Damage { get; protected set; }
    public int AttackSpeed { get; protected set; }
    public int DoubleDamageChance { get; protected set; }

    public Warrior()
    {
        Name = "Warrior";
        Health = 100;
        Armor = 15;
        Damage = 25;
        AttackSpeed = 1;
        DoubleDamageChance = 35;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage - Armor;
    }

    public virtual int MakeDamage()
    {
        int damage = Damage * AttackSpeed;

        if (GetChance(DoubleDamageChance))
        {
            return damage *= 2;
        }
        else
        {
            return damage;
        }
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine("Воин, имеет 75% шанса нанести удвоенный урон.");
    }

    public virtual bool GetChance(int chance)
    {
        Random random = new Random();
        int minChance = 0;
        int maxChance = 101;
        bool isSuccess = false;

        if (chance >= random.Next(minChance, maxChance))
        {
            isSuccess = true;
            return isSuccess;
        }
        else
        {
            return isSuccess;
        }
    }
}

class Tank : Warrior
{
    public Tank() : base()
    {
        Damage = 35;
        Name = "Tank";
        Armor = 30;
    }

    public override void TakeDamage(int damage)
    {
        if (Armor > damage)
        {
            Health += damage;
        }
        else
        {
            Health -= damage - Armor;
        }
    }

    public override int MakeDamage()
    {
        return Damage;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Танк, медленный и неповоротливый, но с мощной дубиной и если ваш удар слаб, ТАНК лишь поднимет свои хп =)");
    }
}

class Priest : Warrior
{
    public int Mana { get; private set; }
    public int Heal { get; private set; }

    public Priest() : base()
    {
        Name = "Priest";
        Armor = 10;
        Mana = 100;
        Heal = 30;
    }

    public override void TakeDamage(int damage)
    {
        int manaCostHeal = 20;

        if (Mana > 0)
        {
            Mana -= manaCostHeal;
            Health -= damage - Armor;
            Health += Heal;
        }
        else
        {
            Health -= damage - Armor;
        }
    }

    public override int MakeDamage()
    {
        return Damage;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Жрец, отхиливается каждый раз на 30% за счет маны.");
    }
}

class Rogue : Warrior
{
    public int lethalHitChance { get; private set; }
    public int ivasionChance { get; private set; }

    public Rogue() : base()
    {
        Name = "Rogue";
        lethalHitChance = 5;
        ivasionChance = 20;
    }

    public override void TakeDamage(int damage)
    {
        if (GetChance(ivasionChance))
        {
            Health -= damage - Armor;
        }
    }

    public override int MakeDamage()
    {
        int damage = Damage * AttackSpeed;
        int lethalHit = 99999;

        if (GetChance(lethalHitChance))
        {
            return lethalHit;
        }
        else
        {
            return damage;
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Разбойник, имеет шанс в 5% отравить быстродействующим смертельным ядом, так же шанс уклониться от атаки в 20%");
    }
}

class Shaman : Warrior
{
    public int Mana { get; private set; }

    public Shaman() : base()
    {
        Name = "Shaman";
        AttackSpeed = 2;
        Mana = 100;
    }

    public override void TakeDamage(int damage)
    {
        if (Health + Armor <= damage && Mana == 100)
        {
            Mana = 0;
            Health += 50;
            Health -= damage - Armor;
        }
        else
        {
            Health -= damage - Armor;
        }
    }

    public override int MakeDamage()
    {
        return Damage * AttackSpeed;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Шаман, имеет изначально повышенную скорость атаки (х2), при смертельном ударе восстанавливает себе 50% жизней за счет всей маны");
    }
}
