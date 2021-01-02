using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Spirit Data", menuName = "BOND/BaseSpiritData")]
public class BaseSpiritData : ScriptableObject
{
#pragma warning disable 0649
    // Base information for a spirit
    [SerializeField] private Sprite spirit_sprite;
    [SerializeField] private string spirit_name;
    [SerializeField] private int health;
    [SerializeField] private int stamina;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;
    [SerializeField] private List<SpiritSkill> spirit_skills;
#pragma warning restore 0649

    // Things to implement in the future
    //ability
    //moveset

    #region Properties

    public Sprite SpiritSprite
    {
        get => (spirit_sprite);
    }

    public string SpiritName
    {
        get => (spirit_name);
    }

    public int Health
    {
        get => (health);
    }

    public int Stamina
    {
        get => (stamina);
    }

    public int Attack
    {
        get => (attack);
    }

    public int Defense
    {
        get => (defense);
    }

    public int Speed
    {
        get => (speed);
    }

    public List<SpiritSkill> Skills
    {
        get => (spirit_skills);
    }
    #endregion
}
