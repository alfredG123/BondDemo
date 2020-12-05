using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTypeMonster : BaseMonster
{
    public PlantTypeMonster(string _name, int _dictionary_number, int _health, int _attack, int _defense, int _speed)
    {
        this.name = _name;
        this.attribute = "Plant";
        this.entry_number = _dictionary_number;
        this.health = _health;
        this.attack = _attack;
        this.defense = _defense;
        this.speed = _speed;
    }
}
