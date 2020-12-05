using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTypeMonster : BaseMonster
{
    public EarthTypeMonster(string _name, int _dictionary_number, int _health, int _attack, int _defense, int _speed)
    {
        this.name = _name;
        this.attribute = "Earth";
        this.entry_number = _dictionary_number;
        this.health = _health;
        this.attack = _attack;
        this.defense = _defense;
        this.speed = _speed;
    }
}
