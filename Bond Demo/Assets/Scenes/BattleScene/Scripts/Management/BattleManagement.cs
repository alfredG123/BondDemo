using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject player_prefab_objects = null;
    [SerializeField] private GameObject enemy_positions = null;
    [SerializeField] private MonsterInLevel level = null;
    [SerializeField] private GameObject actions = null;

    private GameObject game_manager;
    private PlayerManagement player;
    private MovebarManagement move_bar;
    private GameObject current_monster_to_move;

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

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();

        button = actions.transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(() => PlayerMakeMove());
        button = actions.transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(() => GameOver());

        StartNewTurn();
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

            Debug.Log(current_monster_to_move.GetComponent<MonsterPrefab>().Monster.MonsterName + "'s turn.");

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
        actions.SetActive(false);

        Debug.Log("Game is over.");
    }

    private IEnumerator CalculatePostTurn()
    {
        yield return new WaitForSeconds(2f);

        StartNewTurn();
    }
}
