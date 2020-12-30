using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Spirit Data", menuName = "BOND/BaseSpiritData")]
public class BaseSpiritData : ScriptableObject
{
    // Base information for a spirit
    [SerializeField] private Sprite spirit_sprite = null;
    [SerializeField] private string spirit_name = null;
    [SerializeField] private int health = 0;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int speed = 0;

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

    #endregion
}
