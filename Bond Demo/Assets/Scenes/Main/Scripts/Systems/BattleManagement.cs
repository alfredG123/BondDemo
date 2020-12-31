using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject player_prefab_objects;
    [SerializeField] private GameObject enemy_positions;
    [SerializeField] private SpiritInLevel level;
    [SerializeField] private GameObject actions;
    [SerializeField] private GameObject moves;
    [SerializeField] private GameObject maze_manager;
    [SerializeField] private GameObject battle_field;
    [SerializeField] private CameraMovement camera_movement;
#pragma warning restore 0649

    private GameObject _game_manager;
    private PlayerManagement _player;
    private MovebarManagement _move_bar;
    private GameObject _current_spirit_to_move;
    private bool _trigger_encounter = false;

    private void Start()
    {
#if TEMP_LOCK
        game_manager = GameObject.Find("GameManager");

        if (game_manager == null)
        {
            GeneralScripts.ReturnToTitleSceneForErrors("BattleManagement.Start", "The game manager is not found.");
            return;
        }

        player = game_manager.GetComponent<PlayerManagement>();
#endif

        _move_bar = new MovebarManagement();

        _trigger_encounter = true;
    }

    private void Update()
    {
        if (_trigger_encounter)
        {
            maze_manager.GetComponent<MazeManagement>().SetMapVisibility(false);

            battle_field.SetActive(true);

            camera_movement.enabled = false;

            GeneralScripts.SetMainCameraPositionXYOnly(battle_field.transform.position);

            SpawnSpiritForPlayer();
            SpawnSpiritForEnemy();

            StartNewTurn();

            _trigger_encounter = false;
        }
    }

    private void SpawnSpiritForPlayer()
    {
#if TEMP_LOCK
        Spirit spirit = player.GetSpiritFromParty(0);
#endif
        List<BaseSpiritData> party = level.SpiritsInLevel;
        Spirit spirit = new Spirit(party[0]);

        // IsAlly need to be changed to the variable
        SpawnSpirit(spirit, player_prefab_objects, 0, true);  
    }

    private void SpawnSpiritForEnemy()
    {
        List<BaseSpiritData> enemy_party = level.SpiritsInLevel;
        Spirit spirit_to_spawn;

        spirit_to_spawn = new Spirit(enemy_party[1]);
        SpawnSpirit(spirit_to_spawn, enemy_positions, 0, false);

#if TEMP_LOCK
        for (int i = 0; i < enemy_party.Count; i++)
        {
            spirit_to_spawn = new Spirit(enemy_party[i]);
            SpawnSpirit(spirit_to_spawn, enemy_positions, i, false);
        }
#endif
    }

    private void SpawnSpirit(Spirit spirit_to_spawn, GameObject spirit_prefab_objects, int spirit_position_index, bool is_ally)
    {
        GameObject prefab = spirit_prefab_objects.transform.GetChild(spirit_position_index).gameObject;

        SpiritPrefab spirit_component = prefab.GetComponent<SpiritPrefab>();

        spirit_component.SetSpirit(spirit_to_spawn, is_ally);

        _move_bar.AddSpiritToFight(prefab);
    }

    private void StartNewTurn()
    {
        bool is_player_move;

        _current_spirit_to_move = _move_bar.GetFirstSpirit();

        is_player_move = _current_spirit_to_move.GetComponent<SpiritPrefab>().Spirit.IsAlly;

        if (is_player_move)
        {
            actions.SetActive(true);
        }
        else
        {
            EnemyMakeMove();
        }
    }

    public void EnemyMakeMove()
    {
        _current_spirit_to_move.GetComponent<SpiritPrefab>().PlayAttackAnimation();

        StartCoroutine(nameof(CalculatePostTurn));
    }

    public void GameOver()
    {
        battle_field.SetActive(false);

        maze_manager.GetComponent<MazeManagement>().SetMapVisibility(true);
    }

    public void TriggerEncounter()
    {
        _trigger_encounter = true;

        camera_movement.enabled = true;
    }

    private IEnumerator CalculatePostTurn()
    {
        yield return new WaitForSeconds(2f);

        StartNewTurn();
    }

    public void PlayerOrderAction()
    {
        actions.SetActive(false);

        moves.SetActive(true);
    }

    public void PlayerSwitchAction()
    {

    }

    public void PlayerItemAction()
    {

    }

    public void PlayerSkillAction()
    {

    }

    public void PlayerEvoluteAction()
    {

    }


    public void PlayerMove1()
    {
        PerformMove();
    }

    public void PlayerMove2()
    {
        PerformMove();
    }

    public void PlayerMove3()
    {
        PerformMove();
    }

    public void PlayerMove4()
    {
        PerformMove();
    }

    private void PerformMove()
    {
        moves.SetActive(false);

        _current_spirit_to_move.GetComponent<SpiritPrefab>().PlayAttackAnimation();

        StartCoroutine(nameof(CalculatePostTurn));
    }

    public void BackToMain()
    {
        moves.SetActive(false);

        actions.SetActive(true);
    }
}
