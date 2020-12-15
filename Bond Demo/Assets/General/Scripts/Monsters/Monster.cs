using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    private readonly MonsterData base_monster_data = null;

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
        get => (base_monster_data.Health);
    }

    public string HealthText
    {
        get => (base_monster_data.Health.ToString());
    }

    public int Attack
    {
        get => (base_monster_data.Attack);
    }

    public string AttackText
    {
        get => (base_monster_data.Attack.ToString());
    }

    public int Defense
    {
        get => (base_monster_data.Defense);
    }

    public string DefenseText
    {
        get => (base_monster_data.Defense.ToString());
    }

    public int Speed
    {
        get => (base_monster_data.Speed);
    }

    public string SpeedText
    {
        get => (base_monster_data.Speed.ToString());
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

    public Monster(MonsterData _base_monster_data)
    {
        base_monster_data = _base_monster_data;
    }

    public void JoinParty()
    {
        fight_with_player = true;
    }
}
