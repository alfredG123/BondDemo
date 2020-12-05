using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster
{
    protected string name;
    protected string attribute;
    protected int entry_number;
    protected int health;
    protected int attack;
    protected int defense;
    protected int speed;

    public BaseMonster()
    {
        name = "unknown";
        attribute = "unknown";
        entry_number = 0;
        health = 0;
        attack = 0;
        defense = 0;
        speed = 0;
    }

    public string MonsterName
    {
        get { return (this.name); }
    }

    public string Attribute
    {
        get { return (this.attribute); }
    }

    public int Health
    {
        get { return (this.health); }
    }

    public int Attack
    {
        get { return (this.attack); }
    }

    public int Defense
    {
        get { return (this.defense); }
    }

    public int Speed
    {
        get { return (this.speed); }
    }
}
