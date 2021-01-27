using UnityEngine;
using System.Collections.Generic;

public class BaseSpirit : BaseEnumeration
{
    public static BaseSpirit A1 = new BaseSpirit(0, "A1", "SpiritA1", 39f, 52f, 43f, 65f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Fire, TypeAttribute.Plant }, new TypeAttribute[] { }, new BaseMove[] { BaseMove.Tackle });
    public static BaseSpirit B1 = new BaseSpirit(1, "B1", "SpiritB1", 70f, 40f, 50f, 25f, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Water }, new TypeAttribute[] { }, new BaseMove[] { BaseMove.Tackle });
    public static BaseSpirit C1 = new BaseSpirit(2, "C1", "SpiritC1", 50f, 50f, 77f, 91f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Wind }, new TypeAttribute[] { }, new BaseMove[] { BaseMove.Tackle });
    public static BaseSpirit D1 = new BaseSpirit(3, "D1", "SpiritD1", 68f, 72f, 78f, 32f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Plant }, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric }, new BaseMove[] { BaseMove.Tackle });
    public static BaseSpirit E1 = new BaseSpirit(4, "E1", "SpiritE1", 40f, 38f, 35f, 40f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Wind }, new TypeAttribute[] { }, new BaseMove[] { BaseMove.Tackle });

    private readonly float _CriticalChance = .05f;
    private readonly float _Evasion = 0.01f;
    private readonly float _Accuracy = 1;

    private readonly List<TypeAttribute> _Weakness = null;
    private readonly List<TypeAttribute> _Resistance = null;
    private readonly List<TypeAttribute> _Negation = null;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="image_name"></param>
    /// <param name="health"></param>
    /// <param name="attack"></param>
    /// <param name="defense"></param>
    /// <param name="speed"></param>
    /// <param name="weakness"></param>
    /// <param name="resistance"></param>
    /// <param name="negation"></param>
    /// <param name="moveset"></param>
    public BaseSpirit(int id, string name, string image_name, float health, float attack, float defense, float speed, TypeAttribute[] weakness, TypeAttribute[] resistance, TypeAttribute[] negation, BaseMove[] moveset)
        : base(id, name)
    {
        ImageName = image_name;

        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;

        _Weakness = new List<TypeAttribute>();
        _Resistance = new List<TypeAttribute>();
        _Negation = new List<TypeAttribute>();

        for (int i = 0; i < weakness.Length; i++)
        {
            _Weakness.Add(weakness[i]);
        }

        for (int i = 0; i < resistance.Length; i++)
        {
            _Resistance.Add(resistance[i]);
        }

        for (int i = 0; i < negation.Length; i++)
        {
            _Negation.Add(resistance[i]);
        }

        for (int i = 0; i < moveset.Length; i++)
        {
            MoveSet.Add(moveset[i]);
        }
    }

    #region Properties
    public float Health { get; }

    public float Attack { get; }

    public float Defense { get; }

    public float Speed { get; }

    public string ImageName { get; }

    public List<BaseMove> MoveSet { get; private set; } = new List<BaseMove>();
    #endregion

    public bool CheckCriticalHit()
    {
        bool is_critial_hit = false;
        float random_number;

        random_number = Random.Range(0f, 1f);

        if (random_number < _CriticalChance)
        {
            is_critial_hit = true;
        }

        return (is_critial_hit);
    }

    public bool CheckAttackHit()
    {
        bool is_attack_hit = false;
        float random_number;

        random_number = Random.Range(0f, 1f);

        if (random_number < _Accuracy)
        {
            is_attack_hit = true;
        }

        return (is_attack_hit);
    }

    public bool CheckAttackMiss()
    {
        bool is_attack_miss = false;
        float random_number;

        random_number = Random.Range(0f, 1f);

        if (random_number < _Evasion)
        {
            is_attack_miss = true;
        }

        return (is_attack_miss);
    }

    public TypeEffectiveness CheckEffectiveness(TypeAttribute attack_move_attribute)
    {
        TypeEffectiveness effectiveness = TypeEffectiveness.Effective;

        if ((effectiveness == TypeEffectiveness.Effective) && (_Weakness.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.SuperEffective;
        }

        if ((effectiveness == TypeEffectiveness.Effective) && (_Resistance.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.NotEffective;
        }

        if ((effectiveness == TypeEffectiveness.Effective) && (_Negation.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.NoEffect;
        }

        return (effectiveness);
    }
}
