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

    public BaseMove GetMove(int move_to_get_index)
    {
        BaseMove move_to_get;

        if (move_to_get_index == 0)
        {
            move_to_get = BasicAttack;
        }
        else if (move_to_get_index == 1)
        {
            move_to_get = BasicDefend;
        }
        else
        {
            move_to_get_index -= 2;

            move_to_get = MoveSet[move_to_get_index];
        }

        return (move_to_get);
    }

    public void UpgradeMove(int move_to_upgrade_index)
    {
        BaseMove move_to_upgrade;

        if (move_to_upgrade_index == 0)
        {
            BasicAttack = BasicAttack.NextLevelMove;
        }
        else if (move_to_upgrade_index == 1)
        {

        }
        else
        {
            move_to_upgrade_index -= 2;

            move_to_upgrade = MoveSet[move_to_upgrade_index];

            if (move_to_upgrade is EnergyAttackMove energy_move)
            {
                MoveSet[move_to_upgrade_index] = energy_move.NextLevelMove;
            }
        }
    }
}
