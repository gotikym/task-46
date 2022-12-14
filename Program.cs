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
        byte playerCount = 1;
        string namePlayer1 = ReadName(playerCount);
        playerCount++;
        string namePlayer2 = ReadName(playerCount);
        Warrior firstFighter = ChooseWarrior(namePlayer1);
        Warrior secondFighter = ChooseWarrior(namePlayer2);
        Console.WriteLine("Да начнется битва!");

        while (firstFighter.Health > 0 && secondFighter.Health > 0)
        {
            firstFighter.Attack(secondFighter);
            secondFighter.Attack(firstFighter);
            ShowStats(firstFighter);
            ShowStats(secondFighter);
            Console.ReadKey();
        }
    }

    private Warrior ChooseWarrior(string namePlayer)
    {
        List<Warrior> warriors = CreateWarriors();
        Console.WriteLine("\n" + namePlayer + " выберите бойца: ");
        ShowWarriors(warriors);
        int warriorIndex = GetNumber();
        int defoltIndex = 0;

        if (warriorIndex >= warriors.Count)
        {
            Console.WriteLine("Вы странный, ввели то, чего не было в выборе, ваш боец - воин");
            return warriors[defoltIndex];
        }

        return warriors[warriorIndex];
    }

    private List<Warrior> CreateWarriors()
    {
        List<Warrior> applicants = new List<Warrior>();
        applicants.Add(new Tank());
        applicants.Add(new Priest());
        applicants.Add(new Rogue());
        applicants.Add(new Shaman());
        applicants.Add(new Hunter());

        return applicants;
    }

    private string ReadName(byte playerCount)
    {
        Console.WriteLine("Для старта битвы введите своё имя, игрок номер " + playerCount);
        return Console.ReadLine();
    }

    private void ShowWarriors(List<Warrior> warriors)
    {
        byte idWarrior = 0;

        foreach (Warrior warrior in warriors)
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

abstract class Warrior
{
    public string Name { get; protected set; }
    public int Health { get; protected set; }
    public int Armor { get; protected set; }
    public int Damage { get; protected set; }
    public int AttackSpeed { get; protected set; }

    public Warrior()
    {
        Name = "Warrior";
        Health = 100;
        Armor = 15;
        Damage = 25;
        AttackSpeed = 1;
    }

    public virtual void TakeDamage(int damage)
    {
        if (Armor < damage)
        {
            Health -= damage - Armor;
        }
    }

    public virtual void Attack(Warrior fighter)
    {
        fighter.TakeDamage(Damage);
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine("Меня не видно, я абстрактный...");
    }

    public virtual bool GetChance(int chance)
    {
        Random random = new Random();
        int minChance = 0;
        int maxChance = 101;

        return chance >= random.Next(minChance, maxChance);
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

    public override void Attack(Warrior fighter)
    {
        fighter.TakeDamage(Damage);
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

        if (Armor < damage)
        {
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
    }

    public override void Attack(Warrior fighter)
    {
        fighter.TakeDamage(Damage);
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Жрец, отхиливается каждый раз на " + Heal + "% за счет маны.");
    }
}

class Rogue : Warrior
{
    public int LethalHitChance { get; private set; }
    public int IvasionChance { get; private set; }

    public Rogue() : base()
    {
        Name = "Rogue";
        LethalHitChance = 5;
        IvasionChance = 15;
    }

    public override void TakeDamage(int damage)
    {
        if (Armor < damage)
        {
            if (GetChance(IvasionChance))
            {
                Health -= damage - Armor;
            }
        }
    }

    public override void Attack(Warrior fighter)
    {
        int damage = Damage * AttackSpeed;
        int lethalHit = 99999;

        if (GetChance(LethalHitChance))
        {
            fighter.TakeDamage(lethalHit);
        }
        else
        {
            fighter.TakeDamage(damage);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Разбойник, имеет шанс в " + LethalHitChance + "% отравить быстродействующим смертельным ядом, так же шанс уклониться от атаки в " + IvasionChance + "%");
    }
}

class Shaman : Warrior
{
    public int Mana { get; private set; }
    public int AmountHealth { get; private set; }

    public Shaman() : base()
    {
        Name = "Shaman";
        AttackSpeed = 2;
        Mana = 100;
        AmountHealth = 50;
    }

    public override void TakeDamage(int damage)
    {
        if (Armor < damage)
        {
            if (Health + Armor <= damage && Mana == 100)
            {
                Mana = 0;
                Health += AmountHealth;
                Health -= damage - Armor;
            }
            else
            {
                Health -= damage - Armor;
            }
        }
    }

    public override void Attack(Warrior fighter)
    {
        int damage = Damage * AttackSpeed;
        fighter.TakeDamage(damage);
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Шаман, имеет изначально повышенную скорость атаки х" + AttackSpeed + ", при смертельном ударе восстанавливает себе " + AmountHealth + "% жизней за счет всей маны");
    }
}

class Hunter : Warrior
{
    public int AttackSpeedBow { get; private set; }
    public int DamageBow { get; private set; }
    public int Distance { get; private set; }

    public Hunter() : base()
    {
        Name = "Hunter";
        AttackSpeedBow = 2;
        DamageBow = 20;
        Distance = 2;
    }

    public override void TakeDamage(int damage)
    {
        if (Armor < damage)
        {
            Health -= damage - Armor;
        }
    }

    public override void Attack(Warrior fighter)
    {
        int damageBow = AttackSpeedBow * DamageBow;

        if(Distance > 0)
        {
            Distance--;
            fighter.TakeDamage(damageBow);
        }
        else
        {
            fighter.TakeDamage(Damage);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Охотник, урона меньше, чем у остальных, но пока враг дойдет, охотник успеет выстрелить " + Distance * AttackSpeedBow + "раза, а после будет сражаться ближним оружием");
    }
}
