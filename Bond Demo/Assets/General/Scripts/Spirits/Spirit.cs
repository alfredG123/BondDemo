using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit
{
    private readonly BaseSpiritData base_spirit_data = null;

    private bool fight_with_player = false;
    //personality?
    //cystal for growth?
    //linked spirit? Current just one ally spirit

    #region Properties

    public Sprite SpiritSprite
    {
        get => (base_spirit_data.SpiritSprite);
    }

    public string SpiritName
    {
        get => (base_spirit_data.SpiritName);
    }

    public int Health
    {
        get => (base_spirit_data.Health);
    }

    public string HealthText
    {
        get => (base_spirit_data.Health.ToString());
    }

    public int Attack
    {
        get => (base_spirit_data.Attack);
    }

    public string AttackText
    {
        get => (base_spirit_data.Attack.ToString());
    }

    public int Defense
    {
        get => (base_spirit_data.Defense);
    }

    public string DefenseText
    {
        get => (base_spirit_data.Defense.ToString());
    }

    public int Speed
    {
        get => (base_spirit_data.Speed);
    }

    public string SpeedText
    {
        get => (base_spirit_data.Speed.ToString());
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

    public Spirit(BaseSpiritData _base_spirit_data)
    {
        base_spirit_data = _base_spirit_data;
    }

    public void JoinParty()
    {
        fight_with_player = true;
    }
}
