using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData base_monster_data = null;

    private int health;
    private int attack;
    private int defense;
    private int speed;
    private bool fight_with_player = false;
    //personality?
    //cystal for growth?
    //linked monster? Current just one ally monster

    #region Properties

    public Sprite MonsterSprite
    {
        get => (base_monster_data.MonsterSprite);
    }

    public string MonsterName
    {
        get => (base_monster_data.MonsterName);
    }

    public int Health
    {
        get => (health);
    }

    public string HealthText
    {
        get => (health.ToString());
    }

    public int Attack
    {
        get => (attack);
    }

    public string AttackText
    {
        get => (attack.ToString());
    }

    public int Defense
    {
        get => (defense);
    }

    public string DefenseText
    {
        get => (defense.ToString());
    }

    public int Speed
    {
        get => (speed);
    }

    public string SpeedText
    {
        get => (speed.ToString());
    }

    public List<Attribute> Weakness
    {
        get => (base_monster_data.Weakness);
    }

    public bool IsAlly
    {
        get => (fight_with_player);
    }

    public bool IsEnemy
    {
        get => (!fight_with_player);
    }

    #endregion

    private void Awake()
    {
        health = base_monster_data.Health;
        attack = base_monster_data.Attack;
        defense = base_monster_data.Defense;
        speed = base_monster_data.Speed;
    }

    public void JoinParty()
    {
        fight_with_player = true;
    }
}
