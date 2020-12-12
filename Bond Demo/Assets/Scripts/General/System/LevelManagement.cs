using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManagement : MonoBehaviour
{
    [SerializeField] private GameObject sample_monster;

    public List<GameObject> GenerateMonstersForEnemy()
    {
        List<GameObject> enemy_monster_team = new List<GameObject>();

        enemy_monster_team.Add(sample_monster);

        return (enemy_monster_team);
    }
}
