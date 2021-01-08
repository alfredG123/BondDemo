using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgressionManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private SpiritsInLevel TemporarySpiritList;
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

        SetUpBattleField();
    }

    /// <summary>
    /// Set up the spirits on the battle field
    /// </summary>
    public void SetUpBattleField()
    {
        SpawnSpiritForPlayer();

        SpawnSpiritForEnemy();

        GetComponent<BattleCommandsManagement>().SetUpForFirstDecision();
    }

    /// <summary>
    /// Get spirits from the player, and set it up
    /// </summary>
    private void SpawnSpiritForPlayer()
    {
        for (int i = 0; i < PlayerSpiritPrefabObjects.transform.childCount; i++)
        {
            Spirit spirit = new Spirit(TemporarySpiritList.GetSpiritData(0));

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
            Spirit spirit = new Spirit(TemporarySpiritList.GetSpiritData(1));

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

        SpiritPrefab spirit_prefab = prefab.GetComponent<SpiritPrefab>();

        spirit_prefab.SetSpirit(spirit_to_spawn, is_ally);

        _spirit_move_order_list.AddSpiritObjectToList(prefab);
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
        bool target_faint;

        while (_spirit_move_order_list.HasSpiritToMove())
        {
            spirit_to_move = _spirit_move_order_list.GetSpiritToMove();

            prefab = GeneralScripts.GetSpiritPrefabScript(spirit_to_move);

            if (prefab.Spirit.IsAlly)
            {
                move_is_perform = PerformAction(spirit_to_move, prefab.GetTarget(), prefab.GetSkill());
            }
            else
            {
                move_is_perform = PerformAction(spirit_to_move, PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject, prefab.GetSkill());
            }

            yield return new WaitForSeconds(1f);
            
            if (move_is_perform)
            {
                if (prefab.Spirit.IsAlly)
                {
                    target_faint = TakeAction(spirit_to_move, prefab.GetTarget(), prefab.GetSkill());
                }
                else
                {
                    target_faint = TakeAction(spirit_to_move, PlayerSpiritPrefabObjects.transform.GetChild(0).gameObject, prefab.GetSkill());
                }
            }

            yield return new WaitForSeconds(1f);
        }

        GetComponent<BattleCommandsManagement>().SetUpForFirstDecision();
    }

    private bool PerformAction(GameObject spirit_to_move, GameObject target, SpiritSkill skill)
    {
        SpiritPrefab spirit_prefab;
        SpiritSkill skill_to_perform;
        bool move_is_perform;

        spirit_prefab = GeneralScripts.GetSpiritPrefabScript(spirit_to_move);

        skill_to_perform = skill;
        move_is_perform = spirit_prefab.PerformSkill(skill_to_perform);

        spirit_prefab.PlayAttackAnimation();

        return (move_is_perform);
    }

    private bool TakeAction(GameObject spirit_to_move, GameObject target, SpiritSkill skill)
    {
        SpiritPrefab spirit_prefab;
        SpiritPrefab target_prefab;
        SpiritSkill skill_to_perform;
        bool target_faint;

        spirit_prefab = GeneralScripts.GetSpiritPrefabScript(spirit_to_move);

        skill_to_perform = skill;

        target_prefab = GeneralScripts.GetSpiritPrefabScript(target);

        target_faint = target_prefab.TakeSkill(spirit_prefab.CalculateDamage(skill_to_perform));

        target_prefab.PlayHitAnimation();

        return (target_faint);
    }
}
