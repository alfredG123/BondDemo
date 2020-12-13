using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefab : MonoBehaviour
{
    [SerializeField] private MonsterData monster_data = null;

    private Monster monster;

    private void Awake()
    {
        if (monster_data != null)
        {
            monster = new Monster(monster_data);
        }
    }

    public Monster Monster
    {
        get => (monster);
        set { monster = value; }
    }

    public void PlayerMakeMove()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void EnemyMakeMove()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
