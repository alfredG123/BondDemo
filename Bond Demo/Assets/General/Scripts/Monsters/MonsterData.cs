using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Data", menuName = "BOND/Monster")]
public class MonsterData : ScriptableObject
{
    // need to put back to private
    [SerializeField] public Sprite monster_sprite;

    [SerializeField] public string monster_name;
    
    [SerializeField] public int health;

    [SerializeField] public int attack;

    [SerializeField] public int defense;

    [SerializeField] public int speed;

    [SerializeField] public List<Attribute> weakness;

    //[SerializeField] public BaseMonsterTalent talent;

    //[SerializeField] public List<BaseMonsterSkill> skills;

    [SerializeField] public bool is_linked_monster;

    [SerializeField] public bool fight_with_player;
}
