using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject player_prefab_objects;
    [SerializeField] private GameObject enemy_positions;
    [SerializeField] private MonsterInLevel level;
    [SerializeField] private GameObject actions;
    [SerializeField] private GameObject maze_manager;
    [SerializeField] private GameObject battle_field;

    private GameObject game_manager;
    private PlayerManagement player;
    private MovebarManagement move_bar;
    private GameObject current_monster_to_move;

    private bool trigger_encounter = false;

    private void Start()
    {
        Button button;

        game_manager = GameObject.Find("GameManager");

        if (game_manager == null)
        {
            GeneralScripts.ReturnToStarterScene("BattleManagement");
            return;
        }

        player = game_manager.GetComponent<PlayerManagement>();

        move_bar = new MovebarManagement();

        button = actions.transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(() => PlayerMakeMove());

    }

    private void Update()
    {
        if (trigger_encounter)
        {
            maze_manager.GetComponent<MazeManagement>().SetMapVisibility(false);

            battle_field.SetActive(true);

            GeneralScripts.SetMainCameraPositionXYOnly(battle_field.transform.position);

            SpawnMonstersForPlayer();
            SpawnMonstersForEnemy();

            StartNewTurn();

            trigger_encounter = false;
        }
    }

    private void SpawnMonstersForPlayer()
    {
        Monster monster = player.GetMonsterFromParty(0);

        SpawnMonster(monster, player_prefab_objects, 0);  
    }

    private void SpawnMonstersForEnemy()
    {
        List<MonsterData> enemy_party = level.MonstersInLevel;
        Monster monster_to_spawn;

        for (int i = 0; i < enemy_party.Count; i++)
        {
            monster_to_spawn = new Monster(enemy_party[i]);
            SpawnMonster(monster_to_spawn, enemy_positions, i);
        }
    }

    private void SpawnMonster(Monster _monster_to_spawn, GameObject _monster_prefab_objects, int _monster_position_index)
    {
        GameObject prefab = _monster_prefab_objects.transform.GetChild(_monster_position_index).gameObject;

        MonsterPrefab monster_component = prefab.GetComponent<MonsterPrefab>();

        monster_component.Monster = _monster_to_spawn;

        prefab.GetComponent<SpriteRenderer>().sprite = _monster_to_spawn.MonsterSprite;

        Animator animator = prefab.GetComponent<Animator>();

        animator.SetBool("IsAlly", monster_component.Monster.IsAlly);

        move_bar.AddMonsterToFight(prefab);
    }

    private void StartNewTurn()
    {
        bool is_player_move;

        current_monster_to_move = move_bar.GetFirstMonster();

        is_player_move = current_monster_to_move.GetComponent<MonsterPrefab>().Monster.IsAlly;

        if (is_player_move)
        {
            actions.SetActive(true);
        }
        else
        {
            EnemyMakeMove();
        }
    }

    public void PlayerMakeMove()
    {
        actions.SetActive(false);

        current_monster_to_move.GetComponent<MonsterPrefab>().PlayerMakeMove();

        StartCoroutine(nameof(CalculatePostTurn));
    }

    public void EnemyMakeMove()
    {
        current_monster_to_move.GetComponent<MonsterPrefab>().EnemyMakeMove();

        StartCoroutine(nameof(CalculatePostTurn));
    }

    public void GameOver()
    {
        battle_field.SetActive(false);

        maze_manager.GetComponent<MazeManagement>().SetMapVisibility(true);
    }

    public void TriggerEncounter()
    {
        trigger_encounter = true;
    }

    private IEnumerator CalculatePostTurn()
    {
        yield return new WaitForSeconds(2f);

        StartNewTurn();
    }
}
