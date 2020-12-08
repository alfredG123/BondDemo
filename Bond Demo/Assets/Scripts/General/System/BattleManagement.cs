using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject ally_positions = null;
    [SerializeField] private GameObject enemy_positions = null;
    [SerializeField] private GameObject game_manager_prefab = null;

    private GameObject game_manager;
    private PlayerManagement player;
    private LevelManagement level_manager;
    private PrefabStorage prefab_manager;

    private void Start()
    {
        game_manager = GameObject.Find("GeneralGameManager");

        // For debug purpose
        if (game_manager == null)
        {
            game_manager = GeneralScripts.CreateDefaultGameManager(game_manager_prefab);
        }

        player = game_manager.GetComponent<PlayerManagement>();
        level_manager = game_manager.GetComponent<LevelManagement>();
        prefab_manager = game_manager.GetComponent<PrefabStorage>();

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();
    }

    private void SpawnMonstersForPlayer()
    {
        SpawnMonsters(player.Team, ally_positions);
    }

    private void SpawnMonstersForEnemy()
    {
        List<BaseMonsterInfo> enemy_team = level_manager.GenerateMonstersForEnemy();

        SpawnMonsters(enemy_team, enemy_positions);
    }

    private void SpawnMonsters(List<BaseMonsterInfo> _team, GameObject _team_positions)
    {
        Vector2 position;
        Transform position_object_transform;

        for (int i = 0; i < _team.Count; i++)
        {
            position_object_transform = _team_positions.transform.GetChild(i).transform;
            position = position_object_transform.position;
            GameObject.Instantiate(prefab_manager.GetMonsterPrefab(_team[i].EntryNumber), position, Quaternion.identity, position_object_transform);
        }
    }
}
