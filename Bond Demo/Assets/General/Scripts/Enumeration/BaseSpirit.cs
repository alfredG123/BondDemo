using UnityEngine;
using System.Collections.Generic;

public class BaseSpirit : BaseEnumeration
{
    public static BaseSpirit A1 = new BaseSpirit(0, "A1", "SpiritA1", 39f, 39f, 52f, 43f, 65f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Fire, TypeAttribute.Plant }, new TypeAttribute[] { }, BasicAttackMove.Tackle, BasicDefendMove.Protect, new BaseMove[] { EnergyAttackMove.Ember, EnergyAttackMove.FireBlast, StatusMove.DamageUpSmall });
    public static BaseSpirit B1 = new BaseSpirit(1, "B1", "SpiritB1", 70f, 70f, 40f, 50f, 25f, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Water }, new TypeAttribute[] { }, BasicAttackMove.Tackle, BasicDefendMove.Protect, new BaseMove[] { });
    public static BaseSpirit C1 = new BaseSpirit(2, "C1", "SpiritC1", 50f, 50f, 50f, 77f, 91f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Wind }, new TypeAttribute[] { }, BasicAttackMove.Tackle, BasicDefendMove.Protect, new BaseMove[] { StatusMove.Burn, StatusMove.Posion, StatusMove.HarmfulField });
    public static BaseSpirit D1 = new BaseSpirit(3, "D1", "SpiritD1", 68f, 68f, 72f, 78f, 32f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Plant }, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric }, BasicAttackMove.Tackle, BasicDefendMove.Protect, new BaseMove[] { });
    public static BaseSpirit E1 = new BaseSpirit(4, "E1", "SpiritE1", 40f, 40f, 380f, 35f, 40f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Wind }, new TypeAttribute[] { }, BasicAttackMove.Tackle, BasicDefendMove.Protect, new BaseMove[] { });

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="image_name"></param>
    /// <param name="health"></param>
    /// <param name="energy"></param>
    /// <param name="attack"></param>
    /// <param name="defense"></param>
    /// <param name="speed"></param>
    /// <param name="weakness"></param>
    /// <param name="resistance"></param>
    /// <param name="negation"></param>
    /// <param name="basic_attack"></param>
    /// <param name="basic_defend"></param>
    /// <param name="moveset"></param>
    /// <param name="critical_hit_chance"></param>
    /// <param name="critical_hit_modifier"></param>
    /// <param name="evasion_chance"></param>
    /// <param name="accuracy"></param>
    public BaseSpirit(int id, string name, string image_name, float health, float energy, float attack, float defense, float speed, TypeAttribute[] weakness, TypeAttribute[] resistance, TypeAttribute[] negation, BasicAttackMove basic_attack, BasicDefendMove basic_defend, BaseMove[] moveset, float critical_hit_chance = .05f, float critical_hit_modifier = 2f, float evasion_chance = .01f, float accuracy = 1f)
        : base(id, name)
    {
        ImageName = image_name;

        Health = health;
        Energy = energy;
        Attack = attack;
        Defense = defense;
        Speed = speed;

        CriticalHitChance = critical_hit_chance;
        CriticalHitModifier = critical_hit_modifier;
        EvasionChance = evasion_chance;
        Accuracy = accuracy;

        for (int i = 0; i < weakness.Length; i++)
        {
            Weakness.Add(weakness[i]);
        }

        for (int i = 0; i < resistance.Length; i++)
        {
            Resistance.Add(resistance[i]);
        }

        for (int i = 0; i < negation.Length; i++)
        {
            Negation.Add(resistance[i]);
        }

        BasicAttack = basic_attack;
        BasicDefend = basic_defend;

        for (int i = 0; i < moveset.Length; i++)
        {
            MoveSet.Add(moveset[i]);
        }
    }

    #region Properties
    public string ImageName { get; private set; }
    public float Health { get; private set; }
    public float Energy { get; private set; }
    public float Attack { get; private set; }
    public float Defense { get; private set; }
    public float Speed { get; private set; }
    public float CriticalHitChance { get; private set; }
    public float CriticalHitModifier { get; private set; }
    public float EvasionChance { get; private set; }
    public float Accuracy { get; private set; }
    public BasicAttackMove BasicAttack { get; private set; }
    public BasicDefendMove BasicDefend { get; private set; }
    public List<BaseMove> MoveSet { get; private set; } = new List<BaseMove>();
    public List<TypeAttribute> Weakness { get; private set; } = new List<TypeAttribute>();
    public List<TypeAttribute> Resistance { get; private set; } = new List<TypeAttribute>();
    public List<TypeAttribute> Negation { get; private set; } = new List<TypeAttribute>();
    #endregion
}
