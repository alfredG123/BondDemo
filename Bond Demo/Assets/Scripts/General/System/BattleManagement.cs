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

    private GameObject game_manager;
    private PlayerManagement player;
    private LevelManagement level_manager;
    private PrefabStorage prefab_manager;
    private MovebarManagement move_bar;
    private List<GameObject> monsters_in_fight;
    private GameObject player_monster_to_move;
    private BaseMonsterInfo player_target = null;
    private BaseMonsterInfo enemy_target = null;

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

        monsters_in_fight = new List<GameObject>();

        move_bar = new MovebarManagement();

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();

        StartBattle();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D found_collider = Physics2D.OverlapCircle(GeneralScripts.GetMousePositionInWorldSpace(), 0.1f);
            
            if (found_collider != null)
            {
                GameObject found_object = found_collider.gameObject;
                
                if ((found_object.GetComponent<BaseMonsterObject>() != null) && (found_object.GetComponent<BaseMonsterObject>().MonsterInfo.IsEnemy))
                {
                    player_target = found_object.GetComponent<BaseMonsterObject>().MonsterInfo;
                }
            }
        }
    }

    private void SpawnMonstersForPlayer()
    {
        SpawnMonsters(player.Team, ally_positions);

        enemy_target = player.Team[0];
    }

    private void SpawnMonstersForEnemy()
    {
        List<BaseMonsterInfo> enemy_team = level_manager.GenerateMonstersForEnemy();

        SpawnMonsters(enemy_team, enemy_positions);

        player_target = enemy_team[0];
    }

    private void SpawnMonsters(List<BaseMonsterInfo> _team, GameObject _team_positions)
    {
        Vector2 position;
        Transform position_object_transform;
        GameObject monster;
        Vector2 local_scale;

        for (int i = 0; i < _team.Count; i++)
        {
            position_object_transform = _team_positions.transform.GetChild(i).transform;
            position = position_object_transform.position;
            monster = GameObject.Instantiate(prefab_manager.GetMonsterPrefab(_team[i].EntryNumber), position, Quaternion.identity, position_object_transform);
            
            if (_team[i].IsEnemy)
            {
                local_scale = monster.transform.localScale;
                local_scale.x *= -1;
                monster.transform.localScale = local_scale;
            }
            
            move_bar.AddMonsterToFight(monster.GetComponent<BaseMonsterObject>().MonsterInfo);
            monsters_in_fight.Add(monster);
        }
    }

    public void StartBattle()
    {
        Button attack;
        bool is_player_move;
        BaseMonsterInfo monster_to_move = move_bar.GetFirstMonster();

        GameObject monster_to_move_object = monsters_in_fight.Find(monster => monster.GetComponent<BaseMonsterObject>().MonsterInfo.MonsterName == monster_to_move.MonsterName);
        monster_to_move_object.GetComponent<BaseMonsterObject>().ReadyToMove();

        is_player_move = !monster_to_move_object.GetComponent<BaseMonsterObject>().PlanMove();

        if (is_player_move)
        {
            player_monster_to_move = monster_to_move_object;
            attack = actions.transform.GetChild(0).gameObject.GetComponent<Button>();
            attack.onClick.AddListener(() => MakeMove(0));
            actions.SetActive(true);
        }
        else
        {
            monster_to_move_object.GetComponent<BaseMonsterObject>().EnemyMakeMove();
        }
    }

    public void MakeMove(int move_index)
    {
        player_monster_to_move.GetComponent<BaseMonsterObject>().PlayerMakeMove(move_index);
    }
}
