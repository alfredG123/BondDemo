using System.Collections;
using UnityEngine;

public class BattleProgressionManagement : MonoBehaviour
{
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

        SetUpPrefab();

        GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
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
        for (int i = 0; i < PlayerSpiritPrefabObjects.transform.childCount; i++)
        {
            Spirit spirit = new Spirit(TemporaryPlayerTeam.GetRandomSpiritData());

            SpawnSpirit(spirit, PlayerSpiritPrefabObjects, i, true);
        }
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
        int ai_select_target_index = 0;
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
                    target_faint = TakeAction(spirit_to_move, target_spirit, prefab.GetMove());
                }
                else
                {
                    do
                    {
                        target_spirit = PlayerSpiritPrefabObjects.transform.GetChild(ai_select_target_index).gameObject;
                        ai_select_target_index++;
                    }
                    while (!target_spirit.activeSelf);

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
        }

        if (battle_over)
        {
            Debug.Log("Over");
        }
        else
        {
            GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
        }
    }

    private bool PerformAction(GameObject spirit_to_move, SpiritMove move)
    {
        SpiritPrefab spirit_prefab;
        SpiritMove move_to_perform;
        bool move_is_perform;

        spirit_prefab = General.GetSpiritPrefabComponent(spirit_to_move);

        move_to_perform = move;
        move_is_perform = spirit_prefab.PerformMove(move_to_perform);

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

        target_faint = target_prefab.TakeMove(spirit_prefab.CalculateDamage(move_to_perform));

        return (target_faint);
    }

    private bool CheckBattleStatus()
    {
        bool battle_over = false;
        int faint_spirits_count = 0;

        for (int i = 0; i < 3; i++)
        {
            if (!PlayerSpiritPrefabObjects.transform.GetChild(i).gameObject.activeSelf)
            {
                faint_spirits_count++;
            }
        }

        if (faint_spirits_count == 3)
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
}
