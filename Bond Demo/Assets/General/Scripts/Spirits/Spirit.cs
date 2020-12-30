using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit
{
    private readonly BaseSpiritData _base_spirit_data = null;

    private bool fight_with_player = false;
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

    public int Health
    {
        get => (_base_spirit_data.Health);
    }

    public string HealthText
    {
        get => (_base_spirit_data.Health.ToString());
    }

    public int Attack
    {
        get => (_base_spirit_data.Attack);
    }

    public string AttackText
    {
        get => (_base_spirit_data.Attack.ToString());
    }

    public int Defense
    {
        get => (_base_spirit_data.Defense);
    }

    public string DefenseText
    {
        get => (_base_spirit_data.Defense.ToString());
    }

    public int Speed
    {
        get => (_base_spirit_data.Speed);
    }

    public string SpeedText
    {
        get => (_base_spirit_data.Speed.ToString());
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

    public Spirit(BaseSpiritData base_spirit_data)
    {
        _base_spirit_data = base_spirit_data;
    }

    public void JoinParty()
    {
        fight_with_player = true;
    }
}
