using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Data", menuName = "BOND/Monster")]
public class MonsterData : ScriptableObject
{
    // Base information for a monster
    [SerializeField] private Sprite monster_sprite = null;
    [SerializeField] private string monster_name = null;
    [SerializeField] private int health = 0;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int speed = 0;
    [SerializeField] private List<Attribute> weakness = null;

    // Things to implement in the future
    //ability
    //moveset

    #region Properties

    public Sprite MonsterSprite
    {
        get => (monster_sprite);
    }

    public string MonsterName
    {
        get => (monster_name);
    }

    public int Health
    {
        get => (health);
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

    public List<Attribute> Weakness
    {
        get => (weakness);
    }

    #endregion
}
