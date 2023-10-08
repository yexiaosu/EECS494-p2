using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public int ID;
    public string Name;
    public int Cost;
    public int Damage;
    public int Heal;
    public int Sheid;
    public int Times;  // how many times the damage will take effect
    public int Rarity;  // 0: nomarl, 1: rare, 2: legendary
    public bool IsRune;
    public int Mana;

    public Card(int id, string name, int cost, int damage, int heal, int sheid, int times, int rarity, bool isRune = false, int mana = 0)
    {
        ID = id;
        Name = name;
        Cost = cost;
        Damage = damage;
        Heal = heal;
        Sheid = sheid;
        Times = times;
        Rarity = rarity;
        IsRune = isRune;
        Mana = mana;
    }
}

public static class CardsInfo
{
    static List<Card> Cards = new List<Card> {
        new Card(0, "Attack", 0, 5, 0, 0, 1, 0),
        new Card(1, "Heal", 0, 0, 5, 0, 1, 0),
        new Card(2, "Sheid", 0, 0, 0, 10, 1, 0),
        new Card(3, "Attack (II)", 0, 2, 0, 0, 2, 0),
        new Card(4, "Lifesteal", 1, 8, 5, 0, 1, 1),
        new Card(5, "Blizzard", 0, 2, 0, 0, 4, 1),
        new Card(6, "Mana Surge", 0, 5, 0, 0, 1, 1, mana: 2),
        new Card(7, "Fireball", 1, 15, 0, 0, 1, 0),
        new Card(8, "Healing Rune", 0, 0, 2, 0, 1, 2, true),
    };

    public static List<Card> GetCards()
    {
        return Cards;
    }
}
