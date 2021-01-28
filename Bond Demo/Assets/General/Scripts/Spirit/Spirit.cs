﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit
{
    private readonly BaseSpirit _base_spirit_data = null;

    public Spirit(BaseSpirit base_spirit_data)
    {
        _base_spirit_data = base_spirit_data;
    }

    public string Name
    {
        get => _base_spirit_data.Name;
    }

    public string ImageName
    {
        get => _base_spirit_data.ImageName;
    }

    public float Speed
    {
        get => _base_spirit_data.Speed;
    }

    public List<BaseMove> MoveSet
    {
        get => _base_spirit_data.MoveSet;
    }

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

    public int Stamina
    {
        get => (_base_spirit_data.Stamina);
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
