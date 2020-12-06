using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster
{
    protected string name;
    protected int entry_number;
    protected int health;
    protected int attack;
    protected int defense;
    protected int speed;
    protected List<Attribute> weakness;
    protected BaseMonsterTalent talent;
    protected List<BaseMonsterSkill> skills;

    public BaseMonster()
    {
        name = "Origin Of All";
        entry_number = 0;
        
        health = 1;
        attack = 0;
        defense = 0;
        speed = 0;

        weakness = new List<Attribute>();

        talent = new BaseMonsterTalent();

        skills = new List<BaseMonsterSkill>();
        skills.Add(new BaseMonsterSkill());
    }

    #region Properties

    public int EntryNumber
    {
        get { return (entry_number); }
    }

    public string MonsterName
    {
        get { return (name); }
    }

    public int Health
    {
        get { return (health); }
    }

    public int Attack
    {
        get { return (attack); }
    }

    public int Defense
    {
        get { return (defense); }
    }

    public int Speed
    {
        get { return (speed); }
    }

    public List<Attribute> MonsterWeakness
    {
        get { return (weakness); }
    }

    public BaseMonsterTalent Talent
    {
        get { return (talent); }
    }

    #endregion

    public bool check_skill_effectiveness(Attribute _attribute)
    {
        bool is_effective = false;

        if (weakness.Contains(_attribute))
        {
            is_effective = true;
        }

        return (is_effective);
    }
}
