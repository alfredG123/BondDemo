using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject player_prefab_objects;
    [SerializeField] private GameObject enemy_positions;
    [SerializeField] private SpiritInLevel level;
    [SerializeField] private GameObject actions;
    [SerializeField] private GameObject maze_manager;
    [SerializeField] private GameObject battle_field;

    private GameObject game_manager;
    private PlayerManagement player;
    private MovebarManagement move_bar;
    private GameObject current_spirit_to_move;

    private bool trigger_encounter = false;

    private void Start()
    {
        Button button;

        game_manager = GameObject.Find("GameManager");

        if (game_manager == null)
        {
            GeneralScripts.ReturnToTitleSceneForErrors("BattleManagement.Start", "The game manager is not found.");
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

            SpawnSpiritForPlayer();
            SpawnSpiritForEnemy();

            StartNewTurn();

            trigger_encounter = false;
        }
    }

    private void SpawnSpiritForPlayer()
    {
        Spirit spirit = player.GetSpiritFromParty(0);

        SpawnSpirit(spirit, player_prefab_objects, 0);  
    }

    private void SpawnSpiritForEnemy()
    {
        List<BaseSpiritData> enemy_party = level.SpiritsInLevel;
        Spirit spirit_to_spawn;

        for (int i = 0; i < enemy_party.Count; i++)
        {
            spirit_to_spawn = new Spirit(enemy_party[i]);
            SpawnSpirit(spirit_to_spawn, enemy_positions, i);
        }
    }

    private void SpawnSpirit(Spirit _spirit_to_spawn, GameObject _spirit_prefab_objects, int _spirit_position_index)
    {
        GameObject prefab = _spirit_prefab_objects.transform.GetChild(_spirit_position_index).gameObject;

        SpiritPrefab spirit_component = prefab.GetComponent<SpiritPrefab>();

        spirit_component.SetSpirit(_spirit_to_spawn, spirit_component.Spirit.IsAlly);

        move_bar.AddSpiritToFight(prefab);
    }

    private void StartNewTurn()
    {
        bool is_player_move;

        current_spirit_to_move = move_bar.GetFirstSpirit();

        is_player_move = current_spirit_to_move.GetComponent<SpiritPrefab>().Spirit.IsAlly;

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

        current_spirit_to_move.GetComponent<SpiritPrefab>().PlayAttackAnimation();

        StartCoroutine(nameof(CalculatePostTurn));
    }

    public void EnemyMakeMove()
    {
        current_spirit_to_move.GetComponent<SpiritPrefab>().PlayAttackAnimation();

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
