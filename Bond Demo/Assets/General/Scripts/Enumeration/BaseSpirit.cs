using UnityEngine;
using System.Collections.Generic;

public class BaseSpirit : BaseEnumeration
{
    public static BaseSpirit A1 = new BaseSpirit(0, "A1", 39f, 52f, 43f, 60f, 50f, 65f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Fire, TypeAttribute.Plant }, new TypeAttribute[] { });
    public static BaseSpirit B1 = new BaseSpirit(1, "B1", 70f, 40f, 50f, 55f, 50f, 25f, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Water }, new TypeAttribute[] { });
    public static BaseSpirit C1 = new BaseSpirit(2, "C1", 50f, 50f, 77f, 95f, 77f, 91f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Wind }, new TypeAttribute[] { });
    public static BaseSpirit D1 = new BaseSpirit(3, "D1", 68f, 72f, 78f, 38f, 42f, 32f, new TypeAttribute[] { TypeAttribute.Water, TypeAttribute.Plant }, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric });
    public static BaseSpirit E1 = new BaseSpirit(4, "E1", 40f, 38f, 35f, 54f, 35f, 40f, new TypeAttribute[] { TypeAttribute.Earth }, new TypeAttribute[] { TypeAttribute.Electric, TypeAttribute.Plant, TypeAttribute.Wind }, new TypeAttribute[] { });

    private readonly float _critical_chance = .05f;
    private readonly float _evasion = 0.01f;
    private readonly float _accuracy = 1;

    private readonly List<TypeAttribute> _weakness = null;
    private readonly List<TypeAttribute> _resistance = null;
    private readonly List<TypeAttribute> _perfect_guard = null;

    public BaseSpirit(int id, string name, float health, float physicalAttack, float physicalDefense, float etherAttack, float etherDefense, float speed, TypeAttribute[] weakness, TypeAttribute[] resistance, TypeAttribute[] perfect_guard)
        : base(id, name)
    {
        Health = health;
        PhysicalAttack = physicalAttack;
        PhysicalDefense = physicalDefense;
        EtherAttack = etherAttack;
        EtherDefense = etherDefense;
        Speed = speed;

        _weakness = new List<TypeAttribute>();
        _resistance = new List<TypeAttribute>();
        _perfect_guard = new List<TypeAttribute>();

        for (int i = 0; i < weakness.Length; i++)
        {
            _weakness.Add(weakness[i]);
        }

        for (int i = 0; i < resistance.Length; i++)
        {
            _resistance.Add(resistance[i]);
        }

        for (int i = 0; i < perfect_guard.Length; i++)
        {
            _perfect_guard.Add(resistance[i]);
        }
    }

    #region Properties
    public float Health { get; }

    public float PhysicalAttack { get; }

    public float PhysicalDefense { get; }

    public float EtherAttack { get; }

    public float EtherDefense { get; }

    public float Speed { get; }
    #endregion

    public bool CheckCriticalHit()
    {
        bool is_critial_hit = false;
        float random_number;

        random_number = Random.Range(0f, 1f);

        if (random_number < _critical_chance)
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

        if (random_number < _accuracy)
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

        if (random_number < _evasion)
        {
            is_attack_miss = true;
        }

        return (is_attack_miss);
    }

    public TypeEffectiveness CheckEffectiveness(TypeAttribute attack_move_attribute)
    {
        TypeEffectiveness effectiveness = TypeEffectiveness.Effective;

        if ((effectiveness == TypeEffectiveness.Effective) && (_weakness.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.SuperEffective;
        }

        if ((effectiveness == TypeEffectiveness.Effective) && (_resistance.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.NotEffective;
        }

        if ((effectiveness == TypeEffectiveness.Effective) && (_perfect_guard.Contains(attack_move_attribute)))
        {
            effectiveness = TypeEffectiveness.NoEffect;
        }

        return (effectiveness);
    }
}
