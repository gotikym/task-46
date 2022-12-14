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
        List<Warrior> warriors = GetWarriors();
        Console.WriteLine("\n" + namePlayer + " выберите бойца: ");
        ShowApplicants();
        return warriors[GetNumber()];
    }

    private List<Warrior> GetWarriors()
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
        byte idWarrior = 0;
        List<Warrior> applicants = GetWarriors();

        foreach (Warrior warrior in applicants)
        {
            Console.Write(idWarrior++ + "  ");
            warrior.ShowInfo();
        }
    }

    private void ShowStats(Warrior fighter)
    {
        Console.WriteLine(fighter.Name + " HP: " + fighter.Health);
    }

    private int GetNumber()
    {
        bool isParse = false;
        int numberForReturn = 0;

        while (isParse == false)
        {
            string userNumber = Console.ReadLine();
            isParse = int.TryParse(userNumber, out numberForReturn);

            if (isParse == false)
            {
                Console.WriteLine("Вы не корректно ввели число.");
            }
        }

        return numberForReturn;
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
            return damage += damage;
        }
        else
        {
            return damage;
        }
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine("Воин, имеет " + DoubleDamageChance + "% шанса нанести удвоенный урон.");
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
        Console.WriteLine("Жрец, отхиливается каждый раз на " + Heal + "% за счет маны.");
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
        ivasionChance = 15;
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
        Console.WriteLine("Разбойник, имеет шанс в " + lethalHitChance + "% отравить быстродействующим смертельным ядом, так же шанс уклониться от атаки в " + ivasionChance + "%");
    }
}

class Shaman : Warrior
{
    public int Mana { get; private set; }
    public int Heal { get; private set; }

    public Shaman() : base()
    {
        Name = "Shaman";
        AttackSpeed = 2;
        Mana = 100;
        Heal = 50;
    }

    public override void TakeDamage(int damage)
    {
        if (Health + Armor <= damage && Mana == 100)
        {
            Mana = 0;
            Health += Heal;
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
        Console.WriteLine("Шаман, имеет изначально повышенную скорость атаки х" + AttackSpeed + ", при смертельном ударе восстанавливает себе " + Heal + "% жизней за счет всей маны");
    }
}
