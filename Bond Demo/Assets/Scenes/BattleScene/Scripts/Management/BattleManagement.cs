using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject player_positions = null;
    [SerializeField] private GameObject enemy_positions = null;
    [SerializeField] private MonsterInLevel level = null;
    //[SerializeField] private GameObject actions = null;

    private GameObject game_manager;
    private PlayerManagement player;
    private MovebarManagement move_bar;
    //private GameObject player_monster_to_move;
    private GameObject player_target = null;
    private GameObject enemy_target = null;

    private void Start()
    {
        game_manager = GameObject.Find("GeneralGameManager");

        if (game_manager == null)
        {
            GeneralScripts.ReturnToStarterScene("BattleManagement");
            return;
        }

        player = game_manager.GetComponent<PlayerManagement>();

        move_bar = new MovebarManagement();

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();

    //    StartBattle();
    }

    private void SpawnMonstersForPlayer()
    {
        GameObject monster = player.GetMonsterFromParty(0);

        SpawnMonster(monster, player_positions, 0);
        
        enemy_target = monster;
    }

    private void SpawnMonstersForEnemy()
    {
        List<GameObject> enemy_party = level.MonstersInLevel;
        GameObject monster;

        for (int i = 0; i < enemy_party.Count; i++)
        {
            monster = GameObject.Instantiate(enemy_party[i], Vector2.zero, Quaternion.identity);
            SpawnMonster(monster, enemy_positions, i);
        }

        player_target = enemy_party[0];
    }

    private void SpawnMonster(GameObject _monster_to_spawn, GameObject _party_positions, int _monster_position_index)
    {
        //Instantiate?

        Transform position_object_transform;

        position_object_transform = _party_positions.transform.GetChild(_monster_position_index).transform;
        
        _monster_to_spawn.transform.position = position_object_transform.position;
        
        move_bar.AddMonsterToFight(_monster_to_spawn);
    }

    //public void StartBattle()
    //{
    //    Button attack;
    //    bool is_player_move;
    //    MonsterData monster_to_move = move_bar.GetFirstMonster();

    //    GameObject monster_to_move_object = monsters_in_fight.Find(monster => monster.GetComponent<MonsterPrefab>().monster_data.monster_name == monster_to_move.monster_name);
    //    monster_to_move_object.GetComponent<MonsterPrefab>().ReadyToMove();

    //    is_player_move = !monster_to_move_object.GetComponent<MonsterPrefab>().PlanMove();

    //    if (is_player_move)
    //    {
    //        player_monster_to_move = monster_to_move_object;
    //        attack = actions.transform.GetChild(0).gameObject.GetComponent<Button>();
    //        attack.onClick.AddListener(() => MakeMove(0));
    //        actions.SetActive(true);
    //    }
    //    else
    //    {
    //        monster_to_move_object.GetComponent<MonsterPrefab>().EnemyMakeMove();
    //    }
    //}

    //public void MakeMove(int move_index)
    //{
    //    player_monster_to_move.GetComponent<MonsterPrefab>().PlayerMakeMove(move_index);
    //}
}
