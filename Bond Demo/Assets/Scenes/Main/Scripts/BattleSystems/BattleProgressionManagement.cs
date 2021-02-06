using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgressionManagement : MonoBehaviour
{
    [SerializeField] private SpiritSpriteCollection _SpiritSpriteCollection = null;

    [SerializeField] private GameObject _PlayerSpiritPrefabGroup = null;
    [SerializeField] private GameObject _EnemySpiritPrefabGroup = null;

    [SerializeField] private BattleButtonsHanlder _BattleButtonsHanlder = null;
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;

    [SerializeField] private GameObject _TemporaryPlayer = null;

    private SpiritMoveOrderManagement _SpiritMoveOrderManagement = null;

    private void Start()
    {
        _SpiritMoveOrderManagement = new SpiritMoveOrderManagement();
    }

    public void TriggerEncounter()
    {
        _BattleDisplayHanlder.SetUpBattleUI();

        SetUpPrefab();

        _BattleButtonsHanlder.SetUpForFirstDecision();
    }

    /// <summary>
    /// Set up the spirits on the battle field
    /// </summary>
    public void SetUpPrefab()
    {
        SpawnSpiritForPlayer();

        SpawnSpiritForEnemy(3);
    }

    /// <summary>
    /// Get spirits from the player, and set it up
    /// </summary>
    private void SpawnSpiritForPlayer()
    {
        GameObject game_management = GameObject.Find("GameManager");
        List<Spirit> party;

        if (game_management != null)
        {
            party = game_management.GetComponent<PlayerManagement>().Party;
        }
        else
        {
            _TemporaryPlayer.GetComponent<PlayerManagement>().SetUpTemporaryParty();

            party = _TemporaryPlayer.GetComponent<PlayerManagement>().Party;
        }

        for (int i = 0; i < party.Count; i++)
        {
            SpawnSpirit(party[i], _PlayerSpiritPrefabGroup, i);
        }
    }

    /// <summary>
    /// Get spirits from the scriptable object level
    /// </summary>
    private void SpawnSpiritForEnemy(int enemy_count)
    {
        Spirit spirit;

        for (int i = 0; i < enemy_count; i++)
        {
            spirit = new Spirit(BaseSpirit.E1, false);

            SpawnSpirit(spirit, _EnemySpiritPrefabGroup, i);
        }
    }

    /// <summary>
    /// Set up the prefab, and add it to the move order list
    /// </summary>
    /// <param name="spirit_to_spawn"></param>
    /// <param name="spirit_prefab_objects"></param>
    /// <param name="spirit_position_index"></param>
    /// <param name="is_ally"></param>
    private void SpawnSpirit(Spirit spirit_to_spawn, GameObject spirit_prefab_objects, int spirit_position_index)
    {
        GameObject prefab = spirit_prefab_objects.transform.GetChild(spirit_position_index).gameObject;

        prefab.GetComponent<SpiritPrefab>().Spirit = spirit_to_spawn;

        prefab.SetActive(true);

        prefab.GetComponent<SpriteRenderer>().sprite = _SpiritSpriteCollection.GetSpiritSpriteByImageName(spirit_to_spawn.ImageName);

        prefab.transform.GetChild(0).GetComponent<StatusHandler>().InitializeStatus(spirit_to_spawn);
    }

    public void StartBattle()
    {
        SetUpMoveOrder();

        StartCoroutine(nameof(PerformBattle));
    }

    private void SetUpMoveOrder()
    {
        for (int i = 0; i < _PlayerSpiritPrefabGroup.transform.childCount; i++)
        {
            if (_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
            {
                _SpiritMoveOrderManagement.AddSpiritObjectToList(_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < _EnemySpiritPrefabGroup.transform.childCount; i++)
        {
            if (_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
            {
                _EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.GetComponent<EnemyBattleLogic>().SetSpiritBattleInfo();

                _SpiritMoveOrderManagement.AddSpiritObjectToList(_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject);
            }
        }
    }

    private IEnumerator PerformBattle()
    {
        GameObject spirit_to_move;
        GameObject target;
        SpiritPrefab prefab;
        bool is_spirit_faint = false;
        bool is_player_winning;

        while (_SpiritMoveOrderManagement.HasSpiritToMove())
        {
            spirit_to_move = _SpiritMoveOrderManagement.GetSpiritToMove();

            prefab = spirit_to_move.GetComponent<SpiritPrefab>();

            PerformAction(spirit_to_move);

            yield return new WaitForSeconds(1f);

            target = prefab.TargetToAim;

            if (prefab.MoveToPerform.TargetSelectionType != TypeTargetSelection.SelfTarget)
            {
                if (target.activeSelf)
                {
                    is_spirit_faint = TakeAction(spirit_to_move, target, prefab.MoveToPerform);
                }
                else
                {
                    Debug.Log("The target has fainted!");
                }
            }

            if (is_spirit_faint)
            {
                target.SetActive(false);
            }
        }

        if (CheckBattleResult(out is_player_winning))
        {
            if (is_player_winning)
            {
                Debug.Log("Player win!");
            }
            else
            {
                Debug.Log("Player lose!");
            }
        }
        else
        {
            GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
        }
    }

    private void PerformAction(GameObject spirit_to_move)
    {
        SpiritPrefab spirit_prefab;

        spirit_prefab = spirit_to_move.GetComponent<SpiritPrefab>();

        if (spirit_prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.SelfTarget)
        {
            Debug.Log(spirit_prefab.Spirit.Name + " uses " + spirit_prefab.MoveToPerform.Name + "!");
        }
        else
        {
            Debug.Log(spirit_prefab.Spirit.Name + " uses " + spirit_prefab.MoveToPerform.Name + " at " + spirit_prefab.TargetToAim.GetComponent<SpiritPrefab>().Spirit.Name + "!");

            spirit_prefab.PerformMove(spirit_prefab.MoveToPerform);
        }
    }

    private bool TakeAction(GameObject spirit_to_move, GameObject target, BaseMove move)
    {
        SpiritPrefab spirit_prefab;
        SpiritPrefab target_prefab;
        bool target_faint;

        spirit_prefab = spirit_to_move.GetComponent<SpiritPrefab>();

        target_prefab = target.GetComponent<SpiritPrefab>();

        target_faint = target_prefab.TakeMove(spirit_prefab.Spirit, move);

        return (target_faint);
    }

    private bool CheckBattleResult(out bool player_win)
    {
        bool is_battle_over = false;
        player_win = false;
        int fainted_ally_count = _PlayerSpiritPrefabGroup.transform.childCount;
        int fainted_enemy_count = _EnemySpiritPrefabGroup.transform.childCount;

        if (!is_battle_over)
        {
            for (int i = 0; i < _PlayerSpiritPrefabGroup.transform.childCount; i++)
            {
                if (!_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    fainted_ally_count--;
                }
            }

            if (fainted_ally_count == 0)
            {
                is_battle_over = true;
            }
        }

        if (!is_battle_over)
        {
            for (int i = 0; i < _EnemySpiritPrefabGroup.transform.childCount; i++)
            {
                if (!_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    fainted_enemy_count--;
                }
            }

            if (fainted_enemy_count == 0)
            {
                is_battle_over = true;
                player_win = true;
            }
        }

        return (is_battle_over);
    }

    /*
#pragma warning disable 0649
    [SerializeField] private SpiritsInLevel TemporaryPlayerTeam;
    [SerializeField] private SpiritsInLevel TemporaryEnemeyTeam;

    [SerializeField] private GameObject PlayerSpiritPrefabObjects;
    [SerializeField] private GameObject EnemySpiritPrefabObjects;
#pragma warning restore 0649

    private SpiritMoveOrderManagement _spirit_move_order_list;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        _spirit_move_order_list = new SpiritMoveOrderManagement();
    }

    public SpiritMoveOrderManagement SpiritMoveOrderList
    {
        get => _spirit_move_order_list;
    }

    #region INITIAL_SET_UP

    /// <summary>
    /// Set up the spirits on the battle field
    /// </summary>
    public void SetUpPrefab()
    {
        SpawnSpiritForPlayer();

        SpawnSpiritForEnemy();
    }

    /// <summary>
    /// Get spirits from the player, and set it up
    /// </summary>
    private void SpawnSpiritForPlayer()
    {
        Spirit spirit = new Spirit(TemporaryPlayerTeam.GetRandomSpiritData());

        SpawnSpirit(spirit, PlayerSpiritPrefabObjects, 0, true);
    }

    /// <summary>
    /// Get spirits from the scriptable object level
    /// </summary>
    private void SpawnSpiritForEnemy()
    {
        for (int i = 0; i < EnemySpiritPrefabObjects.transform.childCount; i++)
        {
            Spirit spirit = new Spirit(TemporaryEnemeyTeam.GetRandomSpiritData());

            SpawnSpirit(spirit, EnemySpiritPrefabObjects, i, false);
        }
    }

    /// <summary>
    /// Set up the prefab, and add it to the move order list
    /// </summary>
    /// <param name="spirit_to_spawn"></param>
    /// <param name="spirit_prefab_objects"></param>
    /// <param name="spirit_position_index"></param>
    /// <param name="is_ally"></param>
    private void SpawnSpirit(Spirit spirit_to_spawn, GameObject spirit_prefab_objects, int spirit_position_index, bool is_ally)
    {
        GameObject prefab = spirit_prefab_objects.transform.GetChild(spirit_position_index).gameObject;

        prefab.SetActive(true);

        SpiritPrefab spirit_prefab = prefab.GetComponent<SpiritPrefab>();

        spirit_prefab.SetSpirit(spirit_to_spawn, is_ally);

        _spirit_move_order_list.AddSpiritObjectToList(prefab);
    }
    #endregion

    public void TriggerEncounter()
    {
        GetComponent<BattleDisplayHandler>().SetUpBattleUI();

        SetUpPrefab();

        GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
    }

    public void StartBattle()
    {
        _spirit_move_order_list.SetUpMoveOrder();

        StartCoroutine(nameof(PerformBattle));
    }

    private IEnumerator PerformBattle()
    {
        GameObject spirit_to_move;
        SpiritPrefab prefab;
        bool move_is_perform;
        bool target_faint =  false;
        GameObject target_spirit = null;
        bool battle_over = false;

        while (_spirit_move_order_list.HasSpiritToMove())
        {
            spirit_to_move = _spirit_move_order_list.GetSpiritToMove();

            prefab = General.GetSpiritPrefabComponent(spirit_to_move);

            if (prefab.Spirit.IsAlly)
            {
                move_is_perform = PerformAction(spirit_to_move, prefab.GetMove());
            }
            else
            {
                move_is_perform = PerformAction(spirit_to_move, prefab.GetMove());
            }

            yield return new WaitForSeconds(1f);

            if (move_is_perform)
            {
                if (prefab.Spirit.IsAlly)
                {
                    target_spirit = prefab.GetTarget();

                    if (prefab.GetTarget().activeSelf)
                    {
                        target_faint = TakeAction(spirit_to_move, target_spirit, prefab.GetMove());
                    }
                    else
                    {
                        GetComponent<BattleDisplayHandler>().DisplayBattleNarrativeForMissingTarget();
                    }
                }
                else
                {
                    target_spirit = PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject;

                    target_faint = TakeAction(spirit_to_move, target_spirit, prefab.GetMove());
                }
            }

            yield return new WaitForSeconds(1f);

            if (target_faint)
            {
                _spirit_move_order_list.RemoveFaintSpirit(target_spirit);

                target_spirit.SetActive(false);

                battle_over = CheckBattleStatus();
            }

            yield return new WaitForSeconds(1f);

            GetComponent<BattleDisplayHandler>().DisableBattleNarrative();
        }

        if (battle_over)
        {
            bool win = true;
            int faint_spirits_count = 0;

            if (!PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject.activeSelf)
            {
                faint_spirits_count++;
            }

            if (faint_spirits_count == 1)
            {
                win = false;
            }

            if (win)
            {
                GetComponent<BattleDisplayHandler>().DisableBattle();
            }
            else
            {
                General.ReturnToTitleSceneForErrors("Battle", "I lose!!!");
            }
        }
        else
        {
            _spirit_move_order_list.SortList();

            GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();

            General.GetSpiritPrefabComponent(PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject).RestorEenergy();

            for (int i = 0; i < 3; i++)
            {
                General.GetSpiritPrefabComponent(EnemySpiritPrefabObjects.transform.GetChild(i).gameObject).RestorEenergy();
            }
        }
    }

    private bool PerformAction(GameObject spirit_to_move, SpiritMove move_to_perform)
    {
        SpiritPrefab spirit_prefab;
        bool move_is_perform = false;

        spirit_prefab = General.GetSpiritPrefabComponent(spirit_to_move);

        if (spirit_prefab.InDefenseState)
        {
            GetComponent<BattleDisplayHandler>().DisplayBattleNarrativeForDefense(spirit_prefab.Spirit);
        }
        else
        {
            move_is_perform = spirit_prefab.PerformMove(move_to_perform);

            GetComponent<BattleDisplayHandler>().DisplayBattleNarrativeForUsingMove(spirit_prefab.Spirit, General.GetSpiritPrefabComponent(spirit_prefab.GetTarget()).Spirit, move_to_perform);
        }

        return (move_is_perform);
    }

    private bool TakeAction(GameObject spirit_to_move, GameObject target, SpiritMove move)
    {
        SpiritPrefab spirit_prefab;
        SpiritPrefab target_prefab;
        SpiritMove move_to_perform;
        bool target_faint;

        spirit_prefab = General.GetSpiritPrefabComponent(spirit_to_move);

        move_to_perform = move;

        target_prefab = General.GetSpiritPrefabComponent(target);

        target_faint = target_prefab.TakeMove(spirit_prefab.Spirit, move_to_perform, GetComponent<BattleDisplayHandler>());

        return (target_faint);
    }

    private bool CheckBattleStatus()
    {
        bool battle_over = false;
        int faint_spirits_count = 0;

        if (!PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject.activeSelf)
        {
            faint_spirits_count++;
        }

        if (faint_spirits_count == 1)
        {
            battle_over = true;
        }

        if (!battle_over)
        {
            faint_spirits_count = 0;

            for (int i = 0; i < 3; i++)
            {
                if (!EnemySpiritPrefabObjects.transform.GetChild(i).gameObject.activeSelf)
                {
                    faint_spirits_count++;
                }
            }

            if (faint_spirits_count == 3)
            {
                battle_over = true;
            }
        }

        return (battle_over);
    }
    */
}
