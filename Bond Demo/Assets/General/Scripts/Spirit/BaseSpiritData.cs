using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Spirit Data", menuName = "BOND/BaseSpiritData")]
public class BaseSpiritData : ScriptableObject
{
#pragma warning disable 0649
    // Base information for a spirit
    [SerializeField] private Sprite BaseSprite;
    [SerializeField] private string BaseName;

    [SerializeField] private List<TypeAttribute> BaseWeakness;
    [SerializeField] private List<TypeAttribute> BaseResistance;

    [SerializeField] private int BaseHealth;
    [SerializeField] private int BasePhysicalAttack;
    [SerializeField] private int BaseElementalAttack;
    [SerializeField] private int BasePhysicalDefense;
    [SerializeField] private int BaseElementalDefense;
    [SerializeField] private int BaseSpeed;

    [SerializeField] private List<SpiritMove> BaseMoveSet;
#pragma warning restore 0649

    // Things to implement in the future
    //ability
    //moveset

    #region Properties

    public Sprite SpiritSprite
    {
        get => BaseSprite;
    }

    public string SpiritName
    {
        get => BaseName;
    }

    public List<TypeAttribute> Weakness
    {
        get => BaseWeakness;
    }

    public List<TypeAttribute> Resistance
    {
        get => BaseResistance;
    }

    public int Health
    {
        get => BaseHealth;
    }

    public int PhysicalAttack
    {
        get => BasePhysicalAttack;
    }

    public int ElementalAttack
    {
        get => BaseElementalAttack;
    }

    public int PhysicalDefense
    {
        get => BasePhysicalDefense;
    }

    public int ElementalDefense
    {
        get => BaseElementalDefense;
    }

    public int Speed
    {
        get => BaseSpeed;
    }

    public List<SpiritMove> MoveSet
    {
        get => BaseMoveSet;
    }

    public int energy { get; } = 100;

    public int energyRegeneration { get; } = 20;

    public int AttackAccuracy { get; } = 100;

    public int AttackEvasion { get; } = 0;

    public int EffectAccuracy { get; } = 100;

    public int EffectEvasion { get; } = 0;

    public int CritialChance { get; } = 0;

    public int CriticalDamageModifier { get; } = 200;
    #endregion
}
