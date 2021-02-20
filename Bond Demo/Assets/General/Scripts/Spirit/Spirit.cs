using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit
{
    public Spirit(BaseSpirit base_spirit_data, bool is_ally)
        : this(base_spirit_data, base_spirit_data.Name, is_ally)
    {
    }

    public Spirit(BaseSpirit base_spirit_data, string name, bool is_ally)
    {
        IsAlly = is_ally;

        Name = name;
        ImageName = base_spirit_data.ImageName;

        MaxHealth = base_spirit_data.Health;
        CurrentHealth = MaxHealth;

        MaxEnergy = base_spirit_data.Energy;
        CurrentEnergy = MaxEnergy;

        Attack = base_spirit_data.Attack;
        Speed = base_spirit_data.Speed;

        Accuracy = base_spirit_data.Accuracy;
        Evasion = base_spirit_data.EvasionChance;

        CriticalChance = base_spirit_data.CriticalHitChance;
        CriticalModifier = base_spirit_data.CriticalHitModifier;

        Weakness = base_spirit_data.Weakness;
        Resistance = base_spirit_data.Resistance;
        Negation = base_spirit_data.Negation;

        BasicAttack = base_spirit_data.BasicAttack;
        BasicDefend = base_spirit_data.BasicDefend;
        MoveSet = base_spirit_data.MoveSet;
    }

    public bool IsAlly { get; set; }

    public string Name { get; private set; }

    public int Level { get; private set; } = 5;

    public string ImageName { get; private set; }


    public float CurrentHealth { get; set; }
    public float MaxHealth { get; private set; }
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; private set; }
    public float Attack { get; set; }
    public float Speed { get; private set; }
    public float Accuracy { get; private set; }
    public float Evasion { get; private set; }
    public float CriticalChance { get; private set; }
    public float CriticalModifier { get; private set; }
    public List<TypeAttribute> Weakness { get; private set; }
    public List<TypeAttribute> Resistance { get; private set; }
    public List<TypeAttribute> Negation { get; private set; }
    public BasicAttackMove BasicAttack { get; private set; }
    public BasicDefendMove BasicDefend { get; private set; }
    public List<BaseMove> MoveSet { get; private set; }

    /*
    private readonly BaseSpirit _base_spirit_data = null;

    private bool _fight_with_player = false;
    private int _current_health = 0;
    //personality?
    //cystal for growth?
    //linked spirit? Current just one ally spirit

    #region Properties

    public Sprite SpiritSprite
    {
        get => (_base_spirit_data.SpiritSprite);
    }

    public string SpiritName
    {
        get => (_base_spirit_data.SpiritName);
    }

    public int MaxHealth
    {
        get => (_base_spirit_data.Health);
    }

    public int CurrentHealth
    {
        get
        {
            if (_current_health == 0)
            {
                _current_health = _base_spirit_data.Health;
            }

            return (_current_health);
        }

        set
        {
            _current_health = value;
        }
    }

    public string HealthText
    {
        get => (_base_spirit_data.Health.ToString());
    }

    public int Energy
    {
        get => (_base_spirit_data.Energy);
    }

    public int Attack
    {
        get => (_base_spirit_data.PhysicalAttack);
    }

    public string AttackText
    {
        get => (_base_spirit_data.PhysicalAttack.ToString());
    }

    public int Defense
    {
        get => (_base_spirit_data.PhysicalDefense);
    }

    public string DefenseText
    {
        get => (_base_spirit_data.PhysicalDefense.ToString());
    }

    public int Speed
    {
        get => (_base_spirit_data.Speed);
    }

    public string SpeedText
    {
        get => (_base_spirit_data.Speed.ToString());
    }

    public List<SpiritMove> MoveSet
    {
        get => (_base_spirit_data.MoveSet);
    }

    public bool IsAlly
    {
        get => (_fight_with_player);
    }

    public bool IsEnemy
    {
        get => (!_fight_with_player);
    }

    #endregion

    public Spirit(BaseSpirit base_spirit_data)
    {
        _base_spirit_data = base_spirit_data;
    }

    public void JoinParty()
    {
        _fight_with_player = true;
    }*/
}
