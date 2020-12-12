using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject ally_positions = null;
    [SerializeField] private GameObject enemy_positions = null;
    [SerializeField] private GameObject game_manager_prefab = null;
    [SerializeField] private GameObject actions = null;

    [SerializeField] private GameObject game_manager;

    private PlayerManagement player;
    private LevelManagement level_manager;
    private PrefabStorage prefab_manager;
    private MovebarManagement move_bar;
    private List<GameObject> monsters_in_fight;
    private GameObject player_monster_to_move;
    private MonsterData player_target = null;
    private MonsterData enemy_target = null;

    private void Start()
    {
        player = game_manager.GetComponent<PlayerManagement>();
        level_manager = game_manager.GetComponent<LevelManagement>();
    //    prefab_manager = game_manager.GetComponent<PrefabStorage>();
        monsters_in_fight = new List<GameObject>();

    //    move_bar = new MovebarManagement();

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();

    //    StartBattle();
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Collider2D found_collider = Physics2D.OverlapCircle(GeneralScripts.GetMousePositionInWorldSpace(), 0.1f);

    //        if (found_collider != null)
    //        {
    //            GameObject found_object = found_collider.gameObject;

    //            if ((found_object.GetComponent<MonsterPrefab>() != null) && (found_object.GetComponent<MonsterPrefab>().monster_data.fight_with_player))
    //            {
    //                player_target = found_object.GetComponent<MonsterPrefab>().monster_data;
    //            }
    //        }
    //    }
    //}

    private void SpawnMonstersForPlayer()
    {
        GameObject monster = new GameObject("monster");
        monster.AddComponent<SpriteRenderer>();
        monster.AddComponent<Monster>().monster_data = player.player_data.Monsters_in_party[0];
        
        List <GameObject> monsters = new List<GameObject>();

        monsters.Add(monster);

        SpawnMonsters(monsters, ally_positions);

        enemy_target = player.player_data.Monsters_in_party[0];
    }

    private void SpawnMonstersForEnemy()
    {
        List<GameObject> enemy_team = level_manager.GenerateMonstersForEnemy();

        SpawnMonsters(enemy_team, enemy_positions);

        //player_target = enemy_team[0].GetComponent<Monster>().monster_data;
    }

    private void SpawnMonsters(List<GameObject> _team, GameObject _team_positions)
    {
        Vector2 position;
        Transform position_object_transform;
        GameObject monster;

        for (int i = 0; i < _team.Count; i++)
        {
            position_object_transform = _team_positions.transform.GetChild(i).transform;

            _team[i].transform.position = position_object_transform.position;

            //move_bar.AddMonsterToFight(monster.GetComponent<MonsterPrefab>().monster_data);
            //monsters_in_fight.Add(monster);
        }
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
