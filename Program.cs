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
    private List<Warrior> _fighters = new List<Warrior>();
    public Warrior Player1 { get; private set; }
    public Warrior Player2 { get; private set; }    

    public void StartBattle()
    {
        Add();
        byte playerCount = 1;
        string namePlayer1 = EnterName(playerCount);
        playerCount++;
        string namePlayer2 = EnterName(playerCount);
        Player1 = ChooseWar(namePlayer1);
        Player2 = ChooseWar(namePlayer2);

        Console.WriteLine("Да начнется битва!");

        while(Player1.Health > 0 && Player2.Health > 0)
        {
            Player1.TakeDamage(Player2.MakeDamage());
            Player2.TakeDamage(Player1.MakeDamage());
            ShowStats(Player1);
            ShowStats(Player2);
            Console.ReadKey();
        }
    }

    private Warrior ChooseWar(string namePlayer)
    {
        Console.WriteLine(namePlayer + " выберите бойца: ");
        ShowWarrior();
        return _fighters[GetNumber()];   
    }

    private void Add()
    {
        _fighters.Add(new Warrior());
        _fighters.Add(new Tank());
        _fighters.Add(new Priest());
        _fighters.Add(new Rogue());
        _fighters.Add(new Shaman());
    }

    private string EnterName(byte playerCount)
    {        
        Console.WriteLine("Для старта битвы введите своё имя, игрок номер " + playerCount);
        return Console.ReadLine();
    }

    private void ShowWarrior()
    {
        int idWar = 0;

        foreach (Warrior warrior in _fighters)
        {
            Console.Write(idWar++ + " ");
            warrior.ShowInfo();
        }
    }

    private void ShowStats(Warrior player)
    {
        Console.WriteLine(player.Name + " HP: " + player.Health); 
    }

    private int GetNumber()
    {
        bool isParse = false;
        int numberForReturn = 0;

        while (isParse == false)
        {
            string userNumber = Console.ReadLine();

            if ((isParse = int.TryParse(userNumber, out int number)) == false)
            {
                Console.WriteLine("Вы не корректно ввели число.");
            }

            numberForReturn = number;
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
        DoubleDamageChance = 75;
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

        if(chance >= random.Next(minChance, maxChance))
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
        if(Armor > damage)
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

        if(Mana > 0)
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
        ivasionChance = 30;
    }

    public override void TakeDamage(int damage)
    {
        if(GetChance(ivasionChance))
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
        Console.WriteLine("Разбойник, имеет шанс в 5% отравить быстродействующим смертельным ядом, так же шанс уклониться от атаки в 30%");
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
        if(Health+Armor <= damage && Mana == 100)
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